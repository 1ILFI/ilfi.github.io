using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace iBlazorWebAssembly.Services
{
    /// <summary>
    /// 文件操作JavaScript互操作服务
    /// </summary>
    public class FileJsInteropService
    {
        private readonly IJSRuntime _jsRuntime;

        public FileJsInteropService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// 打开文件选择对话框
        /// </summary>
        /// <param name="inputId">文件输入元素ID</param>
        public async Task OpenFileInputAsync(string inputId)
        {
            await _jsRuntime.InvokeVoidAsync("fileOperations.openFileInput", inputId);
        }

        /// <summary>
        /// 读取文件为字节数组
        /// </summary>
        /// <param name="fileReference">文件对象引用</param>
        /// <returns>字节数组</returns>
        public async Task<byte[]> ReadFileAsArrayBufferAsync(IJSObjectReference fileReference)
        {
            return await _jsRuntime.InvokeAsync<byte[]>("fileOperations.readFileAsArrayBuffer", fileReference);
        }

        /// <summary>
        /// 获取文件对象的引用
        /// </summary>
        /// <param name="inputId">文件输入元素ID</param>
        /// <param name="index">文件索引，默认为0</param>
        /// <returns>文件对象引用</returns>
        public async Task<IJSObjectReference> GetFileFromInputAsync(string inputId, int index = 0)
        {
            return await _jsRuntime.InvokeAsync<IJSObjectReference>("fileOperations.getFileFromInput", inputId, index);
        }

        /// <summary>
        /// 获取文件属性
        /// </summary>
        /// <param name="fileReference">文件对象引用</param>
        /// <returns>文件属性对象</returns>
        public async Task<FileProperties> GetFilePropertiesAsync(IJSObjectReference fileReference)
        {
            return await _jsRuntime.InvokeAsync<FileProperties>("fileOperations.getFileProperties", fileReference);
        }
        
        /// <summary>
        /// 下载指定URL的文件
        /// </summary>
        /// <param name="url">文件URL</param>
        /// <param name="fileName">文件名</param>
        public async Task DownloadFileAsync(string url, string fileName)
        {
            await _jsRuntime.InvokeVoidAsync("fileOperations.downloadFile", url, fileName);
        }

        /// <summary>
        /// 复制文本到剪贴板
        /// </summary>
        /// <param name="text">要复制的文本</param>
        /// <returns>是否复制成功</returns>
        public async Task<bool> CopyToClipboardAsync(string text)
        {
            return await _jsRuntime.InvokeAsync<bool>("fileOperations.copyToClipboard", text);
        }

        /// <summary>
        /// 获取拖放的文件
        /// </summary>
        /// <param name="e">拖放事件参数</param>
        /// <returns>文件属性数组</returns>
        public async Task<FileProperties[]> GetDroppedFilesAsync(Microsoft.AspNetCore.Components.Web.DragEventArgs e)
        {
            try
            {
                var dataTransferObj = await _jsRuntime.InvokeAsync<IJSObjectReference>("fileOperations.getDataTransferFromDropEvent", e);
                if (dataTransferObj == null) return Array.Empty<FileProperties>();
                
                var files = await _jsRuntime.InvokeAsync<FileProperties[]>("fileOperations.getFilePropertiesFromDataTransfer", dataTransferObj);
                return files;
            }
            catch (Exception)
            {
                return Array.Empty<FileProperties>();
            }
        }

        /// <summary>
        /// 读取拖放的文件内容
        /// </summary>
        /// <param name="file">文件属性</param>
        /// <returns>文件字节数组</returns>
        public async Task<byte[]> ReadDroppedFileAsync(FileProperties file)
        {
            try
            {
                // 通过文件属性获取文件内容
                return await _jsRuntime.InvokeAsync<byte[]>("fileOperations.readDroppedFileContent", file.Name);
            }
            catch (Exception)
            {
                return Array.Empty<byte>();
            }
        }
    }

    /// <summary>
    /// 文件属性模型
    /// </summary>
    public class FileProperties
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件MIME类型
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// 最后修改时间戳
        /// </summary>
        public long LastModified { get; set; }
    }
}