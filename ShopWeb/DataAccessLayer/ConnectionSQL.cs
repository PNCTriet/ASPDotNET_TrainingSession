using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.DataConnection
{
    public class ConnectionSQL
    {
        private static string connectionString = "Data Source=.;Initial Catalog=Northwind3;Integrated Security=True";

        public static string GetConnectionString()
        {
            return connectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static void OpenConnection(SqlConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public static void CloseConnection(SqlConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
