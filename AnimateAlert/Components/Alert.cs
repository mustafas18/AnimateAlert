using AnimateAlert.Class;
using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimateAlert.Components
{
    public partial class Alert : ComponentBase
    {
        [Parameter]
        public bool IsOpen { get; set; } = false;
        [Parameter]
        public bool IsDismissible { get; set; } = false;
        [Parameter]
        public Color Color { get; set; } = Color.Primary;

        [Parameter]
        public RenderFragment ChildContent { get; set; }


        [Parameter]
        public Animation Animation { get; set; } = Animation.None;

        [Parameter] public string Class { get; set; }

        internal AlertEvent AlertEvent { get; set; }
        [Parameter] public EventCallback<AlertEvent> ClosedEvent { get; set; }
        internal List<EventCallback<AlertEvent>> EventQue { get; set; } = new List<EventCallback<AlertEvent>>();
        [Parameter] public EventCallback<AlertEvent> CloseEvent { get; set; }
        protected string ClassName =>
      new CssBuilder().AddClass("alert")
          .AddClass(IsDismissible ? "alert-dismissible" : "")
          .AddClass($"alert-{Color.ToDescriptionString()}")
            .AddClass($"animate-{Animation.ToDescriptionString()}")
          .AddClass(Class)
      .Build();

        protected override Task OnAfterRenderAsync(bool firstrun)
        {
            for (var i = 0; i < EventQue.Count; i++)
            {
                EventQue[i].InvokeAsync(AlertEvent);
                EventQue.RemoveAt(i);
            }

            return base.OnAfterRenderAsync(false);
        }
        protected void OnClose()
        {
            IsOpen = false;
            CloseAlert();
        }

        protected void CloseAlert()
        {
            AlertEvent = new AlertEvent() { Target = this };
            CloseEvent.InvokeAsync(AlertEvent);
            EventQue.Add(ClosedEvent);
        }

    }
}
