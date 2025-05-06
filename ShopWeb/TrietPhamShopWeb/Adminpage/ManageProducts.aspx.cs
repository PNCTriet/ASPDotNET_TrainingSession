using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using TrietPhamShopWeb.Models;

namespace TrietPhamShopWeb.Adminpage
{
    public partial class ManageProducts : AdminBasePage
    //public partial class ManageProducts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT p.ProductID, p.ProductName, c.CategoryName, p.UnitPrice as Price, p.UnitsInStock as Stock FROM Products p LEFT JOIN Categories c ON p.CategoryID = c.CategoryID", conn))
                    {
                        conn.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gvProducts.DataSource = dt;
                            gvProducts.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                Response.Write($"<script>alert('Lỗi: {ex.Message}');</script>");
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            // TODO: Chuyển hướng đến trang thêm sản phẩm
            Response.Redirect("~/Adminpage/AddProduct.aspx");
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditProduct")
            {
                string productId = e.CommandArgument.ToString();
                Response.Redirect($"EditProduct.aspx?id={productId}");
            }
            else if (e.CommandName == "DeleteProduct")
            {
                string productId = e.CommandArgument.ToString();
                DeleteProduct(productId);
            }
        }

        private void DeleteProduct(string productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID = @ProductID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadProducts(); // Reload the grid
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi xóa sản phẩm: {ex.Message}');</script>");
            }
        }
    }
}