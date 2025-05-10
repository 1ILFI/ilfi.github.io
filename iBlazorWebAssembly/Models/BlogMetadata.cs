using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iBlazorWebAssembly.Models
{
    /// <summary>
    /// 博客元数据基类
    /// </summary>
    public class MetadataBase
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 博客文章元数据
    /// </summary>
    public class BlogPostMetadata : MetadataBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; } = string.Empty;
        
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; } = string.Empty;
        
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; } = string.Empty;
        
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();
        
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; } = string.Empty;
        
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublished { get; set; } = false;
        
        /// <summary>
        /// 访问计数
        /// </summary>
        public int ViewCount { get; set; } = 0;
    }

    /// <summary>
    /// 博客配置元数据
    /// </summary>
    public class BlogSettingsMetadata : MetadataBase
    {
        /// <summary>
        /// 博客名称
        /// </summary>
        public string BlogName { get; set; } = "我的博客";
        
        /// <summary>
        /// 博客描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// 作者名称
        /// </summary>
        public string AuthorName { get; set; } = string.Empty;
        
        /// <summary>
        /// 每页显示文章数
        /// </summary>
        public int PostsPerPage { get; set; } = 10;
        
        /// <summary>
        /// 是否显示摘要
        /// </summary>
        public bool ShowSummary { get; set; } = true;
        
        /// <summary>
        /// 主题
        /// </summary>
        public string Theme { get; set; } = "Default";
        
        /// <summary>
        /// 社交媒体链接
        /// </summary>
        public Dictionary<string, string> SocialLinks { get; set; } = new Dictionary<string, string>();
    }
}