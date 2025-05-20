using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.Entities;
using DataAccessLayer.DataConnection;

namespace DataAccessLayer
{
    public class ProductDAL
    {
        public static List<Product> GetAllProducts()
        {
            var list = new List<Product>();
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = @"SELECT p.ProductID, p.ProductName, c.CategoryName,
                             p.UnitPrice as Price, p.UnitsInStock as Stock
                             FROM Products p
                             LEFT JOIN Categories c ON p.CategoryID = c.CategoryID";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Product
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Stock = Convert.ToInt32(reader["Stock"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public static void DeleteProduct(int id)
        {
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                conn.Open();

                // Xóa trong Order Details trước
                string deleteOrderDetails = "DELETE FROM OrderDetails WHERE ProductID = :ProductID";
                using (OracleCommand cmdOrderDetails = new OracleCommand(deleteOrderDetails, conn))
                {
                    cmdOrderDetails.Parameters.Add(":ProductID", OracleDbType.Int32).Value = id;
                    cmdOrderDetails.ExecuteNonQuery();
                }

                // Sau đó xóa trong Products
                string deleteProduct = "DELETE FROM Products WHERE ProductID = :ProductID";
                using (OracleCommand cmdProduct = new OracleCommand(deleteProduct, conn))
                {
                    cmdProduct.Parameters.Add(":ProductID", OracleDbType.Int32).Value = id;
                    cmdProduct.ExecuteNonQuery();
                }
            }
        }

        public static bool UpdateProduct(int productId, string productName, decimal price, int stock)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
                {
                    string query = @"UPDATE Products 
                                   SET ProductName = :ProductName,
                                       UnitPrice = :Price,
                                       UnitsInStock = :Stock
                                   WHERE ProductID = :ProductID";
                    
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(":ProductName", OracleDbType.Varchar2).Value = productName;
                        cmd.Parameters.Add(":Price", OracleDbType.Decimal).Value = price;
                        cmd.Parameters.Add(":Stock", OracleDbType.Int32).Value = stock;
                        cmd.Parameters.Add(":ProductID", OracleDbType.Int32).Value = productId;

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }
    }
}
