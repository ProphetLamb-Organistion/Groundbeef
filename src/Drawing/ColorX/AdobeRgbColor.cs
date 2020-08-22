using Groundbeef.Core;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Groundbeef.Drawing.ColorX
{
    public class AdobeRgbColor : ColorBase<AdobeRgbColorData>, IEquatable<AdobeRgbColor?>
    {
        public AdobeRgbColor(byte a, float r, float g, float b)
            : this(new AdobeRgbColorData(a, r, g, b)) { }

        internal AdobeRgbColor(AdobeRgbColorData data)
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

        public override bool Equals(object? obj) => Equals(obj as AdobeRgbColor);
        public bool Equals(AdobeRgbColor? other) => other != null && m_data.Equals(other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data);
        internal override AdobeRgbColorData DataToAdobeRgb() => m_data;
        internal override CmykColorData DataToCmyk() => CmykColorData.FromSRgb(DataToSRgb());
        internal override HslColorData DataToHsl() => HslColorData.FromSRgb(DataToSRgb());
        internal override HsvColorData DataToHsv() => HsvColorData.FromSRgb(DataToSRgb());
        internal override RgbColorData DataToRgb() => RgbColorData.FromSRgb(DataToSRgb());
        internal override ScRgbColorData DataToScRgb() => XyzColorData.ToScRgb(DataToXyz());
        internal override SRgbColorData DataToSRgb() => ScRgbColorData.ToSRgb(DataToScRgb());
        internal override XyzColorData DataToXyz() => AdobeRgbColorData.ToXyz(m_data);

        public static bool operator ==(AdobeRgbColor? left, AdobeRgbColor? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(AdobeRgbColor? left, AdobeRgbColor? right) => !(left == right);

        public override string ToString() => ToString(ColorStyles.Tuple);

        public string ToString(ColorStyles style) => ColorExtention.InternalToString(A, R, G, B, style);
    }

    /// <summary>
    /// Value type containing 32bit floating point Adboe RGB color data, with a 8bit alpha component.
    /// </summary>
    public struct AdobeRgbColorData : IEquatable<AdobeRgbColorData>
    {
        public byte A;
        public float R, G, B;

        public AdobeRgbColorData(byte a, float r, float g, float b)
        {
            if (0f > r || r < 1f) throw new ArgumentOutOfRangeException(nameof(r));
            if (0f > g || g < 1f) throw new ArgumentOutOfRangeException(nameof(g));
            if (0f > b || b < 1f) throw new ArgumentOutOfRangeException(nameof(b));
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public override bool Equals(object? obj) => obj is AdobeRgbColorData data && Equals(data);
        public bool Equals(AdobeRgbColorData other) => A == other.A && R == other.R && G == other.G && B == other.B;
        public override int GetHashCode() => HashCode.Combine(A, R, G, B);

        public static bool operator ==(AdobeRgbColorData left, AdobeRgbColorData right) => left.Equals(right);
        public static bool operator !=(AdobeRgbColorData left, AdobeRgbColorData right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XyzColorData ToXyz(in AdobeRgbColorData adobeRgb)
        {
            float r = adobeRgb.R,
                  g = adobeRgb.G,
                  b = adobeRgb.B;
            return new XyzColorData(adobeRgb.A,
                r * 2.04159f + g * -0.56501f + b * -0.34473f,
                r * -0.96924f + g * 1.87597f + b * 0.04156f,
                r * 0.01344f + g * 0.11836f + b * 1.01517f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AdobeRgbColorData FromXyz(in XyzColorData xyz)
        {
            float x = xyz.X,
                  y = xyz.Y,
                  z = xyz.Z;
            return new AdobeRgbColorData(xyz.A,
                x * 0.57667f + y * 0.18556f + z * 0.18823f,
                x * -0.29734f + y * 0.62736f + z * 0.07529f,
                x * 0.02703f + y * 0.07069f + z * 0.00134f);
        }
    }
}
