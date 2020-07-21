using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ProphetLamb.Tools
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class TypeHelper
    {
        /// <summary>
        /// Casts the provided object to a specified type.
        /// </summary>
        /// <param name="value">The source object.</param>
        /// <typeparam name="T">The cast type.</typeparam>
        public static T CastObject<T>(in object value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            return (T)value;
        }

        /// <summary>
        /// Converts the provided object to a specified type. Using the IConvetible interface.
        /// </summary>
        /// <param name="value">The source object.</param>
        /// <typeparam name="T">The conversion type.</typeparam>
        public static T ConvertObject<T>(in object value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
