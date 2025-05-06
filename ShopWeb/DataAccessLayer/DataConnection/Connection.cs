using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class Connection
    {
        private static string connectionString = "Data Source=.;Initial Catalog=ShopDB;Integrated Security=True";

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
