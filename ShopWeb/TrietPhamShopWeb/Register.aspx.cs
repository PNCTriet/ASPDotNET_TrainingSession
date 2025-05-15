using System;
using System.Web.UI;
using BussinessLayer;
using BusinessEntity;

namespace TrietPhamShopWeb
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly UserBL _userBL;

        public Register()
        {
            _userBL = new UserBL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any existing messages
                lblMessage.Visible = false;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate password match
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ShowError("Mật khẩu xác nhận không khớp");
                    return;
                }

                // Create new user
                var user = new User
                {
                    Username = txtEmail.Text.Split('@')[0], // Tạo username từ email
                    Email = txtEmail.Text,
                    PasswordHash = txtPassword.Text,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Phone = txtPhone.Text
                };

                // Try to create user
                if (_userBL.CreateUser(user))
                {
                    // Redirect to login page on success
                    Response.Redirect("~/login2.aspx");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}