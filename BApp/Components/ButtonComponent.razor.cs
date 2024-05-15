using Microsoft.AspNetCore.Components;

namespace BApp.Components
{
    public partial class ButtonComponent : ComponentBase
    {
        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string ButtonType { get; set; } = "button";

        [Parameter]
        public EventCallback OnClick { get; set; }

        private bool IsClicked { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task HandleClick()
        {
            IsClicked = !IsClicked;
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync();
            }
        }
    }
}