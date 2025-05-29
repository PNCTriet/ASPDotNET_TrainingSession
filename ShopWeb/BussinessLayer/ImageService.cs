using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;

namespace BusinessLogic
{
    public class ImageService
    {
        private const int MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
        private const int MAX_IMAGE_SIZE = 800; // 800px
        private const string ALLOWED_EXTENSIONS = ".jpg,.jpeg,.png";
        private const string UPLOAD_DIRECTORY = "~/Uploads/products/";

        public class ImageUploadResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string ImagePath { get; set; }
            public byte[] ImageBytes { get; set; }
        }

        public ImageUploadResult ProcessImageUpload(FileUpload fileUpload, int productId, int imageNumber)
        {
            try
            {
                // 1. Validate file
                var validationResult = ValidateImageFile(fileUpload);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                // 2. Generate safe filename
                string extension = Path.GetExtension(fileUpload.FileName).ToLower();
                string fileName = $"{Guid.NewGuid()}_{productId}_{imageNumber}{extension}";
                string uploadDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", "products");
                string tempPath = Path.Combine(uploadDir, "temp_" + fileName);
                string finalPath = Path.Combine(uploadDir, fileName);

                // 3. Ensure directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // 4. Save temp file
                fileUpload.SaveAs(tempPath);

                try
                {
                    // 5. Process image
                    ProcessImageResize(tempPath, finalPath, extension);

                    // 6. Read file as byte array
                    byte[] imageBytes = File.ReadAllBytes(finalPath);

                    // 7. Return success result
                    return new ImageUploadResult
                    {
                        Success = true,
                        Message = "Upload ảnh thành công",
                        ImagePath = $"/Uploads/products/{fileName}",
                        ImageBytes = imageBytes
                    };
                }
                finally
                {
                    // Cleanup temp file
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ImageUploadResult
                {
                    Success = false,
                    Message = $"Lỗi xử lý ảnh: {ex.Message}"
                };
            }
        }

        private ImageUploadResult ValidateImageFile(FileUpload fileUpload)
        {
            if (fileUpload == null || !fileUpload.HasFile)
            {
                return new ImageUploadResult
                {
                    Success = false,
                    Message = "Vui lòng chọn file ảnh"
                };
            }

            // Check file size
            if (fileUpload.FileBytes.Length > MAX_FILE_SIZE)
            {
                return new ImageUploadResult
                {
                    Success = false,
                    Message = "Kích thước file không được vượt quá 5MB"
                };
            }

            // Check file extension
            string extension = Path.GetExtension(fileUpload.FileName).ToLower();
            if (!ALLOWED_EXTENSIONS.Contains(extension))
            {
                return new ImageUploadResult
                {
                    Success = false,
                    Message = "Chỉ chấp nhận file ảnh định dạng JPG, JPEG hoặc PNG"
                };
            }

            return new ImageUploadResult { Success = true };
        }

        private void ProcessImageResize(string sourcePath, string targetPath, string extension)
        {
            try
            {
                using (System.Drawing.Image originalImage = System.Drawing.Image.FromFile(sourcePath))
                {
                    if (originalImage.Width > MAX_IMAGE_SIZE || originalImage.Height > MAX_IMAGE_SIZE)
                    {
                        int newWidth = originalImage.Width;
                        int newHeight = originalImage.Height;

                        if (newWidth > newHeight)
                        {
                            if (newWidth > MAX_IMAGE_SIZE)
                            {
                                newHeight = (int)((float)newHeight * MAX_IMAGE_SIZE / newWidth);
                                newWidth = MAX_IMAGE_SIZE;
                            }
                        }
                        else
                        {
                            if (newHeight > MAX_IMAGE_SIZE)
                            {
                                newWidth = (int)((float)newWidth * MAX_IMAGE_SIZE / newHeight);
                                newHeight = MAX_IMAGE_SIZE;
                            }
                        }

                        using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                        {
                            using (Graphics g = Graphics.FromImage(resizedImage))
                            {
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                            }

                            if (extension == ".jpg" || extension == ".jpeg")
                            {
                                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                                EncoderParameters encoderParams = new EncoderParameters(1);
                                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90);
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
            catch (Exception ex)
            {
                throw new Exception($"Lỗi resize ảnh: {ex.Message}", ex);
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
    }
}