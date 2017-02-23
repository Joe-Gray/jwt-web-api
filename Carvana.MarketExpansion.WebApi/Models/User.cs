﻿using System.Collections.Generic;

namespace Carvana.MarketExpansion.WebApi.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public List<string> SecurityClaims { get; set; }
    }
}