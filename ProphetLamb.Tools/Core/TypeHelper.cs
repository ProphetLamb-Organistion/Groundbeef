using System;
using System.Collections.Generic;
using System.Linq;

namespace ProphetLamb.Tools
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class TypeHelper
    {
        public static T CastObject<T>(in object input)
        {
            return (T)input;
        }

        public static T ConvertObject<T>(in object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        public static bool IsGenericIEnumerable(in Type type)
        {
            var t = typeof(IEnumerable<>);
            return type != typeof(string) && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == t);
        }

        public static Type GetEnumerableBaseType(in Type type)
        {
            Type baseType = null;
            try
            {
                var elementType = type.GetGenericArguments()[0];
                baseType = elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? elementType.GetGenericArguments()[0]
                    : elementType;
            }
            catch (NotSupportedException) { }
            catch (InvalidOperationException) { }
            return baseType;
        }
    }
}
