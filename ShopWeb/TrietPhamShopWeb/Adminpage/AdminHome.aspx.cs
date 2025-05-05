using System;

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
            // TODO: Xử lý dữ liệu ở đây
        }
    }
} 