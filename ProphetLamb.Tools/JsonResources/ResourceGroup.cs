using System.Threading;
using System.Collections.Specialized;
using System.Security.AccessControl;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProphetLamb.Tools.Collections.Concurrent;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

namespace ProphetLamb.Tools.JsonResources
{
    [JsonConverter(typeof(ResourceGroupConverter))]
    internal class ResourceGroup
    {
        public ResourceGroup(Type valuesType, IList<string> keys, IList values)
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

        public IEnumerable<KeyValuePair<string, object>> ToDictionary()
        {
            int length = Keys.Count;
            for (int i = 0; i < length; i++)
            {
                yield return KeyValuePair.Create(Keys[i], Values[i]);
            }
        }
    }

    internal class ResourceGroupConverter : JsonConverter<ResourceGroup>
    {
        private static readonly Dictionary<Guid, MethodInfo> jTokenToObjectTable = new Dictionary<Guid, MethodInfo>();
        private static readonly Dictionary<Guid, ConstructorInfo> listConstructorTable = new Dictionary<Guid, ConstructorInfo>();
        private static readonly Dictionary<string, Type> elementTypeTable = new Dictionary<string, Type>();

        public override ResourceGroup ReadJson(JsonReader reader, Type objectType, [AllowNull] ResourceGroup existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            // Read type
            string typeName = obj["type"].ToObject<string>();
            if (String.IsNullOrWhiteSpace(typeName))
                throw new JsonSerializationException("The property tag type string cannot be null or whitespace.");
            if (!elementTypeTable.TryGetValue(typeName, out Type resourcesType))
            {
                resourcesType = Type.GetType(typeName);
                elementTypeTable.Add(typeName, resourcesType);
            }
            // Read keys
            IList<string> keys = obj["keys"].ToObject<IList<string>>();
            if (keys is null)
                throw new JsonSerializationException("The property tag keys List<string> cannot be null.");
            // Read values
            JToken[] jValues = obj["values"].ToArray();
            IList values = MakeGenericList(resourcesType, jValues.Length);
            for (int i = 0; i < jValues.Length; i++)
                values.Add(JTokenToObject(jValues[i], resourcesType));
            if (values is null)
                throw new JsonSerializationException("The property tag values List<string> cannot be null.");
            if (keys.Count != values.Count)
                throw new JsonSerializationException("The number of elements in keys does not equal the number of elements in values.");
            return new ResourceGroup(resourcesType, keys, values);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] ResourceGroup value, JsonSerializer serializer)
        {
            var resGrp = new JObject();
            resGrp.AddFirst(new JProperty("type", value.ValuesType.AssemblyQualifiedName));
            resGrp.Add(new JProperty("keys", new JArray(value.Keys)));
            var values = new JToken[value.Values.Count];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = JToken.FromObject(value.Values[i], serializer);
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
                Culture = serializerCulture??CultureInfo.InvariantCulture,
                TypeNameHandling = TypeNameHandling.Arrays,
                Converters = { new ResourceGroupConverter() },
            };
        }

        private static object JTokenToObject(in JToken target, in Type elementType)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (elementType is null)
                throw new ArgumentNullException(nameof(elementType));
            if (!jTokenToObjectTable.TryGetValue(elementType.GUID, out MethodInfo method))
            {
                // One generic type parameter, no other parameters.
                method = typeof(JToken).GetMethod("ToObject", 1, Array.Empty<Type>())
                // Make generic method for the specified type.
                .MakeGenericMethod(elementType);
            }
            return method.Invoke(target, null);
        }

        private static IList MakeGenericList(in Type elementType, int capacity)
        {
            if (elementType is null)
                throw new ArgumentNullException(nameof(elementType));
            Guid guid = elementType.GUID;
            if (!listConstructorTable.TryGetValue(guid, out ConstructorInfo ctor))
            {
                ctor = typeof(List<>)
                    .MakeGenericType(elementType)
                    .GetConstructor(new Type[]{typeof(int)});
                listConstructorTable.Add(guid, ctor);
            }
            return ctor.Invoke(new object[]{capacity}) as IList;
        }
    }
}
