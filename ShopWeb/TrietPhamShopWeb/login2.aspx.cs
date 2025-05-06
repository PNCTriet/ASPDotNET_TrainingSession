using System;
using System.Web.UI;

namespace TrietPhamShopWeb
{
    public partial class Login2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is already logged in
                if (Session["IsAdmin"] != null && (bool)Session["IsAdmin"])
                {
                    Response.Redirect("~/Adminpage/AdminHome.aspx");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Hard-coded authentication
            if (username == "admin" && password == "@123123")
            {
                // Set session
                Session["IsAdmin"] = true;
                Session["Username"] = username;

                // Redirect to admin page
                Response.Redirect("~/Adminpage/AdminHome.aspx");
            }
            else
            {
                lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
        }
    }
}