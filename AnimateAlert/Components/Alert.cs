using AnimateAlert.Class;
using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AnimateAlert.Components
{
    public partial class Alert : ComponentBase
    {
        private bool _isOpen = false;
        [Parameter]
        public bool IsOpen {
            get => _isOpen;
            set
            {
                if (_isOpen == value) return;

                _isOpen = value;
                IsOpenChanged.InvokeAsync(value);
                if (value)
                    CheckAutoHide().ConfigureAwait(false);
    }
}
        [Parameter] public bool IsDismissible { get; set; } = false;
        [Parameter] public Color Color { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public Animation Animation { get; set; } = Animation.None;
        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
        [Parameter] public bool AutoHide { get; set; } = true;
        [Parameter] public int AutoHideDelay { get; set; } = 4000;
        [Parameter] public bool AnimationIsPlay { get; set; } = false;
        [Parameter] public static string cssClass { get; set; }

        internal AlertEvent AlertEvent { get; set; }
        [Parameter] public EventCallback<AlertEvent> ClosedEvent { get; set; }
        internal List<EventCallback<AlertEvent>> EventQue { get; set; } = new List<EventCallback<AlertEvent>>();
        [Parameter] public EventCallback<AlertEvent> CloseEvent { get; set; }
        protected string ClassName = "";
        protected override void OnInitialized()
        {
           
            base.OnInitialized();
        }
        protected override void OnParametersSet()
        {
            if (AnimationIsPlay == false)
            {
                ClassName = new CssBuilder().AddClass("alert")
                    .AddClass(IsDismissible ? "alert-dismissible" : "")
                    .AddClass($"alert-{Color.ToDescriptionString()}")
                    .AddClass(cssClass)
                    .Build();
            }

            base.OnParametersSet();
        }
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
        protected async Task PlayAnimation()
        {
            AnimationIsPlay = true;
            ClassName =
            new CssBuilder().AddClass("alert")
                .AddClass(IsDismissible ? "alert-dismissible" : "")
                .AddClass($"alert-{Color.ToDescriptionString()}")
                .AddClass($"{Animation.ToDescriptionString()}-end")
                .AddClass(cssClass)
            .Build();

            await Task.Delay(1000).ConfigureAwait(true);
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);

            AnimationIsPlay = false;
            ClassName = new CssBuilder().AddClass("alert")
           .AddClass(IsDismissible ? "alert-dismissible" : "")
           .AddClass($"alert-{Color.ToDescriptionString()}")
           .AddClass(cssClass)
           .Build();

            CloseAlert();
   
        }
        protected void CloseAlert()
        {
            IsOpen = false;
            AlertEvent = new AlertEvent() { Target = this };
            CloseEvent.InvokeAsync(AlertEvent);
            EventQue.Add(ClosedEvent);

        
        }
        protected async Task CheckAutoHide()
        {
            if (IsOpen && AutoHide)
            {
                await Task.Delay(AutoHideDelay).ConfigureAwait(true);
                await InvokeAsync(StateHasChanged).ConfigureAwait(false);
                PlayAnimation();
            }
        }
    }
}
