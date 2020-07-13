using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace ProphetLamb.Tools.Localisation.JsonResources
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
        private static readonly Type iResourceType = typeof(IResource),
                                     stringType = typeof(String),
                                     byteArrayType = typeof(byte[]);
        /*
         * TypeName[ASCII]
         * TypeContent[ASCII] handle as byte[] to memorystream
         */
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetInterface(iResourceType.Name) != null;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Resolve type from type name
            string typeName = reader.ReadAsString();
            if (!typeLookupCache.TryGetValue(typeName, out Type type))
            {
                type = Type.GetType(reader.ReadAsString());
                typeLookupCache.Add(typeName, type);
            }
            string content = reader.ReadAsString();
            if (type == stringType) // string
            {
                return content;
            }
            else if (type == byteArrayType) // byte[]
            {
                return Encoding.ASCII.GetBytes(content);
            }
            else if (type.IsValueType) // Structs & Enums:
            {
                return null;
            }
            //Classes: Convert content string to memory stream
            byte[] data = Encoding.ASCII.GetBytes(content);
            using var ms = new MemoryStream(data.Length);
            ms.Write(data);
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
            object o = formatter.Deserialize(stream);
            return o;
        }

        //TODO: FIXX that shit
        byte[] getBytes(CIFSPacket str) {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}