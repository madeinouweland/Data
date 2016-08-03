// Made with ❤ in Berlin by Loek van den Ouweland
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Data
{
    public class JSONSerializer<T>
    {
        public string SerializeAsString(T o)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(ms, o);
                var reader = new StreamReader(ms);
                ms.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public Stream SerializeAsStream(T o)
        {
            var ms = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(ms, o);
            ms.Position = 0;
            return ms;
        }

        public T DeserializeFromString(string json)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var deserializer = new DataContractJsonSerializer(typeof(T));
                return (T)deserializer.ReadObject(ms);
            }
        }

        public T DeserializeFromStream(Stream json)
        {
            var deserializer = new DataContractJsonSerializer(typeof(T));
            return (T)deserializer.ReadObject(json);
        }
    }
}
