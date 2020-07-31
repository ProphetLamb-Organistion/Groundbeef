using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Groundbeef.Json.Resources
{
    [JsonConverter(typeof(ResourceGroupConverter))]
    internal class ResourceGroup
    {
        public ResourceGroup(in Type valuesType, in IList<string> keys, in IList values)
        {
            ValuesType = valuesType;
            Keys = keys;
            Values = values;
        }

        [JsonProperty("type")]
        public Type ValuesType { get; set; }

        [JsonProperty("keys")]
        public IList<string> Keys { get; set; }

        [JsonProperty("values")]
        public IList Values { get; set; }

        public IEnumerable<KeyValuePair<string, object?>> ToDictionary()
        {
            int length = Keys.Count;
            for (int i = 0; i < length; i++)
            {
                yield return KeyValuePair.Create(Keys[i], Values[i]??null); // Make the comiler happy: use the nullcoop
            }
        }
    }

    internal class ResourceGroupConverter : JsonConverter<ResourceGroup>
    {
        private static readonly Dictionary<Guid, MethodInfo> jTokenToObjectTable = new Dictionary<Guid, MethodInfo>();
        private static readonly Dictionary<Guid, ConstructorInfo> listConstructorTable = new Dictionary<Guid, ConstructorInfo>();
        private static readonly Dictionary<string, Type> elementTypeTable = new Dictionary<string, Type>();

        public override ResourceGroup ReadJson(JsonReader reader, Type objectType, [AllowNull] ResourceGroup? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader)??throw new JsonReaderException("Failed to load the JObject form the reader.");
            // Read type
            string? typeName = obj["type"]?.ToObject<string?>();
            if (String.IsNullOrWhiteSpace(typeName))
                throw new JsonReaderException(ExceptionResource.STRING_NULLWHITESPACE);
            if (!elementTypeTable.TryGetValue(typeName, out Type? resourcesType) || resourcesType is null)
            {
                resourcesType = Type.GetType(typeName)??throw new JsonReaderException("The string typeName could not be converted to a type variable.");
                elementTypeTable.Add(typeName, resourcesType);
            }
            // Read keys
            IList<string>? keys = obj["keys"]?.ToObject<IList<string>?>();
            if (keys is null)
                throw new JsonReaderException(ExceptionResource.VALUE_NOTNULL);
            // Read values
            JToken[] jValues = obj["values"]?.ToArray()??throw new JsonReaderException("Failed to convert values to an array.");
            IList values = MakeGenericList(resourcesType, jValues.Length);
            for (int i = 0; i < jValues.Length; i++)
                values.Add(JTokenToObject(jValues[i], resourcesType));
            if (values is null)
                throw new JsonReaderException(ExceptionResource.VALUE_NOTNULL);
            if (keys.Count != values.Count)
                throw new JsonReaderException(ExceptionResource.COLLECTION_LENGTH_INVALID);
            return new ResourceGroup(resourcesType, keys, values);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] ResourceGroup? value, JsonSerializer serializer)
        {
            var resGrp = new JObject();
            resGrp.AddFirst(new JProperty("type", value?.ValuesType.AssemblyQualifiedName));
            resGrp.Add(new JProperty("keys", value is null ? null : new JArray(value.Keys)));
            var values = new JToken[((value?.Values.Count) ?? 0)];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = JToken.FromObject(value?.Values[i]??throw new JsonWriterException("One or more elements of value are null."), serializer);
            }
            resGrp.Add(new JProperty("values", new JArray(values)));
            resGrp.WriteTo(writer, this);
        }

        public static JsonSerializerSettings SettingsFactory(in CultureInfo serializerCulture)
        {
            return new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Culture = serializerCulture ?? CultureInfo.InvariantCulture,
                TypeNameHandling = TypeNameHandling.Arrays,
                Converters = { new ResourceGroupConverter() },
            };
        }

        private static object? JTokenToObject(in JToken target, in Type elementType)
        {
            if (!jTokenToObjectTable.TryGetValue(elementType.GUID, out MethodInfo? method))
            {
                // One generic type parameter, no other parameters.
                method = typeof(JToken).GetMethod("ToObject", 1, Array.Empty<Type>())?
                // Make generic method for the specified type.
                .MakeGenericMethod(elementType);
                if (method is null)
                    throw new InvalidOperationException("Failed to make generic method.");
                jTokenToObjectTable.Add(elementType.GUID, method);
            }
            return method?.Invoke(target, null);
        }

        private static IList MakeGenericList(in Type elementType, int capacity)
        {
            Guid guid = elementType.GUID;
            if (!listConstructorTable.TryGetValue(guid, out ConstructorInfo? ctor))
            {
                ctor = typeof(List<>)
                    .MakeGenericType(elementType)?
                    .GetConstructor(new Type[] { typeof(int) });
                if (ctor is null)
                    throw new InvalidOperationException("Failed to make generic constructor.");
                listConstructorTable.Add(guid, ctor);
            }
            return (IList)ctor.Invoke(new object?[] { capacity })??throw new InvalidOperationException("Failed to invoke the constructor.");
        }
    }
}
