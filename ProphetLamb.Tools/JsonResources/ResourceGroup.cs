using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace ProphetLamb.Tools.JsonResources
{
    internal class IntermidiateResourceGroup
    {
        public IntermidiateResourceGroup(string typeName, IList<JObject> resourcesValues)
        {
            TypeName = typeName;
            ResourcesValues = resourcesValues;
        }

        [JsonProperty("type")]
        public string TypeName { get; private set; }

        [JsonProperty("values")]
        public IList<JObject> ResourcesValues { get; private set; }
    }

    internal class ResourceGroupConverter : JsonConverter<IntermidiateResourceGroup>
    {
        private static ConcurrentDictionary<string, Type> typeLookup = new ConcurrentDictionary<string, Type>();
        private static ConcurrentDictionary<string, MethodInfo> toObjectLookup = new ConcurrentDictionary<string, MethodInfo>();

        public override IntermidiateResourceGroup ReadJson(JsonReader reader, Type objectType, [AllowNull] IntermidiateResourceGroup existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);
            string typeName = item["type"].Value<string>();
            MethodInfo toObjectMethod;
            if (typeLookup.ContainsKey(typeName))
            {
                toObjectMethod = toObjectLookup[typeName];
            }
            else
            {
                Type resourcesType = Type.GetType(typeName);
                typeLookup.Add(typeName, resourcesType);
                //JObject.ToObject<IList<JObject>>
                toObjectMethod = typeof(JObject)
                                .GetMethod("ToObject", BindingFlags.Public | BindingFlags.Instance)
                                .MakeGenericMethod(typeof(IList<>)
                                .MakeGenericType(typeof(JObject)));
                toObjectLookup.Add(typeName, toObjectMethod);
            }
            IList<JObject> resources = toObjectMethod.Invoke(item["values"], new object[]{ serializer }) as IList<JObject>;
            return new IntermidiateResourceGroup(typeName, resources);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] IntermidiateResourceGroup value, JsonSerializer serializer)
        {

        }
    }
}
