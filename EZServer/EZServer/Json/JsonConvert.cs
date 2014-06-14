using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JsonFx.Json;

namespace EZServer
{
    public class JsonConvert
    {
        public static string SerializeObject(object o)
        {
            string json = JsonWriter.Serialize(o);
            return CSharpEncrypt.Encrypt (json);
        }

        public static T DeserializeObject<T>(string json)
        {
            return (T)JsonReader.Deserialize(json, typeof(T));
        }
    }
}
