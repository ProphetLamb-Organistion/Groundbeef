using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ProphetLamb.Tools.Core
{
    public static class CultureInfoExtention
    {
        private static readonly MethodInfo verifyCultureName__CultureInfo_Bool = typeof(CultureInfo).GetMethod("VerifyCultureName", new[] { typeof(CultureInfo), typeof(bool) }),
                                           verifyCultureName__String_Bool = typeof(CultureInfo).GetMethod("VerifyCultureName", new[] { typeof(string), typeof(bool) });

        /// <summary>
        /// Returns whether the string <see cref="CultureInfo"/> has a valid culture name string.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="throwException">if <see cref="true"/> then throws exception if the <see cref="CultureInfo.Name"/> is invalid.</param>
        /// <returns><see cref="true"/> if the <see cref="CultureInfo.Name"/> is a valid culture name; otherwise, <see cref="false"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool VerifyCultureName(this CultureInfo culture, bool throwException = false)
        {
            return (bool)verifyCultureName__CultureInfo_Bool.Invoke(null, new object[] { culture, throwException });
        }

        /// <summary>
        /// Returns whether the string <paramref name="cultureName"/> is a valid culture name string.
        /// </summary>
        /// <param name="cultureName">The name of the culture.</param>
        /// <param name="throwException">if <see cref="true"/> then throws exception if the <paramref name="cultureName"/> is invalid.</param>
        /// <returns><see cref="true"/> if the <paramref name="cultureName"/> is a valid culture name; otherwise, <see cref="false"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool VerifyCulureName(string cultureName, bool throwException = false)
        {
            return (bool)verifyCultureName__String_Bool.Invoke(null, new object[] { cultureName, throwException });
        }
    }
}
