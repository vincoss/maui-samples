
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


