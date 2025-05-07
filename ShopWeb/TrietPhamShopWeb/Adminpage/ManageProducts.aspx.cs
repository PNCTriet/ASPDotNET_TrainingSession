using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessEntity.Entities;

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
                        Response.Redirect($"EditProduct.aspx?id={productId}");
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

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Adminpage/AddProduct.aspx");
        }
    }
}
