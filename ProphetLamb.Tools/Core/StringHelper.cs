using System.Reflection;

namespace ProphetLamb.Tools
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class StringHelper
    {
        internal static readonly MethodInfo methodFastAllocateString = typeof(string).GetMethod("FastAllocateString", BindingFlags.NonPublic | BindingFlags.Static);
        public static string FastAllocateString(int length) => methodFastAllocateString.Invoke(null, new object[] { length }) as string;
    }
}
