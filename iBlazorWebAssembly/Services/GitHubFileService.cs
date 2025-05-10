using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Octokit;
using iBlazorWebAssembly.Models;
using Microsoft.Extensions.Logging;

namespace iBlazorWebAssembly.Services
{
    /// <summary>
    /// GitHub文件服务实现，用于文档文件的托管
    /// </summary>
    public class GitHubFileService : IGitHubFileService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        private readonly ILogger<GitHubFileService>? _logger;

        private readonly string _repoOwner;
        private readonly string _repoName;
        private readonly string _branch;
        private readonly string _tokenKey;
        
        public GitHubFileService(
            ILocalStorageService localStorage,
            HttpClient httpClient,
            AppSettings appSettings,
            ILogger<GitHubFileService>? logger = null)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _appSettings = appSettings;
            _logger = logger;
            
            // 从配置中获取GitHub仓库信息
            _repoOwner = _appSettings.GitHub.RepoOwner;
            _repoName = _appSettings.GitHub.RepoName;
            _branch = _appSettings.GitHub.Branch;
            _tokenKey = _appSettings.GitHub.TokenKey;
            
            _logger?.LogInformation("GitHubFileService 初始化，仓库: {Owner}/{Repo}，分支: {Branch}", _repoOwner, _repoName, _branch);
        }
        
        /// <summary>
        /// 获取GitHub客户端
        /// </summary>
        private async Task<GitHubClient> GetGitHubClientAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(_tokenKey);
            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("GitHub访问令牌未设置");
                throw new InvalidOperationException("GitHub访问令牌未设置。请先配置令牌。");
            }
            
            var github = new GitHubClient(new ProductHeaderValue("iBlazorWebAssembly"))
            {
                Credentials = new Credentials(token)
            };
            
            return github;
        }
        
        /// <summary>
        /// 上传文件到GitHub仓库
        /// </summary>
        public async Task<string> UploadFileAsync(byte[] content, string fileName, string commitMessage)
        {
            _logger?.LogInformation("开始上传文件: {FileName}", fileName);
            
            var github = await GetGitHubClientAsync();
            
            // 生成文件路径 - 按日期和文件类型组织
            var extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
            var fileType = extension.GetFileTypeFolder();
            var path = $"docs/{fileType}/{DateTime.Now:yyyyMMdd}/{Guid.NewGuid()}/{fileName}";
            
            try
            {
                // 创建或更新文件
                var response = await github.Repository.Content.CreateFile(
                    _repoOwner,
                    _repoName,
                    path,
                    new CreateFileRequest(commitMessage, Convert.ToBase64String(content), _branch)
                );
                
                // 返回原始文件URL
                var fileUrl = $"https://raw.githubusercontent.com/{_repoOwner}/{_repoName}/{_branch}/{path}";
                _logger?.LogInformation("文件上传成功: {FileName} -> {Url}", fileName, fileUrl);
                return fileUrl;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "上传文件到GitHub失败: {FileName}", fileName);
                throw new Exception($"上传文件到GitHub失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 下载文件
        /// </summary>
        public async Task<byte[]> DownloadFileAsync(string path)
        {
            _logger?.LogInformation("开始下载文件: {Path}", path);
            var github = await GetGitHubClientAsync();
            
            try
            {
                var content = await github.Repository.Content.GetRawContent(_repoOwner, _repoName, path);
                _logger?.LogInformation("文件下载成功: {Path}", path);
                return content;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从GitHub下载文件失败: {Path}", path);
                throw new Exception($"从GitHub下载文件失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 删除文件
        /// </summary>
        public async Task DeleteFileAsync(string path, string commitMessage)
        {
            _logger?.LogInformation("开始删除文件: {Path}", path);
            var github = await GetGitHubClientAsync();
            
            try
            {
                // 获取文件的SHA值，用于删除操作
                var fileContent = await github.Repository.Content.GetAllContentsByRef(
                    _repoOwner,
                    _repoName,
                    path,
                    _branch
                );
                
                if (fileContent != null && fileContent.Count > 0)
                {
                    await github.Repository.Content.DeleteFile(
                        _repoOwner,
                        _repoName,
                        path,
                        new DeleteFileRequest(commitMessage, fileContent[0].Sha, _branch)
                    );
                    _logger?.LogInformation("文件删除成功: {Path}", path);
                }
                else
                {
                    _logger?.LogWarning("找不到要删除的文件: {Path}", path);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "删除GitHub文件失败: {Path}", path);
                throw new Exception($"删除GitHub文件失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 获取文件列表
        /// </summary>
        public async Task<string[]> GetFileListAsync(string path = "")
        {
            string logPath = string.IsNullOrEmpty(path) ? "根目录" : path;
            _logger?.LogInformation("获取文件列表: {Path}", logPath);
            var github = await GetGitHubClientAsync();
            
            try
            {
                // 处理空路径的情况 - 使用 null 表示根目录
                // Octokit API 在处理 GetAllContentsByRef 时，空字符串会导致错误，而 null 表示根目录
                var contents = await github.Repository.Content.GetAllContentsByRef(
                    _repoOwner,
                    _repoName,
                    string.IsNullOrEmpty(path) ? null : path,
                    _branch
                );
                
                var result = contents.Select(c => c.Path).ToArray();
                _logger?.LogInformation("获取文件列表成功: {Count}个文件", result.Length);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取GitHub文件列表失败: {Path}", logPath);
                throw new Exception($"获取GitHub文件列表失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 检查GitHub访问令牌是否已设置
        /// </summary>
        public async Task<bool> IsTokenSetAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(_tokenKey);
            var isSet = !string.IsNullOrEmpty(token);
            _logger?.LogInformation("GitHub访问令牌状态检查: {IsSet}", isSet ? "已设置" : "未设置");
            return isSet;
        }
        
        /// <summary>
        /// 保存GitHub访问令牌
        /// </summary>
        public async Task SaveTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _logger?.LogWarning("尝试保存空的GitHub访问令牌");
                throw new ArgumentException("访问令牌不能为空");
            }
            
            await _localStorage.SetItemAsync(_tokenKey, token);
            _logger?.LogInformation("GitHub访问令牌已保存");
        }
    }
}