
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

## CDN
https://cdn.jsdelivr.net/npm/vue@2.x/dist/vue.js
https://cdn.jsdelivr.net/npm/vue

## Resources
https://github.com/EdCharbeneau/BlazorSize
https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor
https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0
https://v2.vuejs.org/v2/guide/
https://www.jsdelivr.com/?query=vu
https://vuejs.org/guide/essentials/component-basics.html#defining-a-component
https://stackoverflow.com/questions/52612446/importing-a-package-in-es6-failed-to-resolve-module-specifier-vue
https://remibou.github.io/How-to-keep-js-object-reference-in-Blazor/

https://stackoverflow.com/questions/61487266/render-react-component-inside-blazor-page
https://devkimchi.com/2020/06/03/adding-react-components-to-blazor-webassembly-app/
	https://github.com/devkimchi/Blazor-React-Sample
https://stackoverflow.com/questions/67912304/is-it-possible-to-use-vue-and-vue-component-in-blazor

https://stackoverflow.com/questions/61487266/render-react-component-inside-blazor-page

## Blazor and VUE JS pass data
Now I use session storage for communication (I only have to pass 1 token), but I realized that it can be done by a parameter too



## Issues
```
blazor blazor.webview.js:1 TypeError: Cannot read properties of null (reading 'removeChild')
```
Ensure that the VUE.js or other components are wrapped in parent element.
<div>
	Child elements in here
</div>