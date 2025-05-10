using System;
using System.Collections.Generic;

namespace iBlazorWebAssembly.Models
{
    /// <summary>
    /// 文档元数据模型，用于存储文档相关信息
    /// </summary>
    public class DocumentMetadata : MetadataBase
    {
        /// <summary>
        /// 文档标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// 原始文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; } = string.Empty;
        
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// 文件描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// 文档标签
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();
        
        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// GitHub上的文件URL
        /// </summary>
        public string GitHubUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// 文档是否公开
        /// </summary>
        public bool IsPublic { get; set; } = true;
        
        /// <summary>
        /// 文档内容的Base64编码（用于小型文档的临时存储）
        /// </summary>
        public string? ContentBase64 { get; set; }
        
        /// <summary>
        /// 文档分类
        /// </summary>
        public string Category { get; set; } = string.Empty;
        
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; } = string.Empty;
        
        /// <summary>
        /// 是否已转换为HTML
        /// </summary>
        public bool HasHtmlVersion { get; set; }
        
        /// <summary>
        /// HTML版本URL（如果已转换）
        /// </summary>
        public string? HtmlUrl { get; set; }
        
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; } = "1.0";
        
        /// <summary>
        /// 历史版本ID列表，用于追踪文档的所有历史版本
        /// </summary>
        public List<string> VersionHistory { get; set; } = new List<string>();
        
        /// <summary>
        /// 是否是某文档的历史版本
        /// </summary>
        public bool IsHistoryVersion { get; set; } = false;
        
        /// <summary>
        /// 如果是历史版本，指向主版本文档的ID
        /// </summary>
        public string? ParentDocumentId { get; set; }
        
        /// <summary>
        /// 版本修改说明
        /// </summary>
        public string VersionNote { get; set; } = string.Empty;
    }
}