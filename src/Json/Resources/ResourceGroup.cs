using Groundbeef.Reflection;
using Groundbeef.SharedResources;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

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
                yield return KeyValuePair.Create(Keys[i], Values[i] ?? null); // Make the comiler happy: use the nullcoop
            }
        }
    }

    internal class ResourceGroupConverter : JsonConverter<ResourceGroup>
    {
        private static readonly Dictionary<Guid, Invoker> s_jTokenToObjectTable = new Dictionary<Guid, Invoker>();
        private static readonly Dictionary<Guid, Invoker> s_listConstructorTable = new Dictionary<Guid, Invoker>();
        private static readonly Dictionary<string, Type> s_elementTypeTable = new Dictionary<string, Type>();

        public override ResourceGroup ReadJson(JsonReader reader, Type objectType, [AllowNull] ResourceGroup? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader) ?? throw new JsonReaderException(ExceptionResource.JOBJECT_READER_LOAD_FAILED);
            // Read type
            string? typeName = obj["type"]?.ToObject<string?>();
            if (String.IsNullOrWhiteSpace(typeName))
                throw new JsonReaderException(ExceptionResource.STRING_NULLWHITESPACE);
            if (!s_elementTypeTable.TryGetValue(typeName, out Type? resourcesType) || resourcesType is null)
            {
                resourcesType = Type.GetType(typeName) ?? throw new JsonReaderException(ExceptionResource.TYPE_INDETERMINABLE);
                s_elementTypeTable.Add(typeName, resourcesType);
            }
            // Read keys
            IList<string>? keys = obj["keys"]?.ToObject<IList<string>?>();
            if (keys is null)
                throw new JsonReaderException(ExceptionResource.VALUE_NOTNULL);
            // Read values
            JToken[] jValues = obj["values"]?.ToArray() ?? throw new JsonReaderException(ExceptionResource.JOBJECT_READER_LOAD_FAILED);
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
                values[i] = JToken.FromObject(value?.Values[i] ?? throw new JsonWriterException(ExceptionResource.ANY_ELEMENT_NULL), serializer);
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
            if (!s_jTokenToObjectTable.TryGetValue(elementType.GUID, out Invoker method))
            {
                // One generic type parameter, no other parameters.
                method = typeof(JToken).GetMethod("ToObject", 1, Array.Empty<Type>())
                // Make generic method for the specified type.
                .MakeGenericMethod(elementType).Invoke;
                s_jTokenToObjectTable.Add(elementType.GUID, method);
            }
            return method(target, null);
        }

        private static IList MakeGenericList(in Type elementType, int capacity)
        {
            Guid guid = elementType.GUID;
            if (!s_listConstructorTable.TryGetValue(guid, out Invoker ctor))
            {
                ctor = typeof(List<>)
                    .MakeGenericType(elementType)
                    .GetConstructor(new Type[] { typeof(int) })
                    .Invoke;
                s_listConstructorTable.Add(guid, ctor);
            }
            return (IList)(ctor(null, new object?[] { capacity }) ?? throw new InvalidOperationException(ExceptionResource.CTOR_INVOKE_FAILED));
        }
    }
}
