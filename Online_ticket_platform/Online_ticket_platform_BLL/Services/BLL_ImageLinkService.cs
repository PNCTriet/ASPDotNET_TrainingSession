using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;
using Online_ticket_platform_BLL.Interfaces;
using Online_ticket_platform_DAL.Interfaces;

namespace Online_ticket_platform_BLL.Services
{
    public class BLL_ImageLinkService : BLL_IImageLinkService
    {
        private readonly DAL_IImageLinkRepository _imageLinkRepository;

        public BLL_ImageLinkService()
        {
            _imageLinkRepository = new Online_ticket_platform_DAL.Repositories.DAL_ImageLinkRepository();
        }

        public List<MOD_ImageLink> GetAllImageLinks()
        {
            try
            {
                return _imageLinkRepository.GetAllImageLinks();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.GetAllImageLinks] Error: {ex.Message}");
                throw new Exception("Lỗi lấy danh sách liên kết hình ảnh: " + ex.Message);
            }
        }

        public MOD_ImageLink GetImageLinkById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID liên kết hình ảnh không hợp lệ");

                return _imageLinkRepository.GetImageLinkById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.GetImageLinkById] Error: {ex.Message}");
                throw new Exception("Lỗi lấy thông tin liên kết hình ảnh: " + ex.Message);
            }
        }

        public List<MOD_ImageLink> GetImageLinksByEntity(string entityType, int entityId)
        {
            try
            {
                if (string.IsNullOrEmpty(entityType))
                    throw new ArgumentException("Loại entity không được để trống");

                if (entityId <= 0)
                    throw new ArgumentException("ID entity không hợp lệ");

                return _imageLinkRepository.GetImageLinksByEntity(entityType, entityId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.GetImageLinksByEntity] Error: {ex.Message}");
                throw new Exception("Lỗi lấy danh sách liên kết hình ảnh theo entity: " + ex.Message);
            }
        }

        public List<MOD_ImageLink> GetImageLinksByEvent(int eventId)
        {
            try
            {
                if (eventId <= 0)
                    throw new ArgumentException("ID sự kiện không hợp lệ");

                return _imageLinkRepository.GetImageLinksByEvent(eventId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.GetImageLinksByEvent] Error: {ex.Message}");
                throw new Exception("Lỗi lấy danh sách liên kết hình ảnh theo sự kiện: " + ex.Message);
            }
        }

        public List<MOD_ImageLink> GetImageLinksByOrganization(int organizationId)
        {
            try
            {
                if (organizationId <= 0)
                    throw new ArgumentException("ID tổ chức không hợp lệ");

                return _imageLinkRepository.GetImageLinksByOrganization(organizationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.GetImageLinksByOrganization] Error: {ex.Message}");
                throw new Exception("Lỗi lấy danh sách liên kết hình ảnh theo tổ chức: " + ex.Message);
            }
        }

        public int InsertImageLink(MOD_ImageLink imageLink)
        {
            try
            {
                // Validate input
                if (imageLink == null)
                    throw new ArgumentException("Thông tin liên kết hình ảnh không được để trống");

                if (imageLink.ImageId <= 0)
                    throw new ArgumentException("ID hình ảnh không hợp lệ");

                if (string.IsNullOrEmpty(imageLink.EntityType))
                    throw new ArgumentException("Loại entity không được để trống");

                if (imageLink.EntityId <= 0)
                    throw new ArgumentException("ID entity không hợp lệ");

                if (string.IsNullOrEmpty(imageLink.UsageType))
                    throw new ArgumentException("Loại sử dụng không được để trống");

                if (imageLink.LinkedAt == default(DateTime))
                    imageLink.LinkedAt = DateTime.Now;

                return _imageLinkRepository.InsertImageLink(imageLink);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.InsertImageLink] Error: {ex.Message}");
                throw new Exception("Lỗi thêm liên kết hình ảnh: " + ex.Message);
            }
        }

        public bool UpdateImageLink(MOD_ImageLink imageLink)
        {
            try
            {
                // Validate input
                if (imageLink == null)
                    throw new ArgumentException("Thông tin liên kết hình ảnh không được để trống");

                if (imageLink.Id <= 0)
                    throw new ArgumentException("ID liên kết hình ảnh không hợp lệ");

                if (imageLink.ImageId <= 0)
                    throw new ArgumentException("ID hình ảnh không hợp lệ");

                if (string.IsNullOrEmpty(imageLink.EntityType))
                    throw new ArgumentException("Loại entity không được để trống");

                if (imageLink.EntityId <= 0)
                    throw new ArgumentException("ID entity không hợp lệ");

                if (string.IsNullOrEmpty(imageLink.UsageType))
                    throw new ArgumentException("Loại sử dụng không được để trống");

                return _imageLinkRepository.UpdateImageLink(imageLink);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.UpdateImageLink] Error: {ex.Message}");
                throw new Exception("Lỗi cập nhật liên kết hình ảnh: " + ex.Message);
            }
        }

        public bool DeleteImageLink(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID liên kết hình ảnh không hợp lệ");

                return _imageLinkRepository.DeleteImageLink(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.DeleteImageLink] Error: {ex.Message}");
                throw new Exception("Lỗi xóa liên kết hình ảnh: " + ex.Message);
            }
        }

        public bool DeleteImageLinksByEntity(string entityType, int entityId)
        {
            try
            {
                if (string.IsNullOrEmpty(entityType))
                    throw new ArgumentException("Loại entity không được để trống");

                if (entityId <= 0)
                    throw new ArgumentException("ID entity không hợp lệ");

                return _imageLinkRepository.DeleteImageLinksByEntity(entityType, entityId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BLL_ImageLinkService.DeleteImageLinksByEntity] Error: {ex.Message}");
                throw new Exception("Lỗi xóa liên kết hình ảnh theo entity: " + ex.Message);
            }
        }

        public List<MOD_Organization> GetAllOrganizationsForImageLink()
        {
            return _imageLinkRepository.GetAllOrganizationsForImageLink();
        }

        public List<MOD_Event> GetAllEventsForImageLink()
        {
            return _imageLinkRepository.GetAllEventsForImageLink();
        }
    }
} 