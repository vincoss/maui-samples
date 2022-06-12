
export function vue2ComponentParameter(context)
{
    console.log("vue2ComponentParameter");
    console.log(context);

    // TODO: possible to have watch on context change tand then call function in here

    const app = new Vue(
        {
            data()
            {
                return {
                    context: context
                }
            },
            methods: {
                setContext: function (event)
                {
                    console.log("vue2ComponentParameter.setVueContext");
                    console.log(event);
                    this.context.data = event.data;

                },
                onSave: function (data)
                {
                    console.log(data);
                    DotNet.invokeMethodAsync('BlazorApp_Vue_Samples', 'OnSave', data)
                        .then(data =>
                        {
                            console.log(data);
                            console.log("done");
                        });
                },
                onValidate: function (data)
                {
                    DotNet.invokeMethodAsync('BlazorApp_Vue_Samples', 'OnValidate', data)
                },
            }
        });

    context.save = app.onSave;
    context.validate = app.onValidate;
    app.$mount("#components-demo");

    return app;
};

export function setVueContext(vue, context)
{
    console.log("setVueContext");
    console.log(vue);
    console.log(context);

    vue.setContext(context);
};