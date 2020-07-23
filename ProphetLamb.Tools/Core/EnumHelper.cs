using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ProphetLamb.Tools
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class EnumHelper<T> where T : struct, IConvertible
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();
            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(in string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException(ExceptionResource.STRING_NULLEMPTY, nameof(value));
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string?> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string? LookupResource(Type resourceManagerProvider, string resourceKey, CultureInfo cultureInfo)
        {
            if (String.IsNullOrEmpty(resourceKey))
                throw new ArgumentException(ExceptionResource.STRING_NULLEMPTY, nameof(resourceKey));
            foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
                    System.Resources.ResourceManager? resourceManager = staticProperty.GetValue(null, null) as System.Resources.ResourceManager;
                    return resourceManager?.GetString(resourceKey, cultureInfo);
                }
            }

            return resourceKey; // Fallback with the key name
        }

        public static string? GetDisplayValue(T value)
        {
            return GetDisplayValue(value, CultureInfo.CurrentUICulture);
        }

        public static string? GetDisplayValue(T value, CultureInfo cultureInfo)
        {
            string? name = value.ToString();
            if (name is null) return null;
            FieldInfo? fieldInfo = value.GetType().GetField(name);
            if (fieldInfo is null) return null;
            var descriptionAttributes = (DisplayAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false);
            if (descriptionAttributes[0].ResourceType != null)
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name, cultureInfo);
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}
