using System;
using System.Collections.Generic;

namespace iBlazorWebAssembly.Models
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// GitHub相关配置
        /// </summary>
        public GitHubSettings GitHub { get; set; } = new GitHubSettings();
        
        /// <summary>
        /// 博客设置
        /// </summary>
        public BlogSettings Blog { get; set; } = new BlogSettings();
        
        /// <summary>
        /// 文档设置
        /// </summary>
        public DocumentSettings Document { get; set; } = new DocumentSettings();
    }
    
    /// <summary>
    /// GitHub相关配置
    /// </summary>
    public class GitHubSettings
    {
        /// <summary>
        /// 仓库所有者
        /// </summary>
        public string RepoOwner { get; set; } = "1ILFI";
        
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string RepoName { get; set; } = "ilfi.github.io";
        
        /// <summary>
        /// 分支名称
        /// </summary>
        public string Branch { get; set; } = "main";
        
        /// <summary>
        /// 访问令牌在LocalStorage中的键名
        /// </summary>
        public string TokenKey { get; set; } = "github_personal_access_token";
    }
    
    /// <summary>
    /// 博客设置
    /// </summary>
    public class BlogSettings
    {
        /// <summary>
        /// 网站标题
        /// </summary>
        public string SiteTitle { get; set; } = "个人博客";
        
        /// <summary>
        /// 网站描述
        /// </summary>
        public string SiteDescription { get; set; } = "基于Blazor WebAssembly的个人博客";
        
        /// <summary>
        /// 每页显示的文章数量
        /// </summary>
        public int PostsPerPage { get; set; } = 10;
        
        /// <summary>
        /// 默认作者名称
        /// </summary>
        public string DefaultAuthor { get; set; } = "博客作者";
        
        /// <summary>
        /// 默认主题
        /// </summary>
        public string DefaultTheme { get; set; } = "Light";
        
        /// <summary>
        /// 社交媒体链接
        /// </summary>
        public Dictionary<string, string> SocialLinks { get; set; } = new Dictionary<string, string>();
    }
    
    /// <summary>
    /// 文档设置
    /// </summary>
    public class DocumentSettings
    {
        /// <summary>
        /// 支持的文件类型
        /// </summary>
        public string[] SupportedFileTypes { get; set; } = { ".docx", ".xlsx", ".pptx", ".pdf", ".md", ".txt" };
        
        /// <summary>
        /// 文件大小限制(MB)
        /// </summary>
        public int MaxFileSizeMB { get; set; } = 10;
        
        /// <summary>
        /// 默认分类
        /// </summary>
        public string[] Categories { get; set; } = { 
            "技术文档",
            "学习资源",
            "项目文档",
            "教程",
            "参考手册",
            "其他"
        };
    }
}