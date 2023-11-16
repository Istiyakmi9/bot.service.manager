using bot.service.manager.IService;
using bot.service.manager.Model;

namespace bot.service.manager.Service
{
    public class ActionService: IActionService
    {
        public async Task<string> CheckStatusService(FileDetail fileDetail)
        {
            return await Task.FromResult("Successfull");
        }

        public async Task<string> ReRunFileService(FileDetail fileDetail)
        {
            return await Task.FromResult("Successfull");
        }

        public async Task<string> RunFileService(FileDetail fileDetail)
        {
            return await Task.FromResult("Successfull");
        }

        public async Task<string> StopFileService(FileDetail fileDetail)
        {
            return await Task.FromResult("Successfull");
        }
    }
}
