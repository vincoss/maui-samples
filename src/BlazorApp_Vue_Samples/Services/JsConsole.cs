using Microsoft.JSInterop;


namespace BlazorApp_Vue_Samples.Services
{
    public class JsConsole
    {
        private readonly IJSRuntime _jsRuntime;
        public JsConsole(IJSRuntime jsRuntime)
        {
            this._jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(JSRuntime));
        }

        public async Task WriteLine(string message)
        {
            await this._jsRuntime.InvokeVoidAsync("console.log", message);
        }
    }
}
