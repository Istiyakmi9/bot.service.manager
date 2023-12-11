using bot.service.manager.Model;
using Octokit;

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

        public async Task<GitHubContent> GetFileContentService(GitHubContent gitHubContent)
        {
            var accessToken = "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";
            using (HttpClient client = new HttpClient())
            {
                // Add authentication headers
                client.DefaultRequestHeaders.Add("User-Agent", "YourAppName");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                HttpResponseMessage response = await client.GetAsync(gitHubContent.DownloadUrl);

                if (response.IsSuccessStatusCode)
                {
                    gitHubContent.FileContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine($"Error fetching file content: {response.StatusCode} - {response.ReasonPhrase}");
                }

            }
            return gitHubContent;
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
