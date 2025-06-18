using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_DAL.Interfaces
{
    public interface DAL_IEventRepository
    {
        List<MOD_Event> GetAllEvents();
        MOD_Event GetEventById(int id);
        List<MOD_Event> GetEventsByOrganizationId(int organizationId);
        bool AddEvent(MOD_Event evt);
        bool UpdateEvent(MOD_Event evt);
        bool DeleteEvent(int id);
        bool HasRelatedData(int eventId);
        List<string> GetRelatedDataInfo(int eventId);
        bool DeleteRelatedData(int eventId);
    }
}
