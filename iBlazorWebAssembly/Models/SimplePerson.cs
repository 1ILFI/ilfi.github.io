using System;

namespace iBlazorWebAssembly.Models
{
    /// <summary>
    /// 用于 FluentUI Dialog 示例的 SimplePerson 类
    /// </summary>
    public class SimplePerson
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Firstname { get; set; } = string.Empty;
        
        /// <summary>
        /// 姓氏
        /// </summary>
        public string Lastname { get; set; } = string.Empty;
        
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }
}