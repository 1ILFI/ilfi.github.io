using System;
using Blazored.LocalStorage;
using iBlazorWebAssembly.Models;
using Microsoft.Extensions.DependencyInjection;

namespace iBlazorWebAssembly.Services
{
    /// <summary>
    /// 元数据服务工厂，用于创建和注册不同类型的元数据服务
    /// </summary>
    public static class MetadataServiceFactory
    {
        /// <summary>
        /// 添加所有元数据服务到依赖注入容器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddAllMetadataServices(this IServiceCollection services)
        {
            // 确保已添加LocalStorage服务
            services.AddBlazoredLocalStorage();
            
            // 注册各类元数据服务
            services.AddScoped<IMetadataService<BlogPostMetadata>, LocalStorageMetadataService<BlogPostMetadata>>();
            services.AddScoped<IMetadataService<BlogSettingsMetadata>, LocalStorageMetadataService<BlogSettingsMetadata>>();
            services.AddScoped<IMetadataService<DocumentMetadata>, LocalStorageMetadataService<DocumentMetadata>>();
            
            return services;
        }
        
        /// <summary>
        /// 注册泛型元数据服务
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddMetadataService<T>(this IServiceCollection services) where T : MetadataBase
        {
            services.AddScoped<IMetadataService<T>, LocalStorageMetadataService<T>>();
            return services;
        }
    }
}