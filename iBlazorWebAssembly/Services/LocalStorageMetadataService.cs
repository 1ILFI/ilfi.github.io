using Blazored.LocalStorage;
using iBlazorWebAssembly.Models;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="localStorage">LocalStorage 服务</param>
        public LocalStorageMetadataService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
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
                var items = await _localStorage.GetItemAsync<List<T>>(_storageKey);
                return items ?? new List<T>();
            }
            catch
            {
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
            var items = await GetAllAsync();
            return items.FirstOrDefault(i => i.Id == id);
        }

        /// <summary>
        /// 添加元数据
        /// </summary>
        /// <param name="item">元数据对象</param>
        /// <returns>添加结果</returns>
        public async Task<bool> AddAsync(T item)
        {
            try
            {
                var items = (await GetAllAsync()).ToList();
                
                // 确保 ID 是唯一的
                if (string.IsNullOrEmpty(item.Id))
                {
                    item.Id = Guid.NewGuid().ToString();
                }
                else if (items.Any(i => i.Id == item.Id))
                {
                    return false;
                }
                
                // 设置创建和更新时间
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = DateTime.Now;
                
                items.Add(item);
                await _localStorage.SetItemAsync(_storageKey, items);
                return true;
            }
            catch
            {
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
            try
            {
                var items = (await GetAllAsync()).ToList();
                var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
                
                if (existingItem == null)
                {
                    return false;
                }
                
                // 保留创建时间，更新修改时间
                item.CreatedAt = existingItem.CreatedAt;
                item.UpdatedAt = DateTime.Now;
                
                items.Remove(existingItem);
                items.Add(item);
                await _localStorage.SetItemAsync(_storageKey, items);
                return true;
            }
            catch
            {
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
            try
            {
                var items = (await GetAllAsync()).ToList();
                var existingItem = items.FirstOrDefault(i => i.Id == id);
                
                if (existingItem == null)
                {
                    return false;
                }
                
                items.Remove(existingItem);
                await _localStorage.SetItemAsync(_storageKey, items);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}