using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;

namespace Online_ticket_platform.Page_User
{
    public partial class User_Register : System.Web.UI.Page
    {
        private readonly BLL_IUserService _userService;

        public User_Register()
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
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(txtName.Text.Trim()) || 
                    string.IsNullOrEmpty(txtEmail.Text.Trim()) || 
                    string.IsNullOrEmpty(txtPassword.Text))
                {
                    ShowMessage("Please fill in all required fields.", false);
                    return;
                }

                // Check if email already exists
                var existingUser = _userService.GetUserByEmail(txtEmail.Text.Trim());
                if (existingUser != null)
                {
                    ShowMessage("Email address is already registered. Please use a different email or try logging in.", false);
                    return;
                }

                // Create new user
                var newUser = new MOD_User
                {
                    Email = txtEmail.Text.Trim(),
                    PasswordHash = txtPassword.Text, // In production, hash the password
                    Name = txtName.Text.Trim(),
                    Phone = string.IsNullOrEmpty(txtPhone.Text.Trim()) ? null : txtPhone.Text.Trim(),
                    Role = "user", // Default role for new registrations
                    IsVerified = true, // For testing purposes, set to true
                    RegisteredAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                // Add user using BLL service
                if (_userService.AddUser(newUser))
                {
                    ShowMessage("Registration successful! You can now login with your email and password.", true);
                    
                    // Clear form
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtPhone.Text = "";
                    txtPassword.Text = "";
                    txtConfirmPassword.Text = "";
                }
                else
                {
                    ShowMessage("Registration failed. Please try again.", false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"An error occurred during registration: {ex.Message}", false);
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