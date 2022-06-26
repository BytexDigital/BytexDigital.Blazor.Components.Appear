using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BytexDigital.Blazor.Components.Appear
{
    public partial class VisibilityObserver
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Callback to be executed every time the element appears in the viewport.
        /// </summary>
        [Parameter]
        public EventCallback OnAppeared { get; set; }

        /// <summary>
        /// Callback to be executed every time the element has fully disappeared from the viewport.
        /// </summary>
        [Parameter]
        public EventCallback OnDisappeared { get; set; }

        /// <summary>
        /// Callback to be executed the first time the element appears in the viewport.
        /// </summary>
        [Parameter]
        public EventCallback OnFirstAppeared { get; set; }

        /// <summary>
        /// Modifies the bounding box which is used to detect whether the element is visible on the screen. Defaults to <c>0px</c>.
        /// <para>View <see href="https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API#Intersection_observer_options">'rootMargin' in the IntersectionObserver documentation</see> for details.</para>
        /// </summary>
        [Parameter]
        public string BoundingBoxMargin { get; set; } = "0px";

        /// <summary>
        /// The query selector for the element being observed.
        /// </summary>
        [Parameter]
        public string ElementQuerySelector { get; set; }

        /// <summary>
        /// Value that indicates whether the element has appeared atleast once in the viewport.
        /// </summary>
        public bool HasAppeared { get; private set; }

        /// <summary>
        /// Value that indicates whether the element is currently in the viewport.
        /// </summary>
        public bool IsVisible { get; private set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (!firstRender)
                {
                    return;
                }

                var module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                    "import",
                    "./_content/BytexDigital.Blazor.Components.Appear/main.js");

                await module.InvokeVoidAsync(
                    "observe",
                    ElementQuerySelector,
                    DotNetObjectReference.Create(this),
                    nameof(JsOnAppeared),
                    nameof(JsOnDisappeared),
                    BoundingBoxMargin);
            }
            catch
            {
                // Ignore ex, usually due to bad references when the element is not existant anymore
            }
        }

        [JSInvokable]
        public async Task JsOnAppeared()
        {
            try
            {
                if (!HasAppeared)
                {
                    await OnFirstAppeared.InvokeAsync();
                }

                IsVisible = true;
                HasAppeared = true;
                await OnAppeared.InvokeAsync();
            }
            catch
            {
                // Ignore
            }
        }

        [JSInvokable]
        public async Task JsOnDisappeared()
        {
            try
            {
                IsVisible = false;
                await OnDisappeared.InvokeAsync();
            }
            catch
            {
                // Ignore
            }
        }
    }
}