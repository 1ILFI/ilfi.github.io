using Blazored.LocalStorage;
using iBlazorWebAssembly.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace iBlazorWebAssembly.Services
{
    /// <summary>
    /// 基于 LocalStorage 的元数据服务实现
    /// </summary>
    /// <typeparam name="T">元数据类型</typeparam>
    public class LocalStorageMetadataService<T> : IMetadataService<T> where T : MetadataBase
    {
        private readonly ILocalStorageService _localStorage;
        private readonly string _storageKey;
        private readonly ILogger<LocalStorageMetadataService<T>>? _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="localStorage">LocalStorage 服务</param>
        /// <param name="logger">日志服务（可选）</param>
        public LocalStorageMetadataService(
            ILocalStorageService localStorage, 
            ILogger<LocalStorageMetadataService<T>>? logger = null)
        {
            _localStorage = localStorage;
            _logger = logger;
            _storageKey = $"{typeof(T).Name}_Collection";
        }

        /// <summary>
        /// 获取所有元数据
        /// </summary>
        /// <returns>元数据列表</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                if (await _localStorage.ContainKeyAsync(_storageKey))
                {
                    var items = await _localStorage.GetItemAsync<List<T>>(_storageKey);
                    return items ?? new List<T>();
                }
                
                _logger?.LogInformation("存储键 {StorageKey} 不存在，返回空列表", _storageKey);
                return new List<T>();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从LocalStorage获取{TypeName}集合时出错", typeof(T).Name);
                return new List<T>();
            }
        }

        /// <summary>
        /// 根据ID获取元数据
        /// </summary>
        /// <param name="id">元数据ID</param>
        /// <returns>元数据对象，如果未找到则返回 null</returns>
        public async Task<T?> GetByIdAsync(string id)
        {
            try
            {
                var items = await GetAllAsync();
                var item = items.FirstOrDefault(i => i.Id == id);
                
                if (item == null)
                {
                    _logger?.LogWarning("找不到ID为 {Id} 的 {TypeName} 项", id, typeof(T).Name);
                }
                
                return item;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "查找ID为 {Id} 的 {TypeName} 项时出错", id, typeof(T).Name);
                return null;
            }
        }

        /// <summary>
        /// 添加元数据
        /// </summary>
        /// <param name="item">元数据对象</param>
        /// <returns>添加结果</returns>
        public async Task<bool> AddAsync(T item)
        {
            if (item == null)
            {
                _logger?.LogWarning("尝试添加空的 {TypeName} 项", typeof(T).Name);
                return false;
            }
            
            try
            {
                var items = (await GetAllAsync()).ToList();
                
                // 确保 ID 是唯一的
                if (string.IsNullOrEmpty(item.Id))
                {
                    item.Id = Guid.NewGuid().ToString();
                    _logger?.LogInformation("为 {TypeName} 项生成了新的ID: {Id}", typeof(T).Name, item.Id);
                }
                else if (items.Any(i => i.Id == item.Id))
                {
                    _logger?.LogWarning("ID为 {Id} 的 {TypeName} 项已存在", item.Id, typeof(T).Name);
                    return false;
                }
                
                // 设置创建和更新时间
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = DateTime.Now;
                
                items.Add(item);
                await _localStorage.SetItemAsync(_storageKey, items);
                
                _logger?.LogInformation("成功添加ID为 {Id} 的 {TypeName} 项", item.Id, typeof(T).Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "添加 {TypeName} 项时出错", typeof(T).Name);
                return false;
            }
        }

        /// <summary>
        /// 更新元数据
        /// </summary>
        /// <param name="item">元数据对象</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateAsync(T item)
        {
            if (item == null)
            {
                _logger?.LogWarning("尝试更新空的 {TypeName} 项", typeof(T).Name);
                return false;
            }
            
            if (string.IsNullOrEmpty(item.Id))
            {
                _logger?.LogWarning("尝试更新没有ID的 {TypeName} 项", typeof(T).Name);
                return false;
            }
            
            try
            {
                var items = (await GetAllAsync()).ToList();
                var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
                
                if (existingItem == null)
                {
                    _logger?.LogWarning("找不到要更新的ID为 {Id} 的 {TypeName} 项", item.Id, typeof(T).Name);
                    return false;
                }
                
                // 保留创建时间，更新修改时间
                item.CreatedAt = existingItem.CreatedAt;
                item.UpdatedAt = DateTime.Now;
                
                items.Remove(existingItem);
                items.Add(item);
                await _localStorage.SetItemAsync(_storageKey, items);
                
                _logger?.LogInformation("成功更新ID为 {Id} 的 {TypeName} 项", item.Id, typeof(T).Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新ID为 {Id} 的 {TypeName} 项时出错", item?.Id, typeof(T).Name);
                return false;
            }
        }

        /// <summary>
        /// 删除元数据
        /// </summary>
        /// <param name="id">元数据ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger?.LogWarning("尝试删除ID为空的 {TypeName} 项", typeof(T).Name);
                return false;
            }
            
            try
            {
                var items = (await GetAllAsync()).ToList();
                var existingItem = items.FirstOrDefault(i => i.Id == id);
                
                if (existingItem == null)
                {
                    _logger?.LogWarning("找不到要删除的ID为 {Id} 的 {TypeName} 项", id, typeof(T).Name);
                    return false;
                }
                
                items.Remove(existingItem);
                await _localStorage.SetItemAsync(_storageKey, items);
                
                _logger?.LogInformation("成功删除ID为 {Id} 的 {TypeName} 项", id, typeof(T).Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "删除ID为 {Id} 的 {TypeName} 项时出错", id, typeof(T).Name);
                return false;
            }
        }
    }
}