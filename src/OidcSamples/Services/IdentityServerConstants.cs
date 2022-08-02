using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OidcSamples.Services
{
    public static class IdentityServerConstants
    {
        public const string Scheme = "oidcsamples";
        public static string AuthorityUri = "https://localhost:5010";
        public static string RedirectUri = $"{Scheme}://authenticated";
        public static string ApiUri = "https://localhost:5010";
        public static string ClientId = "TODO:";
        public static string Scope = "TODO:";
    }
}
