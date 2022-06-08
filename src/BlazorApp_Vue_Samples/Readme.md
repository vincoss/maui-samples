
has issue

TypeError: Cannot read properties of null (reading 'removeChild')
   at Microsoft.AspNetCore.Components.WebView.IpcReceiver.OnRenderCompleted(/PageContext pageContext, Int64 batchId, String errorMessageOrNull)
   at Microsoft.AspNetCore.Components.WebView.IpcReceiver.OnMessageReceivedAsync(/PageContext pageContext, String message)
   at /Microsoft.AspNetCore.Components.WebView.WebViewManager.<>c__DisplayClass18_0.<<MessageReceived>b__0>d.MoveNext()
NotifyUnhandledException @ blazor.webview.js:1
(anonymous) @ blazor.webview.js:1
(anonymous) @ VM8:7
(anonymous) @ VM127:1
(anonymous) @ VM127:1


## Resources
https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor
https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0
https://v2.vuejs.org/v2/guide/
https://www.jsdelivr.com/?query=vu
https://vuejs.org/guide/essentials/component-basics.html#defining-a-component
https://stackoverflow.com/questions/52612446/importing-a-package-in-es6-failed-to-resolve-module-specifier-vue


## Issues
```
blazor blazor.webview.js:1 TypeError: Cannot read properties of null (reading 'removeChild')
```
Ensure that the VUE.js or other components are wrapped in parent element.
<div>
	Child elements in here
</div>