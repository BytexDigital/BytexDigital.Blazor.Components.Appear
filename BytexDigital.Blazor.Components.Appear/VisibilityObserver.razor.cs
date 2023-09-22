using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BytexDigital.Blazor.Components.Appear
{
    public partial class VisibilityObserver : IAsyncDisposable
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        /// <summary>
        ///     Callback to be executed every time the element appears in the viewport.
        /// </summary>
        [Parameter]
        public EventCallback OnAppeared { get; set; }

        /// <summary>
        ///     Callback to be executed every time the element has fully disappeared from the viewport.
        /// </summary>
        [Parameter]
        public EventCallback OnDisappeared { get; set; }

        /// <summary>
        ///     Callback to be executed the first time the element appears in the viewport.
        /// </summary>
        [Parameter]
        public EventCallback OnFirstAppeared { get; set; }

        [Parameter]
        public EventCallback<double> OnThresholdReached { get; set; }

        /// <summary>
        ///     Modifies the bounding box which is used to detect whether the element is visible on the screen. Defaults to
        ///     <c>0px</c>.
        ///     <para>
        ///         View
        ///         <see
        ///             href="https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API#Intersection_observer_options">
        ///             'rootMargin'
        ///             in the IntersectionObserver documentation
        ///         </see>
        ///         for details.
        ///     </para>
        /// </summary>
        [Parameter]
        public string BoundingBoxMargin { get; set; } = "0px";

        /// <summary>
        ///     Interval at which the <see cref="OnThresholdReached" /> event will be fired.
        ///     <example>0.1 means every 10%: 0%, 10%, 20%, etc.</example>
        ///     >
        /// </summary>
        [Parameter]
        public double ThresholdInterval { get; set; } = 0.1;

        /// <summary>
        ///     Absolute list of thresholds to fire the <see cref="OnThresholdReached" /> event at. Overwrites automatic intervals
        ///     created by <see cref="ThresholdInterval" />.
        /// </summary>
        [Parameter]
        public List<double> Thresholds { get; set; }

        /// <summary>
        ///     The query selector for the element being observed.
        /// </summary>
        [Parameter]
        public string ElementQuerySelector { get; set; }

        /// <summary>
        ///     User defined data that is useful to identify this observer inside a <see cref="VisibilityComparer" />.
        /// </summary>
        [Parameter]
        public object Data { get; set; }

        /// <summary>
        ///     Value that indicates whether the element has appeared atleast once in the viewport.
        /// </summary>
        public bool HasAppeared { get; protected set; }

        /// <summary>
        ///     Value that indicates whether the element is currently in the viewport.
        /// </summary>
        public bool IsVisible { get; protected set; }

        /// <summary>
        ///     Value that indicates whether the element is currently in the viewport or was in the past.
        /// </summary>

        public bool IsOrWasVisible { get; protected set; }

        /// <summary>
        ///     Last threshold reached.
        /// </summary>
        public double CurrentThreshold { get; protected set; }

        [CascadingParameter]
        public VisibilityComparer Comparer { get; set; }

        [Parameter]
        public EventCallback OnInitialized { get; set; }

        public async ValueTask DisposeAsync()
        {
            if (Comparer != null) await Comparer.DeregisterObserverAsync(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (!firstRender)
                {
                    return;
                }

                if (Comparer != null) await Comparer.RegisterObserverAsync(this);

                var thresholds = new List<double>();

                if (Thresholds != null)
                {
                    thresholds = Thresholds;
                }
                else
                {
                    for (double i = 0; i <= 1; i += ThresholdInterval)
                    {
                        thresholds.Add(i);
                    }
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
                    BoundingBoxMargin,
                    thresholds);

                StateHasChanged();

                await OnInitialized.InvokeAsync();
            }
            catch
            {
                // Ignore ex, usually due to bad references when the element is not existant anymore
            }
        }

        [JSInvokable]
        public async Task JsOnAppeared(double threshold)
        {
            try
            {
                if (!HasAppeared)
                {
                    IsOrWasVisible = true;
                    await OnFirstAppeared.InvokeAsync();
                }

                IsVisible = true;
                HasAppeared = true;

                await OnAppeared.InvokeAsync();

                CurrentThreshold = threshold;
                await OnThresholdReached.InvokeAsync(threshold);

                if (Comparer != null) await Comparer.NotifyThresholdChangedAsync(this);
            }
            catch
            {
                // Ignore
            }
        }

        [JSInvokable]
        public async Task JsOnDisappeared(double threshold)
        {
            try
            {
                IsVisible = false;
                await OnDisappeared.InvokeAsync();

                CurrentThreshold = threshold;
                await OnThresholdReached.InvokeAsync(threshold);

                if (Comparer != null) await Comparer.NotifyThresholdChangedAsync(this);
            }
            catch
            {
                // Ignore
            }
        }
    }
}