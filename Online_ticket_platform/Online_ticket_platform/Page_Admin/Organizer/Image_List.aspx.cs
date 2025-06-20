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
    public partial class Image_List : System.Web.UI.Page
    {
        private readonly BLL_IImageService _imageService;

        public Image_List()
        {
            _imageService = new BLL_ImageService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadImages();
            }
        }

        private void LoadImages()
        {
            try
            {
                var images = _imageService.GetAllImages();
                gvImages.DataSource = images;
                gvImages.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi tải danh sách hình ảnh: {ex.Message}');", true);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(txtFilePath.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng nhập đường dẫn file!');", true);
                    return;
                }

                // Create new image
                var image = new MOD_Image
                {
                    FilePath = txtFilePath.Text.Trim(),
                    FileName = txtFileName.Text.Trim(),
                    FileType = txtFileType.Text.Trim(),
                    FileSize = !string.IsNullOrEmpty(txtFileSize.Text) ? Convert.ToInt32(txtFileSize.Text) : (int?)null,
                    AltText = txtAltText.Text.Trim(),
                    UploadedBy = !string.IsNullOrEmpty(txtUploadedBy.Text) ? Convert.ToInt32(txtUploadedBy.Text) : (int?)null,
                    UploadedAt = DateTime.Now
                };

                int newId = _imageService.InsertImage(image);
                
                if (newId > 0)
                {
                    LoadImages();
                    ClearAddForm();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Thêm hình ảnh thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể thêm hình ảnh!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi thêm hình ảnh: {ex.Message}');", true);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(hdnEditId.Value))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('ID hình ảnh không hợp lệ!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(txtEditFilePath.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Vui lòng nhập đường dẫn file!');", true);
                    return;
                }

                // Update image
                var image = new MOD_Image
                {
                    Id = Convert.ToInt32(hdnEditId.Value),
                    FilePath = txtEditFilePath.Text.Trim(),
                    FileName = txtEditFileName.Text.Trim(),
                    FileType = txtEditFileType.Text.Trim(),
                    FileSize = !string.IsNullOrEmpty(txtEditFileSize.Text) ? Convert.ToInt32(txtEditFileSize.Text) : (int?)null,
                    AltText = txtEditAltText.Text.Trim(),
                    UploadedBy = !string.IsNullOrEmpty(txtEditUploadedBy.Text) ? Convert.ToInt32(txtEditUploadedBy.Text) : (int?)null
                };

                bool result = _imageService.UpdateImage(image);
                
                if (result)
                {
                    LoadImages();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Cập nhật hình ảnh thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể cập nhật hình ảnh!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi cập nhật hình ảnh: {ex.Message}');", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int imageId = Convert.ToInt32(hdnDeleteId.Value);
                bool result = _imageService.DeleteImage(imageId);
                
                if (result)
                {
                    LoadImages();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Xóa hình ảnh thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể xóa hình ảnh!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi xóa hình ảnh: {ex.Message}');", true);
            }
        }

        protected void gvImages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int imageId = Convert.ToInt32(e.CommandArgument);
                LoadImageForEdit(imageId);
            }
        }

        protected void gvImages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvImages.PageIndex = e.NewPageIndex;
            LoadImages();
        }

        private void LoadImageForEdit(int imageId)
        {
            try
            {
                var image = _imageService.GetImageById(imageId);
                if (image != null)
                {
                    hdnEditId.Value = image.Id.ToString();
                    txtEditFilePath.Text = image.FilePath;
                    txtEditFileName.Text = image.FileName;
                    txtEditFileType.Text = image.FileType;
                    txtEditFileSize.Text = image.FileSize?.ToString();
                    txtEditAltText.Text = image.AltText;
                    txtEditUploadedBy.Text = image.UploadedBy?.ToString();

                    ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", 
                        "$(document).ready(function() { $('#editModal').modal('show'); });", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Lỗi tải thông tin hình ảnh: {ex.Message}');", true);
            }
        }

        private void ClearAddForm()
        {
            txtFilePath.Text = "";
            txtFileName.Text = "";
            txtFileType.Text = "";
            txtFileSize.Text = "";
            txtAltText.Text = "";
            txtUploadedBy.Text = "";
        }
    }
}