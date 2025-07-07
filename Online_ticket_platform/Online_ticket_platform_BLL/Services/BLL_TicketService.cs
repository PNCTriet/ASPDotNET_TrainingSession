using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_DAL.Repositories;
using Online_ticket_platform_BLL.Interfaces;

namespace Online_ticket_platform_BLL.Services
{
    public class BLL_TicketService : BLL_ITicketService
    {
        private readonly DAL_ITicketRepository _ticketRepository;

        public BLL_TicketService()
        {
           
            _ticketRepository = new DAL_TicketRepository();
        }

        public List<MOD_Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAllTickets();
        }

        public MOD_Ticket GetTicketById(int id)
        {
            return _ticketRepository.GetTicketById(id);
        }

        public bool AddTicket(MOD_Ticket ticket)
        {
            if (ticket == null)
                return false;

            // Validate ticket data
            if (string.IsNullOrWhiteSpace(ticket.Name))
                return false;

            if (ticket.Price < 0)
                return false;

            if (string.IsNullOrWhiteSpace(ticket.Type))
                return false;

            if (ticket.Total <= 0)
                return false;

            if (ticket.Sold < 0)
                return false;

            if (ticket.StartSaleDate >= ticket.EndSaleDate)
                return false;

            return _ticketRepository.AddTicket(ticket);
        }

        public bool UpdateTicket(MOD_Ticket ticket)
        {
            if (ticket == null)
                return false;

            // Validate ticket data
            if (string.IsNullOrWhiteSpace(ticket.Name))
                return false;

            if (ticket.Price < 0)
                return false;

            if (string.IsNullOrWhiteSpace(ticket.Type))
                return false;

            if (ticket.Total <= 0)
                return false;

            if (ticket.Sold < 0)
                return false;

            if (ticket.StartSaleDate >= ticket.EndSaleDate)
                return false;

            return _ticketRepository.UpdateTicket(ticket);
        }

        public bool DeleteTicket(int id)
        {
            return _ticketRepository.DeleteTicket(id);
        }

        public bool HasRelatedData(int ticketId)
        {
            return _ticketRepository.HasRelatedData(ticketId);
        }

        public bool DeleteRelatedData(int ticketId)
        {
            return _ticketRepository.DeleteRelatedData(ticketId);
        }

        public List<string> GetRelatedDataInfo(int ticketId)
        {
            return _ticketRepository.GetRelatedDataInfo(ticketId);
        }

        public List<MOD_Ticket> GetTicketsByEventId(int eventId)
        {
            return _ticketRepository.GetAllTickets().FindAll(t => t.EventId == eventId);
        }
    }
}
