using bot.service.manager.Model;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace bot.service.manager.Service
{
    public class YamlUtilService
    {
        public YamlModel ReadYamlFile(string filePath)
        {
            string json = ToJson(filePath);

            return JsonConvert.DeserializeObject<YamlModel>(json);
        }

        public string ToJson(string filePath)
        {
            // convert string/file to YAML object
            var r = new StreamReader(filePath);
            var deserializer = new Deserializer();
            var yamlObject = deserializer.Deserialize(r);

            // now convert the object to JSON. Simple!
            Newtonsoft.Json.JsonSerializer js = new Newtonsoft.Json.JsonSerializer();

            var w = new StringWriter();
            js.Serialize(w, yamlObject);
            return w.ToString();
        }
    }
}
