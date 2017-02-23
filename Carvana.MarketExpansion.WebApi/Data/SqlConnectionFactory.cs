using System.Data.SqlClient;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private string connectionString = "Data Source=carvanadev.database.windows.net;Initial Catalog=CarvanaOLTP;User ID=SVC-Application-DEV;Password=Carvana!1;";

        public SqlConnection GetOpenSqlConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}