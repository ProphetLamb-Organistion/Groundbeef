using System;
using System.Collections.Generic;

namespace Groundbeef.Drawing.ColorX
{
    public abstract class ColorBase<TColorData> : IConvertibleColor, IEquatable<ColorBase<TColorData>?> where TColorData : struct
    {
        protected TColorData m_data;

        /// <summary>
        /// Gets the color data structure.
        /// </summary>
        public TColorData Data => m_data;

        public override bool Equals(object? obj) => Equals(obj as ColorBase<TColorData>);
        public bool Equals(ColorBase<TColorData>? other) => other != null && EqualityComparer<TColorData>.Default.Equals(m_data, other.m_data);
        public override int GetHashCode() => HashCode.Combine(m_data, Data);
        internal abstract AdobeRgbColorData DataToAdobeRgb();
        internal abstract CmykColorData DataToCmyk();
        internal abstract HslColorData DataToHsl();
        internal abstract HsvColorData DataToHsv();
        internal abstract RgbColorData DataToRgb();
        internal abstract ScRgbColorData DataToScRgb();
        internal abstract SRgbColorData DataToSRgb();
        internal abstract XyzColorData DataToXyz();
        /// <summary>
        /// Converts the color to a <see cref="AdobeRgbColor"/>.
        /// </summary>
        public AdobeRgbColor ToAdobeRgb() => new AdobeRgbColor(DataToAdobeRgb());
        /// <summary>
        /// Converts the color to a <see cref="CmykColor"/>.
        /// </summary>
        public CmykColor ToCmyk() => new CmykColor(DataToCmyk());
        /// <summary>
        /// Converts the color to a <see cref="HslColor"/>.
        /// </summary>
        public HslColor ToHsl() => new HslColor(DataToHsl());
        /// <summary>
        /// Converts the color to a <see cref="HsvColor"/>.
        /// </summary>
        public HsvColor ToHsv() => new HsvColor(DataToHsv());
        /// <summary>
        /// Converts the color to a <see cref="RgbColor"/>.
        /// </summary>
        public RgbColor ToRgb() => new RgbColor(DataToRgb());
        /// <summary>
        /// Converts the color to a <see cref="ScRgbColor"/>.
        /// </summary>
        public ScRgbColor ToScRgb() => new ScRgbColor(DataToScRgb());
        /// <summary>
        /// Converts the color to a <see cref="SRgbColor"/>.
        /// </summary>
        public SRgbColor ToSRgb() => new SRgbColor(DataToSRgb());
        /// <summary>
        /// Converts the color to a <see cref="XyzColor"/>.
        /// </summary>
        public XyzColor ToXyz() => new XyzColor(DataToXyz());

        public static bool operator ==(ColorBase<TColorData>? left, ColorBase<TColorData>? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(ColorBase<TColorData>? left, ColorBase<TColorData>? right) => !(left == right);
    }
}