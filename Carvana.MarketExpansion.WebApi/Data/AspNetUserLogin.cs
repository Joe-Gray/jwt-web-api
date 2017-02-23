using Dapper.Contrib.Extensions;

namespace Carvana.MarketExpansion.WebApi.Data
{
    [Table("AspNetUserLogins")]
    public class AspNetUserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }
    }
}