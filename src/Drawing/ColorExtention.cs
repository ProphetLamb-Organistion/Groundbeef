using Groundbeef.Core;
using Groundbeef.Drawing.ColorX;

using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Groundbeef.Drawing
{
    public static class ColorExtention
    {
        /// <summary>
        /// Converts the color to an <see cref="RgbColor"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RgbColor ToRgb(this Color color) => new RgbColor(color.A, color.R, color.G, color.B);

        /// <summary>
        /// Converts the color to an <see cref="SRgbColor"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SRgbColor ToSRgb(this Color color) => new SRgbColor(color.A, color.R / 255f, color.G / 255f, color.B / 255f);

        /// <summary>
        /// Converts the color to an <see cref="ScRgbColor"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ScRgbColor ToScRgb(this Color color) => color.ToSRgb().ToScRgb();

        /// <summary>
        /// Converts the color to an <see cref="HslColor"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HslColor ToHsl(this Color color) => color.ToSRgb().ToHsl();

        /// <summary>
        /// Converts the color to an <see cref="HsvColor"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HsvColor ToHsv(this Color color) => color.ToSRgb().ToHsv();

        /// <summary>
        /// Converts the color to an <see cref="CmykColor"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CmykColor ToCmyk(this Color color) => color.ToSRgb().ToCmyk();

        /// <summary>
        /// Converts the color to an <see cref="XyzColor"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XyzColor ToXyz(this Color color) => color.ToSRgb().ToXyz();

        /// <summary>
        /// Converts the color to an <see cref="AdobeRgbColor"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AdobeRgbColor ToAdobeRgb(this Color color) => color.ToSRgb().ToAdobeRgb();

        internal static string InternalToString(byte a, float r, float g, float b, ColorStyles style)
        {
            var sb = new StringBuilder(48).Append('[');
            if (style == ColorStyles.None)
                style = ColorStyles.Tuple;
            switch (style)
            {
                case ColorStyles.Tuple:
                    sb.Append("A = ").Append(a)
                    .Append(", R = ").Append(r.ToString("f2")).Append('%')
                    .Append(", G = ").Append(g.ToString("f2")).Append('%')
                    .Append(", B = ").Append(b.ToString("f2")).Append('%');
                    break;
                case ColorStyles.Integer:
                    sb.Append((uint)a << 0 | (uint)(r * 255f) << 8 | (uint)(g * 255f) << 16 | (uint)(b * 255f) << 24);
                    break;
                case ColorStyles.HexInteger:
                    sb.Append(((uint)a << 0 | (uint)(r * 255f) << 8 | (uint)(g * 255f) << 16 | (uint)(b * 255f) << 24).ToString("X"));
                    break;
                case ColorStyles.Name:
                    if (a != 0xFF)
                        sb.Append("A = ").Append(a).Append(", ");
                    sb.Append("Name = ").Append(EnumHelper<KnownColor>.GetValues().Select(Color.FromKnownColor).Where(c => c.R == (byte)r && c.G == (byte)g && c.B == (byte)b).First().Name);
                    break;
            }
            return sb.Append(']').ToString();
        }
    }
}
