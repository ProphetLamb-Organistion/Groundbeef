using System;

namespace Groundbeef.Core
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class TypeConvert
    {
        /// <summary>
        /// Casts the provided object to a specified type.
        /// </summary>
        /// <param name="value">The source object.</param>
        /// <typeparam name="T">The cast type.</typeparam>
        public static T Cast<T>(in object value)
        {
            return (T)value;
        }

        /// <summary>
        /// Converts the provided object to a specified type. Using the IConvetible interface.
        /// </summary>
        /// <param name="value">The source object.</param>
        /// <typeparam name="T">The conversion type.</typeparam>
        public static T Convert<T>(in object value)
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Indicates that the object can convert to the type specifed using the <see cref="ConvertTo"/> method.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        public interface IConvertible<T>
        {
            T ConvertTo<T>();
        }
    }
}