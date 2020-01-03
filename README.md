![Nuget](https://img.shields.io/nuget/v/BytexDigital.Blazor.Components.Appear.svg?style=flat-square)
![StableVersion](https://img.shields.io/badge/stable_version-v1.0.2-green.svg?style=flat-square)

# BytexDigital.Blazor.Components.Appear

This library is a small Blazor wrapper for the library https://github.com/morr/jquery.appear.
It adds a simple to use component that is aware whether it is on the screen or not, allowing for features such as infinite scrolling or long complex lists that only their render their actual content when on the screen.

## Download

[nuget package](https://www.nuget.org/packages/BytexDigital.Blazor.Components.Appear/)

```
Install-Package BytexDigital.Blazor.Components.Appear
```

## Requirements

- jQuery 2.1.4 or higher

## Usage

1. **Add required JavaScript**

This component library requires client-side javascript code to be included in your project.
This can achieved through 2 ways:

- Use the included `IHtmlHelper` extension method and use it in your `_Host.cshtml` as follows
```csharp
<!-- jQuery 2.1.4 or higher required -->
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

@Html.IncludeAppearScripts()
```

- Include the `bundle.js` file manually
```html
<script src='/_content/BytexDigital.Blazor.Components.Appear/bundle.js'></script>
```


2. **Use the provided component container**

```html
<VisibilityAwareContainer OnAppeared="..." OnDisappeared="..." OnlyFireAppearOnce="false">
    ... content
</VisibilityAwareContainer>
```

### Container Options
- `OnAppeared` Fires every time the container gets close to the viewport.
- `OnDisappeared` Fires every time the container leaves the viewport.
- `OnlyFireAppearOnce` Defaults to `false`. If enabled, `OnAppeared` will only fire once and `OnDisappeared` will be entirely disabled.
- `State` Optional state object that will be passed into `OnAppeared` and `OnDisappeared`.
- `AppearOffset` Defaults to 0. Greater values will make `OnAppeared` fire earlier.
