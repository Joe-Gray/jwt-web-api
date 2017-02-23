using System.Data.SqlClient;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public interface ISqlConnectionFactory
    {
        SqlConnection GetOpenSqlConnection();
    }
}