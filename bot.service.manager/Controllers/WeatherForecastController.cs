using bot.service.manager.IService;
using bot.service.manager.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Renci.SshNet;

namespace Core.Pipeline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFolderDiscoveryService _folderDiscoveryService;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly RemoteServerConfig _remoteServerConfig;

        public WeatherForecastController(IFolderDiscoveryService folderDiscoveryService, 
            ILogger<WeatherForecastController> logger, 
            IOptions<RemoteServerConfig> options)
        {
            _folderDiscoveryService = folderDiscoveryService;
            _logger = logger;
            _remoteServerConfig = options.Value;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet("GetWorkingFolder")]
        public string GetWorkingFolder()
        {
            string host = "192.168.0.101"; // Replace with the IP address or hostname of your Linux system
            string username = "bot"; // Replace with your SSH username
            string password = "lIstiyak@i9_01"; // Replace with your SSH password (or use key-based authentication)

            using (var client = new SshClient(host, username, password))
            {
                client.Connect();

                // Command to execute kubectl remotely
                string kubectlCommand = "/snap/bin/microk8s.kubectl get pods";

                // Run the kubectl command
                var command = client.RunCommand(kubectlCommand);
                Console.WriteLine("Command Output:");
                Console.WriteLine(command.Result);

                client.Disconnect();
            }

            return Directory.GetCurrentDirectory();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // _folderDiscoveryService.GetAllFileService(@"D:\\ws\\testing");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}