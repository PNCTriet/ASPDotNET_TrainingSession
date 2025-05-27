using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using BusinessLogic;
using BusinessEntity.Entities;
using DataAccessLayer.DataConnection;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using WebImage = System.Web.UI.WebControls.Image;
using DrawingImage = System.Drawing.Image;
using System.Linq;

namespace TrietPhamShopWeb.Adminpage
{
    public partial class ManageProducts : AdminBasePage
    {
        public LinkButton CommandSource { get; private set; }
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
                LoadCategories();
            }
        }

        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;
            LoadProducts();
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "EditProduct":
                        LoadProductForEdit(productId);
                        break;

                    case "DeleteProduct":
                        ProductBLL.DeleteProduct(productId);
                        LoadProducts();
                        ShowSuccessMessage("Xóa sản phẩm thành công!");
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi thao tác: " + ex.Message);
            }
        }
        #endregion

        #region Product Management
        // <================================>
        // LOAD PRODUCTS AND CATEGORIES
        // <================================>
        private void LoadProducts()
        {
            try
            {
                var products = ProductBLL.GetAllProducts();
                gvProducts.DataSource = products;
                gvProducts.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = ProductBLL.GetAllCategories();
                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("-- Chọn danh mục --", ""));
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi tải danh mục: " + ex.Message);
            }
        }

        // <================================>
        // EDIT PRODUCT
        // <================================>
        private void LoadProductForEdit(int productId)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)CommandSource).NamingContainer);
            txtProductName.Text = gvProducts.DataKeys[row.RowIndex]["ProductName"].ToString();
            txtPrice.Text = gvProducts.DataKeys[row.RowIndex]["Price"].ToString();
            txtStock.Text = gvProducts.DataKeys[row.RowIndex]["Stock"].ToString();
            hdnProductId.Value = productId.ToString();
            LoadProductImages(productId);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "showEditModal();", true);
        }

        private void LoadProductImages(int productId)
        {
            throw new NotImplementedException();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(hdnProductId.Value);
                string productName = txtProductName.Text.Trim();
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int stock = Convert.ToInt32(txtStock.Text);

                // Cập nhật thông tin sản phẩm
                if (ProductBLL.UpdateProduct(productId, productName, price, stock))
                {
                    // Xử lý upload ảnh nếu có
                    if (FileUpload1.HasFile)
                    {
                        string imagePath = SaveNewProductImage(FileUpload1, productId, 1);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            ProductBLL.AddProductImage(productId, imagePath, productName, "Y");
                        }
                    }
                    if (FileUpload2.HasFile)
                    {
                        string imagePath = SaveNewProductImage(FileUpload2, productId, 2);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            ProductBLL.AddProductImage(productId, imagePath, productName, "N");
                        }
                    }
                    if (FileUpload3.HasFile)
                    {
                        string imagePath = SaveNewProductImage(FileUpload3, productId, 3);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            ProductBLL.AddProductImage(productId, imagePath, productName, "N");
                        }
                    }

                    LoadProducts();
                    ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", 
                        "$('#editProductModal').modal('hide');", true);
                    ShowSuccessMessage("Cập nhật sản phẩm thành công!");
                }
                else
                {
                    ShowErrorMessage("Cập nhật sản phẩm thất bại!");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi cập nhật: " + ex.Message);
            }
        }

        // <================================>
        // CREATE NEW PRODUCT
        // <================================>
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateNewProductInput()) return;

                int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                string productName = txtNewProductName.Text.Trim();
                decimal price = Convert.ToDecimal(txtNewPrice.Text);
                int stock = Convert.ToInt32(txtNewStock.Text);
                string description = txtNewDescription.Text.Trim();

                int productId = ProductBLL.InsertProduct(productName, categoryId, price, stock, description);

                if (productId > 0)
                {
                    // Xử lý ảnh sản phẩm
                    if (newFileUpload1.HasFile)
                    {
                        string imagePath = SaveNewProductImage(newFileUpload1, productId, 1);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            ProductBLL.AddProductImage(productId, imagePath, productName, "Y");
                        }
                    }
                    if (newFileUpload2.HasFile)
                    {
                        string imagePath = SaveNewProductImage(newFileUpload2, productId, 2);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            ProductBLL.AddProductImage(productId, imagePath, productName, "N");
                        }
                    }
                    if (newFileUpload3.HasFile)
                    {
                        string imagePath = SaveNewProductImage(newFileUpload3, productId, 3);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            ProductBLL.AddProductImage(productId, imagePath, productName, "N");
                        }
                    }

                    LoadProducts();
                    ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", 
                        "$('#createProductModal').modal('hide');", true);
                    ShowSuccessMessage("Tạo sản phẩm thành công!");
                }
                else
                {
                    ShowErrorMessage("Tạo sản phẩm thất bại!");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi tạo sản phẩm: " + ex.Message);
            }
        }

        private bool ValidateNewProductInput()
        {
            // Validation is now handled by ASP.NET Validators
            return true;
        }

        private string SaveNewProductImage(FileUpload fileUpload, int productId, int imageNumber)
        {
            try
            {
                if (!ValidateImageFile(fileUpload)) return null;

                string uploadDir = Server.MapPath("~/uploads/products/");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string extension = Path.GetExtension(fileUpload.FileName).ToLower();
                string fileName = $"{productId}_{imageNumber}{extension}";
                string filePath = Path.Combine(uploadDir, fileName);
                string tempPath = Path.Combine(uploadDir, "temp_" + fileName);

                fileUpload.SaveAs(tempPath);

                try
                {
                    ProcessImageResize(tempPath, filePath, extension);
                    return $"~/uploads/products/{fileName}";
                }
                finally
                {
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi lưu ảnh: " + ex.Message);
                return null;
            }
        }

        private bool ValidateImageFile(FileUpload fileUpload)
        {
            if (fileUpload.FileBytes.Length > 5 * 1024 * 1024)
            {
                ShowErrorMessage("Kích thước file không được vượt quá 5MB");
                return false;
            }

            string extension = Path.GetExtension(fileUpload.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
            {
                ShowErrorMessage("Chỉ chấp nhận file ảnh định dạng JPG, JPEG hoặc PNG");
                return false;
            }

            return true;
        }

        private void ProcessImageResize(string sourcePath, string targetPath, string extension)
        {
            using (DrawingImage originalImage = DrawingImage.FromFile(sourcePath))
            {
                if (originalImage.Width > 800 || originalImage.Height > 800)
                {
                    int newWidth = originalImage.Width;
                    int newHeight = originalImage.Height;

                    if (newWidth > newHeight)
                    {
                        if (newWidth > 800)
                        {
                            newHeight = (int)((float)newHeight * 800 / newWidth);
                            newWidth = 800;
                        }
                    }
                    else
                    {
                        if (newHeight > 800)
                        {
                            newWidth = (int)((float)newWidth * 800 / newHeight);
                            newHeight = 800;
                        }
                    }

                    using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics g = Graphics.FromImage(resizedImage))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                        }

                        if (extension == ".jpg" || extension == ".jpeg")
                        {
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                            resizedImage.Save(targetPath, jpgEncoder, encoderParams);
                        }
                        else
                        {
                            resizedImage.Save(targetPath, originalImage.RawFormat);
                        }
                    }
                }
                else
                {
                    File.Copy(sourcePath, targetPath, true);
                }
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        // <================================>
        // IMAGE HANDLING
        // <================================>
        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                int imageNumber = Convert.ToInt32(btn.CommandArgument);
                bool isNewProduct = btn.ID.StartsWith("btnNewUpload");
                
                FileUpload fileUpload = null;
                WebImage preview = null;

                // Xác định FileUpload và Image control tương ứng
                if (isNewProduct)
                {
                    switch (imageNumber)
                    {
                        case 1:
                            fileUpload = newFileUpload1;
                            preview = newPreview1;
                            break;
                        case 2:
                            fileUpload = newFileUpload2;
                            preview = newPreview2;
                            break;
                        case 3:
                            fileUpload = newFileUpload3;
                            preview = newPreview3;
                            break;
                    }
                }
                else
                {
                    switch (imageNumber)
                    {
                        case 1:
                            fileUpload = FileUpload1;
                            preview = preview1;
                            break;
                        case 2:
                            fileUpload = FileUpload2;
                            preview = preview2;
                            break;
                        case 3:
                            fileUpload = FileUpload3;
                            preview = preview3;
                            break;
                    }
                }

                if (fileUpload.HasFile)
                {
                    // Kiểm tra kích thước file (5MB)
                    if (fileUpload.FileBytes.Length > 5 * 1024 * 1024)
                    {
                        ShowErrorMessage("Kích thước file không được vượt quá 5MB");
                        return;
                    }

                    // Kiểm tra định dạng file
                    string extension = Path.GetExtension(fileUpload.FileName).ToLower();
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    {
                        ShowErrorMessage("Chỉ chấp nhận file ảnh định dạng JPG, JPEG hoặc PNG");
                        return;
                    }

                    // Tạo thư mục nếu chưa tồn tại
                    string uploadDir = Server.MapPath("/images/products/");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Tạo tên file ngẫu nhiên
                    string fileName = Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(uploadDir, fileName);

                    // Lưu file tạm thời
                    fileUpload.SaveAs(filePath);

                    try
                    {
                        // Resize ảnh nếu cần
                        using (DrawingImage originalImage = DrawingImage.FromFile(filePath))
                        {
                            if (originalImage.Width > 800 || originalImage.Height > 800)
                            {
                                int newWidth = originalImage.Width;
                                int newHeight = originalImage.Height;

                                if (newWidth > newHeight)
                                {
                                    if (newWidth > 800)
                                    {
                                        newHeight = (int)((float)newHeight * 800 / newWidth);
                                        newWidth = 800;
                                    }
                                }
                                else
                                {
                                    if (newHeight > 800)
                                    {
                                        newWidth = (int)((float)newWidth * 800 / newHeight);
                                        newHeight = 800;
                                    }
                                }

                                using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                                {
                                    using (Graphics g = Graphics.FromImage(resizedImage))
                                    {
                                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                        g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                                    }

                                    // Lưu ảnh đã resize
                                    if (extension == ".jpg" || extension == ".jpeg")
                                    {
                                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                                        EncoderParameters encoderParams = new EncoderParameters(1);
                                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                                        resizedImage.Save(filePath, jpgEncoder, encoderParams);
                                    }
                                    else
                                    {
                                        resizedImage.Save(filePath, originalImage.RawFormat);
                                    }
                                }
                            }
                        }

                        // Cập nhật preview
                        preview.ImageUrl = "/images/products/" + fileName;
                        ShowSuccessMessage("Upload ảnh thành công");

                        // Nếu là edit mode và có productId, lưu vào database
                        if (!isNewProduct && !string.IsNullOrEmpty(hdnProductId.Value))
                        {
                            int productId = Convert.ToInt32(hdnProductId.Value);
                            string imagePath = "/images/products/" + fileName;
                            string altText = txtProductName.Text;
                            string mainImage = imageNumber == 1 ? "Y" : "N";
                            ProductBLL.AddProductImage(productId, imagePath, altText, mainImage);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("Lỗi xử lý ảnh: " + ex.Message);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }
                else
                {
                    ShowErrorMessage("Vui lòng chọn file ảnh");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi upload ảnh: " + ex.Message);
            }
        }

        // <================================>
        // UTILITY METHODS
        // <================================>
        private void ShowSuccessMessage(string message)
        {
            string script = $@"
                console.log('Showing success message: {message}');
                showToast('Thành công', '{message.Replace("'", "\\'")}', 'success');
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccess", script, true);
        }

        private void ShowErrorMessage(string message)
        {
            string script = $@"
                console.log('Showing error message: {message}');
                showToast('Lỗi', '{message.Replace("'", "\\'")}', 'error');
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", script, true);
        }
        #endregion
    }
}
