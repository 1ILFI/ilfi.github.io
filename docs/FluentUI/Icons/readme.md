Icon
The Fluent UI System Icons are a (still growing) collection of familiar, friendly and modern icons from Microsoft. At the moment there are more than 2200 distinct icons available in both filled and outlined versions and in various sizes. In total the collections consists of well over 11k icons in SVG format.

This FluentUI Icons NuGet package contains all these icons, which you can access directly in your projects. To use them, simply reference this package in your project.

Important
As of version 4.11.0 of our Icons package, we are packaging each icon variant (filled, outlined, regular, etc.) in its own assembly. This became necessary due to the large number of icons and some compiler limitations. Nothing has changed on how the Icons package needs to be installed or added to your project. There is a just a small change needed to your imports file (or using statements).

To use the new version in your (upgraded) projects, you need add the following @using statement to your _Imports.razor file:
@using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons
Copy

Alternatively, you can use the full namespace in your code or define the alias for the namespace at the top of your file.

When creating an application through one of the Fluent templates (v4.11.0 or higher), everything will already be correctly set up for you.

During the DotNet Publication process, the unused icons are automatically removed from the final library. You can configure this behavior by setting the PublishTrimmed property in your project file. More details on this page.
⚠️ We recommend always using the Value property to specify the icon to be rendered (and not the Icon property). This ensures that the icon is not deleted from the final library.
<FluentIcon Value="@(new Icons.Regular.Size24.Bookmark())" />
Copy

You can use any of these icons by levaraging the <FluentIcon> component. See below for the parameters and examples. There is also a search capability available on this page wich allows you to browse to all the different icons. At the moment the icons displayed in this explorer below are always using the default blue accent color. When using the icons in your application, they will render in the color set with the Color parameter.

Explore Icons

Overview
Icon examples
A lot of the web components have named slots that declare locations in which content can be rendered. FluentIcon leverages this capability through its Slot parameter. With this you can for example render the icon in front (Slot="start") or after (Slot="end") a label in a FluentButton component.
<FluentStack VerticalAlignment="VerticalAlignment.Center">
    <FluentIcon Value="@(new Icons.Regular.Size24.Save())" Title="Save" />
    <FluentIcon Value="@(new Icons.Regular.Size24.Open())" Title="Open" Color="Color.Error" />
    <FluentIcon Value="@(new Icons.Regular.Size24.Album())" />

    <FluentIcon Value="@(Icon.FromImageUrl("_content/FluentUI.Demo.Shared/images/BlazorLogo.png"))" Width="24px"/>

    @(new Icons.Regular.Size20.Add().ToMarkup())
    @(new Icons.Regular.Size20.Airplane().ToMarkup("16px", "blue"))

    <FluentButton IconStart="@(new Icons.Regular.Size24.ArrowCircleLeft())">
        Back
    </FluentButton>

    <FluentButton IconEnd="@(new Icons.Regular.Size24.ArrowCircleRight().WithColor(Color.Success))">
        Next
    </FluentButton>

    <FluentButton IconEnd="@(new Icons.Regular.Size24.DismissCircle().WithColor("red"))">
        Exit
    </FluentButton>

    <FluentButton Appearance="Appearance.Accent"
                  IconEnd="@(new Icons.Regular.Size24.Alert().WithColor(Color.Fill))">
        Next
    </FluentButton>

</FluentStack>
Copy
Download:
Razor
Color options
Icons can be drawn and filled with a color through the Color parameter which takes a Color enumeration value.

   <FluentIcon Value="@(new Icons.Filled.Size48.Alert())" Color="{···}" />
   <FluentIcon Value="@(new Icons.Filled.Size48.Alert().WithColor({···})" />

With the code above you can use the following options at the {···}:
@{
    const string AdaptedLightDark = "Adapted to Light/Dark theme";
    const string CustomColor = "#570078"; // like Purple color
}
<FluentGrid Style="align-items: center;">
    <FluentGridItem xs="2" title="Color Enumeration value">Color&nbsp;Enum</FluentGridItem>
    <FluentGridItem xs="4" title="Equivalent CSS variable">CSS variable</FluentGridItem>
    <FluentGridItem xs="1" title="@AdaptedLightDark">Theme</FluentGridItem>
    <FluentGridItem xs="5" title="Examples">Examples</FluentGridItem>

    @foreach (var color in Enum.GetValues(typeof(Color)).Cast<Color>())
    {
        var name = Enum.GetName<Color>(color);
        var css = color == Color.Custom ? CustomColor : color.ToAttributeValue();

        <FluentGridItem xs="2">
            @name
        </FluentGridItem>
        <FluentGridItem xs="4">
            <code>@css</code>
        </FluentGridItem>
        <FluentGridItem xs="1" title="@AdaptedLightDark">
            @switch (color)
            {
                case Color.Neutral:
                case Color.Fill:
                case Color.FillInverse:
                case Color.Lightweight:
                    <FluentIcon Value="@(new Icons.Regular.Size24.CheckboxChecked())"
                                Color="@Color.Success" />
                    break;

                default:
                    <FluentIcon Value="@(new Icons.Regular.Size24.CheckboxUnchecked())"
                                Color="@Color.Info" />
                    break;
            }
        </FluentGridItem>
        <FluentGridItem xs="1" Style="background-color: var(--neutral-layer-3);">
            @if (color == Color.Custom)
            {
                <FluentIcon Value="@(new @Icons.Filled.Size24.Alert())"
                            Color="Color.Custom"
                            CustomColor="@CustomColor"
                            Title="@($"Icon with Color=\"Color.Custom\"")" />
            }
            else
            {
                <FluentIcon Value="@(new @Icons.Filled.Size24.Alert())"
                            Color="@color"
                            Title="@($"Icon with Color=\"Color.{name}\"")" />
            }
        </FluentGridItem>
        <FluentGridItem xs="2" Style="background-color: var(--neutral-layer-3);">
            <FluentButton IconStart="@(new Icons.Filled.Size24.Alert().WithColor(css))"
                          Title="@($"Button with an IconStart colorized using .WithColor(\"{css}\")")">
                OK
            </FluentButton>
        </FluentGridItem>
        <FluentGridItem xs="2" Style="background-color: var(--neutral-layer-3);">
            <FluentButton IconStart="@(new Icons.Filled.Size24.Alert().WithColor(css))"
                          Title="@($"Button with an IconStart colorized using .WithColor(\"{css}\")")"
                          Appearance="Appearance.Accent">
                OK
            </FluentButton>
        </FluentGridItem>
    }

</FluentGrid>

<style>
    .fluent-grid > div:nth-of-type(-n+4) {
        font-weight: 600;
    }

    .fluent-grid > div:not(:nth-of-type(-n+4)) {
        height: 60px;
        font-weight: 600;
    }

    .fluent-grid fluent-button {
        font-weight: 600;
    }
</style>

<p>
    With <code>Color.Custom</code>, supply your own color value through the <code>CustomColor</code> parameter. <br/>
    Needs to be formatted as an HTML hex color string (#rrggbb or #rgb) or a CSS variable (var(--...)).
</p>

Copy
Download:
Razor
Customization
You can create your own icons by adding a class like this one:
public static class MyIcons
{
    public class SettingsEmail : Icon { public SettingsEmail() : base("SettingsEmail", IconVariant.Regular, IconSize.Size20, "<svg width=\"20\" height=\"19\" viewBox=\"0 0 20 19\" fill=\"none\" xmlns=\"<http://www.w3.org/2000/svg\">><path d=\"M15.6251 2.5H4.37508L4.2214 2.50428C2.79712 2.58396 1.66675 3.76414 1.66675 5.20833V13.125L1.67103 13.2787C1.75071 14.7029 2.93089 15.8333 4.37508 15.8333H9.76425C9.91725 15.4818 10.1354 15.1606 10.4087 14.8873L10.7126 14.5833H4.37508L4.25547 14.5785C3.50601 14.5177 2.91675 13.8902 2.91675 13.125V6.97833L9.709 10.5531L9.78908 10.5883C9.95267 10.647 10.135 10.6353 10.2912 10.5531L17.0834 6.9775V9.17258C17.5072 9.14483 17.9362 9.21517 18.3334 9.38358V5.20833L18.3292 5.05465C18.2494 3.63038 17.0693 2.5 15.6251 2.5ZM4.37508 3.75H15.6251L15.7447 3.75483C16.4942 3.81568 17.0834 4.44319 17.0834 5.20833V5.565L10.0001 9.29375L2.91675 5.56583V5.20833L2.92158 5.08873C2.98242 4.33926 3.60994 3.75 4.37508 3.75ZM15.9167 10.5579L10.9979 15.4766C10.7112 15.7633 10.5077 16.1227 10.4093 16.5162L10.0279 18.0418C9.86208 18.7052 10.4631 19.3062 11.1265 19.1403L12.6521 18.7588C13.0455 18.6605 13.4048 18.4571 13.6917 18.1703L18.6103 13.2516C19.3542 12.5078 19.3542 11.3018 18.6103 10.5579C17.8665 9.814 16.6605 9.814 15.9167 10.5579Z\" fill=\"#212121\" /></svg>") { } }
}
Copy

If the size of your customized icon (Viewbox) is not one of the standard IconSize sizes, you can use a IconSize.Custom size.
 <FluentIcon Value="@(new MyCircle())" Width="16px" />
 <FluentIcon Value="@(new MyCircle().WithColor("red"))" Width="16px" />
 @new MyCircle().ToMarkup("16px", "blue")

Refresh
 <FluentButton IconStart="@(new MyCircle())">Refresh</FluentButton>
Setting Width="" (an empty string) makes the icon 100% width of the enclosing container and stylable from CSS. By omitting this attribute or assigning to null, for example via Width="@null", the default icon size will be used.

public class MyCircle : Microsoft.FluentUI.AspNetCore.Components.Icon
{
    private const string SVG_CONTENT = "<svg version='1.1' xmlns='http://www.w3.org/2000/svg' viewBox='0 0 320 320'><circle cx='160' cy='160' r='140'/></svg>";

    public MyCircle() : base("MyCircle", IconVariant.Regular, IconSize.Custom, SVG_CONTENT)
    {
        // Default Color (`fill` style)
        WithColor("#F97316");
    }
}
Copy

Using the IconSize.Custom size, certain restrictions will apply: FluentButton.Loading or Icon.ToMarkup could be affected.

Documentation
FluentIcon<Icon> Class
You can use either the Icon or Value parameter to specify the icon to render. The Icon parameter takes a Icon Type, while the Value parameter takes a icon object.
Parameters
Color
Color?
Gets or sets the icon drawing and fill color.
Value comes from the Color enumeration. Defaults to Accent.
CustomColor
string?
Gets or sets the icon drawing and fill color to a custom value.
Needs to be formatted as an HTML hex color string (#rrggbb or #rgb) or CSS variable.
⚠️ Only available when Color is set to Color.Custom.
Focusable
bool
False
Gets or sets whether the icon is focusable (adding tabindex='0' and role='button'),
allows the icon to be focused sequentially (generally with the Tab key).
Slot
string?
Gets or sets the slot where the icon is displayed in.
Title
string?
Gets or sets the title for the icon.
Value
Icon
Gets or sets the Icon object to render.
Width
string?
Gets or sets the icon width.
If not set, the icon size will be used.
EventCallbacks
OnClick
EventCallback<MouseEventArgs>
Allows for capturing a mouse click on an icon.
