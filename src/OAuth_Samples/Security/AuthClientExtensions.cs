using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth_Samples.Security
{
    public static class AuthClientExtensions
    {
        public static AuthLoginResult ToCredentials(this LoginResult loginResult)
        {
            var result = new AuthLoginResult
            {
                AccessToken = loginResult.AccessToken,
                IdentityToken = loginResult.IdentityToken,
                RefreshToken = loginResult.RefreshToken,
                AccessTokenExpiration = loginResult.AccessTokenExpiration
            };

            if (loginResult.User != null && loginResult.User.Claims != null)
            {
                foreach (var claim in loginResult.User.Claims)
                {
                    result.Claims.Add(new AuthLoginResult.ClaimInfo { Key = claim.Type, Value = claim.Value });
                }
            }

            return result;
        }

        public static AuthLoginResult ToCredentials(this RefreshTokenResult refreshTokenResult)
        {
            var result = new AuthLoginResult
            {
                AccessToken = refreshTokenResult.AccessToken,
                IdentityToken = refreshTokenResult.IdentityToken,
                RefreshToken = refreshTokenResult.RefreshToken,
                AccessTokenExpiration = refreshTokenResult.AccessTokenExpiration
            };

            return result;
        }
    }
}
