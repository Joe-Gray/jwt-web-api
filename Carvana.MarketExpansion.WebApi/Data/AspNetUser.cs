using Dapper.Contrib.Extensions;

namespace Carvana.MarketExpansion.WebApi.Data
{
    [Table("AspNetUsers")]
    public class AspNetUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}