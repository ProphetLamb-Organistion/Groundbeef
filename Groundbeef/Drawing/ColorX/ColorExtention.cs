using System.Drawing;
using System;
using Groundbeef.Core;
using System.Text;
using System.Linq;

namespace Groundbeef.Drawing.ColorX
{
    public static class ColorExtention
    {
        public static RgbColor ToRgb(this Color color)
        {
            return new RgbColor(color.A, color.R, color.G, color.B);
        }

        public static SRgbColor ToSRgb(this Color color)
        {
            return new SRgbColor(color.A, color.R/255f, color.G/255f, color.B/255f);
        }

        public static ScRgbColor ToScRgb(this Color color)
        {
            return color.ToSRgb().ToScRgb();
        }

        public static HslColor ToHslColor(this Color color)
        {
            return color.ToSRgb().ToHsl();
        }

        public static HsvColor ToHsvColor(this Color color)
        {
            return color.ToSRgb().ToHsv();
        }

        public static CmykColor ToCmykColor(this Color color)
        {
            return color.ToSRgb().ToCmyk();
        }

        public static XyzColor ToXyzColor(this Color color)
        {
            return color.ToSRgb().ToXyz();
        }

        public static AdobeRgbColor ToAdobeRgbColor(this Color color)
        {
            return color.ToSRgb().ToAdobeRgb();
        }

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
                    sb.Append((uint)a << 0 | (uint)(r*255f) << 8 | (uint)(g * 255f) << 16 | (uint)(b * 255f) << 24);
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
