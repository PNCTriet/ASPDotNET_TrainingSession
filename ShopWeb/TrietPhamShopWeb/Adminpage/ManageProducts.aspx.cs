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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(hdnProductId.Value);
                string productName = txtProductName.Text.Trim();
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int stock = Convert.ToInt32(txtStock.Text);

                if (ProductBLL.UpdateProduct(productId, productName, price, stock))
                {
                    LoadProducts();
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
                    ProcessNewProductImages(productId);
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
            if (string.IsNullOrEmpty(txtNewProductName.Text.Trim()))
            {
                ShowErrorMessage("Vui lòng nhập tên sản phẩm");
                return false;
            }

            if (string.IsNullOrEmpty(ddlCategory.SelectedValue))
            {
                ShowErrorMessage("Vui lòng chọn danh mục");
                return false;
            }

            if (string.IsNullOrEmpty(txtNewPrice.Text) || !decimal.TryParse(txtNewPrice.Text, out decimal price) || price < 0)
            {
                ShowErrorMessage("Vui lòng nhập giá hợp lệ");
                return false;
            }

            if (string.IsNullOrEmpty(txtNewStock.Text) || !int.TryParse(txtNewStock.Text, out int stock) || stock < 0)
            {
                ShowErrorMessage("Vui lòng nhập số lượng tồn kho hợp lệ");
                return false;
            }

            return true;
        }

        // <================================>
        // IMAGE HANDLING
        // <================================>
        private void ProcessNewProductImages(int productId)
        {
            try
            {
                ProcessImageUpload(newFileUpload1, productId, 1, true);
                ProcessImageUpload(newFileUpload2, productId, 2, false);
                ProcessImageUpload(newFileUpload3, productId, 3, false);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi xử lý ảnh sản phẩm: " + ex.Message);
            }
        }

        private void ProcessImageUpload(FileUpload fileUpload, int productId, int imageNumber, bool isMainImage)
        {
            if (fileUpload.HasFile)
            {
                string imagePath = SaveNewProductImage(fileUpload, productId, imageNumber);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    ProductBLL.AddProductImage(productId, imagePath, txtNewProductName.Text, isMainImage ? "Y" : "N");
                }
            }
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
        // UTILITY METHODS
        // <================================>
        private void ShowSuccessMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccess", 
                $"alert('{message}');", true);
        }

        private void ShowErrorMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", 
                $"alert('{message}');", true);
        }
        #endregion
    }
}
