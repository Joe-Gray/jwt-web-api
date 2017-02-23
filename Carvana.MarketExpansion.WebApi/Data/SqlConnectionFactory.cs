using System.Data.SqlClient;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private string connectionString = "Server=.;Database=aspnet-market_web_dev;Trusted_Connection=True;";

        public SqlConnection GetOpenSqlConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}