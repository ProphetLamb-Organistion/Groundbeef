using System;

namespace Groundbeef.Core
{
    public static class ColorExtention
    {

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
        /// Returns the <see cref="UInt32"/> representation of the <see cref="System.Drawing.Color"/>.
        /// </summary>
        public static uint ToInteger(this System.Drawing.Color color)
        {
            return (uint)(color.A << 3) | (uint)(color.R << 2) | (uint)(color.G << 1) | (uint)(color.B << 0);
        }
    }
}
