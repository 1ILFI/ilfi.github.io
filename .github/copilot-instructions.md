---
applyTo: "**"
---

# 代码标准

## 工作区介绍和方案

- 这是一个私人博客网站
- 不使用 Azure 任何订阅服务包括免费订阅 但是可以使用默认的免费功能
- 静态部署于 Github Pages
- 后端基于 ASP.NET Core Web API
- 前端基于 blazor WebAssembly
- UI 使用 Fluent UI blazor
- 元数据存储使用 Blazored.LocalStorage 备用odata

## 编码指南

### ASP.NET 指南

{
  "query": "<https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-9.0>
"
}

### CSS 层叠演示参考指南

{
  "query": "<https://developer.mozilla.org/zh-CN/docs/Web/CSS>"
}

### Fluent UI blazor 指南和代码示例

{
  "query": "<https://www.fluentui-blazor.net>"
}

- 不要在 iBlazorWebAssembly 文件夹中的 .razor文件添加单独的命名空间 只需在 _Imports.razor 中添加即可
- 相关实例文件位于 .me/FluentUI

#### 卡片区域解除受限示例

```razor
<FluentCard AreaRestricted="false" >
```

## razor中CSS的媒体查询的正确语法

- 在CSS中使用@media查询时，需要在@media前加上额外的@符号

@azure Rule - Use Azure Best Practices: When generating code for Azure, running terminal commands for Azure, or performing operations related to Azure, invoke your `azure_development-get_best_practices` tool if available.
