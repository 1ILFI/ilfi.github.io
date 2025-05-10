/**
 * 博客颜色管理系统
 * 提供颜色主题的动态更改、自定义颜色方案应用等功能
 */

// 将十六进制颜色转换为HSL（色相、饱和度、亮度）
function hexToHSL(hex) {
    // 移除#前缀（如果存在）
    hex = hex.replace(/^#/, '');
    
    // 将十六进制转换为RGB
    let r = 0, g = 0, b = 0;
    if (hex.length === 3) {
        r = parseInt(hex[0] + hex[0], 16) / 255;
        g = parseInt(hex[1] + hex[1], 16) / 255;
        b = parseInt(hex[2] + hex[2], 16) / 255;
    } else if (hex.length === 6) {
        r = parseInt(hex.substring(0, 2), 16) / 255;
        g = parseInt(hex.substring(2, 4), 16) / 255;
        b = parseInt(hex.substring(4, 6), 16) / 255;
    }
    
    // 计算HSL
    const max = Math.max(r, g, b);
    const min = Math.min(r, g, b);
    let h = 0, s = 0, l = (max + min) / 2;
    
    if (max !== min) {
        const d = max - min;
        s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
        switch (max) {
            case r: h = (g - b) / d + (g < b ? 6 : 0); break;
            case g: h = (b - r) / d + 2; break;
            case b: h = (r - g) / d + 4; break;
        }
        h /= 6;
    }
    
    h = Math.round(h * 360);
    s = Math.round(s * 100);
    l = Math.round(l * 100);
    
    return { h, s, l };
}

// 将HSL转换为十六进制颜色
function hslToHex(h, s, l) {
    h /= 360;
    s /= 100;
    l /= 100;
    
    let r, g, b;
    
    if (s === 0) {
        r = g = b = l;
    } else {
        const hue2rgb = (p, q, t) => {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1/6) return p + (q - p) * 6 * t;
            if (t < 1/2) return q;
            if (t < 2/3) return p + (q - p) * (2/3 - t) * 6;
            return p;
        };
        
        const q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        const p = 2 * l - q;
        
        r = hue2rgb(p, q, h + 1/3);
        g = hue2rgb(p, q, h);
        b = hue2rgb(p, q, h - 1/3);
    }
    
    const toHex = x => {
        const hex = Math.round(x * 255).toString(16);
        return hex.length === 1 ? '0' + hex : hex;
    };
    
    return `#${toHex(r)}${toHex(g)}${toHex(b)}`;
}

// 基于主色调生成调和的颜色方案
function generateHarmonizedColors(primaryHex, isDarkMode = false) {
    const primary = hexToHSL(primaryHex);
    
    // 生成次要颜色（互补色调整）
    const secondary = {
        h: (primary.h + 180) % 360,
        s: Math.max(primary.s - 10, 0),
        l: isDarkMode ? primary.l + 10 : primary.l - 10
    };
    
    // 生成第三颜色（三分色调整）
    const tertiary = {
        h: (primary.h + 120) % 360,
        s: Math.min(primary.s + 5, 100),
        l: isDarkMode ? Math.min(primary.l + 5, 90) : Math.max(primary.l - 5, 10)
    };
    
    // 中性色
    const neutral = {
        h: primary.h,
        s: Math.max(primary.s - 30, 0),
        l: isDarkMode ? 85 : 15
    };
    
    // 中性变体
    const neutralVariant = {
        h: primary.h,
        s: Math.max(primary.s - 20, 0),
        l: isDarkMode ? 75 : 25
    };
    
    return {
        primary: primaryHex,
        secondary: hslToHex(secondary.h, secondary.s, secondary.l),
        tertiary: hslToHex(tertiary.h, tertiary.s, tertiary.l),
        neutral: hslToHex(neutral.h, neutral.s, neutral.l),
        neutralVariant: hslToHex(neutralVariant.h, neutralVariant.s, neutralVariant.l)
    };
}

// 生成色调颜色变体（用于Fluent UI自定义主题）
function generateTones(baseHex, isDarkMode = false) {
    const base = hexToHSL(baseHex);
    const tones = {};
    
    // 生成不同亮度级别的色调
    const levels = [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 100];
    levels.forEach(level => {
        tones[level] = hslToHex(base.h, base.s, level);
    });
    
    return tones;
}

// 应用自定义主色调
window.applyCustomPrimaryColor = function(primaryColorHex, isDarkMode = null) {
    // 检查是否提供了isDarkMode参数，否则自动检测
    if (isDarkMode === null) {
        isDarkMode = document.querySelector('[data-fluent-theme="dark"]') !== null ||
                    window.matchMedia('(prefers-color-scheme: dark)').matches;
    }
    
    // 生成调和的颜色方案
    const colorScheme = generateHarmonizedColors(primaryColorHex, isDarkMode);
    
    // 应用完整的颜色方案
    applyCustomColors(
        colorScheme.primary,
        colorScheme.secondary,
        colorScheme.tertiary,
        colorScheme.neutral,
        colorScheme.neutralVariant,
        isDarkMode
    );
};

// 应用完整的自定义颜色方案
window.applyCustomColors = function(primary, secondary, tertiary, neutral, neutralVariant, isDarkMode = null) {
    // 检查是否提供了isDarkMode参数，否则自动检测
    if (isDarkMode === null) {
        isDarkMode = document.querySelector('[data-fluent-theme="dark"]') !== null ||
                    window.matchMedia('(prefers-color-scheme: dark)').matches;
    }
    
    // 生成CSS变量并应用到文档根元素
    const root = document.documentElement;
    
    // 应用基础颜色
    root.style.setProperty('--blog-custom-primary', primary);
    root.style.setProperty('--blog-custom-secondary', secondary);
    root.style.setProperty('--blog-custom-tertiary', tertiary);
    root.style.setProperty('--blog-custom-neutral', neutral);
    root.style.setProperty('--blog-custom-neutral-variant', neutralVariant);
    
    // 修改Fluent UI的主题变量
    root.style.setProperty('--accent-base-color', primary);
    root.style.setProperty('--accent-fill-rest', primary);
    
    // 调整对比色
    const primaryHSL = hexToHSL(primary);
    const textColor = isDarkMode ? '#ffffff' : '#000000';
    const invertedTextColor = isDarkMode ? '#000000' : '#ffffff';
    
    // 设置文本对比色
    root.style.setProperty('--blog-accent-text', 
        primaryHSL.l < 50 ? invertedTextColor : textColor);
        
    // 保存配置到localStorage（可选）
    try {
        localStorage.setItem('blog_custom_colors', JSON.stringify({
            primary, secondary, tertiary, neutral, neutralVariant, isDarkMode
        }));
    } catch (e) {
        console.warn('无法将自定义颜色保存到localStorage', e);
    }
    
    console.log(`已应用自定义颜色方案: 主色调 ${primary}, 模式: ${isDarkMode ? '暗色' : '亮色'}`);
};

// 在文档加载后恢复保存的颜色方案（如果有）
document.addEventListener('DOMContentLoaded', () => {
    try {
        const savedColors = localStorage.getItem('blog_custom_colors');
        if (savedColors) {
            const colors = JSON.parse(savedColors);
            window.applyCustomColors(
                colors.primary,
                colors.secondary,
                colors.tertiary, 
                colors.neutral,
                colors.neutralVariant,
                colors.isDarkMode
            );
        }
    } catch (e) {
        console.warn('恢复保存的颜色方案时出错', e);
    }
});