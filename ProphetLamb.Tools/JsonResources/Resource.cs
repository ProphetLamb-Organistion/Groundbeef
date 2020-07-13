using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace ProphetLamb.Tools.JsonResources
{
    public interface IResource
    {
        public Type ObjectType {get;}
        public byte[] ToBytes(string base64);
        public string FromBytes(byte[] bytes);

    }

    public class ResourceConverter : JsonConverter
    {
        private static readonly Dictionary<string, Type> typeLookupCache = new Dictionary<string, Type>();
        private static readonly Dictionary<Guid, ConstructorInfo> ctorLookupCache = new Dictionary<Guid, ConstructorInfo>();
        private static readonly Type iResourceType = typeof(IResource),
                                     stringType = typeof(String),
                                     byteArrayType = typeof(byte[]);
        /*
         * TypeName[ASCII]
         * TypeContent[ASCII] handle as byte[]
         */
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetInterface(iResourceType.Name) != null;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Resolve type from type name.
            string typeName = reader.ReadAsString();
            if (!typeLookupCache.TryGetValue(typeName, out Type type))
            {
                type = Type.GetType(reader.ReadAsString());
                typeLookupCache.Add(typeName, type);
            }
            string content = reader.ReadAsString();
            if (type == stringType)
            {
                // String
                return content;
            }
            else if (type == byteArrayType)
            {
                // Byte[]
                return Convert.FromBase64String(content);
            }
            else if (type.IsValueType)
            {
                // Structs & Enums:
                return StructFromBytes(Convert.FromBase64String(content), type);
            }
            // Classes: Convert content string to memory stream.
            byte[] data = Convert.FromBase64String(content);
            using var ms = new MemoryStream(data);
            return DeserializeFromStream(ms);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            
        }

        public static MemoryStream SerializeToStream(object o)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public static object DeserializeFromStream(MemoryStream stream)
        {
            var formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(stream);
        }

        public static byte[] StructGetBytes(object @object)
        {
            int size = Marshal.SizeOf(@object);
            var array = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(@object, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }

        public static object StructFromBytes(byte[] bytes, Type structType)
        {
            // Atttempt to get the cached constructor.
            if (!ctorLookupCache.TryGetValue(structType.GUID, out ConstructorInfo ctor))
            {
                // Add parameterless ctor to cache.
                ctor = structType.GetConstructor(Array.Empty<Type>());
                ctorLookupCache.Add(structType.GUID, ctor);
            }
            object obj = ctor.Invoke(null);
            int size = Marshal.SizeOf(obj);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size);
            obj = Marshal.PtrToStructure(ptr, obj.GetType());
            Marshal.FreeHGlobal(ptr);
            return obj;
        }
    }
}