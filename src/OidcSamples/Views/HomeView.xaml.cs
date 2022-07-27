using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System.Net.Http.Headers;

namespace OidcSamples.Views;

public partial class HomeView : ContentPage
{
    public static string authority = "";
    public static string callback = "myapp://authenticated";

    public HomeView()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
        try
        {
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri(authority),
                new Uri(callback));

            string accessToken = authResult?.AccessToken;

            // Do something with the token
        }
        catch (TaskCanceledException ex)
        {
            // Use stopped auth
        }
    }

    public static class Constants
    {
        public static string AuthorityUri = "";
        public static string RedirectUri = "myapp:/authenticated";
        public static string ApiUri = "";
        public static string ClientId = "";
        public static string Scope = "";
    }

    public class Browser : IdentityModel.OidcClient.Browser.IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(Constants.RedirectUri));
            return new BrowserResult()
            {
                Response = ParseAuthenticatorResult(authResult)
            };
        }

        string ParseAuthenticatorResult(WebAuthenticatorResult result)
        {
            string code = result?.Properties["code"];
            string scope = result?.Properties["scope"];
            string state = result?.Properties["state"];
            //string sessionState = result?.Properties["session_state"];
            return $"{Constants.RedirectUri}#code={code}&scope={scope}&state={state}";
        }
    }
    
    OidcClient _oidcClient;
    LoginResult _loginResult;
    HttpClient _httpClient;

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var options = new OidcClientOptions
        {
            Authority = Constants.AuthorityUri,
            ClientId = Constants.ClientId,
            Scope = Constants.Scope,
            RedirectUri = Constants.RedirectUri,
          //  ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
            Browser = new Browser()
        };

        _oidcClient = new OidcClient(options);
        _loginResult = await _oidcClient.LoginAsync(new LoginRequest());
        if (_loginResult.IsError)
        {
            Console.WriteLine("ERROR: {0}", _loginResult.Error);
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loginResult?.AccessToken ?? string.Empty);
    }
}