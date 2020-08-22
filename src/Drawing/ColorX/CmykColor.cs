using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Groundbeef.Drawing.ColorX
{
    public class CmykColor : ColorBase<CmykColorData>, IEquatable<CmykColor?>
    {
        public CmykColor(byte a, float c, float m, float y, float k)
            : this(new CmykColorData(a, c, m, y, k)) { }

        internal CmykColor(CmykColorData data)
        {
            m_data = data;
        }

        public byte A
        {
            get => m_data.A;
            set => m_data.A = value;
        }

        public float C
        {
            get => m_data.C;
            set
            {
                if (0f > value)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.C = value;
            }
        }

        public float M
        {
            get => m_data.M;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.M = value;
            }
        }

        public float Y
        {
            get => m_data.Y;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.Y = value;
            }
        }

        public float K
        {
            get => m_data.K;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.K = value;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as CmykColor);
        public bool Equals(CmykColor? other) => other != null && m_data.Equals(other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data);
        internal override AdobeRgbColorData DataToAdobeRgb() => AdobeRgbColorData.FromXyz(DataToXyz());
        internal override CmykColorData DataToCmyk() => m_data;
        internal override HslColorData DataToHsl() => HslColorData.FromSRgb(DataToSRgb());
        internal override HsvColorData DataToHsv() => HsvColorData.FromSRgb(DataToSRgb());
        internal override RgbColorData DataToRgb() => RgbColorData.FromSRgb(DataToSRgb());
        internal override ScRgbColorData DataToScRgb() => ScRgbColorData.FromSRgb(DataToSRgb());
        internal override SRgbColorData DataToSRgb() => CmykColorData.ToSRgb(m_data);
        internal override XyzColorData DataToXyz() => XyzColorData.FromScRgb(DataToScRgb());

        public static bool operator ==(CmykColor? left, CmykColor? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(CmykColor? left, CmykColor? right) => !(left == right);

        public override string ToString()
        {
            var sb = new StringBuilder(32).Append('[');
            if (A != 0xFF)
                sb.Append("A = ").Append(A);
            sb.Append(", C = ").Append(C.ToString("f2")).Append('%');
            sb.Append(", M = ").Append(M.ToString("f2")).Append('%');
            sb.Append(", Y = ").Append(Y.ToString("f2")).Append('%');
            sb.Append(", K = ").Append(K.ToString("f2")).Append('%');
            return sb.Append(']').ToString();
        }
    }

    /// <summary>
    /// Value type containing 32bit floating point CMYK color data, with a 8bit integer alpha component.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 19)]
    public struct CmykColorData : IEquatable<CmykColorData>
    {
        public byte A;
        public float C, M, Y, K;

        public CmykColorData(byte a, float c, float m, float y, float k)
        {
            if (0f > c || c < 1f) throw new ArgumentOutOfRangeException(nameof(c));
            if (0f > m || m < 1f) throw new ArgumentOutOfRangeException(nameof(m));
            if (0f > y || y < 1f) throw new ArgumentOutOfRangeException(nameof(y));
            if (0f > k || k < 1f) throw new ArgumentOutOfRangeException(nameof(k));
            A = a;
            C = c;
            M = m;
            Y = y;
            K = k;
        }

        public override bool Equals(object? obj) => obj is CmykColorData color && Equals(color);
        public bool Equals(CmykColorData other) => C == other.C && M == other.M && Y == other.Y && K == other.K;
        public override int GetHashCode() => HashCode.Combine(C, M, Y, K);

        public static bool operator ==(CmykColorData left, CmykColorData right) => left.Equals(right);
        public static bool operator !=(CmykColorData left, CmykColorData right) => !(left == right);

        public static readonly CmykColorData Empty = new CmykColorData();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SRgbColorData ToSRgb(in CmykColorData cmyk)
        {
            return new SRgbColorData(cmyk.A,
                (1 - cmyk.C) * (1 - cmyk.K),
                (1 - cmyk.M) * (1 - cmyk.K),
                (1 - cmyk.Y) * (1 - cmyk.K));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CmykColorData FromSRgb(in SRgbColorData sRgb)
        {
            float max = Math.Max(sRgb.R, Math.Max(sRgb.G, sRgb.B));
            float k = 1 - max,
                  c = (1 - sRgb.R - k) / (1 - k),
                  m = (1 - sRgb.G - k) / (1 - k),
                  y = (1 - sRgb.B - k) / (1 - k);
            return new CmykColorData(sRgb.A, c, m, y, k);
        }
    }
}
