using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace iBlazorWebAssembly.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;

        private const string STORAGE_KEY = "blog_theme_settings";
        private const string COLOR_STORAGE_KEY = "blog_color_scheme";
        
        // 初始化事件为空事件处理器
        public event Action? OnThemeChanged;
        
        public ThemeService(
            IJSRuntime jsRuntime, 
            ILocalStorageService localStorage,
            NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
        }

        public class ThemeSettings
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public DesignThemeModes Mode { get; set; } = DesignThemeModes.System;
            
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public OfficeColor? BrandColor { get; set; } = OfficeColor.Default;
            
            // 新增：自定义颜色
            public string? CustomColorHex { get; set; }
            
            // 新增：自定义颜色方案
            public ColorScheme? ColorScheme { get; set; }
        }
        
        public class ColorScheme
        {
            public string Primary { get; set; } = string.Empty;
            public string Secondary { get; set; } = string.Empty;
            public string Tertiary { get; set; } = string.Empty;
            public string Neutral { get; set; } = string.Empty;
            public string NeutralVariant { get; set; } = string.Empty;
        }

        public ThemeSettings CurrentTheme { get; private set; } = new ThemeSettings();
        
        public async Task InitializeThemeAsync()
        {
            try
            {
                var savedSettings = await _localStorage.GetItemAsync<ThemeSettings>(STORAGE_KEY);
                if (savedSettings != null)
                {
                    CurrentTheme = savedSettings;
                }
                else
                {
                    // 如果没有保存的设置，检测系统主题偏好
                    bool prefersDark = await _jsRuntime.InvokeAsync<bool>(
                        "window.matchMedia('(prefers-color-scheme: dark)').matches");
                    CurrentTheme.Mode = DesignThemeModes.System;
                }
                
                // 如果有自定义颜色方案，应用它
                await ApplyCustomColorsIfNeededAsync();
            }
            catch
            {
                // 如果出现任何错误，使用默认设置
                CurrentTheme = new ThemeSettings();
            }
            
            NotifyThemeChanged();
        }
        
        // 应用自定义颜色
        private async Task ApplyCustomColorsIfNeededAsync()
        {
            try
            {
                if (CurrentTheme.ColorScheme != null)
                {
                    await _jsRuntime.InvokeVoidAsync(
                        "applyCustomColors", 
                        CurrentTheme.ColorScheme.Primary,
                        CurrentTheme.ColorScheme.Secondary,
                        CurrentTheme.ColorScheme.Tertiary,
                        CurrentTheme.ColorScheme.Neutral,
                        CurrentTheme.ColorScheme.NeutralVariant,
                        CurrentTheme.Mode.ToString().ToLower() == "dark");
                }
                else if (!string.IsNullOrEmpty(CurrentTheme.CustomColorHex))
                {
                    // 如果只有单个自定义颜色，也应用它
                    await _jsRuntime.InvokeVoidAsync("applyCustomPrimaryColor", CurrentTheme.CustomColorHex);
                }
            }
            catch
            {
                // 忽略任何失败，以防JS函数不存在
            }
        }

        public async Task SetThemeModeAsync(DesignThemeModes mode)
        {
            CurrentTheme.Mode = mode;
            await SaveThemeSettingsAsync();
        }

        public async Task SetBrandColorAsync(OfficeColor? color)
        {
            CurrentTheme.BrandColor = color;
            CurrentTheme.CustomColorHex = null; // 清除自定义色
            CurrentTheme.ColorScheme = null; // 清除自定义方案
            await SaveThemeSettingsAsync();
        }
        
        public async Task SetCustomColorAsync(string hexColor)
        {
            CurrentTheme.CustomColorHex = hexColor;
            CurrentTheme.BrandColor = null; // 清除品牌色
            CurrentTheme.ColorScheme = null; // 清除自定义方案
            await SaveThemeSettingsAsync();
        }
        
        public async Task SetColorSchemeAsync(ColorScheme scheme)
        {
            CurrentTheme.ColorScheme = scheme;
            CurrentTheme.CustomColorHex = null; // 清除单个自定义色
            CurrentTheme.BrandColor = null; // 清除品牌色
            await SaveThemeSettingsAsync();
        }
        
        // 获取当前颜色是否为暗色系
        public async Task<bool> IsDarkModeAsync()
        {
            if (CurrentTheme.Mode == DesignThemeModes.Dark)
                return true;
            
            if (CurrentTheme.Mode == DesignThemeModes.Light)
                return false;
            
            // 系统模式下检测系统偏好
            try
            {
                return await _jsRuntime.InvokeAsync<bool>(
                    "window.matchMedia('(prefers-color-scheme: dark)').matches");
            }
            catch
            {
                return false; // 默认亮色模式
            }
        }
        
        private async Task SaveThemeSettingsAsync()
        {
            await _localStorage.SetItemAsync(STORAGE_KEY, CurrentTheme);
            await ApplyCustomColorsIfNeededAsync();
            NotifyThemeChanged();
        }
        
        private void NotifyThemeChanged()
        {
            OnThemeChanged?.Invoke();
        }
    }
}