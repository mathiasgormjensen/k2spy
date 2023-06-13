using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public static class Json
    {
        public static string PrettyPrint(string json)
        {
            using (StringReader stringReader = new StringReader(json))
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(stringReader))
                    {
                        using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented })
                        {
                            jsonWriter.WriteToken(jsonReader);
                            return stringWriter.ToString();
                        }
                    }
                }
            }
        }

        public static string Linearize(string json)
        {
            using (StringReader stringReader = new StringReader(json))
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(stringReader))
                    {
                        using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.None })
                        {
                            jsonWriter.WriteToken(jsonReader);
                            return stringWriter.ToString();
                        }
                    }
                }
            }
        }
    }
}