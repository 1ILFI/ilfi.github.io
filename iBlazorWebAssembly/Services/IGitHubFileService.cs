using System;
using System.Threading.Tasks;

namespace iBlazorWebAssembly.Services
{
    /// <summary>
    /// GitHub文件服务接口，用于文档文件的托管
    /// </summary>
    public interface IGitHubFileService
    {
        /// <summary>
        /// 上传文件到GitHub仓库
        /// </summary>
        /// <param name="content">文件内容字节数组</param>
        /// <param name="fileName">文件名</param>
        /// <param name="commitMessage">提交消息</param>
        /// <returns>文件的GitHub原始URL</returns>
        Task<string> UploadFileAsync(byte[] content, string fileName, string commitMessage);
        
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path">文件在仓库中的路径</param>
        /// <returns>文件内容字节数组</returns>
        Task<byte[]> DownloadFileAsync(string path);
        
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件在仓库中的路径</param>
        /// <param name="commitMessage">提交消息</param>
        Task DeleteFileAsync(string path, string commitMessage);
        
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>文件列表</returns>
        Task<string[]> GetFileListAsync(string path = "");
        
        /// <summary>
        /// 检查GitHub访问令牌是否已设置
        /// </summary>
        /// <returns>如果令牌已设置则为true</returns>
        Task<bool> IsTokenSetAsync();
        
        /// <summary>
        /// 保存GitHub访问令牌
        /// </summary>
        /// <param name="token">访问令牌</param>
        Task SaveTokenAsync(string token);
    }
}