using bot.service.manager.Model;
using Microsoft.Extensions.Options;
using Octokit;

namespace bot.service.manager.Service
{
    public class EditorService
    {
        private readonly YamlUtilService _yamlUtilService;
        private readonly RemoteServerConfig _remoteServerConfig;

        public EditorService(YamlUtilService yamlUtilService, IOptions<RemoteServerConfig> options)
        {
            _yamlUtilService = yamlUtilService;
            _remoteServerConfig = options.Value;
        }

        public async Task<GitHubContent> UpdateFileContentService(GitHubContent gitHubContent)
        {
            if (string.IsNullOrEmpty(_remoteServerConfig.owner))
                throw new Exception("Invalid github user owner detail");

            if (string.IsNullOrEmpty(_remoteServerConfig.repo))
                throw new Exception("Invalid github location");

            if (string.IsNullOrEmpty($"ghp_{_remoteServerConfig.accessToken}"))
                throw new Exception("Invalid github access token");

            GitHubClient client = new GitHubClient(new ProductHeaderValue("GitHubApiExample"));
            var tokenAuth = new Credentials($"ghp_{_remoteServerConfig.accessToken}");
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

                var updateFile = await client.Repository.Content.UpdateFile(_remoteServerConfig.owner, _remoteServerConfig.repo, gitHubContent.Path, updateRequest);
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
