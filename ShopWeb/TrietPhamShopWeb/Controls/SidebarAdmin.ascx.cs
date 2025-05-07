using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TrietPhamShopWeb.Controls
{
    public partial class SidebarAdmin : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Có thể thêm logic khởi tạo ở đây nếu cần
            }
        }
    }
}