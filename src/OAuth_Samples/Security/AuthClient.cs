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
            try
            {
                return await _oidcClient.LoginAsync();
            }
            catch (Exception ex) // Will crash if disco is not available
            {
                // TODO: log here
                return new LoginResult("Error", "Contact admin error");
            }
        }

        public async Task<BrowserResult> LogoutAsync()
        {
            try
            {
                var result = await _oidcClient.LogoutAsync();

                if (result != null)
                {
                }

                return null;
            }
            catch (Exception ex) // Will crash if disco is not available
            {
                // TODO: log here

                return new BrowserResult
                {
                    ResultType = BrowserResultType.HttpError,
                    ErrorDescription = "Contact admin error"
            };
            }
        }
        public async Task<AuthLoginResult> RefreshToken(string refreshToken)
        {
            try
            {
                var refreshTokenResult = await _oidcClient.RefreshTokenAsync(refreshToken);
                return refreshTokenResult.ToCredentials();
            }
            catch (Exception ex) // Will crash if disco is not available
            {

                return new AuthLoginResult { Error = ex.ToString() };
            }
        }
    }
}