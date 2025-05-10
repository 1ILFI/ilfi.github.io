# Label

`FluentLabel` is a component that can be used to easily insert a small formatted piece of text into your application. You choose a type of element from the `Typography` enum and the necessary styling will be applied automatically.

## Overview
## Examples

### Label Types
Razor
Colors
The FluentLabel component can be styled with different colors. You can use one of the predefinded colors from the Color enumeration, or provide your own color through the CustomColor parameter or Style parameter.
A 'Header' using Color.Warning
A 'Body' label using Color.Disabled

A 'Body' label using a custom color through the CustomColor parameter. Just specify a valid color string value.

A 'Body' label using a custom color through the Style parameter. In this case a valid CSS color specification needs to be provided.

When specifying both CustomColor and Style, the latter wins.

Download: 
Razor
Documentation
### Colors

The FluentLabel component can be styled with different colors. You can use one of the predefined colors from the `Color` enumeration, or provide your own color through the `CustomColor` parameter or `Style` parameter.

Examples:

- A 'Header' using `Color.Warning`
- A 'Body' label using `Color.Disabled`
- A 'Body' label using a custom color through the `CustomColor` parameter. Just specify a valid color string value.
- A 'Body' label using a custom color through the `Style` parameter. In this case a valid CSS color specification needs to be provided.

> **Note:** When specifying both `CustomColor` and `Style`, the latter wins.
Gets or sets the color of the label to a custom value.
Needs to be formatted as a valid CSS color value (HTML hex color string (#rrggbb or #rgb), CSS variable or named color).

## Documentation

### FluentLabel Class Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| Alignment | HorizontalAlignment? | | Gets or sets the text-align on the component. |
| ChildContent | RenderFragment? | | Gets or sets the child content of component. |
| Color | Color? | | Gets or sets the color of the component. It supports the theme colors. |
| CustomColor | string? | | Gets or sets the color of the label to a custom value.<br>Needs to be formatted as a valid CSS color value (HTML hex color string (#rrggbb or #rgb), CSS variable or named color).<br>⚠️ Only available when Color is set to Color.Custom. |
| Disabled | bool | False | Activates or deactivates the component (changes the color). |
| MarginBlock | string? | | Gets or sets the margin block of the component.<br>'default' to use the margin-block predefined by browser.<br>If not set, the MarginBlock will be 0px. |
| Typo | Typography | Body | Applies the theme typography styles. |
| Weight | FontWeight | Normal | Gets or sets the font weight of the component:<br>Normal (400), Bold (600) or Bolder (800). |