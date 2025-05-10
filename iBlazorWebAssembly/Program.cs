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

// 注册应用程序配置
builder.Services.AddSingleton(provider => {
    // 创建应用程序配置实例
    var appSettings = new AppSettings();
    
    // 从Configuration中加载配置项（如果存在）
    var config = provider.GetService<IConfiguration>();
    config?.GetSection("AppSettings")?.Bind(appSettings);
    
    return appSettings;
});

// 使用工厂模式注册所有元数据服务
builder.Services.AddAllMetadataServices();

// 注册文档系统服务
builder.Services.AddScoped<IGitHubFileService, GitHubFileService>();
builder.Services.AddScoped<IDocumentProcessingService, DocumentProcessingService>();

// 注册管理后台身份认证服务
builder.Services.AddScoped<AdminAuthService>();

// 注册主题服务
builder.Services.AddScoped<ThemeService>();

// 注册文件操作JS互操作服务
builder.Services.AddScoped<FileJsInteropService>();

// 添加配置服务
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

await builder.Build().RunAsync();
