using System;
using System.Collections.Generic;
using System.Web.UI;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;
using Online_ticket_platform.Page_User.Controls.User_Home;

namespace Online_ticket_platform.Page_User
{
    public partial class User_Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDynamicEvents();
            }
        }

        private void LoadDynamicEvents()
        {
            var eventService = new BLL_EventService();
            var ticketService = new Online_ticket_platform_BLL.Services.BLL_TicketService();
            var events = eventService.GetAllEvents();

            // Map to 6 event cards (if less than 6, fill with empty)
            var eventCards = new[] { EventCarte1, EventCarte2, EventCarte3, EventCarte4, EventCarte5, EventCarte6 };
            for (int i = 0; i < eventCards.Length; i++)
            {
                if (i < events.Count)
                {
                    var evt = events[i];
                    var tickets = ticketService.GetTicketsByEventId(evt.Id);
                    eventCards[i].EventTitle = evt.Name;
                    eventCards[i].EventDate = evt.EventDate.ToString("dd/MM/yyyy HH:mm");
                    eventCards[i].EventLocation = evt.Location;
                    // For demo, use a default image if none
                    eventCards[i].BannerUrl = $"../../../Public/assets/images/events/concert{(i % 6) + 1}.png";
                    eventCards[i].TicketCount = tickets.Count + " loại vé";
                    eventCards[i].EventId = evt.Id.ToString();
                    eventCards[i].TicketTypes = new List<TicketType>();
                    foreach (var t in tickets)
                    {
                        eventCards[i].TicketTypes.Add(new TicketType { Name = t.Name, Price = t.Price });
                    }
                    eventCards[i].BindData();
                }
                else
                {
                    // Hide or clear unused cards
                    eventCards[i].Visible = false;
                }
            }
        }
    }
}