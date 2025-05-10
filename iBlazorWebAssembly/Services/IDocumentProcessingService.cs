using System.Threading.Tasks;
using iBlazorWebAssembly.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace iBlazorWebAssembly.Services
{
    /// <summary>
    /// 文档处理服务接口，用于处理不同类型的文档
    /// </summary>
    public interface IDocumentProcessingService
    {
        /// <summary>
        /// 从文档中提取文本
        /// </summary>
        /// <param name="content">文件内容字节数组</param>
        /// <param name="fileType">文件类型</param>
        /// <returns>提取的文本内容</returns>
        Task<string> ExtractTextFromDocumentAsync(byte[] content, string fileType);
        
        /// <summary>
        /// 将文档转换为HTML
        /// </summary>
        /// <param name="content">文件内容字节数组</param>
        /// <param name="fileType">文件类型</param>
        /// <returns>HTML格式内容</returns>
        Task<string> ConvertDocumentToHtmlAsync(byte[] content, string fileType);
        
        /// <summary>
        /// 从IBrowserFile中处理文档并创建元数据
        /// </summary>
        /// <param name="file">浏览器上传的文件</param>
        /// <param name="extractText">是否提取文本</param>
        /// <returns>文档元数据</returns>
        Task<DocumentMetadata> ProcessDocumentFromBrowserFileAsync(IBrowserFile file, bool extractText = true);
        
        /// <summary>
        /// 验证文件类型是否支持
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>如果支持则返回true</returns>
        bool IsFileTypeSupported(string fileName);
        
        /// <summary>
        /// 获取支持的文件类型列表
        /// </summary>
        /// <returns>支持的文件扩展名数组</returns>
        string[] GetSupportedFileTypes();
        
        /// <summary>
        /// 创建文档的新版本
        /// </summary>
        /// <param name="originalDocument">原始文档元数据</param>
        /// <param name="file">新版本文件</param>
        /// <param name="versionNote">版本修改说明</param>
        /// <returns>新版本的文档元数据</returns>
        Task<DocumentMetadata> CreateDocumentVersionAsync(DocumentMetadata originalDocument, IBrowserFile file, string versionNote);
        
        /// <summary>
        /// 获取文档的所有版本
        /// </summary>
        /// <param name="documentId">文档ID</param>
        /// <returns>包含所有版本的文档元数据列表</returns>
        Task<List<DocumentMetadata>> GetDocumentVersionsAsync(string documentId);
        
        /// <summary>
        /// 比较两个文档版本的差异
        /// </summary>
        /// <param name="originalDocumentId">原始文档ID</param>
        /// <param name="newDocumentId">新文档ID</param>
        /// <returns>差异内容的HTML表示</returns>
        Task<string> CompareDocumentVersionsAsync(string originalDocumentId, string newDocumentId);
        
        /// <summary>
        /// 还原到指定版本
        /// </summary>
        /// <param name="versionDocumentId">要还原的版本ID</param>
        /// <returns>还原后的文档元数据</returns>
        Task<DocumentMetadata> RestoreDocumentVersionAsync(string versionDocumentId);
    }
}