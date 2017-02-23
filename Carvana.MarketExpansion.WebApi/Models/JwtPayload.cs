using System.Collections.Generic;

namespace Carvana.MarketExpansion.WebApi.Models
{
    public class JwtPayload
    {
        public string iss { get; set; }
        public string sub { get; set; }
        public string aud { get; set; }
        public string exp { get; set; }
        public string nbf { get; set; }
        public string iat { get; set; }
        public string jti { get; set; }
        public string userEmail { get; set; }
        public string userId { get; set; }
        public List<string> userSecurityClaims { get; set; }
        public string tokenType { get; set; }
    }
}