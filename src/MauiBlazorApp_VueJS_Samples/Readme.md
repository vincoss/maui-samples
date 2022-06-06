
## Issues
```
blazor blazor.webview.js:1 TypeError: Cannot read properties of null (reading 'removeChild')
```
Ensure that the VUE.js or other components are wrapped in parent element.
<div>
	Child elements in here
</div>