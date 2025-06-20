
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace WebAuthenticator_Sample.Views;

public partial class PageOne : ContentPage
{
    public PageOne()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var scheme = DeviceInfo.Platform == DevicePlatform.iOS ? "Apple" : null;

        await OnAuthenticate(scheme);
    }

    /// <summary>
    /// scheme: Apple
    /// </summary>
    /// <param name="scheme"></param>
    /// <returns></returns>
    private async Task OnAuthenticate(string? scheme)
    {
        var localAuthenticationUrl = $"{WebAuthenticatorConstants.AuthenticationUrl.TrimEnd(new[] { '/' })}?returnUrl={WebAuthenticatorConstants.CallbackUrl}";
        WebAuthenticatorResult? result = null;

#if WINDOWS

             try
             {
                result = await WinWebAuthenticator.AuthenticateAsync(
                         new Uri(localAuthenticationUrl),
                         new Uri(WebAuthenticatorConstants.CallbackUrl));
             } 
             catch (OperationCanceledException)
             {
                entryError.Text = "Login canceled.";
             }
             catch(Exception ex)
             {
                entryError.Text = ex.ToString();
             }
#else
        try
        {
            if (string.IsNullOrWhiteSpace(scheme) == false && scheme.Equals("Apple", StringComparison.OrdinalIgnoreCase)
                && DeviceInfo.Platform == DevicePlatform.iOS
                && DeviceInfo.Version.Major >= 13)
            {
                // Make sure to enable Apple Sign In in both the entitlements and the provisioning profile.
                var options = new AppleSignInAuthenticator.Options
                {
                    IncludeEmailScope = true,
                    IncludeFullNameScope = true,
                };
                result = await AppleSignInAuthenticator.AuthenticateAsync(options);
            }
            else
            {
                var authUrl = new Uri(localAuthenticationUrl);
                var callbackUrl = new Uri(WebAuthenticatorConstants.CallbackUrl);

                result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
            }
        }
        catch (OperationCanceledException)
        {
            entryError.Text = "Login canceled.";
        }
        catch (Exception ex)
        {
            entryError.Text = ex.ToString();
        }
#endif

        if(result != null)
        {
            entryCallbackUri.Text = result.CallbackUri.ToString();
            entryTimestamp.Text = result.Timestamp.ToString();
            entryAccessToken.Text = result.AccessToken;
            entryRefreshToken.Text = result.RefreshToken.ToString();
            entryIdToken.Text = result.IdToken.ToString();
            entryRefreshTokenExpiresIn.Text = result.RefreshTokenExpiresIn.ToString();
            entryExpiresIn.Text = result.ExpiresIn.ToString();
        }
    }
}