
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
    console.log("vue2ComponentParameter");
    console.log(context);

    const app = new Vue(
        {
            data()
            {
                return {
                    context: context
                }
                    },
            //data: {
            //    context: context
            //},
            methods: {
                setContext: function (event)
                {
                    console.log("vue2ComponentParameter.setVueContext");
                    console.log(event);
                    this.context.data = event.data;
                    
                    DotNet.invokeMethodAsync('BlazorApp_Vue_Samples', 'testSave')
                        .then(data =>
                        {
                            console.log("done");
                        });
                },
            }
        });
    app.$mount("#components-demo");

    return app;
};

window.setVueContext = function (vue, context)
{
    console.log("setVueContext");
    console.log(vue);
    console.log(context);

    vue.setContext(context);
};

export function setVueContexte(vue, context)
{
    console.log("setVueContext");
    console.log(vue);
    console.log(context);

    vue.setContext(context);
}