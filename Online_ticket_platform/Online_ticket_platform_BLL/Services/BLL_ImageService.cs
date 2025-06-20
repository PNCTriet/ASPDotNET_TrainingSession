using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;
using Online_ticket_platform_BLL.Interfaces;
using Online_ticket_platform_DAL.Interfaces;

namespace Online_ticket_platform_BLL.Services
{
    public class BLL_ImageService : BLL_IImageService
    {
        private readonly DAL_IImageRepository _imageRepository;

        public BLL_ImageService()
        {
            _imageRepository = new Online_ticket_platform_DAL.Repositories.DAL_ImageRepository();
        }

        public List<MOD_Image> GetAllImages()
        {
            try
            {
                return _imageRepository.GetAllImages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageService.GetAllImages] Error: {ex.Message}");
                throw new Exception("Lỗi lấy danh sách hình ảnh: " + ex.Message);
            }
        }

        public MOD_Image GetImageById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID hình ảnh không hợp lệ");

                return _imageRepository.GetImageById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageService.GetImageById] Error: {ex.Message}");
                throw new Exception("Lỗi lấy thông tin hình ảnh: " + ex.Message);
            }
        }

        public int InsertImage(MOD_Image image)
        {
            try
            {
                // Validate input
                if (image == null)
                    throw new ArgumentException("Thông tin hình ảnh không được để trống");

                if (string.IsNullOrEmpty(image.FilePath))
                    throw new ArgumentException("Đường dẫn file không được để trống");

                if (image.UploadedAt == default(DateTime))
                    image.UploadedAt = DateTime.Now;

                return _imageRepository.InsertImage(image);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageService.InsertImage] Error: {ex.Message}");
                throw new Exception("Lỗi thêm hình ảnh: " + ex.Message);
            }
        }

        public bool UpdateImage(MOD_Image image)
        {
            try
            {
                // Validate input
                if (image == null)
                    throw new ArgumentException("Thông tin hình ảnh không được để trống");

                if (image.Id <= 0)
                    throw new ArgumentException("ID hình ảnh không hợp lệ");

                if (string.IsNullOrEmpty(image.FilePath))
                    throw new ArgumentException("Đường dẫn file không được để trống");

                return _imageRepository.UpdateImage(image);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageService.UpdateImage] Error: {ex.Message}");
                throw new Exception("Lỗi cập nhật hình ảnh: " + ex.Message);
            }
        }

        public bool DeleteImage(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID hình ảnh không hợp lệ");

                return _imageRepository.DeleteImage(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageService.DeleteImage] Error: {ex.Message}");
                throw new Exception("Lỗi xóa hình ảnh: " + ex.Message);
            }
        }

        public List<MOD_Image> GetImagesByUploader(int uploadedBy)
        {
            try
            {
                if (uploadedBy <= 0)
                    throw new ArgumentException("ID người upload không hợp lệ");

                return _imageRepository.GetImagesByUploader(uploadedBy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageService.GetImagesByUploader] Error: {ex.Message}");
                throw new Exception("Lỗi lấy danh sách hình ảnh theo người upload: " + ex.Message);
            }
        }
    }
} 