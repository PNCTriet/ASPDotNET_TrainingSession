using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TrietPhamShopWeb.Adminpage
{
    public partial class Adminsite : System.Web.UI.MasterPage
    {
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Xóa session
            Session.Clear();
            Session.Abandon();

            // Xóa authentication cookie
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            // Xóa Forms Authentication cookie
            FormsAuthentication.SignOut();

            // Chuyển hướng về trang login
            Response.Redirect("~/Login2.aspx");
        }
    }
}