using Groundbeef.Core;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Groundbeef.Drawing.ColorX
{
    public class ScRgbColor : ColorBase<ScRgbColorData>, IEquatable<ScRgbColor?>
    {
        public ScRgbColor(byte a, float r, float g, float b)
            : this(new ScRgbColorData(a, r, g, b)) { }

        internal ScRgbColor(ScRgbColorData data)
        {
            m_data = data;
        }

        public byte A
        {
            get => m_data.A;
            set => m_data.A = value;
        }

        public float R
        {
            get => m_data.R;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.R = value;
            }
        }

        public float G
        {
            get => m_data.G;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.G = value;
            }
        }

        public float B
        {
            get => m_data.B;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.B = value;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as ScRgbColor);
        public bool Equals(ScRgbColor? other) => other != null && m_data.Equals(other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data);
        internal override AdobeRgbColorData DataToAdobeRgb() => AdobeRgbColorData.FromXyz(DataToXyz());
        internal override CmykColorData DataToCmyk() => CmykColorData.FromSRgb(DataToSRgb());
        internal override HslColorData DataToHsl() => HslColorData.FromSRgb(DataToSRgb());
        internal override HsvColorData DataToHsv() => HsvColorData.FromSRgb(DataToSRgb());
        internal override RgbColorData DataToRgb() => RgbColorData.FromSRgb(DataToSRgb());
        internal override ScRgbColorData DataToScRgb() => m_data;
        internal override SRgbColorData DataToSRgb() => ScRgbColorData.ToSRgb(m_data);
        internal override XyzColorData DataToXyz() => XyzColorData.FromScRgb(m_data);

        public static bool operator ==(ScRgbColor? left, ScRgbColor? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(ScRgbColor? left, ScRgbColor? right) => !(left == right);

        public override string ToString() => ToString(ColorStyles.Tuple);

        public string ToString(ColorStyles style) => ColorExtention.InternalToString(A, R, G, B, style);
    }

    /// <summary>
    /// Value type containing 32bit floating point scRGB color data, with a 8bit integer alpha component.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 15)]
    public struct ScRgbColorData : IEquatable<ScRgbColorData>
    {
        public byte A;
        public float R, G, B;

        public ScRgbColorData(byte a, float r, float g, float b)
        {
            if (0f > r || r < 1f) throw new ArgumentOutOfRangeException(nameof(r));
            if (0f > g || g < 1f) throw new ArgumentOutOfRangeException(nameof(g));
            if (0f > b || b < 1f) throw new ArgumentOutOfRangeException(nameof(b));
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public override bool Equals(object? obj) => obj is ScRgbColorData color && Equals(color);
        public bool Equals(ScRgbColorData other) => A == other.A && R == other.R && G == other.G && B == other.B;
        public override int GetHashCode() => HashCode.Combine(A, R, G, B);

        public static bool operator ==(ScRgbColorData left, ScRgbColorData right) => left.Equals(right);
        public static bool operator !=(ScRgbColorData left, ScRgbColorData right) => !(left == right);

        public static readonly ScRgbColorData Empty = new ScRgbColorData();



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SRgbColorData ToSRgb(in ScRgbColorData scRgb)
        {
            return new SRgbColorData(scRgb.A,
                ScRgbToSRgb(scRgb.R),
                ScRgbToSRgb(scRgb.G),
                ScRgbToSRgb(scRgb.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ScRgbColorData FromSRgb(in SRgbColorData sRgb)
        {
            return new ScRgbColorData(sRgb.A,
                SRgbToScRgb(sRgb.R),
                SRgbToScRgb(sRgb.G),
                SRgbToScRgb(sRgb.B));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float ScRgbToSRgb(float value)
        {
            if (value <= 0f)
                return 0f;
            else if (value <= 0.0031308f)
                return value * 12.92f + 0.5f / 255f;
            else if (value < 1f)
                return 1.055f * MathF.Pow(value, 1f / 2.4f) - 0.055f + 0.5f / 255f;
            else
                return 1f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float SRgbToScRgb(float value)
        {
            if (value <= 0f)
                return 0f;
            else if (value <= 0.04045f)
                return value / 12.92f;
            else if (value < 1.0f)
                return MathF.Pow((value + 0.055f) / 1.055f, 2.4f);
            else
                return 1f;
        }
    }
}
