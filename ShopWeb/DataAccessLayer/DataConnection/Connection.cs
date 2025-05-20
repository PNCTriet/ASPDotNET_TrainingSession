using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace DataAccessLayer.DataConnection
{
    public class Connection
    {
        private static string connectionString = ConfigurationManager
            .ConnectionStrings["OracleConn"]
            .ConnectionString;

        public static string GetConnectionString()
        {
            return connectionString;
        }

        public static OracleConnection GetConnection()
        {
            try
            {
                return new OracleConnection(connectionString);
            }
            catch (OracleException ex)
            {
                throw new Exception("Failed to create Oracle connection: " + ex.Message);
            }
        }

        public static void OpenConnection(OracleConnection connection)
        {
            try
            {
                if (connection == null)
                    throw new ArgumentNullException(nameof(connection), "Connection object cannot be null");

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Failed to open Oracle connection: {ex.Message}. Error Code: {ex.Number}");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while opening connection: " + ex.Message);
            }
        }

        public static void CloseConnection(OracleConnection connection)
        {
            try
            {
                if (connection == null) return;

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            catch (OracleException ex)
            {
                throw new Exception($"Failed to close Oracle connection: {ex.Message}. Error Code: {ex.Number}");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while closing connection: " + ex.Message);
            }
        }
    }
}
