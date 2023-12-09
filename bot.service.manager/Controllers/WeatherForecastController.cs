using bot.service.manager.IService;
using Microsoft.AspNetCore.Mvc;

namespace Core.Pipeline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFolderDiscoveryService _folderDiscoveryService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IFolderDiscoveryService folderDiscoveryService, ILogger<WeatherForecastController> logger)
        {
            _folderDiscoveryService = folderDiscoveryService;
            _logger = logger;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet("GetWorkingFolder")]
        public string GetWorkingFolder()
        {
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