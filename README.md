![Nuget](https://img.shields.io/nuget/v/BytexDigital.Blazor.Components.Appear.svg?style=flat-square)

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
- `BoundingBoxMargin` Defaults to 0px. Greater values will make `OnAppeared` fire earlier. View https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API#Intersection_observer_options for more details on this option.
- `Tag` The HTML tag used for the container.

## Usage VisibilityObserver

This component can be used when you do not want to wrap your content in a VisibilityAwareContainer.

```html
<VisibilityObserver OnFirstAppeared="..."
                    OnAppeared="..."
                    OnDisappeared="..."
                    BoundingBoxMargin="0px"
                    ElementQuerySelector="#myElementId"
                    ThresholdInterval="0.1" />
```

## Usage VisibilityComparer

The VisibilityComparer is used to determine the most visible observer of a collection of observers. This can be useful
when e.g. implementing nav bars that update their active link according to the currently visible section of the page.

A simple usage example is attached to implement this scenario.

```html
@page "/"

@code {

    public string VisibleSectionId { get; set; }
    
    private void Callback(VisibilityObserver obj)
    {
        VisibleSectionId = obj.ElementQuerySelector;
        
        // Could also use the Data property to decide what to do
        // obj.Data~~~~

        StateHasChanged();
    }

}

<VisibilityComparer OnMostVisibleElementChanged="Callback">
    <VisibilityObserver ThresholdInterval="0.1" ElementQuerySelector="#section1" />
    <VisibilityObserver ThresholdInterval="0.1" ElementQuerySelector="#section2" />
    <!-- You can also the Data property to pass meaningful info through to the callback above. -->
    <!-- <VisibilityObserver ThresholdInterval="0.1" ElementQuerySelector="#section2" Data="..." /> -->
</VisibilityComparer>

<header>
    <a href="#section1" class="@(VisibleSectionId == "#section1" ? "active" : null)"></a>
    <a href="#section2" class="@(VisibleSectionId == "#section2" ? "active" : null)"></a>
</header>

<section id="section1">
    ...
</section>

<section id="section2">
    ...
</section>
```
