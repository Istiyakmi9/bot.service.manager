using bot.service.manager.Model;
using Octokit;

namespace bot.service.manager.Service
{
    public class EditorService
    {
        private readonly YamlUtilService _yamlUtilService;

        public EditorService(YamlUtilService yamlUtilService)
        {
            _yamlUtilService = yamlUtilService;
        }

        public async Task<GitHubContent> UpdateFileContentService(GitHubContent gitHubContent)
        {
            string owner = "Marghubur";
            string repo = "ems-k8s";
            string accessToken = "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";

            GitHubClient client = new GitHubClient(new ProductHeaderValue("GitHubApiExample"));
            var tokenAuth = new Credentials(accessToken);
            client.Credentials = tokenAuth;
            try
            {
                if (gitHubContent == null)
                    throw new Exception("Object is invalid");

                if (string.IsNullOrEmpty(gitHubContent.FileContent))
                    throw new Exception("Content is null or empty");

                if (string.IsNullOrEmpty(gitHubContent.Sha))
                    throw new Exception("SHA is null or empty");

                if (string.IsNullOrEmpty(gitHubContent.Path))
                    throw new Exception("Path is null or empty");

                var updateRequest = new UpdateFileRequest("Updating file", gitHubContent.FileContent, gitHubContent.Sha)
                {
                    Branch = "main"
                };

                var updateFile = await client.Repository.Content.UpdateFile(owner, repo, gitHubContent.Path, updateRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return gitHubContent;
        }

        public async Task<GitHubContent> GetFileContentService(GitHubContent gitHubContent)
        {
            ValidateGithubContentModel(gitHubContent);
            gitHubContent.FileContent = await _yamlUtilService.ReadGithubYamlFile(gitHubContent.DownloadUrl);
            return gitHubContent;
        }

        private void ValidateGithubContentModel(GitHubContent gitHubContent)
        {
            if (gitHubContent == null)
                throw new Exception("Invalid object requested");

            if (string.IsNullOrEmpty(gitHubContent.DownloadUrl))
                throw new Exception("Invalid url");
        }

    }
}
