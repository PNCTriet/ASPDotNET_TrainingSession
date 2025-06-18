using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_DAL.Repositories;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_BLL.Services
{
    public class BLL_OrganizationService : BLL_IOrganizationService
    {
        private readonly DAL_IOrganizationRepository _organizationRepository;

        public BLL_OrganizationService()
        {
            _organizationRepository = new DAL_OrganizationRepository();
        }

        public List<MOD_Organization> GetAllOrganizations()
        {
            try
            {
                return _organizationRepository.GetAllOrganizations();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tổ chức: " + ex.Message);
            }
        }

        public MOD_Organization GetOrganizationById(int id)
        {
            try
            {
                return _organizationRepository.GetOrganizationById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể lấy thông tin tổ chức với ID {id}.", ex);
            }
        }

        public bool AddOrganization(MOD_Organization organization)
        {
            try
            {
                if (organization == null)
                    throw new ArgumentNullException(nameof(organization));

                // Validate required fields
                if (string.IsNullOrWhiteSpace(organization.Name))
                    throw new ArgumentException("Tên tổ chức không được để trống");

                if (string.IsNullOrWhiteSpace(organization.ContactEmail))
                    throw new ArgumentException("Email không được để trống");

                if (string.IsNullOrWhiteSpace(organization.Phone))
                    throw new ArgumentException("Số điện thoại không được để trống");

                if (string.IsNullOrWhiteSpace(organization.Address))
                    throw new ArgumentException("Địa chỉ không được để trống");

                // Check if email already exists
                var existingOrg = _organizationRepository.GetOrganizationByEmail(organization.ContactEmail);
                if (existingOrg != null)
                    throw new Exception("Email này đã được sử dụng bởi một tổ chức khác");

                return _organizationRepository.AddOrganization(organization);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm tổ chức mới: " + ex.Message);
            }
        }

        public bool UpdateOrganization(MOD_Organization organization)
        {
            try
            {
                if (organization == null)
                    throw new ArgumentNullException(nameof(organization));

                // Validate required fields
                if (string.IsNullOrWhiteSpace(organization.Name))
                    throw new ArgumentException("Tên tổ chức không được để trống");

                if (string.IsNullOrWhiteSpace(organization.ContactEmail))
                    throw new ArgumentException("Email không được để trống");

                if (string.IsNullOrWhiteSpace(organization.Phone))
                    throw new ArgumentException("Số điện thoại không được để trống");

                if (string.IsNullOrWhiteSpace(organization.Address))
                    throw new ArgumentException("Địa chỉ không được để trống");

                // Check if email exists for other organizations
                var existingOrg = _organizationRepository.GetOrganizationByEmail(organization.ContactEmail);
                if (existingOrg != null && existingOrg.Id != organization.Id)
                    throw new Exception("Email này đã được sử dụng bởi một tổ chức khác");

                return _organizationRepository.UpdateOrganization(organization);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật thông tin tổ chức: " + ex.Message);
            }
        }

        public bool DeleteOrganization(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID tổ chức không hợp lệ");

                // Check if organization exists
                var organization = _organizationRepository.GetOrganizationById(id);
                if (organization == null)
                    throw new Exception("Không tìm thấy tổ chức cần xóa");

                return _organizationRepository.DeleteOrganization(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa tổ chức: " + ex.Message);
            }
        }

        public bool HasRelatedData(int organizationId)
        {
            try
            {
                return _organizationRepository.HasRelatedData(organizationId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra dữ liệu liên quan: " + ex.Message);
            }
        }

        public void DeleteRelatedData(int organizationId)
        {
            try
            {
                if (organizationId <= 0)
                    throw new ArgumentException("ID tổ chức không hợp lệ");

                // Xóa tất cả dữ liệu liên quan
                _organizationRepository.DeleteRelatedData(organizationId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa dữ liệu liên quan: " + ex.Message);
            }
        }

        public List<string> GetRelatedDataInfo(int organizationId)
        {
            try
            {
                if (organizationId <= 0)
                    throw new ArgumentException("ID tổ chức không hợp lệ");

                var constraints = new List<string>();

                // Lấy thông tin về các ràng buộc
                var relatedData = _organizationRepository.GetRelatedDataInfo(organizationId);
                if (relatedData != null && relatedData.Any())
                {
                    foreach (var data in relatedData)
                    {
                        switch (data.Key)
                        {
                            case "events":
                                constraints.Add($"Sự kiện ({data.Value}): Cần xóa các sự kiện trước khi xóa tổ chức");
                                break;
                            case "user_organizations":
                                constraints.Add($"Người dùng ({data.Value}): Cần xóa liên kết người dùng trước khi xóa tổ chức");
                                break;
                            case "webhook_subscriptions":
                                constraints.Add($"Webhook ({data.Value}): Cần xóa các webhook subscription trước khi xóa tổ chức");
                                break;
                            case "image_links":
                                constraints.Add($"Hình ảnh ({data.Value}): Cần xóa các liên kết hình ảnh trước khi xóa tổ chức");
                                break;
                        }
                    }
                }

                return constraints;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin dữ liệu liên quan: " + ex.Message);
            }
        }
    }
}
