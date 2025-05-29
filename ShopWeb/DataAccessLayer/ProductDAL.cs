using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
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

        public static int InsertProductImage(int productId, string imagePath, string altText, string mainImage, byte[] imageBlob)
        {
            using (var conn = new OracleConnection(Connection.GetConnectionString()))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO ProductImages (
                                ImageID, ProductID, ImagePath, AltText, MainImage, CreatedAt, ImageBlob
                            ) VALUES (
                                productimages_seq.nextval, :ProductID, :ImagePath, :AltText, :MainImage, SYSDATE, :ImageBlob
                            )
                            RETURNING ImageID INTO :ImageID";

                        using (var cmd = new OracleCommand(query, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandTimeout = 300;

                            cmd.Parameters.Add(":ProductID", OracleDbType.Int32).Value = productId;
                            cmd.Parameters.Add(":ImagePath", OracleDbType.Varchar2).Value = imagePath;
                            cmd.Parameters.Add(":AltText", OracleDbType.Varchar2).Value = altText;
                            cmd.Parameters.Add(":MainImage", OracleDbType.Varchar2, 1).Value = mainImage;

                            // Handle BLOB parameter
                            var imageBlobParam = new OracleParameter(":ImageBlob", OracleDbType.Blob);
                            if (imageBlob == null)
                            {
                                imageBlobParam.Value = DBNull.Value;
                            }
                            else
                            {
                                imageBlobParam.Value = imageBlob;
                            }
                            cmd.Parameters.Add(imageBlobParam);

                            var imageIdParam = new OracleParameter(":ImageID", OracleDbType.Int32)
                            {
                                Direction = System.Data.ParameterDirection.Output
                            };
                            cmd.Parameters.Add(imageIdParam);

                            cmd.ExecuteNonQuery();
                            int imageId = Convert.ToInt32(imageIdParam.Value.ToString());

                            transaction.Commit();
                            return imageId;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[InsertProductImage] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }


        public static List<ProductImage> GetProductImages(int productId)
        {
            var list = new List<ProductImage>();
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = @"SELECT ImageID, ProductID, ImagePath, AltText, MainImage, CreatedAt
                               FROM ProductImages
                               WHERE ProductID = :ProductID
                               ORDER BY CreatedAt DESC";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":ProductID", OracleDbType.Int32).Value = productId;
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ProductImage
                            {
                                ImageID = Convert.ToInt32(reader["ImageID"]),
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ImagePath = reader["ImagePath"].ToString(),
                                AltText = reader["AltText"].ToString(),
                                MainImage = reader["MainImage"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public static bool DeleteProductImage(int imageId)
        {
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = "DELETE FROM ProductImages WHERE ImageID = :ImageID";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":ImageID", OracleDbType.Int32).Value = imageId;
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public static bool UpdateMainImage(int productId, int imageId)
        {
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                conn.Open();
                using (OracleTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Reset tất cả ảnh của sản phẩm về 'N'
                        string resetQuery = @"UPDATE ProductImages 
                                           SET MainImage = 'N'
                                           WHERE ProductID = :ProductID";
                        using (OracleCommand resetCmd = new OracleCommand(resetQuery, conn))
                        {
                            resetCmd.Transaction = transaction;
                            resetCmd.Parameters.Add(":ProductID", OracleDbType.Int32).Value = productId;
                            resetCmd.ExecuteNonQuery();
                        }

                        // Set ảnh được chọn thành ảnh chính
                        string updateQuery = @"UPDATE ProductImages 
                                            SET MainImage = 'Y'
                                            WHERE ImageID = :ImageID";
                        using (OracleCommand updateCmd = new OracleCommand(updateQuery, conn))
                        {
                            updateCmd.Transaction = transaction;
                            updateCmd.Parameters.Add(":ImageID", OracleDbType.Int32).Value = imageId;
                            updateCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool UpdateProductImage(int imageId, string imagePath, string altText, string mainImage, byte[] imageBlob)
        {
            using (var conn = new OracleConnection(Connection.GetConnectionString()))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE ProductImages 
                            SET ImagePath = :ImagePath,
                                AltText = :AltText,
                                MainImage = :MainImage,
                                ImageBlob = :ImageBlob
                            WHERE ImageID = :ImageID";

                        using (var cmd = new OracleCommand(query, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandTimeout = 300;

                            cmd.Parameters.Add(":ImageID", OracleDbType.Int32).Value = imageId;
                            cmd.Parameters.Add(":ImagePath", OracleDbType.Varchar2).Value = imagePath;
                            cmd.Parameters.Add(":AltText", OracleDbType.Varchar2).Value = altText;
                            cmd.Parameters.Add(":MainImage", OracleDbType.Varchar2, 1).Value = mainImage;

                            // Handle BLOB parameter
                            var imageBlobParam = new OracleParameter(":ImageBlob", OracleDbType.Blob);
                            if (imageBlob == null)
                            {
                                imageBlobParam.Value = DBNull.Value;
                            }
                            else
                            {
                                imageBlobParam.Value = imageBlob;
                            }
                            cmd.Parameters.Add(imageBlobParam);

                            int result = cmd.ExecuteNonQuery();
                            transaction.Commit();
                            return result > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[UpdateProductImage] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public static int InsertProduct(string productName, int categoryId, decimal price, int stock, string description = null)
        {
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = @"INSERT INTO Products (ProductID, ProductName, CategoryID, UnitPrice, UnitsInStock, Description)
                               VALUES (products_seq.nextval, :ProductName, :CategoryID, :Price, :Stock, :Description)
                               RETURNING ProductID INTO :ProductID";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":ProductName", OracleDbType.Varchar2).Value = productName;
                    cmd.Parameters.Add(":CategoryID", OracleDbType.Int32).Value = categoryId;
                    cmd.Parameters.Add(":Price", OracleDbType.Decimal).Value = price;
                    cmd.Parameters.Add(":Stock", OracleDbType.Int32).Value = stock;
                    cmd.Parameters.Add(":Description", OracleDbType.Varchar2).Value = (object)description ?? DBNull.Value;

                    var productIdParam = new OracleParameter(":ProductID", OracleDbType.Int32);
                    productIdParam.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(productIdParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(productIdParam.Value.ToString());
                }
            }
        }

        public static Product GetProductById(int productId)
        {
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = @"SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryName,
                             p.UnitPrice as Price, p.UnitsInStock as Stock, p.Description
                             FROM Products p
                             LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                             WHERE p.ProductID = :ProductID";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":ProductID", OracleDbType.Int32).Value = productId;
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                                CategoryName = reader["CategoryName"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static List<Category> GetAllCategories()
        {
            var list = new List<Category>();
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = @"SELECT CategoryID, CategoryName 
                               FROM Categories 
                               ORDER BY CategoryName";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Category
                            {
                                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                                CategoryName = reader["CategoryName"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
