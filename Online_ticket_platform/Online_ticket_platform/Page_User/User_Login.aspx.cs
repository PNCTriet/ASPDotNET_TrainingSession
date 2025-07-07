using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;

namespace Online_ticket_platform.Page_User
{
    public partial class User_Login : System.Web.UI.Page
    {
        private readonly BLL_IUserService _userService;

        public User_Login()
        {
            _userService = new BLL_UserService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is already logged in
                if (Session["UserID"] != null)
                {
                    Response.Redirect("User_Home.aspx");
                }

                // Check for remember me cookie
                CheckRememberMeCookie();
            }
        }

        private void CheckRememberMeCookie()
        {
            HttpCookie rememberCookie = Request.Cookies["RememberMe"];
            if (rememberCookie != null)
            {
                string email = rememberCookie["Email"];
                if (!string.IsNullOrEmpty(email))
                {
                    txtEmail.Text = email;
                    chkRememberMe.Checked = true;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    ShowMessage("Please enter both email and password.", false);
                    return;
                }

                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                // Authenticate user using BLL service
                if (!_userService.AuthenticateUser(email, password))
                {
                    ShowMessage("Invalid email or password.", false);
                    return;
                }

                // Get user details for session
                var user = _userService.GetUserByEmail(email);
                if (user == null)
                {
                    ShowMessage("User not found.", false);
                    return;
                }

                // Login successful
                LoginUser(user);

                // Handle remember me
                if (chkRememberMe.Checked)
                {
                    SetRememberMeCookie(email);
                }
                else
                {
                    RemoveRememberMeCookie();
                }

                // Redirect based on user role
                RedirectBasedOnRole(user.Role);

            }
            catch (Exception ex)
            {
                ShowMessage($"An error occurred during login: {ex.Message}", false);
            }
        }

        private void LoginUser(MOD_User user)
        {
            // Set session variables
            Session["UserID"] = user.Id;
            Session["UserEmail"] = user.Email;
            Session["UserName"] = user.Name;
            Session["UserRole"] = user.Role;
            Session["IsLoggedIn"] = true;

            // Create authentication ticket with user data
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: user.Email,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(30),
                isPersistent: chkRememberMe.Checked,
                userData: user.Role, // Store role in userData
                cookiePath: FormsAuthentication.FormsCookiePath
            );

            // Encrypt the ticket
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            if (chkRememberMe.Checked)
            {
                authCookie.Expires = ticket.Expiration;
            }
            Response.Cookies.Add(authCookie);
        }

        private void SetRememberMeCookie(string email)
        {
            HttpCookie cookie = new HttpCookie("RememberMe");
            cookie["Email"] = email;
            cookie.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(cookie);
        }

        private void RemoveRememberMeCookie()
        {
            HttpCookie cookie = new HttpCookie("RememberMe");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
        }

        private void RedirectBasedOnRole(string role)
        {
            switch (role.ToLower())
            {
                case "admin":
                    Response.Redirect("../Page_Admin/Organizer/Admin_Home");
                    break;
                case "organizer":
                    Response.Redirect("../Page_Admin/Organizer/Admin_Home");
                    break;
                case "user":
                case "guest":
                default:
                    Response.Redirect("User_Home.aspx");
                    break;
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            litMessage.Text = message;
        }
    }
}