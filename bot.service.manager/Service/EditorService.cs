using bot.service.manager.Model;

namespace bot.service.manager.Service
{
    public class EditorService
    {
        public async Task<FileDetail> UpdateFileContentService(FileDetail fileDetail)
        {
            try
            {
                ValidateModel(fileDetail);

                // Read the entire content of the file
                await File.WriteAllTextAsync(fileDetail.FullPath!, fileDetail.FileContent!);
            }
            catch
            {
                throw;
            }

            return fileDetail;
        }

        public async Task<FileDetail> GetFileContentService(FileDetail fileDetail)
        {
            try
            {
                ValidateModel(fileDetail);

                // Read the entire content of the file
                string fileContent = await File.ReadAllTextAsync(fileDetail.FullPath);

                fileDetail.FileContent = fileContent;
            }
            catch
            {
                throw;
            }

            return fileDetail;
        }

        private void ValidateModel(FileDetail fileDetail)
        {
            if (fileDetail == null)
                throw new Exception("Invalid object requested");

            if (string.IsNullOrEmpty(fileDetail.FullPath))
                throw new Exception("Invalid file path given");

            if (!File.Exists(fileDetail.FullPath))
                throw new Exception("Requested file not exists");
        }
    }
}
