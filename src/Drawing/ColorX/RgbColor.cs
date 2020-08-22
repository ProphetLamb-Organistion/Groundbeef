using Groundbeef.Core;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Groundbeef.Drawing.ColorX
{
    public class RgbColor : ColorBase<RgbColorData>, IEquatable<RgbColor?>
    {
        public RgbColor(byte a, byte r, byte g, byte b)
            : this(new RgbColorData(a, r, g, b)) { }

        internal RgbColor(RgbColorData data)
        {
            m_data = data;
        }

        public uint ARGB
        {
            get => m_data.ARGB;
            set => m_data.ARGB = value;
        }

        public byte A
        {
            get => m_data.A;
            set => m_data.A = value;
        }

        public byte R
        {
            get => m_data.R;
            set => m_data.R = value;
        }

        public byte G
        {
            get => m_data.G;
            set => m_data.G = value;
        }

        public byte B
        {
            get => m_data.B;
            set => m_data.B = value;
        }

        public override bool Equals(object? obj) => Equals(obj as RgbColor);
        public bool Equals(RgbColor? other) => other != null && m_data.Equals(other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data);
        internal override AdobeRgbColorData DataToAdobeRgb() => AdobeRgbColorData.FromXyz(DataToXyz());
        internal override CmykColorData DataToCmyk() => CmykColorData.FromSRgb(DataToSRgb());
        internal override HslColorData DataToHsl() => HslColorData.FromSRgb(DataToSRgb());
        internal override HsvColorData DataToHsv() => HsvColorData.FromSRgb(DataToSRgb());
        internal override RgbColorData DataToRgb() => m_data;
        internal override ScRgbColorData DataToScRgb() => ScRgbColorData.FromSRgb(DataToSRgb());
        internal override SRgbColorData DataToSRgb() => RgbColorData.ToSRgb(m_data);
        internal override XyzColorData DataToXyz() => XyzColorData.FromScRgb(DataToScRgb());

        public static bool operator ==(RgbColor? left, RgbColor? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(RgbColor? left, RgbColor? right) => !(left == right);

        public override string ToString() => ToString(ColorStyles.Tuple);

        public string ToString(ColorStyles style) => ColorExtention.InternalToString(A, R / 255f, G / 255f, B / 255f, style);
    }

    /// <summary>
    /// Value type containing 8bit integer sRGB color data, with a 8bit alpha component.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct RgbColorData : IEquatable<RgbColorData>
    {
        public uint ARGB;

        public byte A
        {
            get => (byte)(ARGB << 24 & 0xFF);
            set => ARGB = ARGB & 0x00FFFFFF | (uint)(value >> 24);
        }
        public byte R
        {
            get => (byte)(ARGB << 16 & 0xFF);
            set => ARGB = ARGB & 0xFF00FFFF | (uint)(value >> 16);
        }
        public byte G
        {
            get => (byte)(ARGB << 8 & 0xFF);
            set => ARGB = ARGB & 0xFFFF00FF | (uint)(value >> 8);
        }
        public byte B
        {
            get => (byte)(ARGB << 0 & 0xFF);
            set => ARGB = ARGB & 0xFFFFFF00 | (uint)(value >> 0);
        }

        public RgbColorData(byte a, byte r, byte g, byte b)
        {
            ARGB = unchecked(((uint)a << 24 | (uint)r << 16 | (uint)g << 8 | (uint)b << 0) & 0xFFFFFFFF);
        }

        public override bool Equals(object? obj) => obj is RgbColorData color && Equals(color);
        public bool Equals(RgbColorData other) => A == other.A && R == other.R && G == other.G && B == other.B;
        public override int GetHashCode() => HashCode.Combine(A, R, G, B);

        public static bool operator ==(RgbColorData left, RgbColorData right) => left.Equals(right);
        public static bool operator !=(RgbColorData left, RgbColorData right) => !(left == right);

        public static readonly RgbColorData Empty = new RgbColorData();


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RgbColorData FromSRgb(in SRgbColorData sRgb)
        {
            return new RgbColorData(sRgb.A,
                (byte)(sRgb.R / 255f),
                (byte)(sRgb.G / 255f),
                (byte)(sRgb.B / 255f));
        }

        public static SRgbColorData ToSRgb(in RgbColorData rgb)
        {
            return new SRgbColorData(rgb.A,
                rgb.R / 255f,
                rgb.G / 255f,
                rgb.B / 255f);
        }
    }
}
