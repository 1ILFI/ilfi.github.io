# Code Setup

## Getting Started

### Using our dotnet templates

The easiest way to get started is by using our Templates. These mimic the regular Blazor templates and come with the design and components pre-configured. You install them with this command:

dotnet add package Microsoft.FluentUI.AspNetCore.Components
Copy
If you want to extend the functionality of the library with icons or emoji, you can install additional packages for that:

dotnet add package Microsoft.FluentUI.AspNetCore.Components.Icons
dotnet add package Microsoft.FluentUI.AspNetCore.Components.Emoji
Copy
Script
We wrap the Fluent UI Web Components, which are implemented in a script file, for quite a few of our components. This file is included in the library itself and does not have to be downloaded or pulled from a CDN.

By including the script in the library we can safeguard that you are always using the best matching script version.

<link href="{PROJECT_NAME}.styles.css" rel="stylesheet" />
Copy
It is possible that the line is already there (but commented out).

IMPORTANT: When you change the root namespace/assembly name of your project, you need to update the in your code accordingly.

You can always add your own styles, using the or attribute on the components. By default, the classes are organised and checked by the component itself (in particular by checking that the class names are valid). Some frameworks, such as Tailwind CSS, add exceptions to class names (e.g. or ). In this case, you need to disable class name validation by adding this code to your file:classstylemin-h-[16px]bg-[#ff0000]Program.cs

builder.Services.AddFluentUIComponents(options =>
{
    options.ValidateClassNames = false;
});
In some cases, like when using .NET 8's new SSR (Static Server Rendering) rendermode, it might be necessary to include our library script in your App.razor manually. You can do so as follows:

builder.Services.AddFluentUIComponents();
Copy
If you're running your application on Blazor Server, make sure a default is registered before the method.HttpClientAddFluentUIComponents

builder.Services.AddHttpClient();
Copy
Add Component Providers
Add the following components at the end of your file. These providers are used by associated services to display Toasts, Dialog boxes, Tooltips or Message Bars correctly.MainLayout.razor

<FluentToastProvider />
<FluentDialogProvider />
<FluentTooltipProvider />
<FluentMessageBarProvider />
**IMPORTANT**: When you change the root namespace/assembly name of your project, you need to update the {PROJECT_NAME} in your code accordingly.

You can always add your own styles, using the `class` or `style` attribute on the components. By default, the classes are organised and checked by the component itself (in particular by checking that the class names are valid). Some frameworks, such as Tailwind CSS, add exceptions to class names (e.g. `min-h-[16px]` or `bg-[#ff0000]`). In this case, you need to disable class name validation by adding this code to your Program.cs file:

<FluentCard>
  <h2>Hello World!</h2>
  <FluentButton Appearance="@Appearance.Accent">Click Me</FluentButton>
</FluentCard>
Copy
Configuring the Design System
The Fluent UI library components are built on FAST's (Adaptive UI) technology, which enables design customization and personalization, while automatically maintaining accessibility. This is accomplished through setting various "design tokens". The library exposes all design tokens, which you can use both from code as in a declarative way in your pages. The different ways of working with design tokens are described in the design tokens page..razor

For Right-To-Left languages
One of the most common design tokens is the design token. It is required to make the application components visually configured for the right-to-left languages. In order to implement such configuration you need to use the design token together with the component in the main layout of your Right-to-Left pages:DirectionDirectionFluentDesignTheme

@* MainRtlLayout.razor *@

@using Microsoft.FluentUI.AspNetCore.Components.DesignTokens
When creating a site that is hosted in a different base path, it might be necessary to remove the leading '/' from the stylesheet link.

## Register Services

Add the following in Program.cs:

Temporary workaround for MAUI/WPF/Windows Forms issues
Currently when using the WebView to run Blazor (so all Hybrid variants) the web-components script is not imported automatically (see #404). There is also an issue with loading the custom event handlers that are being configured by the web-components script. Until these are fixed on the WebView side, there is a workaround available, namely to intercept and provide proper JS initializers file (created by build). The needed has been added to the library and needs to be included with a script tag before the script tag:'_framework/blazor.modules.json'initializersLoader.webview.js_framework/blazor.webview.js

<script app-name="{NAME OF YOUR APP}" src="./_content/Microsoft.FluentUI.AspNetCore.Components/js/initializersLoader.webview.js"></script>
<script src="_framework/blazor.webview.js"></script>
Copy
The attribute needs to match your app's assembly name - initializersLoader uses 'app-name' to resolve name of the file with initializers. initializersLoader replaces standard function with one which provides the correct file in place of the empty . is restored to its original state once request is intercepted.app-namefetchblazor.modules.jsonfetch_framework/blazor.modules.json

For more information regarding the bug, see issue 15234 in the MAUI repo.

Use the DataGrid component with EF Core
If you want to use the with data provided through EF Core, you need to install an additional package so the grid knows how to resolve queries asynchronously for efficiency. .<FluentDataGrid>

Installation
Install the package by running the command:

## Working with Icons and Emoji

We have additional packages available that include the complete Fluent UI System icons and Fluent UI Emoji collections. Please refer to the Icons and Emoji page for more information.

## Usage

With the package installed, you can begin using the Fluent UI Razor components in the same way as any other Razor component.

### Add Imports

After the package is added, you need to add the following in your _Imports.razor:
### For Right-To-Left languages

One of the most common design tokens is the Direction design token. It is required to make the application components visually configured for the right-to-left languages. In order to implement such configuration you need to use the Direction design token together with the FluentDesignTheme component in the main layout of your Right-to-Left pages:
### Temporary workaround for MAUI/WPF/Windows Forms issues

Currently when using the WebView to run Blazor (so all Hybrid variants) the web-components script is not imported automatically (see #404). There is also an issue with loading the custom event handlers that are being configured by the web-components script. Until these are fixed on the WebView side, there is a workaround available, namely to intercept '_framework/blazor.modules.json' and provide proper JS initializers file (created by build). The needed initializersLoader.webview.js has been added to the library and needs to be included with a script tag before the _framework/blazor.webview.js script tag:
### Installation

Install the package by running the command: