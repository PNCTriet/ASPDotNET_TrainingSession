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

namespace TrietPhamShopWeb.Adminpage
{
    public partial class ManageProducts : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadProducts();
        }

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
                        // Lấy dữ liệu từ DataKeys
                        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        txtProductName.Text = gvProducts.DataKeys[row.RowIndex]["ProductName"].ToString();
                        txtPrice.Text = gvProducts.DataKeys[row.RowIndex]["Price"].ToString();
                        txtStock.Text = gvProducts.DataKeys[row.RowIndex]["Stock"].ToString();
                        hdnProductId.Value = productId.ToString();
                        // Load ảnh sản phẩm
                        LoadProductImages(productId);
                        // Hiển thị modal
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "showEditModal();", true);
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

        [WebMethod]
        public static object GetProductById(int productId)
        {
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                string query = @"SELECT p.ProductID, p.ProductName, c.CategoryName, 
                                p.UnitPrice as Price, p.UnitsInStock as Stock
                                FROM Products p
                                LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                                WHERE p.ProductID = @ProductID";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new
                            {
                                ProductID = reader["ProductID"],
                                ProductName = reader["ProductName"],
                                CategoryName = reader["CategoryName"],
                                Price = reader["Price"],
                                Stock = reader["Stock"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        [WebMethod]
        public static bool UpdateProduct(int productId, string productName, decimal price, int stock)
        {
            return ProductBLL.UpdateProduct(productId, productName, price, stock);
        }

        [WebMethod]
        public static List<Product> GetAllProducts()
        {
            try
            {
                return ProductBLL.GetAllProducts();
            }
            catch (Exception ex)
            {
                // Log error
                return new List<Product>();
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Adminpage/AddProduct.aspx");
        }

        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                int imageNumber = Convert.ToInt32(btn.CommandArgument);
                int productId = Convert.ToInt32(hdnProductId.Value);

                // Lấy FileUpload control tương ứng
                FileUpload fileUpload = null;
                WebImage preview = null;
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

                if (fileUpload.HasFile)
                {
                    // Kiểm tra dung lượng file (giới hạn 5MB)
                    if (fileUpload.FileBytes.Length > 5 * 1024 * 1024)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", "alert('Dung lượng file không được vượt quá 5MB');", true);
                        return;
                    }

                    // Kiểm tra định dạng file
                    string extension = Path.GetExtension(fileUpload.FileName).ToLower();
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", "alert('Chỉ chấp nhận file ảnh .jpg, .jpeg, .png');", true);
                        return;
                    }

                    // Tạo thư mục nếu chưa tồn tại
                    string uploadDir = Server.MapPath("~/images/products/");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Tạo tên file
                    string fileName = $"{productId}_{imageNumber}{extension}";
                    string filePath = Path.Combine(uploadDir, fileName);

                    // Lưu file
                    fileUpload.SaveAs(filePath);

                    // Resize ảnh nếu cần
                    using (DrawingImage originalImage = DrawingImage.FromFile(filePath))
                    {
                        if (originalImage.Width > 800 || originalImage.Height > 800)
                        {
                            // Tính toán kích thước mới
                            int newWidth, newHeight;
                            if (originalImage.Width > originalImage.Height)
                            {
                                newWidth = 800;
                                newHeight = (int)((float)originalImage.Height * 800 / originalImage.Width);
                            }
                            else
                            {
                                newHeight = 800;
                                newWidth = (int)((float)originalImage.Width * 800 / originalImage.Height);
                            }

                            // Tạo ảnh mới với kích thước đã tính
                            using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                            {
                                using (Graphics graphics = Graphics.FromImage(resizedImage))
                                {
                                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                                }

                                // Lưu ảnh đã resize
                                ImageFormat format = originalImage.RawFormat;
                                resizedImage.Save(filePath, format);
                            }
                        }
                    }

                    // Lưu thông tin ảnh vào database
                    string imagePath = $"~/images/products/{fileName}";
                    string altText = txtProductName.Text;
                    bool isMainImage = (imageNumber == 1);

                    int imageId = ProductBLL.AddProductImage(productId, imagePath, altText, isMainImage ? "Y" : "N");
                    if (imageId > 0)
                    {
                        // Cập nhật preview
                        preview.ImageUrl = imagePath;
                        ScriptManager.RegisterStartupScript(this, GetType(), "success", "alert('Upload ảnh thành công');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", "alert('Lưu thông tin ảnh thất bại');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "alert('Vui lòng chọn ảnh để upload');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"alert('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        private void LoadProductImages(int productId)
        {
            try
            {
                var images = ProductBLL.GetProductImages(productId);
                if (images != null && images.Count > 0)
                {
                    // Reset tất cả preview về ảnh mặc định
                    preview1.ImageUrl = "~/images/no-image.png";
                    preview2.ImageUrl = "~/images/no-image.png";
                    preview3.ImageUrl = "~/images/no-image.png";

                    // Hiển thị ảnh từ database
                    for (int i = 0; i < Math.Min(images.Count, 3); i++)
                    {
                        WebImage preview = null;
                        switch (i)
                        {
                            case 0:
                                preview = preview1;
                                break;
                            case 1:
                                preview = preview2;
                                break;
                            case 2:
                                preview = preview3;
                                break;
                        }

                        if (preview != null)
                        {
                            preview.ImageUrl = images[i].ImagePath;
                            if (images[i].MainImage == "Y")
                            {
                                preview.CssClass = "img-fluid mb-2 main-image";
                            }
                            else
                            {
                                preview.CssClass = "img-fluid mb-2";
                            }
                        }
                    }
                }
                else
                {
                    // Nếu không có ảnh, hiển thị ảnh mặc định
                    preview1.ImageUrl = "~/images/no-image.png";
                    preview2.ImageUrl = "~/images/no-image.png";
                    preview3.ImageUrl = "~/images/no-image.png";
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi tải ảnh sản phẩm: " + ex.Message);
            }
        }

        private DrawingImage ResizeImage(DrawingImage image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }
    }
}
