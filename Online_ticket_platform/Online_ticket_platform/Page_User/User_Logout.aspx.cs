using System;
using System.Web;
using System.Web.Security;

namespace Online_ticket_platform.Page_User
{
    public partial class User_Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear session
            Session.Clear();
            Session.Abandon();

            // Clear authentication cookie
            FormsAuthentication.SignOut();

            // Clear remember me cookie
            HttpCookie rememberCookie = Request.Cookies["RememberMe"];
            if (rememberCookie != null)
            {
                rememberCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(rememberCookie);
            }

            // Redirect to login page
            Response.Redirect("User_Login.aspx");
        }
    }
} 