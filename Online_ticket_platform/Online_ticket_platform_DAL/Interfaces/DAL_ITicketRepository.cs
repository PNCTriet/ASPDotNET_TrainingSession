using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_DAL.Interfaces
{
    public interface DAL_ITicketRepository
    {
        List<MOD_Ticket> GetAllTickets();
        MOD_Ticket GetTicketById(int id);
        bool AddTicket(MOD_Ticket ticket);
        bool UpdateTicket(MOD_Ticket ticket);
        bool DeleteTicket(int id);
        bool HasRelatedData(int ticketId);
        bool DeleteRelatedData(int ticketId);
        List<string> GetRelatedDataInfo(int ticketId);
    }
}
