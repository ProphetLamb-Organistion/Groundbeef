using System.ComponentModel;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Globalization;
using System.Drawing;
using System.Text;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Groundbeef.Core;
using Groundbeef.Collections;
using System.Collections;
using System.Reflection;

namespace Groundbeef.Text
{
    [ComVisible(true)]
    public static class StringHelper
    {
        internal static readonly MethodInfo methodFastAllocateString = typeof(string).GetMethod("FastAllocateString", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly char[] defaultAlphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public const string DefaultCollectionSeparator= ", ";
        public const string DefaultKeyValuePairSeparator = ": ";

        public static string? FastAllocateString(int length) => methodFastAllocateString.Invoke(null, new object[] { length }) as string;

        /// <summary>
        /// Returns a new randomly generated string with the specified <paramref name="length"/> containing lower and uppercase alphanumeric characters 0-9a-zA-Z.
        /// </summary>
        /// <param name="length">The number of chars in the random string.</param>
        /// <returns>A new randomly generated string with the specified <paramref name="length"/> containing lower and uppercase alphanumeric characters 0-9a-zA-Z.</returns>
        public static string RandomString(int length) => RandomString(length, defaultAlphabet, new Random());

        /// <summary>
        /// Returns a new randomly generated string with the specified <paramref name="length"/> containing characters in the <paramref name="alphabet"/>.
        /// </summary>
        /// <param name="length">The number of chars in the random string.</param>
        /// <param name="alphabet">The alphabet used to generate characters in the string.</param>
        /// <returns>A new randomly generated string with the specified <paramref name="length"/> containing characters in the <paramref name="alphabet"/>.</returns>
        public static unsafe string RandomString(int length, ReadOnlySpan<char> alphabet) => RandomString(length, alphabet, new Random());
        /// <summary>
        /// Returns a new randomly generated string using the provided <paramref name="random"/> with the specified <paramref name="length"/> containing characters in the <paramref name="alphabet"/>.
        /// </summary>
        /// <param name="length">The number of chars in the random string.</param>
        /// <param name="alphabet">The alphabet used to generate characters in the string.</param>
        /// <param name="random">The random generator used.</param>
        /// <returns>A new randomly generated string using the provided <paramref name="random"/> with the specified <paramref name="length"/> containing characters in the <paramref name="alphabet"/>.</returns>
        public static unsafe string RandomString(int length, ReadOnlySpan<char> alphabet, in Random random)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), ExceptionResource.INTEGER_POSITIVEZERO);
            if (alphabet.Length < 2)
                throw new ArgumentException("The alphabet contains less then two characters.");
            string str = FastAllocateString(length) ?? String.Empty;
            int alphabetEndIndex = alphabet.Length - 1;
            fixed (char* outStr = str)
            fixed (char* alphabetPtr = &MemoryMarshal.GetReference(alphabet))
            {
                for (int i = 0; i != length; i++)
                {
                    outStr[i] = alphabetPtr[random.Next(0, alphabetEndIndex)];
                }
            }
            return str;
        }

        /// <summary>
        /// Encodes all characters in the <see cref="string"/> into a squence of <see cref="bytes"/> using the specified encoding
        /// </summary>
        /// <param name="encoding">The character encoding used to encode the characters.</param>
        /// <returns>A <see cref="byte[]"/> containing the encoded <see cref="string"/>.</returns>
        public static byte[] ToByteArray(this string self, Encoding encoding) => encoding.GetBytes(self);

        /// <summary>
        /// Encodes all characters in the <see cref="string"/> into a squence of <see cref="bytes"/> using the <see cref="Encoding.UTF8"/>
        /// </summary>
        /// <returns>A <see cref="byte[]"/> containing the encoded <see cref="string"/>.</returns>
        public static byte[] GetUTF8Bytes(this string self) => Encoding.UTF8.GetBytes(self);

        /// <summary>
        /// Encodes all characters in the <see cref="string"/> into a squence of <see cref="bytes"/> using the <see cref="Encoding.ASCII"/>
        /// </summary>
        /// <returns>A <see cref="byte[]"/> containing the encoded <see cref="string"/>.</returns>
        public static byte[] GetASCIIBytes(this string self) => Encoding.ASCII.GetBytes(self);

        /// <summary>
        /// Returns whether the string only contains ASCII characters.
        /// </summary>
        /// <returns><see cref="true"/> if the string contains only ASCII characters; otherwise, <see cref="false"/>.</returns>
        public static bool IsASCIIString(this string self)
        {
#if FEATURE_UTF32
            return value.Length == Encoding.UTF32.GetByteCount(value);
#else
            return self.Length == Encoding.UTF8.GetByteCount(self);
#endif
        }

        /// <summary>
        /// Returns a new instance <see cref="StringBuilder"/> appending <paramref name="value"/> to the <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="string" to append.</param>
        /// <returns>A new instance <see cref="StringBuilder"/> appending <paramref name="value"/> to the <see cref="string"/>.</returns>
        public static StringBuilder Append(this string self, in string value) => new StringBuilder(self).Append(value);

        /// <summary>
        /// Indicates whether the string is empty ("").
        /// </summary>
        /// <returns><see cref="true"/> if the string is empty; otherwise <see cref="false"/>.</returns>
        public static bool IsEmpty(this string self)
        {
            return String.IsNullOrEmpty(self);
        }

        /// <summary>
        /// Indicates whether the string is empty or consisits only of white-space characters.
        /// </summary>
        /// <returns><see cref="true"/> if the string is empty or consisits only of white-space characters; otherwise <see cref="false"/>.</returns>
        public static bool IsWhitespace(this string self)
        {
            return String.IsNullOrWhiteSpace(self);
        }

        #region Conversions
        public static Byte ToByte(this string self) => ToByte(self.AsSpan());
        public static Byte ToByte(this ReadOnlySpan<char> self)
        {
            return Byte.Parse(self);
        }
        public static Byte ToByte(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToByte(self.AsSpan(), numberStyles, provider);
        public static Byte ToByte(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Byte.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static SByte ToSByte(this string self) => ToSByte(self.AsSpan());
        public static SByte ToSByte(this ReadOnlySpan<char> self)
        {
            return SByte.Parse(self);
        }

        public static SByte ToSByte(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToSByte(self.AsSpan(), numberStyles, provider);
        public static SByte ToSByte(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return SByte.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static UInt16 ToUInt16(this string self) => ToUInt16(self.AsSpan());
        public static UInt16 ToUInt16(this ReadOnlySpan<char> self)
        {
            return UInt16.Parse(self);
        }

        public static UInt16 ToUInt16(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToUInt16(self.AsSpan(), numberStyles, provider);
        public static UInt16 ToUInt16(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return UInt16.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static Int16 ToInt16(this string self) => ToInt16(self.AsSpan());
        public static Int16 ToInt16(this ReadOnlySpan<char> self)
        {
            return Int16.Parse(self);
        }

        public static Int16 ToInt16(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToInt16(self.AsSpan(), numberStyles, provider);
        public static Int16 ToInt16(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Int16.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static UInt32 ToUInt32(this string self) => ToUInt32(self.AsSpan());
        public static UInt32 ToUInt32(this ReadOnlySpan<char> self)
        {
            return UInt32.Parse(self);
        }

        public static UInt32 ToUInt32(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToUInt32(self.AsSpan(), numberStyles, provider);
        public static UInt32 ToUInt32(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return UInt32.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static Int32 ToInt32(this string self) => ToInt32(self.AsSpan());
        public static Int32 ToInt32(this ReadOnlySpan<char> self)
        {
            return Int32.Parse(self);
        }

        public static Int32 ToInt32(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToInt32(self.AsSpan(), numberStyles, provider);
        public static Int32 ToInt32(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Int32.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static UInt64 ToUInt64(this string self) => ToUInt64(self.AsSpan());
        public static UInt64 ToUInt64(this ReadOnlySpan<char> self)
        {
            return UInt64.Parse(self);
        }

        public static UInt64 ToUInt64(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToUInt64(self.AsSpan(), numberStyles, provider);
        public static UInt64 ToUInt64(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return UInt64.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static Int64 ToInt64(this string self) => ToInt64(self.AsSpan());
        public static Int64 ToInt64(this ReadOnlySpan<char> self)
        {
            return Int64.Parse(self);
        }

        public static Int64 ToInt64(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToInt64(self.AsSpan(), numberStyles, provider);
        public static Int64 ToInt64(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Int64.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static Single ToSingle(this string self) => ToSingle(self.AsSpan());
        public static Single ToSingle(this ReadOnlySpan<char> self)
        {
            return Int64.Parse(self);
        }

        public static Single ToSingle(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToSingle(self.AsSpan(), numberStyles, provider);
        public static Single ToSingle(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Single.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static Double ToDouble(this string self) => ToDouble(self.AsSpan());
        public static Double ToDouble(this ReadOnlySpan<char> self)
        {
            return Double.Parse(self);
        }

        public static Double ToDouble(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToDouble(self.AsSpan(), numberStyles, provider);
        public static Double ToDouble(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Int32.Parse(self, numberStyles??NumberStyles.Integer, provider);
        }

        public static DateTime ToDateTime(this string self, IFormatProvider? provider = null, DateTimeStyles styles = DateTimeStyles.None) => ToDateTime(self.AsSpan(), provider, styles);
        public static DateTime ToDateTime(this ReadOnlySpan<char> self, IFormatProvider? provider = null, DateTimeStyles styles = DateTimeStyles.None)
        {
            return DateTime.Parse(self, provider, styles);
        }

        public static Color ParseColor(this string self, ColorStyles colorStyles = ColorStyles.None)
        {
            // Set to default ColorStyles if None
            colorStyles = colorStyles == ColorStyles.None ? (ColorStyles.Integer | ColorStyles.HexNumber) : colorStyles;

            string trimmed = self.Trim();
            // Hex or Int
            if ((colorStyles & (ColorStyles.Integer | ColorStyles.HexNumber)) != ColorStyles.None)
            {
                NumberStyles styles = NumberStyles.None
                | ((colorStyles & ColorStyles.Integer) == ColorStyles.Integer ? NumberStyles.Integer : NumberStyles.None)
                | ((colorStyles & ColorStyles.HexNumber) == ColorStyles.HexNumber ? NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier : NumberStyles.None);
                if (Int32.TryParse(trimmed, styles, CultureInfo.CurrentCulture.NumberFormat, out int value))
                    return Color.FromArgb(value);
            }
            // Name
            if ((colorStyles & ColorStyles.Name) == ColorStyles.Name && EnumHelper<KnownColor>.GetNames().Contains(trimmed))
                return Color.FromKnownColor(EnumHelper<KnownColor>.Parse(trimmed));
            // Bytes
            if ((colorStyles & ColorStyles.Bytes) == ColorStyles.Bytes)
            {
                var bytes = new byte[4];
                var values = trimmed.Split(',');
                int length = Math.Min(bytes.Length, values.Length);
                for (int i = 0; i < length; i++)
                {
                    if (!byte.TryParse(values[i], out byte b))
                    {
                        length = -1;
                        break;
                    }
                    bytes[i] = b;
                }
                if (length == 3)
                    return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
                if (length == 4)
                    return Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
            }
            // None
            throw new FormatException("No known format recognized.");
        }

        // ********* IEnumerable ********
        /// <summary>
        /// Converts the instance of <paramref name="collection"/> to a string by joining it with ','.
        /// </summary>
        /// <param name="formatter">The formatter used to convert the objects of the collection to string.</param>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        public static string ConvertToString<T>(this IEnumerable<T> collection, Func<T, string> formatter) => ConvertToString(collection, ',', formatter);

        /// <summary>
        /// Converts the instance of <paramref name="collection"/> to a string by joining it with the <paramref name="separator"/> specified.
        /// </summary>
        /// <param name="separator">The character to use as a separator.</param>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        public static string ConvertToString<T>(this IEnumerable<T> collection, char separator) => ConvertToString(collection, separator, o => o?.ToString()??String.Empty);

        /// <summary>
        /// Converts the instance of <paramref name="collection"/> to a string by joining it with the <paramref name="separator"/> specified.
        /// </summary>
        /// <param name="separator">The character to use as a separator.</param>
        /// <param name="formatter">The formatter used to convert the objects of the collection to string.</param>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        public static string ConvertToString<T>(this IEnumerable<T> collection, char separator, Func<T, string> formatter)
        {
            var sb = new StringBuilder();
            IEnumerator<T> en = collection.GetEnumerator();
            while (en.MoveNext())
                sb.Append(formatter(en.Current)).Append(separator);
            sb.Length--; //Remove tailing spearator
            return sb.ToString();
        }

        /// <summary>
        /// Converts the instance of <paramref name="collection"/> to a string by joining it with the <paramref name="separator"/> specified.
        /// </summary>
        /// <param name="separator">The character to use as a separator.</param>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        public static string ConvertToString<T>(this IEnumerable<T> collection, string separator = DefaultCollectionSeparator) => ConvertToString(collection, separator, o => o?.ToString()??String.Empty);

        /// <summary>
        /// Converts the instance of <paramref name="collection"/> to a string by joining it with the <paramref name="separator"/> specified.
        /// </summary>
        /// <param name="separator">The character to use as a separator.</param>
        /// <param name="formatter">The formatter used to convert the objects of the collection to string.</param>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        public static string ConvertToString<T>(this IEnumerable<T> collection, string separator, Func<T, string> formatter)
        {
            var sb = new StringBuilder();
            IEnumerator<T> en = collection.GetEnumerator();
            while (en.MoveNext())
                sb.Append(formatter(en.Current)).Append(separator);
            sb.Length -= separator.Length; //Remove tailing spearator
            return sb.ToString();
        }

        /// <summary>
        /// Converts the instance of <see cref="KeyValuePair{string, string}"/> to a string.
        /// </summary>
        /// <param name="separator">The <see cref="string"/> separating the key from the value.</param>
        public static string ConvertToString(this KeyValuePair<string, string> self, string separator = DefaultKeyValuePairSeparator)
        {
            return String.Concat(self.Key, separator, self.Value);
        }

        /// <summary>
        /// Converts the instance of <see cref="IDictionary{string, string}"/> to a string.
        /// </summary>
        /// <param name="elementSeparator">The <see cref="string"/> separating each <see cref="KeyValuePair{string, string}"/>.</param>
        /// <param name="kvpSeparator">The <see cref="string"/> separating the key from the value.</param>
        public static string ConvertToString(this IDictionary<string, string> self, string elementSeparator = DefaultCollectionSeparator, string kvpSeparator = DefaultKeyValuePairSeparator)
        {
            return self.ConvertToString(elementSeparator, kvp => kvp.ConvertToString(kvpSeparator));
        }

        public static object ConvertStringToValue(string value, Type type)
        {
            int index = ConvertibleTypes.IndexOf(t => type.GUID == t.GUID);
            // Handle for generic types by thier definition
            if (index == -1 && type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
                index = ConvertibleTypes.IndexOf(t => type.GUID == t.GUID);
            }
            switch(index)
            {
                case 0:
                return value.ToByte();
                case 1:
                return 
            }
        }

        public static readonly Type[] ConvertibleTypes = {
            typeof(Byte),
            typeof(SByte),
            typeof(UInt16),
            typeof(Int16),
            typeof(UInt32),
            typeof(Int32),
            typeof(UInt64),
            typeof(Int64),
            typeof(Single),
            typeof(Double),
            typeof(Enum),
            typeof(Color),
            typeof(DateTime),
            typeof(IEnumerable<>),
            typeof(KeyValuePair<,>),
            typeof(IDictionary<,>)
        };
        #endregion

        #region Split
        // ******** Char ********
        /// <summary>
        /// Splits a <see cref="string"/> into substrings based on the <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The char that delimits the substrings in this string.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, char separator)
        {
            string[] parts = self.Split(separator);
            for(int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="string"/> into substrings based on the <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The char that delimits the substrings in this string.</param>
        /// <param name="trim">An array of Unicode characters to remove from the substring if leading, or trailing.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, char separator, char[] trim, int count = -1, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] parts = count == -1 ? self.Split(separator, options) : self.Split(separator, count, options);
            for(int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim(trim);
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="string"/> into substrings based on the <paramref name="separators"/>.
        /// </summary>
        /// <param name="separators">The char array that delimits the substrings in this string, an empty array that contains no delimiters.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, params char[] separators)
        {
            string[] parts = self.Split(separators);
            for(int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="string"/> into substrings based on the <paramref name="separators"/>.
        /// </summary>
        /// <param name="separators">The char array that delimits the substrings in this string, an empty array that contains no delimiters.</param>
        /// <param name="trim">An array of Unicode characters to remove from the substring if leading, or trailing.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, char[] separators, char[] trim, int count = -1, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] parts = count == -1 ? self.Split(separators, options) : self.Split(separators, count, options);
            for(int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim(trim);
            return parts;
        }

        // ******** String ********
        /// <summary>
        /// Splits a <see cref="string"/> into substrings based on the <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The string that delimits the substrings in this string.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, string separator)
        {
            string[] parts = self.Split(separator);
            for(int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="string"/> into substrings based on the <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The string that delimits the substrings in this string.</param>
        /// <param name="trim">An array of Unicode characters to remove from the substring if leading, or trailing.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, string separator, char[] trim, int count = -1, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] parts = count == -1 ? self.Split(separator, options) : self.Split(separator, count, options);
            for(int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim(trim);
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="string"/> into substrings based on the <paramref name="separators"/>.
        /// </summary>
        /// <param name="separators">The string array that delimits the substrings in this string, an empty array that contains no delimiters.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, params string[] separators)
        {
            string[] parts = self.Split(separators, StringSplitOptions.None);
            for(int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="string"/> into substrings based on the <paramref name="separators"/>.
        /// </summary>
        /// <param name="separators">The string array that delimits the substrings in this string, an empty array that contains no delimiters.</param>
        /// <param name="trim">An array of Unicode characters to remove from the substring if leading, or trailing.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, string[] separators, char[] trim, int count = -1, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] parts = count == -1 ? self.Split(separators, options) : self.Split(separators, count, options);
            for(int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim(trim);
            return parts;
        }
        #endregion
    }
}
