using IdentityModel.Client;
using IdentityModel.OidcClient.Browser;


namespace OAuth_Samples.Security
{
    public class WebBrowserAuthenticator : IdentityModel.OidcClient.Browser.IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            try
            {
                //WebAuthenticatorResult result = await WebAuthenticator.Default.AuthenticateAsync(
                //   new Uri(options.StartUrl),
                //   new Uri(options.EndUrl));

                WebAuthenticatorResult result = null;

#if WINDOWS
                    result = await OAuth_Samples.WinWebAuthenticator.AuthenticateAsync(
                           new Uri(options.StartUrl),
                           new Uri(options.EndUrl));
#else
                result = await WebAuthenticator.Default.AuthenticateAsync(
                               new Uri(options.StartUrl),
                               new Uri(options.EndUrl));
#endif

                var url = new RequestUrl(options.EndUrl)
                    .Create(new Parameters(result.Properties));

                return new BrowserResult
                {
                    Response = url,
                    ResultType = BrowserResultType.Success
                };
            }
            catch (TaskCanceledException)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UserCancel,
                    ErrorDescription = "Login canceled by the user."
                };
            }
        }
    }
}
