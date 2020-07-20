using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ProphetLamb.Tools
{
    [ComVisible(true)]
    public static class StringHelper
    {
        internal static readonly MethodInfo methodFastAllocateString = typeof(string).GetMethod("FastAllocateString", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly char[] defaultAlphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string FastAllocateString(int length) => methodFastAllocateString.Invoke(null, new object[] { length }) as string;

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
        public static unsafe string RandomString(int length, ReadOnlySpan<char> alphabet, Random random)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), ExceptionResource.INTEGER_POSITIVEZERO);
            if (alphabet.Length < 2)
                throw new ArgumentException("The alphabet must contain at least two characters.");
            if (random is null)
                throw new ArgumentNullException(nameof(random));
            string str = FastAllocateString(length);
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
    }
}
