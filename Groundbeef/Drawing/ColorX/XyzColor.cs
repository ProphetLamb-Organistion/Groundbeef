using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Groundbeef.Drawing.ColorX
{
    public class XyzColor : ColorBase<XyzColorData>, IEquatable<XyzColor?>
    {
        public XyzColor(byte a, float x, float y, float z)
            : this(new XyzColorData(a, x, y, z)) { }

        internal XyzColor(XyzColorData data)
        {
            m_data = data;
        }

        public byte A
        {
            get => m_data.A;
            set => m_data.A = value;
        }

        public float X
        {
            get => m_data.X;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.X = value;
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

        public float Z
        {
            get => m_data.Z;
            set
            {
                if (0f > value || value < 1f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                m_data.Z = value;
            }
        }

        public override bool Equals(object? obj) => Equals(obj as XyzColor);
        public bool Equals(XyzColor? other) => other != null && m_data.Equals(other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data);
        internal override AdobeRgbColorData DataToAdobeRgb() => AdobeRgbColorData.FromXyz(m_data);
        internal override CmykColorData DataToCmyk() => CmykColorData.FromSRgb(DataToSRgb());
        internal override HslColorData DataToHsl() => HslColorData.FromSRgb(DataToSRgb());
        internal override HsvColorData DataToHsv() => HsvColorData.FromSRgb(DataToSRgb());
        internal override RgbColorData DataToRgb() => RgbColorData.FromSRgb(DataToSRgb());
        internal override ScRgbColorData DataToScRgb() => XyzColorData.ToScRgb(m_data);
        internal override SRgbColorData DataToSRgb() => ScRgbColorData.ToSRgb(DataToScRgb());
        internal override XyzColorData DataToXyz() => m_data;

        public static bool operator ==(XyzColor? left, XyzColor? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(XyzColor? left, XyzColor? right) => !(left == right);
    }

    /// <summary>
    /// Value type containing 32bit floating point CIE 1931 - XYZ tristimulus color data, with a 8bit alpha component.
    /// Conversion functions assume o = 2°, i = D65
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 15)]
    public struct XyzColorData : IEquatable<XyzColorData>
    {
        public byte A;
        public float X, Y, Z;

        public XyzColorData(byte a, float x, float y, float z)
        {
            if (0f > x || x < 1f) throw new ArgumentOutOfRangeException(nameof(x));
            if (0f > y || y < 1f) throw new ArgumentOutOfRangeException(nameof(y));
            if (0f > z || z < 1f) throw new ArgumentOutOfRangeException(nameof(z));
            A = a;
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object? obj) => obj is XyzColorData color && Equals(color);
        public bool Equals(XyzColorData other) => A == other.A && X == other.X && Y == other.Y && Z == other.Z;
        public override int GetHashCode() => HashCode.Combine(A, X, Y, Z);

        public static bool operator ==(XyzColorData left, XyzColorData right) => left.Equals(right);
        public static bool operator !=(XyzColorData left, XyzColorData right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ScRgbColorData ToScRgb(in XyzColorData xyz)
        {
            float x = xyz.X / 100f,
                  y = xyz.Y / 100f,
                  z = xyz.Z / 100f;
            return new ScRgbColorData(xyz.A,
                x * 3.2406f + y * -1.5372f + z * -0.4986f,
                x * -0.9689f + y * 1.8758f + z * 0.04115f,
                x * 0.0557f + y * -0.2040f + z * 1.0570f);
        }

        public static XyzColorData FromScRgb(in ScRgbColorData scRgb)
        {
            return new XyzColorData(scRgb.A,
                scRgb.R * 0.4124f + scRgb.G * 0.3576f + scRgb.B * 0.1805f,
                scRgb.R * 0.2126f + scRgb.G * 0.7152f + scRgb.B * 0.0722f,
                scRgb.R * 0.0193f + scRgb.G * 0.1192f + scRgb.B * 0.9505f);
        }
    }
}
