![Nuget](https://img.shields.io/nuget/v/BytexDigital.Blazor.Components.Appear.svg?style=flat-square)
![StableVersion](https://img.shields.io/badge/stable_version-v1.0.3-green.svg?style=flat-square)

# BytexDigital.Blazor.Components.Appear

This library adds a simple to use component that is aware of whether it is on the screen or not, allowing for features such as infinite scrolling or long complex lists that only render their actual content when on the screen.

## Download

[nuget package](https://www.nuget.org/packages/BytexDigital.Blazor.Components.Appear/)

```
Install-Package BytexDigital.Blazor.Components.Appear
```

## Requirements

- No requirements

## Usage VisibilityAwareContainer

```html
<VisibilityAwareContainer OnAppeared="..." OnDisappeared="..." OnFirstAppeared="..." Tag="div">
    <h1>My content</h1>
</VisibilityAwareContainer>
```

### Container Options
- `OnAppeared` Fires every time the container gets close to the viewport.
- `OnDisappeared` Fires every time the container leaves the viewport.
- `OnFirstAppeared` Fires only the first time the container appears in the viewport.
- `DetectionBoundingBoxMargin` Defaults to 0px. Greater values will make `OnAppeared` fire earlier. View https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API#Intersection_observer_options for more details on this option.

## Usage VisibilityObserver

This component can be used when you do not want to wrap your content in a VisibilityAwareContainer.

```html
<VisibilityObserver OnFirstAppeared="..."
                    OnAppeared="..."
                    OnDisappeared="..."
                    DetectionBoundingBoxMargin="0px"
                    ElementQuerySelector="#myElementId"  />
```