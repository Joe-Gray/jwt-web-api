using System;
using System.Collections.Generic;

namespace Carvana.MarketExpansion.WebApi.Models
{
    public class User
    {
        public int SecurityUserId { get; set; }
        public Guid SecurityUserGuid { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string RefreshTokenId { get; set; }
        public List<string> SecurityClaims { get; set; }
    }
}