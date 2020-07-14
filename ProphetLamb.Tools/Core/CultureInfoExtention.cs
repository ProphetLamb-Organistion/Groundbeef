using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProphetLamb.Tools.Core
{
    public static class CultureInfoExtention
    {
        /// <summary>
        /// Returns whether the string <see cref="CultureInfo"/> has a valid culture name string.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="throwException">if <see cref="true"/> then throws exception if the <see cref="CultureInfo.Name"/> is invalid.</param>
        /// <returns><see cref="true"/> if the <see cref="CultureInfo.Name"/> is a valid culture name; otherwise, <see cref="false"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VerifyCultureName(this CultureInfo culture, bool throwException = false)
        {
            return VerifyCultureName(culture.Name, throwException);
        }

        /// <summary>
        /// Returns whether the string <paramref name="cultureName"/> is a valid culture name string.
        /// </summary>
        /// <param name="cultureName">The name of the culture.</param>
        /// <param name="throwException">if <see cref="true"/> then throws exception if the <paramref name="cultureName"/> is invalid.</param>
        /// <returns><see cref="true"/> if the <paramref name="cultureName"/> is a valid culture name; otherwise, <see cref="false"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool VerifyCultureName(string cultureName, bool throwException = false)
        {
            for (int i = 0; i < cultureName.Length; i++)
            {
                char c = cultureName[i];
                if (!Char.IsLetterOrDigit(c) && c != '-' && c != '_')
                {
                    if (throwException)
                        throw new ArgumentException("Invalid culture name.", cultureName);
                    return false;
                }
            }
            return true;
        }
    }
}
