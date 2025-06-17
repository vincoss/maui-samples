
using System.Threading.Tasks;
using System.Web;


namespace WebAuthenticator_Sample.Views;

public partial class PageOne : ContentPage
{
    private const string _authenticationUrl = "https://localhost:7254/Identity/Account/Login";
    private const string _callbackUrl = "com.companyname.webauthenticator.sample://callback";
    private string AuthToken { get; set; }

    public PageOne()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await OnAuthenticate(null);
    }


    private static string GetReturnUrlQuery()
    {
        var uriBuilder = new UriBuilder(_authenticationUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query.Add("redirect_uri", _callbackUrl);
        query.Add("state", Guid.NewGuid().ToString());

        return query.ToString();
    }

    /// <summary>
    /// scheme: Apple
    /// </summary>
    /// <param name="scheme"></param>
    /// <returns></returns>
    private async Task OnAuthenticate(string scheme)
    {
        var returnUrl = GetReturnUrlQuery();
        var localAuthenticationUrl = $"{_authenticationUrl.TrimEnd(new[] { '/' })}?returnUrl=/{_callbackUrl}?{returnUrl}";

#if WINDOWS

             try
             {
              OAuth_Samples.WebAuthenticatorResult result = null;
                    result = await OAuth_Samples.WinWebAuthenticator.AuthenticateAsync(
                           new Uri(localAuthenticationUrl),
                           new Uri(_callbackUrl));

                           
                var url = OAuth_Samples.WebAuthenticatorResult.ToRawIdentityUrl(_callbackUrl, result);
             }
             catch(Exception ex)
             {
             }
#endif
        try
        {
            WebAuthenticatorResult r = null;

            if (scheme.Equals("Apple", StringComparison.Ordinal)
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
                var callbackUrl = new Uri(_callbackUrl);

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