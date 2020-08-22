using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Groundbeef.Drawing.ColorX
{
    public class HsvColor : ColorBase<HsvColorData>, IEquatable<HsvColor?>
    {
        public HsvColor(byte a, float h, float s, float v)
            : this(new HsvColorData(a, h, s, v)) { }

        internal HsvColor(HsvColorData data)
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
                m_data.H = value % 360f;
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

        public float V
        {
            get => m_data.V;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.V = value;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as HsvColor);
        public bool Equals(HsvColor? other) => other != null && m_data.Equals(other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data);
        internal override AdobeRgbColorData DataToAdobeRgb() => AdobeRgbColorData.FromXyz(DataToXyz());
        internal override CmykColorData DataToCmyk() => CmykColorData.FromSRgb(DataToSRgb());
        internal override HslColorData DataToHsl() => HslColorData.FromSRgb(DataToSRgb());
        internal override HsvColorData DataToHsv() => m_data;
        internal override RgbColorData DataToRgb() => RgbColorData.FromSRgb(DataToSRgb());
        internal override ScRgbColorData DataToScRgb() => ScRgbColorData.FromSRgb(DataToSRgb());
        internal override SRgbColorData DataToSRgb() => HsvColorData.ToSRgb(m_data);
        internal override XyzColorData DataToXyz() => XyzColorData.FromScRgb(DataToScRgb());

        public static bool operator ==(HsvColor? left, HsvColor? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(HsvColor? left, HsvColor? right) => !(left == right);

        public override string ToString()
        {
            var sb = new StringBuilder(32).Append('[');
            if (A != 0xFF)
                sb.Append("A = ").Append(A);
            sb.Append(", H = ").Append(H.ToString("f2")).Append('°');
            sb.Append(", S = ").Append(S.ToString("f2")).Append('%');
            sb.Append(", V = ").Append(V.ToString("f2")).Append('%');
            return sb.Append(']').ToString();
        }
    }

    /// <summary>
    /// Value type containing 32bit floating point HSV color data, with a 8bit integer alpha component.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 15)]
    public struct HsvColorData : IEquatable<HsvColorData>
    {
        public byte A;
        public float H, S, V;

        public HsvColorData(byte a, float h, float s, float v)
        {
            if (0f > h) throw new ArgumentOutOfRangeException(nameof(h));
            if (0f > s || s < 1f) throw new ArgumentOutOfRangeException(nameof(s));
            if (0f > v || v < 1f) throw new ArgumentOutOfRangeException(nameof(v));
            A = a;
            H = h % 360;
            S = s;
            V = v;
        }

        public override bool Equals(object? obj) => obj is HsvColorData color && Equals(color);
        public bool Equals(HsvColorData other) => A == other.A && H == other.H && S == other.S && V == other.V;
        public override int GetHashCode() => HashCode.Combine(A, H, S, V);

        public static bool operator ==(HsvColorData left, HsvColorData right) => left.Equals(right);
        public static bool operator !=(HsvColorData left, HsvColorData right) => !(left == right);

        public static readonly HsvColorData Empty = new HsvColorData();


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SRgbColorData ToSRgb(in HsvColorData hsv)
        {
            float c = hsv.S * hsv.V,
                  x = c * (1 - MathF.Abs((hsv.H / 60f) % 2) - 1),
                  m = hsv.V - c;
            float r, g, b;
            // Tetrant in circle
            switch ((int)(hsv.H / 60.0))
            {
                case 0: // 0..60°
                    r = c; g = x; b = 0;
                    break;
                case 1: // 60..120°
                    r = 0; g = c; b = x;
                    break;
                case 2:
                    r = 0; g = x; b = c;
                    break;
                case 3:
                    r = x; g = 0; b = c;
                    break;
                default: // case 4:
                    r = c; g = 0; b = x;
                    break;
            }
            return new SRgbColorData(hsv.A, r + m, g + m, b + m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HsvColorData FromSRgb(in SRgbColorData sRgb)
        {
            float max = Math.Max(sRgb.R, Math.Max(sRgb.G, sRgb.B)),
                  min = Math.Min(sRgb.R, Math.Min(sRgb.G, sRgb.B)),
                  delta = max - min;
            float h = 60f,
                  s = max == 0f ? 0f : delta / max;
            if (delta == 0)
                h *= 0;
            else if (max == sRgb.R)
                h *= ((sRgb.G - sRgb.B) / delta) % 6;
            else if (max == sRgb.G)
                h *= (sRgb.B - sRgb.R) / delta + 2f;
            else if (max == sRgb.B)
                h *= (sRgb.R - sRgb.G) / delta + 2f;
            return new HsvColorData(sRgb.A, h, s, max);
        }
    }
}
