using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Isis.ApplicationBlocks.DataMigrations
{
	internal static class DatabaseManager
	{
		internal static void GetSmoConnection(string connectionString, out SqlConnection conn, out ServerConnection serverConnection, out Database database)
		{
			conn = new SqlConnection(connectionString);
			serverConnection = new ServerConnection(conn);
			Server server = new Server(serverConnection);
			database = server.Databases[serverConnection.DatabaseName];
		}

		internal static string CreateDatabase(string databaseServer, string databaseName)
		{
			Server server = new Server(databaseServer);
			Database db = new Database(server, databaseName);
			db.Create();

			return string.Format(@"Server={0};Database={1};Integrated Security=True", databaseServer, databaseName);
		}
	}
}