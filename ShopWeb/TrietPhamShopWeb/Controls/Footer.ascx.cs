using System;
using System.Web.UI;

namespace TrietPhamShopWeb.Controls
{
    public partial class Footer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set active link based on current page
                string currentPage = Request.Path.ToLower();
                
                lnkFooterHome.CssClass = currentPage.Contains("default.aspx") ? "active" : "";
                lnkFooterProducts.CssClass = currentPage.Contains("products.aspx") ? "active" : "";
                lnkFooterAbout.CssClass = currentPage.Contains("about.aspx") ? "active" : "";
                lnkFooterContact.CssClass = currentPage.Contains("contact.aspx") ? "active" : "";
            }
        }
    }
}