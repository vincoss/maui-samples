using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using OidcSamples.Services;
using System.Net.Http.Headers;

namespace OidcSamples.Views;

public partial class HomeView : ContentPage
{
    public HomeView()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
        try
        {
            var clientId = "TODO:";
            var scrope = "scopename";

            var url = $"https://localhost:5010?client_id={clientId}&response_type=code&redirect_uri=oidcsamples://authenticated&scope={scrope}";

#if WINDOWS
        WebAuthenticatorResult authResult = await OidcSamples.Platforms.WinWebAuthenticator.AuthenticateAsync(
               new Uri(url),
               new Uri(IdentityServerConstants.RedirectUri));

       string accessToken = authResult?.AccessToken;

       if(accessToken != null)
       {
       }
            // Do something with the token
#endif

            //WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
            //    new Uri(IdentityServerConstants.AuthorityUri),
            //    new Uri(IdentityServerConstants.RedirectUri));

            //string accessToken = authResult?.AccessToken;

            // Do something with the token
        }
        catch (TaskCanceledException ex)
        {
            // Use stopped auth
        }
    }

    public class Browser : IdentityModel.OidcClient.Browser.IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
#if WINDOWS
        WebAuthenticatorResult authResult = await OidcSamples.Platforms.WinWebAuthenticator.AuthenticateAsync(
               new Uri(IdentityServerConstants.AuthorityUri),
               new Uri(IdentityServerConstants.RedirectUri));

        return new BrowserResult()
            {
                Response = ParseAuthenticatorResult(authResult)
            };
#endif

            throw new InvalidOperationException();
            //WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(IdentityServerConstants.RedirectUri));
            //return new BrowserResult()
            //{
            //    Response = ParseAuthenticatorResult(authResult)
            //};
        }

        /*
        TokenMethod = (grantType, code) =>
        let
        query = [
        client_id = client_id,
        client_secret = client_secret,
        code = code,
        grant_type = "authorization_code",
        code_verifier = "1vu5QqcA1kVvL-TlxpnD8cmDB1AZi3nPc5NIcFzsk0Y",
        redirect_uri = redirect_uri], 
        */
        string ParseAuthenticatorResult(WebAuthenticatorResult result)
        {
            string code = result?.Properties["code"];
            string scope = result?.Properties["scope"];
            string state = result?.Properties["state"];
            //string sessionState = result?.Properties["session_state"];
            return $"https://localhost:5010/connect/token?client_id=TODO:&code={code}&grant_type=authorization_code&state={state}&redirect_uri={IdentityServerConstants.RedirectUri}";
            // return $"{Constants.RedirectUri}#code={code}&scope={scope}&state={state}";
        }
    }
    
    OidcClient _oidcClient;
    LoginResult _loginResult;
    HttpClient _httpClient;

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var options = new OidcClientOptions
        {
            Authority = IdentityServerConstants.AuthorityUri,
            ClientId = IdentityServerConstants.ClientId,
            Scope = IdentityServerConstants.Scope,
            RedirectUri = IdentityServerConstants.RedirectUri,
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