using Microsoft.JSInterop;

namespace BlazorApp_Vue_Samples.Services
{
    // TODO: throws null reference
    public class JsConsole
    {
        private readonly IJSRuntime JsRuntime;
        public JsConsole(IJSRuntime jSRuntime)
        {
            this.JsRuntime = jSRuntime;
        }

        public async Task WriteLine(string message)
        {
            await this.JsRuntime.InvokeVoidAsync("console.log", message);
        }
    }
}
