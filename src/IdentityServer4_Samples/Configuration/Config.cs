using IdentityServer4.Models;


namespace IdentityServer4_Samples.Configuration
{
    public class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "IdentityServer4_Samples")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1", "openid" },
                    RedirectUris = new[]
                    {
                       "oauthsamples://callback"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "oauthsamples://callback"
                    },
                }
            };
    }
}