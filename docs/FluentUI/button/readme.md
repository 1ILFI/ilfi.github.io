# Button

## Overview

As defined by the W3C:

> A button is a widget that enables users to trigger an action or event, such as submitting a form, opening a dialog, canceling an action, or performing a delete operation.

### FluentButton Class

#### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| Action | string? | | See button element for more details. |
| Appearance | Appearance? | Neutral | Gets or sets the visual appearance. See `Appearance`.<br>Defaults to `Appearance.Neutral` |
| Autofocus | bool? | | Determines if the element should receive document focus on page load. |
| BackgroundColor | string? | | Gets or sets the background color of this button (overrides the `FluentButton.Appearance` property).<br>Set the value 'rgba(0, 0, 0, 0)' to display a transparent button. |
| ChildContent | RenderFragment? | | Gets or sets the content to be rendered inside the component. |
| Color | string? | | Gets or sets the color of the font (overrides the `FluentButton.Appearance` property). |
| CurrentValue | string? | | Gets or sets the element's current value. |
| Disabled | bool | False | Disables the form control, ensuring it doesn't participate in form submission. |
| Enctype | string? | | See button element for more details. |
| FormId | string? | | Gets or sets the id of a form to associate the element to.<br>Both the `FluentComponentBase.Id` and the `FormId` must be set if the button is placed outside of a form. |
| IconEnd | Icon? | | Gets or sets the Icon displayed at the end of button content. |
| IconStart | Icon? | | Gets or sets the Icon displayed at the start of button content. |
| Loading | bool | False | Display a progress ring and disable the button. |
| Method | string? | | See button element for more details. |
| Name | string? | | Gets or sets the name of the element.<br>Allows access by name from the associated form. |
| NoValidate | bool? | | See button element for more details. |
| Required | bool | False | Gets or sets a value indicating whether the element needs to have a value. |
| StopPropagation | bool | False | Gets or sets a way to prevent further propagation of the current event in the capturing and bubbling phases. |
| Target | string? | | See button element for more details.<br>Possible values: '_self' \| '_blank' \| '_parent' \| '_top' |
| Title | string? | | Gets or sets the title of the button.<br>The text usually displayed in a 'tooltip' popup when the mouse is over the button. |
| Type | ButtonType? | Button | Gets or sets the button type. See `ButtonType` for more details.<br>Default is `ButtonType.Button`. |
| Value | string? | | Gets or sets the value of the element. |

#### EventCallbacks

| Event | Type | Description |
|-------|------|-------------|
| OnClick | EventCallback<MouseEventArgs> | Command executed when the user clicks on the button. |

#### Methods

| Method | Parameters | Return Type | Description |
|--------|------------|-------------|-------------|
| DisposeAsync | | ValueTask | |
| SetDisabled | bool disabled | void | |
SetDisabled
bool disabled
void
