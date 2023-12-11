using bot.service.manager.IService;
using bot.service.manager.Model;
using Microsoft.AspNetCore.Mvc;

namespace bot.service.manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly IActionService _actionService;

        public ActionController(IActionService actionService)
        {
            _actionService = actionService;
        }

        [HttpPost("RunFile")]
        public async Task<ApiResponse> RunFile([FromBody] GitHubContent gitHubContent)
        {
            var result = await _actionService.RunFileService(gitHubContent);
            return ApiResponse.BuildResponse(result);
        }

        [HttpPost("ReRunFile")]
        public async Task<ApiResponse> ReRunFile([FromBody] FileDetail fileDetail)
        {
            var result = await _actionService.ReRunFileService(fileDetail);
            return ApiResponse.BuildResponse(result);
        }

        [HttpPost("StopFile")]
        public async Task<ApiResponse> StopFile([FromBody] FileDetail fileDetail)
        {
            var result = await _actionService.StopFileService(fileDetail);
            return ApiResponse.BuildResponse(result);
        }

        [HttpPost("CheckStatus")]
        public async Task<ApiResponse> CheckStatus([FromBody] KubectlModel kubectlModel)
        {
            var result = await _actionService.CheckStatusService(kubectlModel);
            return ApiResponse.BuildResponse(result);
        }

        [HttpGet("DelayResult/{Counter}")]
        public async Task<ApiResponse> DelayResult(int Counter)
        {
            await Task.Delay(1000);
            if (Counter == 2)
            {
                return ApiResponse.BuildResponse("working");
            }
            return ApiResponse.BadRequest($"Fail {Counter} time");
        }
    }
}
