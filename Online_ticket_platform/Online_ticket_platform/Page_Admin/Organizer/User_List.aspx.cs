using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;
using System.Web.Services;

namespace Online_ticket_platform.Page_Admin.Organizer
{
    public partial class User_List : System.Web.UI.Page
    {
        private readonly BLL_IUserService _userService;

        public User_List()
        {
            _userService = new BLL_UserService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            var users = _userService.GetAllUsers();
            gvUsers.DataSource = users;
            gvUsers.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var user = new MOD_User
                {
                    Email = txtEmail.Text.Trim(),
                    PasswordHash = txtPassword.Text.Trim(), // Trong thực tế cần mã hóa mật khẩu
                    Name = txtName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Role = ddlRole.SelectedValue,
                    IsVerified = chkIsVerified.Checked,
                    RegisteredAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                if (_userService.AddUser(user))
                {
                    LoadUsers();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Thêm người dùng thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể thêm người dùng!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(hdnEditId.Value);
                var user = _userService.GetUserById(userId);
                if (user == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không tìm thấy người dùng!');", true);
                    return;
                }

                // Kiểm tra email mới có bị trùng không
                var existingUser = _userService.GetUserByEmail(txtEditEmail.Text.Trim());
                if (existingUser != null && existingUser.Id != userId)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Email này đã được sử dụng bởi người dùng khác!');", true);
                    return;
                }

                // Cập nhật thông tin
                user.Email = txtEditEmail.Text.Trim();
                user.Name = txtEditName.Text.Trim();
                user.Phone = txtEditPhone.Text.Trim();
                user.Role = ddlEditRole.SelectedValue;
                user.IsVerified = chkEditIsVerified.Checked;

                if (_userService.UpdateUser(user))
                {
                    LoadUsers();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Cập nhật người dùng thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể cập nhật người dùng! Vui lòng kiểm tra lại thông tin.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(hdnDeleteId.Value);
                bool forceDelete = Request.Form["forceDelete"] == "true";

                if (forceDelete)
                {
                    _userService.DeleteRelatedData(userId);
                }

                if (_userService.DeleteUser(userId))
                {
                    LoadUsers();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Xóa người dùng thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể xóa người dùng!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        [WebMethod]
        public static List<string> CheckConstraints(int userId)
        {
            try
            {
                var userService = new BLL_UserService();
                return userService.GetRelatedDataInfo(userId);
            }
            catch
            {
                return new List<string>();
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    hdnEditId.Value = userId.ToString();
                    txtEditEmail.Text = user.Email;
                    txtEditName.Text = user.Name;
                    txtEditPhone.Text = user.Phone;
                    ddlEditRole.SelectedValue = user.Role;
                    chkEditIsVerified.Checked = user.IsVerified;

                    ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", 
                        "$(document).ready(function() { $('#editModal').modal('show'); });", true);
                }
            }
            else if (e.CommandName == "DeleteUser")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    hdnDeleteId.Value = userId.ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showDeleteModal", 
                        "$(document).ready(function() { $('#deleteModal').modal('show'); });", true);
                }
            }
        }
    }
}