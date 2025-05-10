---
applyTo: "**"
---

# 项目开发标准

## 工作区介绍和方案

### 介绍

- 这是一个私人博客网站
- 不使用 Azure 任何订阅服务包括免费订阅，但可以使用默认的免费功能

### 主要架构

- 静态部署于 Github Pages
- 后端基于 ASP.NET Core Web API
- 前端基于 Blazor WebAssembly

### UI系统

- UI 必须优先使用 Fluent UI Blazor
- 主题系统和颜色系统结合 CSS 完成

### 文档系统

- 元数据存储使用 Blazored.LocalStorage , OData 作为备用
- 文档解析库使用 DocumentFormat.OpenXml 和微软 Markitdown
- 文件托管使用 GitHub API 访问令牌 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 , 存储库 1IFLI/ilfi.github.io , 账户名 1ILFI 邮箱 <ILeroyFioriI@outlook.com>
- 使用 FluentUI Blazor 的上传组件


## 编码指南

### ASP.NET 指南

请参考 [ASP.NET Core 文档](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-9.0)

### CSS 参考指南

请参考 [MDN CSS 文档](https://developer.mozilla.org/zh-CN/docs/Web/CSS)
### Fluent UI Blazor 指南

请参考 [Fluent UI Blazor 官方网站](https://www.fluentui-blazor.net)

- Fluent UI Blazor 代码设置说明文件是 C:\Users\ILFI\ibolg\docs\FluentUI\codesetup.md
- Fluent UI Blazor label 组件说明文件是 C:\Users\ILFI\ibolg\docs\FluentUI\lable.md
- Fluent UI Blazor 的命名空间引用必须且仅只有三个，分别是:
  - `@using Microsoft.FluentUI.AspNetCore.Components`
  - `@using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons`
  - `@using Emojis = Microsoft.FluentUI.AspNetCore.Components.Emojis`
- Fluent UI Icons 必须参考 C:\Users\ILFI\ibolg\docs\FluentUI\Icons 目录的所有示例文件及说明文件
- 当 Fluent UI Blazor Icons 无法识别定义时通常是用了错误的定义，需查找官方图标浏览器，如无法确定，请人工干预
- dialog 组件必须参考 C:\Users\ILFI\ibolg\docs\FluentUI\Dialog 目录的所有示例文件及说明文件
- button 组件必须参考 C:\Users\ILFI\ibolg\docs\FluentUI\button 目录的所有示例文件及说明文件

### 代码示例

#### Fluent UI 卡片区域解除受限

```razor
<FluentCard AreaRestricted="false">
    <!-- 内容 -->
</FluentCard>
```

### CSS 编写指南

- 在 Razor 文件的 CSS 中使用 @media 查询时，需要在 @media 前加上额外的 @ 符号，例如：

```css
@@media (max-width: 768px) {
    .element {
        width: 100%;
    }
}
```

## 用于开发的重要提示

- Blazor 项目中 .razor 格式文件的全局命名空间导入都应该在 _Imports.razor 文件中进行配置，各个 Razor 组件文件不应该单独添加命名空间引用
- 必须优先参考 Fluent UI 相关新版实例文件 , 位于 .me/FluentUI 目录 , 不许使用 FluentUI 过时标准
