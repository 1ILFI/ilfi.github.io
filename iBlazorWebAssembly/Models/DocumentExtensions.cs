using Microsoft.FluentUI.AspNetCore.Components;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iBlazorWebAssembly.Models
{
    /// <summary>
    /// 文档相关的扩展方法
    /// </summary>
    public static class DocumentExtensions
    {
        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="bytes">文件大小（字节）</param>
        /// <returns>格式化后的文件大小</returns>
        public static string FormatFileSize(this long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int counter = 0;
            decimal number = bytes;
            
            while (Math.Round(number / 1024) >= 1 && counter < suffixes.Length - 1)
            {
                number /= 1024;
                counter++;
            }
            
            return $"{number:n1} {suffixes[counter]}";
        }
        
        /// <summary>
        /// 根据文件类型获取FluentUI图标
        /// </summary>
        /// <param name="fileType">文件类型扩展名</param>
        /// <returns>对应的FluentUI图标</returns>
        public static Icon GetIconForFileType(this string fileType)
        {
            if (string.IsNullOrEmpty(fileType))
                return new Icons.Regular.Size24.Document();
                
            return fileType.ToLowerInvariant() switch
            {
                ".docx" or ".doc" => new Icons.Regular.Size24.Document(),
                ".xlsx" or ".xls" => new Icons.Regular.Size24.Table(),
                ".pptx" or ".ppt" => new Icons.Regular.Size24.SlideText(),
                ".pdf" => new Icons.Regular.Size24.DocumentPdf(),
                ".md" => new Icons.Regular.Size24.DocumentText(),
                ".txt" => new Icons.Regular.Size24.TextAlignLeft(),
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => new Icons.Regular.Size24.Image(),
                ".mp4" or ".avi" or ".mov" => new Icons.Regular.Size24.Video(),
                ".mp3" or ".wav" or ".ogg" => new Icons.Regular.Size24.MusicNote1(),
                ".zip" or ".rar" or ".7z" => new Icons.Regular.Size24.Archive(),
                ".cs" or ".js" or ".ts" or ".html" or ".css" or ".json" => new Icons.Regular.Size24.Code(),
                _ => new Icons.Regular.Size24.Document()
            };
        }
        
        /// <summary>
        /// 获取文档类型的分类名称
        /// </summary>
        /// <param name="extension">文件扩展名</param>
        /// <returns>文件类型分类</returns>
        public static string GetFileTypeFolder(this string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return "other";
                
            return extension.ToLowerInvariant() switch
            {
                ".docx" or ".doc" => "word",
                ".xlsx" or ".xls" => "excel",
                ".pptx" or ".ppt" => "powerpoint",
                ".pdf" => "pdf",
                ".md" => "markdown",
                ".txt" => "text",
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => "images",
                ".mp4" or ".avi" or ".mov" => "videos",
                ".mp3" or ".wav" or ".ogg" => "audio",
                ".zip" or ".rar" or ".7z" => "archives",
                ".cs" or ".js" or ".ts" or ".html" or ".css" or ".json" => "code",
                _ => "other"
            };
        }
        
        /// <summary>
        /// 获取文件扩展名的MIME类型
        /// </summary>
        /// <param name="extension">文件扩展名</param>
        /// <returns>MIME类型</returns>
        public static string GetMimeType(this string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return "application/octet-stream";
                
            return extension.ToLowerInvariant() switch
            {
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".doc" => "application/msword",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".xls" => "application/vnd.ms-excel",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pdf" => "application/pdf",
                ".md" => "text/markdown",
                ".txt" => "text/plain",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".mp4" => "video/mp4",
                ".avi" => "video/x-msvideo",
                ".mov" => "video/quicktime",
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                ".ogg" => "audio/ogg",
                ".zip" => "application/zip",
                ".rar" => "application/vnd.rar",
                ".7z" => "application/x-7z-compressed",
                ".html" => "text/html",
                ".css" => "text/css",
                ".js" => "application/javascript",
                ".ts" => "application/typescript",
                ".json" => "application/json",
                ".cs" => "text/plain",
                _ => "application/octet-stream"
            };
        }
        
        /// <summary>
        /// 从元数据中提取关键词
        /// </summary>
        /// <param name="metadata">文档元数据</param>
        /// <returns>关键词列表</returns>
        public static IEnumerable<string> ExtractKeywords(this DocumentMetadata metadata)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            // 添加标题关键词
            if (!string.IsNullOrEmpty(metadata.Title))
            {
                foreach (var word in metadata.Title.Split(new[] { ' ', '.', ',', ';', ':', '(', ')', '[', ']', '{', '}', '-', '_', '/' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (word.Length >= 2) // 只添加2个字符以上的词
                    {
                        result.Add(word.ToLowerInvariant());
                    }
                }
            }
            
            // 添加所有标签
            if (metadata.Tags != null && metadata.Tags.Any())
            {
                foreach (var tag in metadata.Tags)
                {
                    result.Add(tag.ToLowerInvariant());
                }
            }
            
            // 添加分类作为关键词
            if (!string.IsNullOrEmpty(metadata.Category))
            {
                result.Add(metadata.Category.ToLowerInvariant());
            }
            
            // 添加文件类型
            if (!string.IsNullOrEmpty(metadata.FileType))
            {
                result.Add(metadata.FileType.TrimStart('.').ToLowerInvariant());
            }
            
            return result;
        }
    }
}