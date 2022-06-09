
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
    console.log("vue2ComponentParameter: " + context);

    const app = new Vue(
        {
            data: {
                context: context
            }
        });
    app.$mount("#components-demo");
};