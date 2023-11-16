using bot.service.manager.IService;
using bot.service.manager.Model;
using Microsoft.AspNetCore.Mvc;

namespace Core.Pipeline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderDiscoveryController : ControllerBase
    {
        private readonly IFolderDiscoveryService _folderDiscoveryService;

        public FolderDiscoveryController(IFolderDiscoveryService folderDiscoveryService)
        {
            _folderDiscoveryService = folderDiscoveryService;
        }

        [HttpPost("GetFolder")]
        public async Task<FolderDiscovery> GetFolderDetail([FromBody] FolderDiscovery folderDiscovery)
        {
            var result = await _folderDiscoveryService.GetFolderDetailService(folderDiscovery.TargetDirectory);
            return result;
        }

        [HttpGet("RunCommand")]
        public async Task<string> RunCommand()
        {
            var result = await _folderDiscoveryService.RunCommandService();
            return result;
        }

    }
}
