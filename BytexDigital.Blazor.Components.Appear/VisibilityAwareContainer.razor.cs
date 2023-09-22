using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BytexDigital.Blazor.Components.Appear
{
    public partial class VisibilityAwareContainer
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public RenderFragment<VisibilityAwareContainer> ChildContent { get; set; }

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
        
        [Parameter]
        public EventCallback<double> OnThresholdReached { get; set; }

        /// <summary>
        /// Modifies the bounding box which is used to detect whether the element is visible on the screen. Defaults to <c>0px</c>.
        /// <para>View <see href="https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API#Intersection_observer_options">'rootMargin' in the IntersectionObserver documentation</see> for details.</para>
        /// </summary>
        [Parameter]
        public string BoundingBoxMargin { get; set; } = "0px";

        [Parameter]
        public string Tag { get; set; } = "div";

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }

        /// <summary>
        /// Value that indicates whether the element has appeared atleast once in the viewport.
        /// </summary>
        public bool HasAppeared => _visibilityObserver?.HasAppeared ?? false;

        /// <summary>
        /// Value that indicates whether the element is currently in the viewport.
        /// </summary>
        public bool IsVisible => _visibilityObserver?.IsVisible ?? false;
        
        
        
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
        ///     User defined data that is useful to identify this observer inside a <see cref="VisibilityComparer" />.
        /// </summary>
        [Parameter]
        public object Data { get; set; }


        private RenderFragment _content;
        private VisibilityObserver _visibilityObserver;

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            if (!parameters.TryGetValue(nameof(Id), out string containerId) && string.IsNullOrEmpty(Id))
            {
                Id = $"id-{Guid.NewGuid()}";
            }

            await base.SetParametersAsync(parameters);
        }

        protected async Task OnVisibilityObserverInitializedAsync()
        {
            await InvokeAsync(StateHasChanged);
        }

        protected override void OnParametersSet()
        {
            _content = builder =>
            {
                var i = 0;

                builder.OpenElement(i++, Tag);
                builder.AddAttribute(i++, "_appear-id", Id);
                builder.AddMultipleAttributes(i++, AdditionalAttributes);
                builder.AddContent(i++, ChildContent, this);
                builder.CloseElement();
            };

            base.OnParametersSet();
        }
    }
}
