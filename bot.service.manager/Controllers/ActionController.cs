﻿using bot.service.manager.IService;
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
        public async Task<ApiResponse> RunFile([FromBody] FileDetail fileDetail)
        {
            var result = await _actionService.RunFileService(fileDetail);
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
    }
}
