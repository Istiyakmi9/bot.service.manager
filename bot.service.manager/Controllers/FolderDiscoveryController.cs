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
        public async Task<ApiResponse> GetFolderDetail([FromBody] FolderDiscovery folderDiscovery)
        {
            var result = await _folderDiscoveryService.GetFolderDetailService(folderDiscovery.TargetDirectory);
            return ApiResponse.BuildResponse(result);
        }

        [HttpPost("RunCommand")]
        public async Task<ApiResponse> RunCommand(KubectlModel kubectlModel)
        {
            var result = await _folderDiscoveryService.RunCommandService(kubectlModel);
            return ApiResponse.BuildResponse(result);
        }
       
    }
}
