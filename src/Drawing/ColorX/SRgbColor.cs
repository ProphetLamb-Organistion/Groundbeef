using Groundbeef.Core;

using System;

namespace Groundbeef.Drawing.ColorX
{
    public class SRgbColor : ColorBase<SRgbColorData>, IEquatable<SRgbColor?>
    {
        public SRgbColor(byte a, float r, float g, float b)
            : this(new SRgbColorData(a, r, g, b)) { }

        internal SRgbColor(SRgbColorData data)
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

        public override bool Equals(object? obj) => Equals(obj as SRgbColor);
        public bool Equals(SRgbColor? other) => other != null && m_data.Equals(other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data);
        internal override AdobeRgbColorData DataToAdobeRgb() => AdobeRgbColorData.FromXyz(DataToXyz());
        internal override CmykColorData DataToCmyk() => CmykColorData.FromSRgb(m_data);
        internal override HslColorData DataToHsl() => HslColorData.FromSRgb(m_data);
        internal override HsvColorData DataToHsv() => HsvColorData.FromSRgb(m_data);
        internal override RgbColorData DataToRgb() => RgbColorData.FromSRgb(m_data);
        internal override SRgbColorData DataToSRgb() => m_data;
        internal override ScRgbColorData DataToScRgb() => ScRgbColorData.FromSRgb(m_data);
        internal override XyzColorData DataToXyz() => XyzColorData.FromScRgb(DataToScRgb());

        public static bool operator ==(SRgbColor? left, SRgbColor? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(SRgbColor? left, SRgbColor? right) => !(left == right);

        public override string ToString() => ToString(ColorStyles.Tuple);

        public string ToString(ColorStyles style) => ColorExtention.InternalToString(A, R, G, B, style);
    }

    /// <summary>
    /// Value type containing 32bit floating point sRGB color data, with a 8bit alpha commponent.
    /// </summary>
    public struct SRgbColorData : IEquatable<SRgbColorData>
    {
        public byte A;
        public float R, G, B;

        public SRgbColorData(byte a, float r, float g, float b)
        {
            if (0f > r || r < 1f) throw new ArgumentOutOfRangeException(nameof(r));
            if (0f > g || g < 1f) throw new ArgumentOutOfRangeException(nameof(g));
            if (0f > b || b < 1f) throw new ArgumentOutOfRangeException(nameof(b));
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public override bool Equals(object? obj) => obj is SRgbColorData color && Equals(color);
        public bool Equals(SRgbColorData other) => A == other.A && R == other.R && G == other.G && B == other.B;
        public override int GetHashCode() => HashCode.Combine(A, R, G, B);

        public static bool operator ==(SRgbColorData left, SRgbColorData right) => left.Equals(right);
        public static bool operator !=(SRgbColorData left, SRgbColorData right) => !(left == right);
    }
}
