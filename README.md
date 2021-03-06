# AnimateAlert

AnimateAlert is a component for Blazor apps.

## Example

```
@using AnimateAlert.Components

<Alert @ref="alert">Hello World!</Alert>

<button @onclick="ShowAlert">Show Alert!</button>

@code {
  Alert alert;
  private void ShowAlert()
  {
   alert.Color = AnimateAlert.Color.Info;
   alert.Animation = AnimateAlert.Animation.FadeInOut;
   alert.IsOpen = true;
  }
}
```
## Properties

| Property  | Type | Description |
| ------------- | ------------- | ------------- |
| Animation  | Animation  | Animates the alert. |
| AutoHide  | bool  | Auto hides the alert after the specified delay in milliseconds  (Default is 4000ms). |
| AutoHideDelay  | int  | Sets the delay in milliseconds for auto hiding the alert. |
| ChildContent  | RenderFragment  | Sets alert content. |
| Color  | Color  | Applies the selected color to the alert. |
| IsDismissible  | bool  | Adds the close button to the alert. |
| IsOpen  | bool  | Opens the alert. |

## Contributing
All contributers are welcomed.
