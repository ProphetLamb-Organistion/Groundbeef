using System;

namespace Groundbeef
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class MathHelper
    {
        /// <summary>
        /// Returns a modulus n as a positive 32bit integer value. 
        /// a elo [min,max] -> [0,n)
        /// </summary>
        public static int NonNegativeModulus(this int a, int n)
        {
            var mod = a - (n * (int)MathF.Truncate(a / (float)n));
            return mod < 0 ? mod + n : mod;
        }

        /// <summary>
        /// Returns a modulus n as a positive 64bit floating point value. 
        /// a elo [min,max] -> [0,n)
        /// </summary>
        public static double NonNegativeModulus(this double a, double n)
        {
            var mod = a - (n * (int)Math.Truncate(a / n));
            return mod < 0 ? mod + n : mod;
        }
    }
}
