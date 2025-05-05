using System;
using System.Web.UI;

namespace TrietPhamShopWeb.Controls
{
    public partial class Navbar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set active link based on current page
                string currentPage = Request.Path.ToLower();
                
                lnkHome.CssClass = currentPage.Contains("default.aspx") ? "nav-link active" : "nav-link";
                lnkAdmin.CssClass = currentPage.Contains("adminhome.aspx") ? "nav-link active" : "nav-link";
            }
        }
    }
}