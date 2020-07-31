using System;
using System.Windows.Media;

namespace Groundbeef.WPF
{
    public static class ColorExtention
    {
        /// <summary>
        /// Returns the perceptive lightness value of a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color</param>
        /// <returns>The perceptive lightness value of a <see cref="Color"/>.</returns>
        public static float GetLightness(this Color color)
        {
            return GetLightness(color.R, color.G, color.B);
        }

        /// <summary>
        /// Returns the perceptive lightness value of a <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="color">The color</param>
        /// <returns>The perceptive lightness value of a <see cref="System.Drawing.Color"/>.</returns>
        public static float GetLightness(this System.Drawing.Color color)
        {
            return GetLightness(color.R, color.G, color.B);
        }

        // HSP color model birghtness, reference: http://alienryderflex.com/hsp.html
        private static float GetLightness(float r, float g, float b)
        {
            return MathF.Sqrt(0.299f * MathF.Pow(r, 2) + 0.587f * MathF.Pow(g, 2) + 0.114f * MathF.Pow(b, 2));
        }

        /// <summary>
        /// Converts a <see cref="Color"/> to its <see cref="System.Drawing.Color"/> representation.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="System.Drawing.Color"/> of the <see cref="Color"/>.</returns>
        public static System.Drawing.Color ToDrawingColor(this Color color) => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

        /// <summary>
        /// Converts a <see cref="System.Drawing.Color"/> to its <see cref="Color"/> representation.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="Color"/> of the <see cref="System.Drawing.Color"/>.</returns>
        public static Color ToMediaColor(this System.Drawing.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
