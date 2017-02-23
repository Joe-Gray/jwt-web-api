using Dapper.Contrib.Extensions;

namespace Carvana.MarketExpansion.WebApi.Data
{
    [Table("AspNetRoles")]
    public class AspNetRole
    {
        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}