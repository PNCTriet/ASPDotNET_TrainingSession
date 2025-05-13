using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace TrietPhamShopWeb
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initial page load logic here if needed
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate if passwords match
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    lblMessage.Text = "Mật khẩu xác nhận không khớp!";
                    lblMessage.Visible = true;
                    return;
                }

                // TODO: Add your registration logic here
                // For example:
                // 1. Check if email already exists
                // 2. Hash the password
                // 3. Save user data to database
                // 4. Send confirmation email
                // 5. Redirect to login page

                // For now, just show a success message
                lblMessage.Text = "Đăng ký thành công!";
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Visible = true;

                // Clear form
                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                txtPhone.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Có lỗi xảy ra: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }
    }
}