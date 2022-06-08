
window.vue2Mimimal = function ()
{
    var appvuenew = new Vue({
        el: '#appvue',
        data: {
            message: 'Hello Vue!!!'
        }
    });
};

window.vue2Parameter = function (message)
{
    var appvuenew = new Vue({
        el: '#appvue',
        data: {
            message: message
        }
    });
};

window.vue2ComponentParameter = function (context)
{
    Vue.component('data-context-component', './assets/src/components/DataContextComponent.vue');

    // activate Vue on the <div> that contains the component
    new Vue({ el: '#components-demo' }) 
};