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
        public async Task<string> RunFile([FromBody] FileDetail fileDetail)
        {
            var result = await _actionService.RunFileService(fileDetail);
            return result;
        }

        [HttpPost("ReRunFile")]
        public async Task<string> ReRunFile([FromBody] FileDetail fileDetail)
        {
            var result = await _actionService.ReRunFileService(fileDetail);
            return result;
        }

        [HttpPost("StopFile")]
        public async Task<string> StopFile([FromBody] FileDetail fileDetail)
        {
            var result = await _actionService.StopFileService(fileDetail);
            return result;
        }

        [HttpPost("CheckStatus")]
        public async Task<string> CheckStatus([FromBody] FileDetail fileDetail)
        {
            var result = await _actionService.CheckStatusService(fileDetail);
            return result;
        }
    }
}
