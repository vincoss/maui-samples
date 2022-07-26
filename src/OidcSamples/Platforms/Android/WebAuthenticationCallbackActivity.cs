using Android.App;
using Android.Content.PM;

namespace OidcSamples.Platforms
{
    //[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    //[IntentFilter(new[] { Android.Content.Intent.ActionView },
    //     Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
    //     DataScheme = MainPage.CallbackUri)]
    //public class WebAuthenticationCallbackActivity : Microsoft.Maui.WebAuthenticatorCallbackActivity
    //{
    //}

    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
              Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
              DataScheme = MainPage.CallbackUri)]
    public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {

    }
}
