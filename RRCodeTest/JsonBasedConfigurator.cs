using Newtonsoft.Json;  //Using third party libarary Json.NET to read json files.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRCodeTest
{
    public class JsonBasedConfigurator : IConfigurator
    {
        /// <summary>
        /// Returns the ConnectionString property of the JSON object found in configuration.json
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            using ( StreamReader r = new StreamReader("configuration.json"))
            {
                string json = r.ReadToEnd();
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return dictionary["ConnectionString"];
            }
        }
    }
}
