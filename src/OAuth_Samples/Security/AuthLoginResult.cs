using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth_Samples.Security
{
    public class AuthLoginResult
    {
        public string AccessToken { get; set; } = "";
        public string IdentityToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTimeOffset AccessTokenExpiration { get; set; }
        public string Error { get; set; } = "";
        public bool IsError => !string.IsNullOrEmpty(Error);
        
        public IList<ClaimInfo> Claims { get; private set; } = new List<ClaimInfo>();

        public class ClaimInfo
        { 
            public string Key { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return $"{Key}-{Value}";
            }
        }

    }
}
