using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Groundbeef.Drawing.ColorX
{
    public class HslColor : ColorBase<HslColorData>, IEquatable<HslColor?>
    {
        public HslColor(byte a, float h, float s, float l)
            : this(new HslColorData(a, h, s, l)) { }

        internal HslColor(HslColorData data)
        {
            m_data = data;
        }

        public byte A
        {
            get => m_data.A;
            set => m_data.A = value;
        }

        public float H
        {
            get => m_data.H;
            set
            {
                if (0f > value)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.H = value;
            }
        }

        public float S
        {
            get => m_data.S;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.S = value;
            }
        }

        public float L
        {
            get => m_data.L;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.L = value;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as HslColor);
        public bool Equals(HslColor? other) => other != null && m_data.Equals(other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data);
        internal override AdobeRgbColorData DataToAdobeRgb() => AdobeRgbColorData.FromXyz(DataToXyz());
        internal override CmykColorData DataToCmyk() => CmykColorData.FromSRgb(DataToSRgb());
        internal override HslColorData DataToHsl() => m_data;
        internal override HsvColorData DataToHsv() => HsvColorData.FromSRgb(DataToSRgb());
        internal override RgbColorData DataToRgb() => RgbColorData.FromSRgb(DataToSRgb());
        internal override ScRgbColorData DataToScRgb() => ScRgbColorData.FromSRgb(DataToSRgb());
        internal override SRgbColorData DataToSRgb() => HslColorData.ToSRgb(m_data);
        internal override XyzColorData DataToXyz() => XyzColorData.FromScRgb(DataToScRgb());

        public static bool operator ==(HslColor? left, HslColor? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(HslColor? left, HslColor? right) => !(left == right);

        public override string ToString()
        {
            var sb = new StringBuilder(32).Append('[');
            if (A != 0xFF)
                sb.Append("A = ").Append(A);
            sb.Append(", H = ").Append(H.ToString("f2")).Append('°');
            sb.Append(", S = ").Append(S.ToString("f2")).Append('%');
            sb.Append(", L = ").Append(L.ToString("f2")).Append('%');
            return sb.Append(']').ToString();
        }
    }

    /// <summary>
    /// Value type containing 32bit floating point HSL color data, with a 8bit integer alpha component.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 15)]
    public struct HslColorData : IEquatable<HslColorData>
    {
        public byte A;
        public float H, S, L;

        public HslColorData(byte a, float h, float s, float l)
        {
            if (0f > h) throw new ArgumentOutOfRangeException(nameof(h));
            if (0f > s || s < 1f) throw new ArgumentOutOfRangeException(nameof(s));
            if (0f > l || l < 1f) throw new ArgumentOutOfRangeException(nameof(l));
            A = a;
            H = h % 360;
            S = s;
            L = l;
        }

        public override bool Equals(object? obj) => obj is HslColorData color && Equals(color);
        public bool Equals(HslColorData other) => A == other.A && H == other.H && S == other.S && L == other.L;
        public override int GetHashCode() => HashCode.Combine(A, H, S, L);

        public static bool operator ==(HslColorData left, HslColorData right) => left.Equals(right);
        public static bool operator !=(HslColorData left, HslColorData right) => !(left == right);

        public static readonly HslColorData Empty = new HslColorData();


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SRgbColorData ToSRgb(in HslColorData hsl)
        {
            // Normalize hue
            float h = hsl.H / 360f;
            float q = 1 < 0.5f ? hsl.L * (1 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S);
            float p = (2f * hsl.S) - q;
            return new SRgbColorData(hsl.A,
                HueToRgb(p, q, h + 1f / 3f),
                HueToRgb(p, q, h),
                HueToRgb(p, q, h - 1f / 3f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HslColorData FromSRgb(in SRgbColorData sRgb)
        {
            float max = Math.Max(sRgb.R, Math.Max(sRgb.G, sRgb.B)),
                  min = Math.Min(sRgb.R, Math.Min(sRgb.G, sRgb.B)),
                  delta = max - min;
            float h = 60f,
                  l = (max + min) * .5f,
                  s = max - min == 0 ? 0 : delta / (1 - Math.Abs(2 * l - 1));
            if (delta == 0)
                h *= 0;
            else if (max == sRgb.R)
                h *= ((sRgb.G - sRgb.B) / delta) % 6;
            else if (max == sRgb.G)
                h *= (sRgb.B - sRgb.R) / delta + 2f;
            else if (max == sRgb.B)
                h *= (sRgb.R - sRgb.G) / delta + 2f;
            return new HslColorData(sRgb.A, h, s, l);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float HueToRgb(float p, float q, float t)
        {
            if (t < 0f) t++;
            if (t > 1f) t--;
            if (t < 1f / 6f)
                return p + (q - p) * 6f * t;
            if (t < 0.5f)
                return q;
            if (t < 2f / 3f)
                return p + (q - p) * (2f / 3f - t) * 6f;
            return p;
        }
    }
}
