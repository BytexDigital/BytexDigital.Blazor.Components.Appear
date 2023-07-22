using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BytexDigital.Blazor.Components.Appear
{
    public partial class VisibilityComparer
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public List<VisibilityObserver> Observers { get; } = new();

        /// <summary>
        ///     Fires if any threshold of the child <see cref="VisibilityObserver" />s is reached.
        /// </summary>
        [Parameter]
        public EventCallback OnThresholdsReached { get; set; }

        /// <summary>
        ///     Fires once the most visible element has been determined.
        /// </summary>
        [Parameter]
        public EventCallback<VisibilityObserver> OnMostVisibleElementChanged { get; set; }

        /// <summary>
        ///     The current most visible observer or null.
        /// </summary>
        public VisibilityObserver MostVisibleObserver { get; set; }

        public async Task NotifyThresholdChangedAsync(VisibilityObserver observer)
        {
            var bestObserver = Observers.OrderByDescending(x => x.CurrentThreshold).FirstOrDefault();

            await OnThresholdsReached.InvokeAsync();

            if (bestObserver != MostVisibleObserver)
            {
                MostVisibleObserver = bestObserver;

                if (MostVisibleObserver != null)
                {
                    await OnMostVisibleElementChanged.InvokeAsync(MostVisibleObserver);
                }
            }
        }

        public Task RegisterObserverAsync(VisibilityObserver observer)
        {
            Observers.Add(observer);

            return Task.CompletedTask;
        }

        public Task DeregisterObserverAsync(VisibilityObserver observer)
        {
            Observers.Remove(observer);

            return Task.CompletedTask;
        }
    }
}