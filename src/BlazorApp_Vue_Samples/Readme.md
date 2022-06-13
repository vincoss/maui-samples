
## CDN
https://cdn.jsdelivr.net/npm/vue@2.x/dist/vue.js
https://cdn.jsdelivr.net/npm/vue
https://www.jsdelivr.com/?query=vu

## Resources
https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor
https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0
https://github.com/EdCharbeneau/BlazorSize
https://remibou.github.io/How-to-keep-js-object-reference-in-Blazor/

### Vue
https://jsfiddle.net/
https://v2.vuejs.org/v2/guide/
https://vuejs.org/guide/essentials/component-basics.html#defining-a-component
https://stackoverflow.com/questions/52612446/importing-a-package-in-es6-failed-to-resolve-module-specifier-vue

## Issues
```
blazor blazor.webview.js:1 TypeError: Cannot read properties of null (reading 'removeChild')
```
Ensure that the VUE.js or other components are wrapped in parent element.
<div>
	<div id="appvue">
       <h1>{{ message }}</h1>
    </div>
</div>