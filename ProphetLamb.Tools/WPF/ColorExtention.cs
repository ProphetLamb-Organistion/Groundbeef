using System;
using System.Windows.Media;

namespace ProphetLamb.Tools.WPF
{
    public static class ColorExtention
    {
        public static float GetLightness(this Color color)
        {
            return GetLightness(color.R, color.G, color.B);
        }

        public static float GetLightness(this System.Drawing.Color color)
        {
            return GetLightness(color.R, color.G, color.B);
        }

        private static float GetLightness(float r, float g, float b)
        {
            return MathF.Sqrt(0.299f * MathF.Pow(r, 2) + 0.587f * MathF.Pow(g, 2) + 0.114f * MathF.Pow(b, 2));
        }

        public static System.Drawing.Color ToDrawingColor(this Color color) => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

        public static Color ToMediaColor(this System.Drawing.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
