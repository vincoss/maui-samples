// Create a component called <editable-text>
Vue.component('editable-text', {
    data: function () {
        return {
            message: "Change me"
        }
    },
    template: `<div><p>Message is: {{ message }}</p>
<input v-model="message" placeholder="edit me" /></div>`
})

// activate Vue on the <div> that contains the component
new Vue({ el: '#components-demo' })