using System;
using System.Data;
using Oracle.ManagedDataAccess.Client; 

namespace DataAccessLayer.DataConnection
{
    public class Connection
    {
        private static string connectionString = "User Id=banhcuon;Password=123123;Data Source=localhost:1521/XEPDB1;";




        public static string GetConnectionString()
        {
            return connectionString;
        }

        public static OracleConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public static void OpenConnection(OracleConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (OracleException ex)
            {
                // Log error hoặc throw ra tầng trên xử lý
                throw new Exception("Failed to open Oracle connection: " + ex.Message);
            }
        }

        public static void CloseConnection(OracleConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (OracleException ex)
            {
                // Log error hoặc throw ra tầng trên xử lý
                throw new Exception("Failed to close Oracle connection: " + ex.Message);
            }
        }

    }
}
