using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using BusinessLogic;
using BusinessEntity.Entities;
using DataAccessLayer.DataConnection;
using System.Collections.Generic;

namespace TrietPhamShopWeb.Adminpage
{
    public partial class ManageProducts : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                var products = ProductBLL.GetAllProducts();
                gvProducts.DataSource = products;
                gvProducts.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi tải dữ liệu: {ex.Message}');</script>");
            }
        }

        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;
            LoadProducts();
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "EditProduct":
                        // Không cần xử lý ở đây nữa vì đã chuyển sang Ajax
                        break;

                    case "DeleteProduct":
                        ProductBLL.DeleteProduct(productId);
                        LoadProducts(); // Refresh lại dữ liệu
                        // Thông báo thành công và refresh DataTables
                        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteSuccess", 
                            "alert('Xóa sản phẩm thành công!'); " +
                            "if (typeof $.fn.DataTable !== 'undefined') { " +
                            "   var table = $('#" + gvProducts.ClientID + "').DataTable(); " +
                            "   table.destroy(); " +
                            "   table = $('#" + gvProducts.ClientID + "').DataTable(); " +
                            "}", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi thao tác: {ex.Message}');</script>");
            }
        }

        [WebMethod]
        public static object GetProductById(int productId)
        {
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                string query = @"SELECT p.ProductID, p.ProductName, c.CategoryName, 
                                p.UnitPrice as Price, p.UnitsInStock as Stock
                                FROM Products p
                                LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                                WHERE p.ProductID = @ProductID";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new
                            {
                                ProductID = reader["ProductID"],
                                ProductName = reader["ProductName"],
                                CategoryName = reader["CategoryName"],
                                Price = reader["Price"],
                                Stock = reader["Stock"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        [WebMethod]
        public static bool UpdateProduct(int productId, string productName, decimal price, int stock)
        {
            return ProductBLL.UpdateProduct(productId, productName, price, stock);
        }

        [WebMethod]
        public static List<Product> GetAllProducts()
        {
            try
            {
                return ProductBLL.GetAllProducts();
            }
            catch (Exception ex)
            {
                // Log error
                return new List<Product>();
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Adminpage/AddProduct.aspx");
        }
    }
}
