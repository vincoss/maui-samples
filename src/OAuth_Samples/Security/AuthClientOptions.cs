
namespace OAuth_Samples.Security
{
    public class AuthClientOptions
    {
        public AuthClientOptions()
        {
            Scope = "openid";
            RedirectUri = "oauthsamples://callback";
            Browser = new WebBrowserAuthenticator();
        }

        public string Domain { get; set; }

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string Scope { get; set; }

        public IdentityModel.OidcClient.Browser.IBrowser Browser { get; set; }
    }
}
