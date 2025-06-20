using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_BLL.Interfaces
{
    public interface BLL_IImageLinkService
    {
        List<MOD_ImageLink> GetAllImageLinks();
        MOD_ImageLink GetImageLinkById(int id);
        List<MOD_ImageLink> GetImageLinksByEntity(string entityType, int entityId);
        List<MOD_ImageLink> GetImageLinksByEvent(int eventId);
        List<MOD_ImageLink> GetImageLinksByOrganization(int organizationId);
        int InsertImageLink(MOD_ImageLink imageLink);
        bool UpdateImageLink(MOD_ImageLink imageLink);
        bool DeleteImageLink(int id);
        bool DeleteImageLinksByEntity(string entityType, int entityId);
        List<MOD_Organization> GetAllOrganizationsForImageLink();
        List<MOD_Event> GetAllEventsForImageLink();
    }
} 