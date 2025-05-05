using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrietPhamShopWeb.Adminpage
{
    public partial class AdminHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Xử lý logic khi trang được tải
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Xử lý logic khi nút Submit được nhấn
            string productEdit = txtProductEdit.Text;
            string productName = txtProductName.Text;
            string supplier = ddlSupplier.SelectedValue;
            string category = txtCategory.Text;
            string quantityPerUnit = txtQuantityPerUnit.Text;
            decimal unitPrice = decimal.Parse(txtUnitPrice.Text);
            int unitStock = int.Parse(txtUnitStock.Text);
            // TODO: Xử lý dữ liệu ở đây

            // Xóa nội dung các input sau khi submit
            txtProductEdit.Text = string.Empty;
            txtProductName.Text = string.Empty;
            ddlSupplier.SelectedIndex = 0;
            txtCategory.Text = string.Empty;
            txtQuantityPerUnit.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtUnitStock.Text = string.Empty;
        }
    }
}