using Microsoft.AspNetCore.Components;

namespace BApp.Components.InputFields
{
    public partial class InputTextComponent
    {
        [Parameter, EditorRequired]
        public string Label { get; set; }

        [Parameter, EditorRequired]
        public string Value { get; set; }

        [Parameter]
        public string InputType { get; set; } = "text";

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        private async Task OnValueChanged(ChangeEventArgs e)
        {
            Value = (string)e.Value;
            await ValueChanged.InvokeAsync(Value);
        }
    }
}
