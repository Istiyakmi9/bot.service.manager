using bot.service.manager.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace bot.service.manager.Service
{
    public class YamlUtilService
    {
        private readonly RemoteServerConfig _remoteServerConfig;

        public YamlUtilService(IOptions<RemoteServerConfig> options)
        {
            _remoteServerConfig = options.Value;
        }

        private YamlModel ReadYamlFile(string filePath)
        {
            string json = ToJson(filePath);

            return JsonConvert.DeserializeObject<YamlModel>(json);
        }

        private string ToJson(string filePath)
        {
            // convert string/file to YAML object
            //var r = new StreamReader(filePath);
            var deserializer = new Deserializer();
            var yamlObject = deserializer.Deserialize(filePath);

            // now convert the object to JSON. Simple!
            Newtonsoft.Json.JsonSerializer js = new Newtonsoft.Json.JsonSerializer();

            var w = new StringWriter();
            js.Serialize(w, yamlObject);
            return w.ToString();
        }

        public async Task<string> ReadGithubYamlFile(string downloadUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Add authentication headers
                    client.DefaultRequestHeaders.Add("User-Agent", "YourAppName");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer ghp_{_remoteServerConfig.accessToken}");

                    HttpResponseMessage response = await client.GetAsync(downloadUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Error fetching file content: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<YamlModel> GetGithubYamlFile(string downloadUrl)
        {
            var result = await ReadGithubYamlFile(downloadUrl);
            var data = ToJson(result);
            return JsonConvert.DeserializeObject<YamlModel>(data);
        }
    }
}
