using System;
using System.Collections.Generic;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_DAL.Repositories;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_BLL
{
    public class BLL_EventService : BLL_IEventService
    {
        private readonly DAL_IEventRepository _eventRepository;

        public BLL_EventService()
        {
            _eventRepository = new DAL_EventRepository();
        }

        public List<MOD_Event> GetAllEvents()
        {
            return _eventRepository.GetAllEvents();
        }

        public MOD_Event GetEventById(int id)
        {
            return _eventRepository.GetEventById(id);
        }

        public List<MOD_Event> GetEventsByOrganizationId(int organizationId)
        {
            return _eventRepository.GetEventsByOrganizationId(organizationId);
        }

        public bool AddEvent(MOD_Event evt)
        {
            if (string.IsNullOrEmpty(evt.Name) || string.IsNullOrEmpty(evt.Location))
                return false;

            if (evt.EventDate < DateTime.Now)
                return false;

            

            return _eventRepository.AddEvent(evt);
        }

        public bool UpdateEvent(MOD_Event evt)
        {
            if (string.IsNullOrEmpty(evt.Name) || string.IsNullOrEmpty(evt.Location))
                return false;

            if (evt.EventDate < DateTime.Now)
                return false;

           
            return _eventRepository.UpdateEvent(evt);
        }

        public bool DeleteEvent(int id)
        {
            if (_eventRepository.HasRelatedData(id))
                return false;

            return _eventRepository.DeleteEvent(id);
        }

        public bool HasRelatedData(int eventId)
        {
            return _eventRepository.HasRelatedData(eventId);
        }

        public List<string> GetRelatedDataInfo(int eventId)
        {
            return _eventRepository.GetRelatedDataInfo(eventId);
        }

        public bool DeleteRelatedData(int eventId)
        {
            return _eventRepository.DeleteRelatedData(eventId);
        }
    }
}
