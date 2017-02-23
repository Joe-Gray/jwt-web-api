using Dapper.Contrib.Extensions;

namespace Carvana.MarketExpansion.WebApi.Data
{
    [Table("AspNetRoleClaims")]
    public class AspNetRoleClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string RoleId { get; set; }
    }
}