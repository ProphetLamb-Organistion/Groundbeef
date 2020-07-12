using System;
using System.Collections.Generic;
using System.Text;

namespace ProphetLamb.Tools
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
            var mod = a - n * (int)MathF.Truncate(a / (float)n);
            return mod < 0 ? mod + n : mod;
        }
    }
}
