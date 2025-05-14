using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLayer;
using BusinessEntity.Entities;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Load danh sách vai trò
                    LoadRoles();

                    // Load danh sách người dùng
                    DataTable dtUsers = _userBL.GetAllUsers();
                    System.Diagnostics.Debug.WriteLine($"Page_Load: Number of users from BL: {dtUsers.Rows.Count}");
                    
                    gvUsers.DataSource = dtUsers;
                    gvUsers.DataBind();

                    System.Diagnostics.Debug.WriteLine($"Page_Load: GridView rows after binding: {gvUsers.Rows.Count}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in Page_Load: {ex.Message}");
                    // Xử lý lỗi ở đây
                }
            }
        }

        private void LoadRoles()
        {
            try
            {
                DataTable dtRoles = _userBL.GetAllRoles();
                ddlRole.DataSource = dtRoles;
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataValueField = "RoleID";
                ddlRole.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading roles: {ex.Message}");
                throw;
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                // Xử lý sự kiện edit
            }
            else if (e.CommandName == "DeleteUser")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    if (_userBL.DeleteUser(userId))
                    {
                        // Refresh GridView
                        DataTable dtUsers = _userBL.GetAllUsers();
                        gvUsers.DataSource = dtUsers;
                        gvUsers.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error deleting user: {ex.Message}");
                    // Xử lý lỗi ở đây
                }
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            DataTable dtUsers = _userBL.GetAllUsers();
            gvUsers.DataSource = dtUsers;
            gvUsers.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(hdnUserId.Value);
                User user = _userBL.GetUserById(userId);
                
                if (user != null)
                {
                    user.Username = txtUsername.Text;
                    user.Email = txtEmail.Text;
                    user.IsActive = chkIsActive.Checked;
                    user.RoleID = Convert.ToInt32(ddlRole.SelectedValue);

                    if (_userBL.UpdateUser(user))
                    {
                        // Refresh GridView
                        DataTable dtUsers = _userBL.GetAllUsers();
                        gvUsers.DataSource = dtUsers;
                        gvUsers.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving user: {ex.Message}");
                // Xử lý lỗi ở đây
            }
        }
    }
}