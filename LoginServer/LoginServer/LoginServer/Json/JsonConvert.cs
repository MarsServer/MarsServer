using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pathfinding.Serialization.JsonFx;

namespace LoginServer
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
            return JsonReader.Deserialize<T>(json);
        }
    }
}
