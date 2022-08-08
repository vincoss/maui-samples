using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.Client;


namespace OAuth_Samples.Security
{
    public class AuthClient
    {
        private readonly OidcClient _oidcClient;

        public AuthClient(AuthClientOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = $"https://{options.Domain.ToLowerInvariant()}",
                ClientId = options.ClientId,
                Scope = options.Scope,
                RedirectUri = options.RedirectUri,
                Browser = options.Browser
            });
        }

        public IdentityModel.OidcClient.Browser.IBrowser Browser
        {
            get
            {
                return _oidcClient.Options.Browser;
            }
            set
            {
                _oidcClient.Options.Browser = value;
            }
        }

        public async Task<LoginResult> LoginAsync()
        {
            return await _oidcClient.LoginAsync();
        }

        public async Task<BrowserResult> LogoutAsync()
        {
            var logoutParameters = new Dictionary<string, string>
            {
                  {"client_id", _oidcClient.Options.ClientId },
                  {"returnTo", _oidcClient.Options.RedirectUri }
            };

            var logoutRequest = new LogoutRequest();
            var endSessionUrl = new RequestUrl($"{_oidcClient.Options.Authority}/v2/logout").Create(new Parameters(logoutParameters));
            var browserOptions = new BrowserOptions(endSessionUrl, _oidcClient.Options.RedirectUri)
            {
                Timeout = TimeSpan.FromSeconds(logoutRequest.BrowserTimeout),
                DisplayMode = logoutRequest.BrowserDisplayMode
            };

            var browserResult = await _oidcClient.Options.Browser.InvokeAsync(browserOptions);

            return browserResult;
        }
    }
}