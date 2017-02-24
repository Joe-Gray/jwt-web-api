using System.Data.SqlClient;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private string connectionString = "Data Source=carvanadev.database.windows.net;Initial Catalog=CarvanaOLTP;User ID=SVC-Application-DEV;Password=Carvana!1;";
        private string marketAuthConnectionString = "Data Source=carvanadev.database.windows.net;Initial Catalog=microservices;User ID=CVDeveloper;Password=57Ch3vY!;";

        public SqlConnection GetOpenSqlConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public SqlConnection GetOpenAuthDatabaseSqlConnection()
        {
            var connection = new SqlConnection(marketAuthConnectionString);
            connection.Open();
            return connection;
        }
    }
}