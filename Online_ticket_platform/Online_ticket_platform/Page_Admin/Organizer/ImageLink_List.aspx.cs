using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Online_ticket_platform_Model;
using Online_ticket_platform_BLL.Interfaces;
using Online_ticket_platform_BLL.Services;

namespace Online_ticket_platform.Page_Admin.Organizer
{
    public partial class ImageLink_List : System.Web.UI.Page
    {
        private readonly BLL_IImageLinkService _imageLinkService;
        private readonly BLL_IImageService _imageService;

        public ImageLink_List()
        {
            _imageLinkService = new BLL_ImageLinkService();
            _imageService = new BLL_ImageService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadImageLinks();
                LoadDropdownData();
            }
        }

        private void LoadImageLinks()
        {
            try
            {
                var imageLinks = _imageLinkService.GetAllImageLinks();
                gvImageLinks.DataSource = imageLinks;
                gvImageLinks.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi tải danh sách liên kết hình ảnh: {ex.Message}');", true);
            }
        }

        private void LoadDropdownData()
        {
            try
            {
                // Load images
                var images = _imageService.GetAllImages();
                ddlImage.Items.Clear();
                ddlImage.Items.Add(new ListItem("-- Chọn hình ảnh --", ""));
                foreach (var image in images)
                {
                    ddlImage.Items.Add(new ListItem($"{image.FileName} ({image.FilePath})", image.Id.ToString()));
                }

                ddlEditImage.Items.Clear();
                ddlEditImage.Items.Add(new ListItem("-- Chọn hình ảnh --", ""));
                foreach (var image in images)
                {
                    ddlEditImage.Items.Add(new ListItem($"{image.FileName} ({image.FilePath})", image.Id.ToString()));
                }

                // Load organizations
                var organizations = _imageLinkService.GetAllOrganizationsForImageLink();
                ddlOrganization.Items.Clear();
                ddlOrganization.Items.Add(new ListItem("-- Chọn tổ chức --", ""));
                foreach (var org in organizations)
                {
                    ddlOrganization.Items.Add(new ListItem(org.Name, org.Id.ToString()));
                }
                ddlEditOrganization.Items.Clear();
                ddlEditOrganization.Items.Add(new ListItem("-- Chọn tổ chức --", ""));
                foreach (var org in organizations)
                {
                    ddlEditOrganization.Items.Add(new ListItem(org.Name, org.Id.ToString()));
                }

                // Load events
                var events = _imageLinkService.GetAllEventsForImageLink();
                ddlEvent.Items.Clear();
                ddlEvent.Items.Add(new ListItem("-- Chọn sự kiện --", ""));
                foreach (var evt in events)
                {
                    ddlEvent.Items.Add(new ListItem(evt.Name, evt.Id.ToString()));
                }
                ddlEditEvent.Items.Clear();
                ddlEditEvent.Items.Add(new ListItem("-- Chọn sự kiện --", ""));
                foreach (var evt in events)
                {
                    ddlEditEvent.Items.Add(new ListItem(evt.Name, evt.Id.ToString()));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi tải dữ liệu dropdown: {ex.Message}');", true);
            }
        }

        protected void btnAddImageLink_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(ddlImage.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng chọn hình ảnh!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(ddlEntityType.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng chọn loại entity!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(txtEntityId.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng nhập ID entity!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(ddlUsageType.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng chọn loại sử dụng!');", true);
                    return;
                }

                // Create new image link
                var imageLink = new MOD_ImageLink
                {
                    ImageId = Convert.ToInt32(ddlImage.SelectedValue),
                    EntityType = ddlEntityType.SelectedValue,
                    EntityId = Convert.ToInt32(txtEntityId.Text),
                    UsageType = ddlUsageType.SelectedValue,
                    OrganizationId = !string.IsNullOrEmpty(ddlOrganization.SelectedValue) ? Convert.ToInt32(ddlOrganization.SelectedValue) : (int?)null,
                    EventId = !string.IsNullOrEmpty(ddlEvent.SelectedValue) ? Convert.ToInt32(ddlEvent.SelectedValue) : (int?)null,
                    LinkedAt = DateTime.Now
                };

                int newId = _imageLinkService.InsertImageLink(imageLink);
                
                if (newId > 0)
                {
                    LoadImageLinks();
                    ClearAddForm();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Thêm liên kết hình ảnh thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể thêm liên kết hình ảnh!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi thêm liên kết hình ảnh: {ex.Message}');", true);
            }
        }

        protected void btnUpdateImageLink_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(hdnEditImageLinkId.Value))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('ID liên kết hình ảnh không hợp lệ!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(ddlEditImage.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng chọn hình ảnh!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(ddlEditEntityType.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng chọn loại entity!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(txtEditEntityId.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng nhập ID entity!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(ddlEditUsageType.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng chọn loại sử dụng!');", true);
                    return;
                }

                // Update image link
                var imageLink = new MOD_ImageLink
                {
                    Id = Convert.ToInt32(hdnEditImageLinkId.Value),
                    ImageId = Convert.ToInt32(ddlEditImage.SelectedValue),
                    EntityType = ddlEditEntityType.SelectedValue,
                    EntityId = Convert.ToInt32(txtEditEntityId.Text),
                    UsageType = ddlEditUsageType.SelectedValue,
                    OrganizationId = !string.IsNullOrEmpty(ddlEditOrganization.SelectedValue) ? Convert.ToInt32(ddlEditOrganization.SelectedValue) : (int?)null,
                    EventId = !string.IsNullOrEmpty(ddlEditEvent.SelectedValue) ? Convert.ToInt32(ddlEditEvent.SelectedValue) : (int?)null
                };

                bool result = _imageLinkService.UpdateImageLink(imageLink);
                
                if (result)
                {
                    LoadImageLinks();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Cập nhật liên kết hình ảnh thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể cập nhật liên kết hình ảnh!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi cập nhật liên kết hình ảnh: {ex.Message}');", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int imageLinkId = Convert.ToInt32(hdnDeleteId.Value);
                bool result = _imageLinkService.DeleteImageLink(imageLinkId);
                
                if (result)
                {
                    LoadImageLinks();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Xóa liên kết hình ảnh thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể xóa liên kết hình ảnh!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi xóa liên kết hình ảnh: {ex.Message}');", true);
            }
        }

        protected void gvImageLinks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int imageLinkId = Convert.ToInt32(e.CommandArgument);
                LoadImageLinkForEdit(imageLinkId);
            }
        }

        protected void gvImageLinks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvImageLinks.PageIndex = e.NewPageIndex;
            LoadImageLinks();
        }

        private void LoadImageLinkForEdit(int imageLinkId)
        {
            try
            {
                var imageLink = _imageLinkService.GetImageLinkById(imageLinkId);
                if (imageLink != null)
                {
                    hdnEditImageLinkId.Value = imageLink.Id.ToString();
                    ddlEditImage.SelectedValue = imageLink.ImageId.ToString();
                    ddlEditEntityType.SelectedValue = imageLink.EntityType;
                    txtEditEntityId.Text = imageLink.EntityId.ToString();
                    ddlEditUsageType.SelectedValue = imageLink.UsageType;
                    
                    if (imageLink.OrganizationId.HasValue)
                        ddlEditOrganization.SelectedValue = imageLink.OrganizationId.ToString();
                    
                    if (imageLink.EventId.HasValue)
                        ddlEditEvent.SelectedValue = imageLink.EventId.ToString();

                    ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", 
                        "$(document).ready(function() { $('#editModal').modal('show'); });", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi tải thông tin liên kết hình ảnh: {ex.Message}');", true);
            }
        }

        private void ClearAddForm()
        {
            ddlImage.SelectedIndex = 0;
            ddlEntityType.SelectedIndex = 0;
            txtEntityId.Text = "";
            ddlUsageType.SelectedIndex = 0;
            ddlOrganization.SelectedIndex = 0;
            ddlEvent.SelectedIndex = 0;
        }
    }
}