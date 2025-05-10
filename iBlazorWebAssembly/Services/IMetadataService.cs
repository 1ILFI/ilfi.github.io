using iBlazorWebAssembly.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iBlazorWebAssembly.Services
{
    /// <summary>
    /// 元数据服务接口
    /// </summary>
    /// <typeparam name="T">元数据类型</typeparam>
    public interface IMetadataService<T> where T : MetadataBase
    {
        /// <summary>
        /// 获取所有元数据
        /// </summary>
        /// <returns>元数据列表</returns>
        Task<IEnumerable<T>> GetAllAsync();
        
        /// <summary>
        /// 根据ID获取元数据
        /// </summary>
        /// <param name="id">元数据ID</param>
        /// <returns>元数据对象，如果未找到则返回 null</returns>
        Task<T?> GetByIdAsync(string id);
        
        /// <summary>
        /// 添加元数据
        /// </summary>
        /// <param name="item">元数据对象</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync(T item);
        
        /// <summary>
        /// 更新元数据
        /// </summary>
        /// <param name="item">元数据对象</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync(T item);
        
        /// <summary>
        /// 删除元数据
        /// </summary>
        /// <param name="id">元数据ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync(string id);
    }
}