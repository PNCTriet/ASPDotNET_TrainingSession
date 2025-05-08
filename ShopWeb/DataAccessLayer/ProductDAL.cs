using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                string query = @"SELECT p.ProductID, p.ProductName, c.CategoryName,
                             p.UnitPrice as Price, p.UnitsInStock as Stock
                             FROM Products p
                             LEFT JOIN Categories c ON p.CategoryID = c.CategoryID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
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
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                conn.Open();

                // Xóa trong Order Details trước
                string deleteOrderDetails = "DELETE FROM [Order Details] WHERE ProductID = @ProductID";
                using (SqlCommand cmdOrderDetails = new SqlCommand(deleteOrderDetails, conn))
                {
                    cmdOrderDetails.Parameters.AddWithValue("@ProductID", id);
                    cmdOrderDetails.ExecuteNonQuery();
                }

                // Sau đó xóa trong Products
                string deleteProduct = "DELETE FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand cmdProduct = new SqlCommand(deleteProduct, conn))
                {
                    cmdProduct.Parameters.AddWithValue("@ProductID", id);
                    cmdProduct.ExecuteNonQuery();
                }
            }
        }

        public static bool UpdateProduct(int productId, string productName, decimal price, int stock)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
                {
                    string query = @"UPDATE Products 
                                   SET ProductName = @ProductName,
                                       UnitPrice = @Price,
                                       UnitsInStock = @Stock
                                   WHERE ProductID = @ProductID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Stock", stock);

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
