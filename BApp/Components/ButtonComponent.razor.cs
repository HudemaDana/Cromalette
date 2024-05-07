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

        private bool IsClicked { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        private void HandleClick()
        {
            IsClicked = !IsClicked;
        }
    }
}