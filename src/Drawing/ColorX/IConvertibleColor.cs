namespace Groundbeef.Drawing.ColorX
{
    public interface IConvertibleColor
    {
        /// <summary>
        /// Returns a new <see cref="RgbColor"/> representing the color of this instance.
        /// </summary>
        RgbColor ToRgb();

        /// <summary>
        /// Returns a new <see cref="SRgbColor"/> representing the color of this instance.
        /// </summary>
        SRgbColor ToSRgb();

        /// <summary>
        /// Returns a new <see cref="ScRgbColor"/> representing the color of this instance.
        /// </summary>
        ScRgbColor ToScRgb();

        /// <summary>
        /// Returns a new <see cref="HslColor"/> representing the color of this instance.
        /// </summary>
        HslColor ToHsl();

        /// <summary>
        /// Returns a new <see cref="HsvColor"/> representing the color of this instance.
        /// </summary>
        HsvColor ToHsv();

        /// <summary>
        /// Returns a new <see cref="CmykColor"/> representing the color of this instance.
        /// </summary>
        CmykColor ToCmyk();

        /// <summary>
        /// Returns a new <see cref="XyzColor"/> representing the color of this instance.
        /// </summary>
        XyzColor ToXyz();

        /// <summary>
        /// Returns a new <see cref="AdobeRgbColor"/> representing the color of this instance.
        /// </summary>
        AdobeRgbColor ToAdobeRgb();
    }
}