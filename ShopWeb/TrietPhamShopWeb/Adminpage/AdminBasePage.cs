using System;
using System.Web.UI;

namespace TrietPhamShopWeb.Adminpage
{
    public class AdminBasePage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CheckAdminAuthentication();
        }

        private void CheckAdminAuthentication()
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
            {
                Response.Redirect("~/Login2.aspx");
            }
        }
    }
} 