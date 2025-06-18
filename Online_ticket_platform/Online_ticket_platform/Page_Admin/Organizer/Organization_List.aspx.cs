using System;
using System.Data;
using Online_ticket_platform_BLL.Services;
using Online_ticket_platform_Model;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace Online_ticket_platform.Page_Admin
{
    public partial class Organization_List : System.Web.UI.Page
    {
        private readonly BLL_OrganizationService _organizationService;

        public Organization_List()
        {
            _organizationService = new BLL_OrganizationService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrganizations();
            }
        }

        private void LoadOrganizations()
        {
            try
            {
                var organizations = _organizationService.GetAllOrganizations();
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("contact_email", typeof(string));
                dt.Columns.Add("phone", typeof(string));
                dt.Columns.Add("address", typeof(string));
                dt.Columns.Add("created_at", typeof(DateTime));

                foreach (var org in organizations)
                {
                    dt.Rows.Add(org.Id, org.Name, org.ContactEmail, org.Phone, org.Address, org.CreatedAt);
                }

                gvOrganizations.DataSource = dt;
                gvOrganizations.DataBind();
            }
            catch (Exception ex)
            {
                ShowError("Không thể tải danh sách tổ chức. Vui lòng thử lại sau.", ex.Message);
            }
        }

        private void ClearCreateForm()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress.Text = string.Empty;
        }

        private void ClearEditForm()
        {
            hdnEditId.Value = string.Empty;
            txtEditName.Text = string.Empty;
            txtEditEmail.Text = string.Empty;
            txtEditPhone.Text = string.Empty;
            txtEditAddress.Text = string.Empty;
        }

        private bool ValidateOrganization(MOD_Organization organization, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(organization.Name))
            {
                errorMessage = "Tên tổ chức không được để trống.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(organization.ContactEmail))
            {
                errorMessage = "Email không được để trống.";
                return false;
            }

            if (!IsValidEmail(organization.ContactEmail))
            {
                errorMessage = "Email không hợp lệ.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(organization.Phone))
            {
                errorMessage = "Số điện thoại không được để trống.";
                return false;
            }

            if (!IsValidPhone(organization.Phone))
            {
                errorMessage = "Số điện thoại không hợp lệ.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(organization.Address))
            {
                errorMessage = "Địa chỉ không được để trống.";
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^[0-9]{10,11}$");
        }

        private void ShowError(string message, string systemError = null)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowError",
                $"showError('{message}', '{systemError}');", true);
        }

        private void ShowSuccess(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccess",
                $"showSuccess('{message}');", true);
        }

        protected void gvOrganizations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int organizationId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditOrganization")
            {
                try
                {
                    var organization = _organizationService.GetOrganizationById(organizationId);
                    if (organization != null)
                    {
                        hdnEditId.Value = organization.Id.ToString();
                        txtEditName.Text = organization.Name;
                        txtEditEmail.Text = organization.ContactEmail;
                        txtEditPhone.Text = organization.Phone;
                        txtEditAddress.Text = organization.Address;

                        // Show edit modal using JavaScript
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", 
                            @"$(document).ready(function() {
                                $('#editModal').modal('show');
                            });", true);
                    }
                    else
                    {
                        ShowError("Không tìm thấy thông tin tổ chức.");
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Có lỗi xảy ra khi tải thông tin tổ chức.", ex.Message);
                }
            }
            else if (e.CommandName == "DeleteOrganization")
            {
                hdnDeleteId.Value = organizationId.ToString();
                // Show delete confirmation modal using JavaScript
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowDeleteModal", 
                    @"$(document).ready(function() {
                        $('#deleteModal').modal('show');
                    });", true);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var organization = new MOD_Organization
                {
                    Name = txtName.Text.Trim(),
                    ContactEmail = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };

                string errorMessage;
                if (!ValidateOrganization(organization, out errorMessage))
                {
                    ShowError(errorMessage);
                    return;
                }

                if (_organizationService.AddOrganization(organization))
                {
                    // Clear form
                    ClearCreateForm();

                    // Hide modal using JavaScript
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideCreateModal", 
                        "$('#createModal').modal('hide');", true);

                    // Show success message
                    ShowSuccess("Thêm tổ chức mới thành công!");

                    // Reload data
                    LoadOrganizations();
                }
                else
                {
                    ShowError("Không thể thêm tổ chức mới. Vui lòng kiểm tra lại thông tin.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Có lỗi xảy ra khi thêm tổ chức mới.", ex.Message);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var organization = new MOD_Organization
                {
                    Id = Convert.ToInt32(hdnEditId.Value),
                    Name = txtEditName.Text.Trim(),
                    ContactEmail = txtEditEmail.Text.Trim(),
                    Phone = txtEditPhone.Text.Trim(),
                    Address = txtEditAddress.Text.Trim()
                };

                string errorMessage;
                if (!ValidateOrganization(organization, out errorMessage))
                {
                    ShowError(errorMessage);
                    return;
                }

                if (_organizationService.UpdateOrganization(organization))
                {
                    // Clear form
                    ClearEditForm();

                    // Hide modal using JavaScript
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideEditModal", 
                        "$('#editModal').modal('hide');", true);

                    // Show success message
                    ShowSuccess("Cập nhật thông tin tổ chức thành công!");

                    // Reload data
                    LoadOrganizations();
                }
                else
                {
                    ShowError("Không thể cập nhật thông tin tổ chức. Vui lòng kiểm tra lại thông tin.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Có lỗi xảy ra khi cập nhật thông tin tổ chức.", ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int organizationId = Convert.ToInt32(hdnDeleteId.Value);
                bool forceDelete = Request.Form["forceDelete"] == "true";

                if (forceDelete)
                {
                    // Xóa tất cả dữ liệu liên quan
                    DeleteRelatedData(organizationId);
                }
                else if (_organizationService.HasRelatedData(organizationId))
                {
                    throw new Exception("Không thể xóa tổ chức này vì có dữ liệu liên quan. Vui lòng xóa các dữ liệu liên quan trước hoặc sử dụng tùy chọn 'Xóa tất cả dữ liệu liên quan'.");
                }

                if (_organizationService.DeleteOrganization(organizationId))
                {
                    // Hide modal using JavaScript
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideDeleteModal", 
                        "$('#deleteModal').modal('hide');", true);

                    // Show success message
                    ShowSuccess("Xóa tổ chức thành công!");

                    // Reload data
                    LoadOrganizations();
                }
                else
                {
                    ShowError("Không thể xóa tổ chức. Vui lòng thử lại sau.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Có lỗi xảy ra khi xóa tổ chức.", ex.Message);
            }
        }

        private void DeleteRelatedData(int organizationId)
        {
            // Xóa tất cả dữ liệu liên quan thông qua service
            _organizationService.DeleteRelatedData(organizationId);
        }

        [WebMethod]
        public static List<string> CheckConstraints(int organizationId)
        {
            var constraints = new List<string>();
            var organizationService = new BLL_OrganizationService();

            if (organizationService.HasRelatedData(organizationId))
            {
                // Lấy thông tin ràng buộc từ service
                var relatedData = organizationService.GetRelatedDataInfo(organizationId);
                constraints.AddRange(relatedData);
            }

            return constraints;
        }
    }
}