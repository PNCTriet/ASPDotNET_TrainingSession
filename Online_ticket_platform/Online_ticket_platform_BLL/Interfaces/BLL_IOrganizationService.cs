using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_BLL
{
    public interface BLL_IOrganizationService
    {
        List<MOD_Organization> GetAllOrganizations();
        MOD_Organization GetOrganizationById(int id);
        bool AddOrganization(MOD_Organization organization);
        bool UpdateOrganization(MOD_Organization organization);
        bool DeleteOrganization(int id);
        bool HasRelatedData(int organizationId);
        void DeleteRelatedData(int organizationId);
        List<string> GetRelatedDataInfo(int organizationId);
    }
}
