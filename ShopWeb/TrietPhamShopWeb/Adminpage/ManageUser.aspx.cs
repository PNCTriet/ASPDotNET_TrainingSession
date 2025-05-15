using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLayer;
using BusinessEntity;

namespace TrietPhamShopWeb.Adminpage
{
    public partial class ManageUser : System.Web.UI.Page
    {
        private readonly UserBL _userBL;

        public ManageUser()
        {
            _userBL = new UserBL();
        }

        protected string GetStatusClass(object isActive)
        {
            if (isActive == null) return "status-blocked";
            bool active = Convert.ToBoolean(isActive);
            return active ? "status-active" : "status-inactive";
        }

        protected string GetStatusText(object isActive)
        {
            if (isActive == null) return "Blocked";
            bool active = Convert.ToBoolean(isActive);
            return active ? "Active" : "Inactive";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
                LoadRoles();
            }
        }

        private void LoadUsers()
        {
            try
            {
                List<User> users = _userBL.GetAllUsers();
                gvUsers.DataSource = users;
                gvUsers.DataBind();
                
                // Add script to show logs in browser console
                string script = $"console.log('[BL] Number of users loaded: {users.Count}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "LogMessage", script, true);
            }
            catch (Exception ex)
            {
                // Show error message
                string script = $"console.error('{ex.Message.Replace("'", "\\'")}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", script, true);
            }
        }

        private void LoadRoles()
        {
            try
            {
                List<Role> roles = _userBL.GetAllRoles();
                ddlRole.DataSource = roles;
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataValueField = "RoleID";
                ddlRole.DataBind();
            }
            catch (Exception ex)
            {
                string script = $"console.error('{ex.Message.Replace("'", "\\'")}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", script, true);
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                try
                {
                    int userId = Convert.ToInt32(e.CommandArgument);
                    if (_userBL.DeleteUser(userId))
                    {
                        LoadUsers();
                        string script = "console.log('[BL] User deleted successfully');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "LogMessage", script, true);
                    }
                }
                catch (Exception ex)
                {
                    string script = $"console.error('{ex.Message.Replace("'", "\\'")}');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", script, true);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                User user = new User
                {
                    UserID = Convert.ToInt32(hdnUserId.Value),
                    Username = txtUsername.Text,
                    Email = txtEmail.Text,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    RoleID = Convert.ToInt32(ddlRole.SelectedValue),
                    IsActive = chkIsActive.Checked
                };

                if (_userBL.UpdateUser(user))
                {
                    LoadUsers();
                    string script = "console.log('[BL] User updated successfully');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "LogMessage", script, true);
                }
            }
            catch (Exception ex)
            {
                string script = $"console.error('{ex.Message.Replace("'", "\\'")}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ErrorMessage", script, true);
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            LoadUsers();
        }
    }
}