using System;
using System.Configuration;

namespace TrietPhamShopWeb.Models
{
    public class Connection
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
        }
    }
} 