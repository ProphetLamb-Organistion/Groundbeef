using Groundbeef.Collections;
using Groundbeef.Core;
using Groundbeef.Reflection;
using Groundbeef.SharedResources;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Groundbeef.Text
{
    [ComVisible(true)]
    public static class StringHelper
    {
        private static readonly Invoker s_fastAllocateStringMethod = typeof(string).GetMethod("FastAllocateString", BindingFlags.NonPublic | BindingFlags.Static).Invoke;
        private static readonly object[] s_fastAllocateStringParameters = new object[1];
        private static readonly Dictionary<Guid, Type[]?> s_genericTypeArguments = new Dictionary<Guid, Type[]?>();
        private static readonly Dictionary<Guid, int> s_convertibleTypeIndicies = new Dictionary<Guid, int>();
        private static readonly int s_iEnumerableTypeDefinitionIndex = s_convertibleTypes.IndexOf(typeof(IEnumerable<>)),
                                    s_iDictionaryTypeDefinitionIndex = s_convertibleTypes.IndexOf(typeof(IDictionary<,>));

        internal const int ConvertibleTypeIndex = 0x7FFFFFFF;

        private static readonly Type[] s_convertibleTypes = {
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
            typeof(IDictionary<,>),
            typeof(Range<>),
        };

        static StringHelper()
        {
            // Feed convertible type dictionary 
            foreach (Type t in s_convertibleTypes)
            {
                _ = VerifyTypeAndGetGenericArguments(t, out _);
            }

            _ = FastAllocateString(10);
        }

        /// <summary>
        /// Returns whether the type is convertible to and from a string using the <see cref="StringHelper"/> functions. 
        /// Eligeble are primitives, <see cref="Color"/>, <see cref="DateTime"/>, <see cref="IEnumerable{}"/>, <see cref="KeyValuePair{,}"/>, <see cref="IDictionary{,}"/>, <see cref="Range{}"/>,
        /// and all types that have a public static method with the <see cref="FromString"/> and <see cref="ToString"/> attribute.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><see cref="true"/> if the type is eligable; otherwise, <see cref="false"/>.</returns>
        public static bool IsStringConvertible(this Type type) => VerifyTypeAndGetGenericArguments(type, out _) != -1;

        internal static int VerifyTypeAndGetGenericArguments(in Type type, out Type[]? genericArguments)
        {
            Guid originGuid = type.GUID;
            if (s_genericTypeArguments.TryGetValue(originGuid, out genericArguments))
                return s_convertibleTypeIndicies[originGuid];
            Type t;
            int index = -1;
            if (type.HasInterface(typeof(IEnumerable<>)))
            {
                t = typeof(IEnumerable<>);
                index = s_iEnumerableTypeDefinitionIndex;
            }
            else if (type.HasInterface(typeof(IDictionary<,>)))
            {
                t = typeof(IDictionary<,>);
                index = s_iDictionaryTypeDefinitionIndex;
            }
            else if (type.IsGenericType)
            {
                t = type.GetGenericTypeDefinition();
                genericArguments = type.GetGenericArguments();
            }
            else if (type.IsEnum)
            {
                t = typeof(Enum);
            }
            else
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
                MethodInfo? toString = methods.WithAttribute<ToString>().FirstOrDefault(),
                            fromString = methods.WithAttribute<FromString>().FirstOrDefault();
                if (!(toString is null || fromString is null))
                    index = ConvertibleTypeIndex;
                t = type;
            }
            index = index == -1 ? s_convertibleTypes.IndexOf(t) : index;
            s_genericTypeArguments.Add(originGuid, genericArguments);
            s_convertibleTypeIndicies.Add(originGuid, index);
            return index;
        }

        public static string? FastAllocateString(int length)
        {
            s_fastAllocateStringParameters[0] = length;
            return s_fastAllocateStringMethod(null, s_fastAllocateStringParameters) as string;
        }

        /// <summary>
        /// Encodes all characters in the <see cref="String"/> into a squence of <see cref="bytes"/> using the specified encoding
        /// </summary>
        /// <param name="encoding">The character encoding used to encode the characters.</param>
        /// <returns>A <see cref="Byte[]"/> containing the encoded <see cref="String"/>.</returns>
        public static byte[] GetBytes(this string self, Encoding encoding) => encoding.GetBytes(self);

        /// <summary>
        /// Encodes all characters in the <see cref="String"/> into a squence of <see cref="bytes"/> using the <see cref="Encoding.UTF8"/>
        /// </summary>
        /// <returns>A <see cref="Byte[]"/> containing the encoded <see cref="String"/>.</returns>
        public static byte[] GetUTF8Bytes(this string self) => Encoding.UTF8.GetBytes(self);

        /// <summary>
        /// Encodes all characters in the <see cref="String"/> into a squence of <see cref="bytes"/> using the <see cref="Encoding.ASCII"/>
        /// </summary>
        /// <returns>A <see cref="Byte[]"/> containing the encoded <see cref="String"/>.</returns>
        public static byte[] GetASCIIBytes(this string self) => Encoding.ASCII.GetBytes(self);

        /// <summary>
        /// Returns whether the string only contains ASCII characters.
        /// </summary>
        /// <returns><see cref="true"/> if the string contains only ASCII characters; otherwise, <see cref="false"/>.</returns>
        public static bool IsASCIIString(this string self)
        {
            return self.Length == Encoding.UTF8.GetByteCount(self);
        }

        /// <summary>
        /// Returns a new instance <see cref="StringBuilder"/> appending <paramref name="value"/> to the <see cref="String"/>.
        /// </summary>
        /// <param name="value">The <see cref="String" to append.</param>
        /// <returns>A new instance <see cref="StringBuilder"/> appending <paramref name="value"/> to the <see cref="String"/>.</returns>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhitespace(this string self)
        {
            return String.IsNullOrWhiteSpace(self);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsIgnoreCase(this string left, string right)
        {
            return String.Equals(left, right, StringComparison.CurrentCultureIgnoreCase);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsInvariant(this string left, string right)
        {
            return String.Equals(left, right, StringComparison.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsInvariantIgnoreCase(this string left, string right)
        {
            return String.Equals(left, right, StringComparison.InvariantCultureIgnoreCase);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsOrdinal(this string left, string right)
        {
            return String.Equals(left, right, StringComparison.Ordinal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsOrdinalIgnoreCase(this string left, string right)
        {
            return String.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }

        public static string NullSafe(this string? self)
        {
            return self??String.Empty;
        }

        #region Random string
        private static readonly char[] s_defaultAlphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        /// <summary>
        /// Returns a new randomly generated string with the specified <paramref name="length"/> containing lower and uppercase alphanumeric characters 0-9a-zA-Z.
        /// </summary>
        /// <param name="length">The number of chars in the random string.</param>
        /// <returns>A new randomly generated string with the specified <paramref name="length"/> containing lower and uppercase alphanumeric characters 0-9a-zA-Z.</returns>
        public static string RandomString(int length) => RandomString(length, s_defaultAlphabet, new Random());

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
                throw new ArgumentException(ExceptionResource.ALPHABET_LENGTH_LESS_2);
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
        #endregion

        #region String to value
        private static readonly Dictionary<Guid, Invoker> s_toKeyValuePairMethods = new Dictionary<Guid, Invoker>(),
                                                          s_toDictionaryMethods = new Dictionary<Guid, Invoker>(),
                                                          s_toConvertibleTypeMethods = new Dictionary<Guid, Invoker>();
        private static readonly object?[] s_toKeyValuePairParameters = new object[1],
                                          s_toDictionaryParameters = new object[1],
                                          s_toConvertibleTypeByAttributeParameters = new object[1];

        public static Byte ToByte(this string self) => ToByte(self.AsSpan());
        public static Byte ToByte(this ReadOnlySpan<char> self)
        {
            return Byte.Parse(self);
        }
        public static Byte ToByte(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToByte(self.AsSpan(), numberStyles, provider);
        public static Byte ToByte(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Byte.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static SByte ToSByte(this string self) => ToSByte(self.AsSpan());
        public static SByte ToSByte(this ReadOnlySpan<char> self)
        {
            return SByte.Parse(self);
        }

        public static SByte ToSByte(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToSByte(self.AsSpan(), numberStyles, provider);
        public static SByte ToSByte(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return SByte.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static UInt16 ToUInt16(this string self) => ToUInt16(self.AsSpan());
        public static UInt16 ToUInt16(this ReadOnlySpan<char> self)
        {
            return UInt16.Parse(self);
        }

        public static UInt16 ToUInt16(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToUInt16(self.AsSpan(), numberStyles, provider);
        public static UInt16 ToUInt16(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return UInt16.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static Int16 ToInt16(this string self) => ToInt16(self.AsSpan());
        public static Int16 ToInt16(this ReadOnlySpan<char> self)
        {
            return Int16.Parse(self);
        }

        public static Int16 ToInt16(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToInt16(self.AsSpan(), numberStyles, provider);
        public static Int16 ToInt16(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Int16.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static UInt32 ToUInt32(this string self) => ToUInt32(self.AsSpan());
        public static UInt32 ToUInt32(this ReadOnlySpan<char> self)
        {
            return UInt32.Parse(self);
        }

        public static UInt32 ToUInt32(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToUInt32(self.AsSpan(), numberStyles, provider);
        public static UInt32 ToUInt32(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return UInt32.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static Int32 ToInt32(this string self) => ToInt32(self.AsSpan());
        public static Int32 ToInt32(this ReadOnlySpan<char> self)
        {
            return Int32.Parse(self);
        }

        public static Int32 ToInt32(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToInt32(self.AsSpan(), numberStyles, provider);
        public static Int32 ToInt32(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Int32.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static UInt64 ToUInt64(this string self) => ToUInt64(self.AsSpan());
        public static UInt64 ToUInt64(this ReadOnlySpan<char> self)
        {
            return UInt64.Parse(self);
        }

        public static UInt64 ToUInt64(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToUInt64(self.AsSpan(), numberStyles, provider);
        public static UInt64 ToUInt64(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return UInt64.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static Int64 ToInt64(this string self) => ToInt64(self.AsSpan());
        public static Int64 ToInt64(this ReadOnlySpan<char> self)
        {
            return Int64.Parse(self);
        }

        public static Int64 ToInt64(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToInt64(self.AsSpan(), numberStyles, provider);
        public static Int64 ToInt64(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Int64.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static Single ToSingle(this string self) => ToSingle(self.AsSpan());
        public static Single ToSingle(this ReadOnlySpan<char> self)
        {
            return Single.Parse(self);
        }

        public static Single ToSingle(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToSingle(self.AsSpan(), numberStyles, provider);
        public static Single ToSingle(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Single.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static Double ToDouble(this string self) => ToDouble(self.AsSpan());
        public static Double ToDouble(this ReadOnlySpan<char> self)
        {
            return Double.Parse(self);
        }

        public static Double ToDouble(this string self, NumberStyles? numberStyles = null, IFormatProvider? provider = null) => ToDouble(self.AsSpan(), numberStyles, provider);
        public static Double ToDouble(this ReadOnlySpan<char> self, NumberStyles? numberStyles = null, IFormatProvider? provider = null)
        {
            return Double.Parse(self, numberStyles ?? NumberStyles.Integer, provider);
        }

        public static object ToEnum(this string self, Type enumType) => Enum.Parse(enumType, self);
        public static T ToEnum<T>(this string self) where T : struct, IConvertible => Enum.Parse<T>(self);

        public static DateTime ToDateTime(this string self, IFormatProvider? provider = null, DateTimeStyles styles = DateTimeStyles.None) => ToDateTime(self.AsSpan(), provider, styles);
        public static DateTime ToDateTime(this ReadOnlySpan<char> self, IFormatProvider? provider = null, DateTimeStyles styles = DateTimeStyles.None)
        {
            return DateTime.Parse(self, provider, styles);
        }

        public static Color ToColor(this string self, ColorStyles colorStyles = ColorStyles.None)
        {
            // Set to default ColorStyles if None
            colorStyles = colorStyles == ColorStyles.None ? ColorStyles.HexInteger : colorStyles;

            string trimmed = self.Trim();
            // Hex or Int
            if ((colorStyles & (ColorStyles.Integer | ColorStyles.HexInteger)) != ColorStyles.None)
            {
                NumberStyles styles = NumberStyles.None
                | ((colorStyles & ColorStyles.Integer) == ColorStyles.Integer ? NumberStyles.Integer : NumberStyles.None)
                | ((colorStyles & ColorStyles.HexInteger) == ColorStyles.HexInteger ? NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier : NumberStyles.None);
                if (Int32.TryParse(trimmed, styles, CultureInfo.CurrentCulture.NumberFormat, out int value))
                    return Color.FromArgb(value);
            }
            // Name
            if ((colorStyles & ColorStyles.Name) == ColorStyles.Name && EnumHelper<KnownColor>.GetNames().Contains(trimmed))
                return Color.FromKnownColor(EnumHelper<KnownColor>.Parse(trimmed));
            // Bytes
            if ((colorStyles & ColorStyles.Tuple) == ColorStyles.Tuple)
            {
                var bytes = new byte[4];
                var values = trimmed.Split(',');
                int length = Math.Min(bytes.Length, values.Length);
                for (int i = 0; i < length; i++)
                {
                    if (!Byte.TryParse(values[i], out byte b))
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
            throw new FormatException(ExceptionResource.FORMAT_NOT_RECOGNIZED);
        }

        public static IEnumerable<T> ToSequence<T>(this string self)
        {
            var collection = JsonConvert.DeserializeObject<string[]>(self);
            return collection.Select(serialized => StringToConvertibleValue<T>(serialized));
        }

        public static IEnumerable ToSequence(string self, Type elementType)
        {
            var collection = JsonConvert.DeserializeObject<string[]>(self);
            return collection.Select(serialized => StringToConvertibleValue(serialized, elementType));
        }

        public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this string self)
        {
            var kvp = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(self);
            TKey key = StringToConvertibleValue<TKey>(kvp.Key);
            TValue value = StringToConvertibleValue<TValue>(kvp.Value);
            return KeyValuePair.Create(key, value);
        }

        public static object ToKeyValuePair(string self, Type keyType, Type valueType)
        {
            Guid guid = keyType.GUID.Combine(valueType.GUID);
            if (!s_toKeyValuePairMethods.TryGetValue(guid, out Invoker keyValuePairMethod))
            {
                keyValuePairMethod = typeof(StringHelper).GetMethod("ToKeyValuePair", 2, new[] { typeof(string), typeof(string) }).Invoke;
                s_toKeyValuePairMethods.Add(guid, keyValuePairMethod);
            }
            s_toKeyValuePairParameters[0] = self;
            return keyValuePairMethod(null, s_toKeyValuePairParameters) ?? throw new NullReferenceException(ExceptionResource.METHOD_INVOKE_FAILED);
        }

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this string self)
        {
            return new Dictionary<TKey, TValue>(JsonConvert.DeserializeObject<IDictionary<string, string>>(self).Select(kvp =>
            {
                TKey key = StringToConvertibleValue<TKey>(kvp.Key);
                TValue value = StringToConvertibleValue<TValue>(kvp.Value);
                return KeyValuePair.Create(key, value);
            }));
        }

        public static IDictionary ToDictionary(string self, Type keyType, Type valueType)
        {
            Guid guid = keyType.GUID.Combine(valueType.GUID);
            if (!s_toDictionaryMethods.TryGetValue(guid, out Invoker dictionaryMethod))
            {
                dictionaryMethod = typeof(StringHelper).GetMethod("ToDictionary", 2, new[] { typeof(string), typeof(string), typeof(string) }).Invoke;
                s_toDictionaryMethods.Add(guid, dictionaryMethod);
            }
            s_toDictionaryParameters[0] = self;
            return (IDictionary)(dictionaryMethod(null, s_toDictionaryParameters) ?? throw new NullReferenceException(ExceptionResource.METHOD_INVOKE_FAILED));
        }

        public static Range<T> ToRange<T>(this string value) where T : IComparable<T>
        {
            var tuple = JsonConvert.DeserializeObject<ValueTuple<string, string>>(value);
            return new Range<T>(StringToConvertibleValue<T>(tuple.Item1), StringToConvertibleValue<T>(tuple.Item2));
        }

        private static object ToConvertibleTypeByAttribute(string value, Type convertibleType)
        {
            if (!s_toConvertibleTypeMethods.TryGetValue(convertibleType.GUID, out Invoker fromString))
            {
                fromString = (convertibleType.GetMethods().WithAttribute<FromString>().First() ?? throw new MissingMethodException(ExceptionResource.METHOD_NOT_HAVE_ATTRIBUTE)).Invoke;
                s_toConvertibleTypeMethods.Add(convertibleType.GUID, fromString);
            }
            s_toConvertibleTypeByAttributeParameters[0] = value;
            return fromString(null, s_toConvertibleTypeByAttributeParameters) ?? throw new NullReferenceException(ExceptionResource.METHOD_INVOKE_FAILED);
        }

        /// <summary>
        /// Converts a string value to the type specified.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The string value.</param>
        /// <returns>A object of the type specified.</returns>
        public static T StringToConvertibleValue<T>(string value,
            in IFormatProvider? provider = null, in NumberStyles? numberStyles = null,
            in ColorStyles colorStyles = ColorStyles.None, in DateTimeStyles dateTimeStyles = DateTimeStyles.None)
        {
            return (T)StringToConvertibleValue(value, typeof(T), provider, numberStyles, colorStyles, dateTimeStyles);
        }

        /// <summary>
        /// Converts a string value to the type specified.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="type">The type of the object.</param>
        /// <returns>A object of the type specified.</returns>
        public static object StringToConvertibleValue(in string value, Type type,
            in IFormatProvider? provider = null, in NumberStyles? numberStyles = null,
            in ColorStyles colorStyles = ColorStyles.None, in DateTimeStyles dateTimeStyles = DateTimeStyles.None)
        {
            if (value.IsWhitespace())
                throw new ArgumentException(ExceptionResource.STRING_NULLWHITESPACE);
            int index = VerifyTypeAndGetGenericArguments(type, out Type[]? arguments);
            return index switch
            {
                0 => ToByte(value, numberStyles, provider),
                1 => ToSByte(value, numberStyles, provider),
                2 => ToUInt16(value, numberStyles, provider),
                3 => ToInt16(value, numberStyles, provider),
                4 => ToUInt32(value, numberStyles, provider),
                5 => ToInt32(value, numberStyles, provider),
                6 => ToUInt64(value, numberStyles, provider),
                7 => ToInt64(value, numberStyles, provider),
                8 => ToSingle(value, numberStyles, provider),
                9 => ToDouble(value, numberStyles, provider),
                10 => ToEnum(value, type),
                11 => ToColor(value, colorStyles),
                12 => ToDateTime(value, provider, dateTimeStyles),
                13 => ToSequence(value, type.GetGenericArguments()[0]),
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                14 => ToKeyValuePair(value, arguments[0], arguments[1]),
                15 => ToDictionary(value, arguments[0], arguments[1]),
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                ConvertibleTypeIndex => ToConvertibleTypeByAttribute(value, type),
                _ => throw new FormatException(ExceptionResource.TYPE_CANNOT_CONVERT_TO_TYPE),
            };
        }
        #endregion

        #region Value to string
        private static readonly Dictionary<Guid, Invoker> s_fromKeyValuePairMethods = new Dictionary<Guid, Invoker>(),
                                                          s_fromDictionaryMethods = new Dictionary<Guid, Invoker>(),
                                                          s_fromConvertibleTypeMethods = new Dictionary<Guid, Invoker>();
        private static readonly object?[] s_fromKeyValuePairParameters = new object[1],
                                          s_fromDictionaryParameters = new object[1],
                                          s_fromConvertibleTypeByAttributeParameters = new object[1];

        public static string ConvertToString(this Color self, ColorStyles colorStyles = ColorStyles.None)
        {
            return colorStyles switch
            {
                ColorStyles.Tuple => String.Concat(self.A, ",", self.R, ",", self.G, ",", self.B),
                ColorStyles.HexInteger => self.ToArgb().ToString("X"),
                ColorStyles.Integer => self.ToArgb().ToString(),
                ColorStyles.Name => self.IsNamedColor ? self.Name : ConvertToString(self, ColorStyles.HexInteger),
                _ => throw new FormatException(),
            };
        }

        public static string ConvertToString<T>(this IEnumerable<T> self)
        {
            IList<string> collection;
            if (self is IList<T> c)
            {
                collection = new string[c.Count];
                for (int i = 0; i < c.Count; i++)
                    collection[i] = ConvertibleValueToString(c[i]);
            }
            else
            {
                collection = new List<string>();
                IEnumerator en = self.GetEnumerator();
                while (en.MoveNext())
                    collection.Add(ConvertibleValueToString(en.Current));
            }
            return JsonConvert.SerializeObject(collection);
        }

        public static string SequenceToString(IEnumerable self, Type elementType)
        {
            IList<string> collection;
            if (self is IList c)
            {
                collection = new string[c.Count];
                for (int i = 0; i < c.Count; i++)
                    collection[i] = ConvertibleValueToString(c[i], elementType);
            }
            else
            {
                collection = new List<string>();
                IEnumerator en = self.GetEnumerator();
                while (en.MoveNext())
                    collection.Add(ConvertibleValueToString(en.Current, elementType));
            }
            return JsonConvert.SerializeObject(collection);
        }

        public static string ConvertToString<TKey, TValue>(this KeyValuePair<TKey, TValue> self)
        {
            return JsonConvert.SerializeObject(KeyValuePair.Create(ConvertibleValueToString(self.Key), ConvertibleValueToString(self.Value)));
        }

        public static string KeyValuePairToString(object keyValuePair, Type type)
        {
            if (!s_fromKeyValuePairMethods.TryGetValue(type.GUID, out Invoker fromKeyValuePairMethod))
            {
                fromKeyValuePairMethod = typeof(StringHelper).GetMethod("ConvertToString", 2, new[] { typeof(KeyValuePair<,>) }).Invoke;
                s_fromKeyValuePairMethods.Add(type.GUID, fromKeyValuePairMethod);
            }
            s_fromKeyValuePairParameters[0] = keyValuePair;
            return (string)(fromKeyValuePairMethod(null, s_fromKeyValuePairParameters) ?? throw new NullReferenceException(ExceptionResource.METHOD_INVOKE_FAILED));
        }

        public static string ConvertToString<TKey, TValue>(this IDictionary<TKey, TValue> self)
        {
            return JsonConvert.SerializeObject(self.Select(kvp => KeyValuePair.Create(ConvertibleValueToString(kvp.Key), ConvertibleValueToString(kvp.Value))));
        }

        public static string DictionaryToString(object dictionary, Type type)
        {
            if (!s_fromDictionaryMethods.TryGetValue(type.GUID, out Invoker fromDictionaryMethod))
            {
                fromDictionaryMethod = typeof(StringHelper).GetMethod("ConvertToString", 2, new[] { typeof(Dictionary<,>) }).Invoke;
                s_fromDictionaryMethods.Add(type.GUID, fromDictionaryMethod);
            }
            s_fromDictionaryParameters[0] = dictionary;
            return (string)(fromDictionaryMethod(null, s_fromDictionaryParameters) ?? throw new NullReferenceException(ExceptionResource.METHOD_INVOKE_FAILED));
        }

        public static string ConvertToString<T>(this Range<T> range) where T : IComparable<T>
        {
            return JsonConvert.SerializeObject(ValueTuple.Create(ConvertibleValueToString(range.Minimum), ConvertibleValueToString(range.Maximum)));
        }

        private static string FromConvertibleTypeByAttribute(object value, Type convertibleType)
        {
            if (!s_fromConvertibleTypeMethods.TryGetValue(convertibleType.GUID, out Invoker toString))
            {
                toString = (convertibleType.GetMethods().WithAttribute<ToString>().First() ?? throw new MissingMethodException(ExceptionResource.METHOD_NOT_HAVE_ATTRIBUTE)).Invoke;
                s_fromConvertibleTypeMethods.Add(convertibleType.GUID, toString);
            }
            s_fromConvertibleTypeByAttributeParameters[0] = value;
            return (string)(toString(null, s_fromConvertibleTypeByAttributeParameters) ?? throw new NullReferenceException(ExceptionResource.METHOD_INVOKE_FAILED));
        }

        /// <summary>
        /// Converts an object to its string representation.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The object to convert.</param>
        /// <returns>A string representing the object.</returns>
        public static string ConvertibleValueToString<T>(T value, in IFormatProvider? provider = null, in string? formatString = null, in ColorStyles colorStyles = ColorStyles.None)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            return ConvertibleValueToString(value, typeof(T), provider, formatString, colorStyles);
        }

        /// <summary>
        /// Converts an object to its string representation.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="type">The type of the object.</param>
        /// <returns>A string representing the object.</returns>
        public static string ConvertibleValueToString(in object value, Type type, in IFormatProvider? provider = null, in string? formatString = null, in ColorStyles colorStyles = ColorStyles.None)
        {
            Type[] arguments = null!;
            // Handle for generic types by thier definition
            if (type.IsGenericType)
            {
                arguments = type.GetGenericArguments();
                type = type.GetGenericTypeDefinition();
            }
            int index = s_convertibleTypes.IndexOf(t => t == type);
            // Handle enum type
            if (type.IsEnum)
                index = s_convertibleTypes.IndexOf(t => t == typeof(Enum));
            return index switch
            {
                0 => formatString is null ? ((byte)value).ToString(provider) : ((byte)value).ToString(formatString, provider),
                1 => formatString is null ? ((sbyte)value).ToString(provider) : ((sbyte)value).ToString(formatString, provider),
                2 => formatString is null ? ((ushort)value).ToString(provider) : ((ushort)value).ToString(formatString, provider),
                3 => formatString is null ? ((short)value).ToString(provider) : ((short)value).ToString(formatString, provider),
                4 => formatString is null ? ((uint)value).ToString(provider) : ((uint)value).ToString(formatString, provider),
                5 => formatString is null ? ((int)value).ToString(provider) : ((int)value).ToString(formatString, provider),
                6 => formatString is null ? ((ulong)value).ToString(provider) : ((ulong)value).ToString(formatString, provider),
                7 => formatString is null ? ((long)value).ToString(provider) : ((long)value).ToString(formatString, provider),
                8 => formatString is null ? ((float)value).ToString(provider) : ((float)value).ToString(formatString, provider),
                9 => formatString is null ? ((double)value).ToString(provider) : ((double)value).ToString(formatString, provider),
                10 => Enum.GetName(type, value),
                11 => ConvertToString((Color)value, colorStyles),
                12 => ((DateTime)value).ToString(formatString, provider),
                13 => SequenceToString((IEnumerable)value, arguments[0]),
                14 => KeyValuePairToString(value, type),
                15 => DictionaryToString(value, type),
                ConvertibleTypeIndex => FromConvertibleTypeByAttribute(value, type),
                _ => throw new FormatException(ExceptionResource.TYPE_CANNOT_CONVERT_TO_TYPE),
            };
        }
        #endregion

        #region Split
        /// <summary>
        /// Splits a <see cref="String"/> into substrings based on the <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The char that delimits the substrings in this string.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, char separator)
        {
            string[] parts = self.Split(separator);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="String"/> into substrings based on the <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The char that delimits the substrings in this string.</param>
        /// <param name="trim">An array of Unicode characters to remove from the substring if leading, or trailing.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, char separator, char[] trim, int count = -1, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] parts = count == -1 ? self.Split(separator, options) : self.Split(separator, count, options);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim(trim);
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="String"/> into substrings based on the <paramref name="separators"/>.
        /// </summary>
        /// <param name="separators">The char array that delimits the substrings in this string, an empty array that contains no delimiters.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, params char[] separators)
        {
            string[] parts = self.Split(separators);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="String"/> into substrings based on the <paramref name="separators"/>.
        /// </summary>
        /// <param name="separators">The char array that delimits the substrings in this string, an empty array that contains no delimiters.</param>
        /// <param name="trim">An array of Unicode characters to remove from the substring if leading, or trailing.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, char[] separators, char[] trim, int count = -1, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] parts = count == -1 ? self.Split(separators, options) : self.Split(separators, count, options);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim(trim);
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="String"/> into substrings based on the <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The string that delimits the substrings in this string.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, string separator)
        {
            string[] parts = self.Split(separator);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="String"/> into substrings based on the <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The string that delimits the substrings in this string.</param>
        /// <param name="trim">An array of Unicode characters to remove from the substring if leading, or trailing.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, string separator, char[] trim, int count = -1, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] parts = count == -1 ? self.Split(separator, options) : self.Split(separator, count, options);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim(trim);
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="String"/> into substrings based on the <paramref name="separators"/>.
        /// </summary>
        /// <param name="separators">The string array that delimits the substrings in this string, an empty array that contains no delimiters.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, params string[] separators)
        {
            string[] parts = self.Split(separators, StringSplitOptions.None);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();
            return parts;
        }

        /// <summary>
        /// Splits a <see cref="String"/> into substrings based on the <paramref name="separators"/>.
        /// </summary>
        /// <param name="separators">The string array that delimits the substrings in this string, an empty array that contains no delimiters.</param>
        /// <param name="trim">An array of Unicode characters to remove from the substring if leading, or trailing.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public static string[] SplitAndTrim(this string self, string[] separators, char[] trim, int count = -1, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] parts = count == -1 ? self.Split(separators, options) : self.Split(separators, count, options);
            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim(trim);
            return parts;
        }
        #endregion
    }
}
