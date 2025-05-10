# Dialog

As defined by the W3C:

A dialog is a window overlaid on either the primary window or another dialog window. Windows under a modal dialog are inert. That is, users cannot interact with content outside an active dialog window. Inert content outside an active dialog is typically visually obscured or dimmed so it is difficult to discern, and in some implementations, attempts to interact with the inert content cause the dialog to close.

Like non-modal dialogs, modal dialogs contain their tab sequence. That is, Tab and Shift + Tab do not move focus outside the dialog. However, unlike most non-modal dialogs, modal dialogs do not provide means for moving keyboard focus outside the dialog window without closing the dialog.

`<FluentDialog>` wraps the `<fluent-dialog>` element, a web component implementation of a dialog leveraging the Fluent UI design system. `<FluentDialog>` acts as a shell for the dialog content, which can be specified in a number of ways.
## DialogService

The `DialogService` is a service which is used to show different types of dialogs. It is registered as a scoped service, so it can be injected into pages/components that use it. For more information on the `DialogService`, see the Dialog Service page.

## Dialog Content

Normally, the dialog content is specified by a component which implements `IDialogContentComponent<T>`. This component is then passed to the `DialogService` to be shown. The `DialogService` will then render a `<FluentDialog>` with the component inside of it.

Alternatively, the dialog content can be specified manually by setting the `ChildContent` parameter of `<FluentDialog>`. This is useful if you want to show a simple dialog without having to create a component for it or if you do not want to use the `DialogService` for it.
## WASM Publication Notes

During the WASM DotNet Publication process, the unused classes are automatically removed from the final library. The Dialog razor file will be removed if no reference are set by your code. For example, if you call `var = await DialogService.ShowDialogAsync<SimpleDialog>(simplePerson, parameters);`, the SimpleDialog will be removed by the WASM Publication process. You can configure this behavior by setting the PublishTrimmed property in your project file or you can create a temporary instance to force the Publication process to keep this class in the final library with: `var temp1 = new SimpleDialog();`

## Exchange Data Between Dialog and Calling Component

There are two ways available to exchange data between the dialog and the component which shows it:
1. By capturing the `IDialogReference` returned from one of the `DialogService.Show...Async` methods and then use that reference to get the dialog's result (of type `DialogResult`)
2. By using an `EventCalback` parameter as part of the `DialogParameters`

Both ways are demonstrated in the samples below.

    </nav>
    <div class="content">
    <article id="article">
    @Body
    </article>
    </div>
    <FluentDialogProvider />
    </main>
Copy
Examples
These examples show how to use the to display a dialog. The content of the dialog is specified by a component which implements . Here, that is done in . The dialog is automatically styled and centered. DialogServiceIDialogContentComponent<T>SimpleDialog.razor

Interaction with parent dialog can be made by injecting FluentDialog as Cascading Parameter. See for more details. SimpleDialog.razor

Documentation
FluentDialogProvider Class
Methods
## Documentation

### FluentDialogProvider Class
#### Methods
- **DismissAll**: `void`

### FluentDialog Class
#### Parameters
- **AriaDescribedby**: `string?`  
  Gets or sets the id of the element describing the dialog.
  
- **AriaLabel**: `string?`  
  Gets or sets the label surfaced to assistive technologies.
  
- **AriaLabelledby**: `string?`  
  Gets or sets the id of the element labeling the dialog.
  
- **ChildContent**: `RenderFragment?`  
  Used when not calling the `DialogService` to show a dialog.
  
- **Hidden**: `bool` (Default: False)  
  Gets or sets a value indicating whether the dialog is hidden.
  
- **Instance**: `DialogInstance`  
  Gets or sets the instance containing the programmatic API for the dialog.
  
- **Modal**: `bool?`  
  Gets or sets a value indicating whether the element is modal. When modal, user mouse interaction will be limited to the contents of the element by a modal overlay. Clicks on the overlay will cause the dialog to emit a 'dismiss' event.
  
- **PreventScroll**: `bool` (Default: True)  
  Prevents scrolling outside of the dialog while it is shown.
  
- **TrapFocus**: `bool?`  
  Gets or sets a value indicating whether that the dialog should trap focus.
The event callback invoked when  change.FluentDialog.Hidden
OnDialogResult
EventCallback<DialogResult>
The event callback invoked to return the dialog result.
#### EventCallbacks
- **HiddenChanged**: `EventCallback<bool>`  
  The event callback invoked when `FluentDialog.Hidden` change.
  
- **OnDialogResult**: `EventCallback<DialogResult>`  
  The event callback invoked to return the dialog result.

#### Methods
- **CancelAsync**: `Task`  
  Closes the dialog with a cancel result.
  
- **CancelAsync<T>**: `T returnValue`, `Task`  
  Closes the dialog with a cancel result.
  
- **CloseAsync**: `Task`  
  Closes the dialog
  
- **CloseAsync**: `DialogResult dialogResult`, `Task`  
  Closes the dialog
  
- **CloseAsync<T>**: `T returnValue`, `Task`  
  Closes the dialog with a OK result.
  
- **Hide**: `void`  
  Hides the dialog
  
- **Show**: `void`  
  Shows the dialog
  
- **TogglePrimaryActionButton**: `bool isEnabled`, `void`  
  Toggle the primary action button
  
- **ToggleSecondaryActionButton**: `bool isEnabled`, `void`  
  Toggle the secondary action button

To alter the properties of a dialog which uses the `DialogService` to display it, use the `DialogParameters` class.
Gets or sets the dialog position:
left (full height), right (full height)
or screen middle (using Width and Height properties).
HorizontalAlignment.Stretch is not supported for this property.
AriaDescribedby
string?
Gets or sets the element that describes the element on which the attribute is set.
AriaLabel
string?
Gets or sets the value that labels an interactive element.
AriaLabelledby
string?
Gets or sets the element that labels the element it is applied to.
Content
TContent
Gets or sets the content to pass to and from the dialog.
DialogBodyStyle
string
Gets or sets the extra styles applied to the Body content.
DialogType
DialogType
Dialog
Gets or sets the type of dialog.
DismissTitle
string?
Close
Gets or sets the Title of the dismiss button, display in a tooltip.
Defaults to 'Close'.
Height
string?
Gets or sets the height of the dialog. Must be a valid CSS height value like '600px' or '3em'
Only used if Alignment is set to 'HorizontalAlignment.Center'
Item
Object

Modal
bool?
True
Determines if the dialog is modal. Defaults to true.
Obscures the area around the dialog.
PreventDismissOnOverlayClick
bool
False
Determines if a modal dialog is dismissible by clicking
outside the dialog. Defaults to false.
PreventScroll
bool
True
Prevents scrolling outside of the dialog while it is shown.
PrimaryAction
string?
OK
Gets or sets the text to display for the primary action.
PrimaryActionEnabled
bool
True
When true, primary action's button is enabled.
SecondaryAction
string?
Cancel
Gets or sets the text to display for the secondary action.
SecondaryActionEnabled
bool
True
When true, secondary action's button is enabled.
ShowDismiss
bool
True
Gets or sets a value indicating whether show the dismiss button in the header.
Defaults to true.
ShowTitle
bool
True
Gets or sets a value indicating whether show the title in the header.
Defaults to true.
Title
string?
Gets or sets the title of the dialog.
TrapFocus
bool?
True
Gets or sets a value indicating whether if dialog should trap focus.
Defaults to true.
ValidateDialogAsync
Func<Task<bool>>
Function that is called and awaited before the dialog is closed.
Visible
bool
True
Gets or sets if a dialog is visible or not (Hidden)
Width
string?
Gets or sets the width of the dialog. Must be a valid CSS width value like '600px' or '3em'
EventCallbacks
OnDialogClosing
EventCallback<DialogInstance>
Callback function that is called and awaited before the dialog fully closes.
OnDialogOpened
EventCallback<DialogInstance>
Callback function that is called and awaited after the dialog renders for the first time.
OnDialogResult
EventCallback<DialogResult>
Callback function for the result.
Methods
Add
string parameterName
Object value
void

Get<T>
string parameterName
T?

GetDictionary
Dictionary<string, Object>

GetEnumerator
IEnumerator<KeyValuePair<string, Object>>

TryGet<T>
string parameterName
T?

Dialog header and footer
The dialog header and footer can be changed by using the and component.FluentDialogHeaderFluentDialogFooter

The default implementation uses the and components (see documentation below). You can use the content of these components as the base for your own implementation: FluentDialogHeaderFluentDialogFooter

Default dialog header (simplified version)

    <FluentStack Orientation="Orientation.Horizontal" VerticalAlignment="VerticalAlignment.Top">
    <div style="width: 100%;">
    <FluentLabel Typo="Typography.PaneHeader">@Title</FluentLabel>
    </div>
    <FluentButton Appearance="Appearance.Stealth">
    <FluentIcon Icon="CoreIcons.Regular.Size24.Dismiss" />
    <FluentButton>
    </FluentStack>
Copy
Default dialog footer (simplified version)

    <FluentStack Orientation="Orientation.Horizontal" HorizontalAlignment="HorizontalAlignment.Right" VerticalAlignment="VerticalAlignment.Bottom">
    <FluentButton Title="@PrimaryAction" Appearance="Appearance.Accent" Disabled="@PrimaryActionEnabled">
    @PrimaryAction
    </FluentButton>
    <FluentButton Title="@SecondaryAction" Appearance="Appearance.Neutral" Disabled="@SecondaryActionEnabled">
    @SecondaryAction
    </FluentButton>
    </FluentStack>
Copy
FluentDialogHeader Class
Parameters
ChildContent
RenderFragment?
Gets or sets the content to be rendered inside the component.
ShowDismiss
bool?
When true, shows the dismiss button in the header.
If defined, this value will replace the one defined in the .DialogParameters
ShowDismissTooltip
bool?
True
When true, shows the 'Close' button tooltip in the header.
TabIndex
int?
0
Allows developers to make elements sequentially focusable and determine their relative ordering for navigation (usually with the Tab key).
Visible
bool
True
When true, the header is visible.
Default is True.
FluentDialogFooter Class
Parameters
ChildContent
RenderFragment?
Gets or sets the content to be rendered inside the component.
Visible
bool
True
When true, the footer is visible.
Default is True.
FluentDialogBody Class
Parameters
ChildContent
RenderFragment?
Gets or sets the content to be rendered inside the component.
DialogHelper Class
Methods
From<TDialog>
DialogHelper<TDialog>
Create a dialog helper for the specified dialog type
ShowDialogAsync<TDialog, TData>
IDialogService svc
DialogHelper<TDialog> dialogHelper
TData data
DialogParameters parameters
Task<IDialogReference>
Shows a dialog with the component type as the body,
passing the specified data
ShowDialogAsync<TDialog, TData>
DialogHelper<TDialog> dialogHelper
TData data
IDialogService svc
DialogParameters parameters
Task<IDialogReference>
Shows a dialog with the component type as the body,
passing the specified data
