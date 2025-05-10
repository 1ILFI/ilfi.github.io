using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using iBlazorWebAssembly;
using Blazored.LocalStorage;
using iBlazorWebAssembly.Services;
using iBlazorWebAssembly.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
// 使用完全限定的命名空间引用 App 组件
builder.RootComponents.Add<iBlazorWebAssembly.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluentUIComponents();
builder.Services.AddBlazoredLocalStorage(); // 添加 LocalStorage 服务

// 注册元数据服务
builder.Services.AddScoped<IMetadataService<BlogPostMetadata>, LocalStorageMetadataService<BlogPostMetadata>>();
builder.Services.AddScoped<IMetadataService<BlogSettingsMetadata>, LocalStorageMetadataService<BlogSettingsMetadata>>();

// 注册管理后台身份认证服务
builder.Services.AddScoped<AdminAuthService>();

await builder.Build().RunAsync();
