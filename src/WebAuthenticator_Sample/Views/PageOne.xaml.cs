
using System.Threading.Tasks;
using System.Web;


namespace WebAuthenticator_Sample.Views;

public partial class PageOne : ContentPage
{
    private string AuthToken { get; set; }

    public PageOne()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await OnAuthenticate(null);
    }

    /// <summary>
    /// scheme: Apple
    /// </summary>
    /// <param name="scheme"></param>
    /// <returns></returns>
    private async Task OnAuthenticate(string scheme)
    {
        var localAuthenticationUrl = $"{WebAuthenticatorConstants.AuthenticationUrl.TrimEnd(new[] { '/' })}?returnUrl={WebAuthenticatorConstants.CallbackUrl}";


#if WINDOWS

             try
             {

            // var auth = WebAuthenticator.Default;
             
            //var options = new WebAuthenticatorOptions();
            //options.CallbackUrl = new Uri(_callbackUrl);
            //options.Url = new Uri(_authenticationUrl);

            //WebAuthenticatorResult resulta = null;

            // resulta =  await auth.AuthenticateAsync(options);

            // if(resulta != null)
            // {
            // }

              var result = await OAuth_Samples.WinWebAuthenticator.AuthenticateAsync(
                           new Uri(localAuthenticationUrl),
                           new Uri(WebAuthenticatorConstants.CallbackUrl));

                           
                //var url = OAuth_Samples.WebAuthenticatorResult.ToRawIdentityUrl(WebAuthenticatorConstants.CallbackUrl, result);
             }
             catch(Exception ex)
             {
             }

             return;
#endif
        try
        {
            WebAuthenticatorResult r = null;

            if (string.IsNullOrWhiteSpace(scheme) == false && scheme.Equals("Apple", StringComparison.Ordinal)
                && DeviceInfo.Platform == DevicePlatform.iOS
                && DeviceInfo.Version.Major >= 13)
            {
                // Make sure to enable Apple Sign In in both the
                // entitlements and the provisioning profile.
                var options = new AppleSignInAuthenticator.Options
                {
                    IncludeEmailScope = true,
                    IncludeFullNameScope = true,
                };
                r = await AppleSignInAuthenticator.AuthenticateAsync(options);
            }
            else
            {
                var authUrl = new Uri(localAuthenticationUrl);
                var callbackUrl = new Uri(WebAuthenticatorConstants.CallbackUrl);

                r = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
            }

            AuthToken = string.Empty;
            if (r.Properties.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
                AuthToken += $"Name: {name}{Environment.NewLine}";
            if (r.Properties.TryGetValue("email", out var email) && !string.IsNullOrEmpty(email))
                AuthToken += $"Email: {email}{Environment.NewLine}";
            AuthToken += r?.AccessToken ?? r?.IdToken;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Login canceled.");

            AuthToken = string.Empty;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Failed: {ex.Message}");

            AuthToken = string.Empty;
        }
    }
}