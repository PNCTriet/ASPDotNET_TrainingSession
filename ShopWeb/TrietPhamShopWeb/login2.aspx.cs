using System;
using System.Web.UI;
using System.Web.Services;
using BussinessLayer;
using BusinessEntity;

namespace TrietPhamShopWeb
{
    public partial class login2 : System.Web.UI.Page
    {
        private readonly UserBL _userBL;

        public login2()
        {
            _userBL = new UserBL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if this is a logout request
                if (Request.QueryString["logout"] == "true")
                {
                    // Clear all session variables
                    Session.Clear();
                    Session.Abandon();
                    lblMessage.Text = "Bạn đã đăng xuất thành công.";
                }
                else
                {
                    // Clear any existing session
                    Session.Clear();
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Log to debug console
                System.Diagnostics.Debug.WriteLine($"[Login] Attempting login for user: {username}");

                // Validate user credentials
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    System.Diagnostics.Debug.WriteLine("[Login] Empty username or password");
                    lblMessage.Text = "Vui lòng nhập đầy đủ thông tin đăng nhập.";
                    return;
                }

                // Create instance of UserBL
                UserBL userBL = new UserBL();

                // Validate user - now returns bool
                System.Diagnostics.Debug.WriteLine("[Login] Validating user credentials...");
                bool isValid = userBL.ValidateUser(username, password);
                System.Diagnostics.Debug.WriteLine($"[Login] Validation result: {isValid}");

                if (isValid)
                {
                    // Get user details for session
                    System.Diagnostics.Debug.WriteLine("[Login] Getting user details...");
                    User user = userBL.GetUserByUsername(username);
                    System.Diagnostics.Debug.WriteLine($"[Login] User found - ID: {user.UserID}, Role: {user.RoleID}");
                    
                    // Set session variables
                    Session["IsAdmin"] = true;
                    Session["Username"] = username;
                    Session["UserID"] = user.UserID;
                    Session["RoleID"] = user.RoleID;

                    System.Diagnostics.Debug.WriteLine("[Login] Session variables set, redirecting to admin page...");
                    // Redirect to admin page
                    Response.Redirect("~/Adminpage/AdminHome.aspx");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[Login] Invalid credentials");
                    lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Login] Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[Login] Stack trace: {ex.StackTrace}");
                lblMessage.Text = "Có lỗi xảy ra: " + ex.Message;
            }
        }

        [WebMethod]
        public static string GetDebugInfo(string username)
        {
            try
            {
                UserBL userBL = new UserBL();
                User user = userBL.GetUserByUsername(username);
                
                if (user != null)
                {
                    return $"User found - ID: {user.UserID}, Username: {user.Username}, Hash: {user.PasswordHash}";
                }
                return "User not found";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}