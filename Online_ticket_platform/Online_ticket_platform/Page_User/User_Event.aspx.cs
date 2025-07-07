using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Online_ticket_platform_BLL;
using Online_ticket_platform_BLL.Services;
using Online_ticket_platform_Model;
using System.Globalization;

namespace Online_ticket_platform.Page_User
{
    public partial class User_Event : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEventDetail();
            }
        }

        private void LoadEventDetail()
        {
            string eventIdStr = Request.QueryString["eventId"];
            if (int.TryParse(eventIdStr, out int eventId))
            {
                var eventService = new BLL_EventService();
                var ticketService = new BLL_TicketService();
                var imageLinkService = new BLL_ImageLinkService();
                MOD_Event evt = eventService.GetEventById(eventId);
                if (evt != null)
                {
                    lblEventName.Text = evt.Name;
                    lblEventDescription.Text = evt.Description;
                    lblEventTime.Text = evt.EventDate.ToString("dd/MM/yyyy HH:mm");
                    lblEventLocation.Text = evt.Location;
                    // Lấy vé đầu tiên để demo giá vé, có thể sửa lại để hiển thị danh sách vé
                    var tickets = ticketService.GetTicketsByEventId(eventId);
                    if (tickets != null && tickets.Count > 0)
                    {
                        lblEventPrice.Text = tickets[0].Price.ToString("N0") + "đ";
                    }
                    else
                    {
                        lblEventPrice.Text = "-";
                    }
                    // Lấy hình ảnh sự kiện từ image_links
                    var imageLinks = imageLinkService.GetImageLinksByEvent(eventId);
                    if (imageLinks != null && imageLinks.Count > 0 && imageLinks[0].Image != null && !string.IsNullOrEmpty(imageLinks[0].Image.FilePath))
                    {
                        imgEvent.ImageUrl = imageLinks[0].Image.FilePath;
                    }
                    else
                    {
                        imgEvent.ImageUrl = "../Public/assets/images/events/concert1.png";
                    }
                }
                else
                {
                    lblEventName.Text = "Không tìm thấy sự kiện";
                }
            }
            else
            {
                lblEventName.Text = "ID sự kiện không hợp lệ";
            }
        }
    }
}