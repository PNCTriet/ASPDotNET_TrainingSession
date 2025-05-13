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
                ShowErrorMessage("Lỗi tải dữ liệu: " + ex.Message);
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
                        // Lấy dữ liệu từ DataKeys
                        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        txtProductName.Text = gvProducts.DataKeys[row.RowIndex]["ProductName"].ToString();
                        txtPrice.Text = gvProducts.DataKeys[row.RowIndex]["Price"].ToString();
                        txtStock.Text = gvProducts.DataKeys[row.RowIndex]["Stock"].ToString();
                        // Hiển thị modal
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "showEditModal();", true);
                        break;

                    case "DeleteProduct":
                        ProductBLL.DeleteProduct(productId);
                        LoadProducts();
                        ShowSuccessMessage("Xóa sản phẩm thành công!");
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi thao tác: " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(hdnProductId.Value);
                string productName = txtProductName.Text.Trim();
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int stock = Convert.ToInt32(txtStock.Text);

                if (ProductBLL.UpdateProduct(productId, productName, price, stock))
                {
                    LoadProducts();
                    ShowSuccessMessage("Cập nhật sản phẩm thành công!");
                }
                else
                {
                    ShowErrorMessage("Cập nhật sản phẩm thất bại!");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void ShowSuccessMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccess", 
                $"alert('{message}');", true);
        }

        private void ShowErrorMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", 
                $"alert('{message}');", true);
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
