using bot.service.manager.Model;
using bot.service.manager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bot.service.manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly EditorService _editorService;

        public EditorController(EditorService editorService)
        {
            _editorService = editorService;
        }

        [HttpPost("getfile")]
        public async Task<FileDetail> GetFileContent([FromBody] FileDetail fileDetail)
        {
            return await _editorService.GetFileContentService(fileDetail);
        }

        [HttpPost("updatefile")]
        public async Task<FileDetail> UpdateFileContent([FromBody] FileDetail fileDetail)
        {
            return await _editorService.UpdateFileContentService(fileDetail);
        }
    }
}
