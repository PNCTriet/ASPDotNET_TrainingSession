using System;
using System.Collections.Generic;
using System.Web.UI;
using Online_ticket_platform.Page_User.Controls.User_Home;

namespace Online_ticket_platform.Page_User
{
    public partial class User_Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Sample data for EventCarte1
                EventCarte1.EventTitle = "Hòa nhạc Mùa Hè 2024";
                EventCarte1.EventDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy HH:mm");
                EventCarte1.EventLocation = "Nhà hát Hòa Bình, TP.HCM";
                EventCarte1.BannerUrl = "../../../Public/assets/images/events/concert1.png";
                EventCarte1.TicketCount = "3 loại vé";
                EventCarte1.DetailUrl = "/Page_User/Event_Detail.aspx?id=1";
                EventCarte1.TicketTypes = new List<TicketType>
                {
                    new TicketType { Name = "VIP", Price = 1500000 },
                    new TicketType { Name = "Regular", Price = 800000 },
                    new TicketType { Name = "Early Bird", Price = 500000 }
                };
                EventCarte1.BindData();

                // Sample data for EventCarte2
                EventCarte2.EventTitle = "Lễ hội Âm nhạc Quốc tế";
                EventCarte2.EventDate = DateTime.Now.AddDays(14).ToString("dd/MM/yyyy HH:mm");
                EventCarte2.EventLocation = "Công viên Tao Đàn, TP.HCM";
                EventCarte2.BannerUrl = "../../../Public/assets/images/events/concert2.png";
                EventCarte2.TicketCount = "2 loại vé";
                EventCarte2.DetailUrl = "/Page_User/Event_Detail.aspx?id=2";
                EventCarte2.TicketTypes = new List<TicketType>
                {
                    new TicketType { Name = "VIP", Price = 2000000 },
                    new TicketType { Name = "Regular", Price = 1000000 }
                };
                EventCarte2.BindData();

                // Sample data for EventCarte3
                EventCarte3.EventTitle = "Đêm nhạc Acoustic";
                EventCarte3.EventDate = DateTime.Now.AddDays(21).ToString("dd/MM/yyyy HH:mm");
                EventCarte3.EventLocation = "Quán Bar Acoustic, Quận 1";
                EventCarte3.BannerUrl = "../../../Public/assets/images/events/concert3.png";
                EventCarte3.TicketCount = "1 loại vé";
                EventCarte3.DetailUrl = "/Page_User/Event_Detail.aspx?id=3";
                EventCarte3.TicketTypes = new List<TicketType>
                {
                    new TicketType { Name = "Standard", Price = 300000 }
                };
                EventCarte3.BindData();

                // Sample data for EventCarte4
                EventCarte4.EventTitle = "Hòa nhạc Giao hưởng";
                EventCarte4.EventDate = DateTime.Now.AddDays(28).ToString("dd/MM/yyyy HH:mm");
                EventCarte4.EventLocation = "Nhà hát Thành phố, TP.HCM";
                EventCarte4.BannerUrl = "../../../Public/assets/images/events/concert4.png";
                EventCarte4.TicketCount = "4 loại vé";
                EventCarte4.DetailUrl = "/Page_User/Event_Detail.aspx?id=4";
                EventCarte4.TicketTypes = new List<TicketType>
                {
                    new TicketType { Name = "VIP", Price = 2500000 },
                    new TicketType { Name = "Premium", Price = 1800000 },
                    new TicketType { Name = "Regular", Price = 1200000 },
                    new TicketType { Name = "Student", Price = 800000 }
                };
                EventCarte4.BindData();

                // Sample data for EventCarte5
                EventCarte5.EventTitle = "Lễ hội Rock";
                EventCarte5.EventDate = DateTime.Now.AddDays(35).ToString("dd/MM/yyyy HH:mm");
                EventCarte5.EventLocation = "Sân vận động Quân khu 7, TP.HCM";
                EventCarte5.BannerUrl = "../../../Public/assets/images/events/concert5.png";
                EventCarte5.TicketCount = "3 loại vé";
                EventCarte5.DetailUrl = "/Page_User/Event_Detail.aspx?id=5";
                EventCarte5.TicketTypes = new List<TicketType>
                {
                    new TicketType { Name = "VIP", Price = 1800000 },
                    new TicketType { Name = "Regular", Price = 900000 },
                    new TicketType { Name = "Early Bird", Price = 600000 }
                };
                EventCarte5.BindData();

                // Sample data for EventCarte6
                EventCarte6.EventTitle = "Đêm nhạc Jazz";
                EventCarte6.EventDate = DateTime.Now.AddDays(42).ToString("dd/MM/yyyy HH:mm");
                EventCarte6.EventLocation = "Khách sạn Continental, Quận 1";
                EventCarte6.BannerUrl = "../../../Public/assets/images/events/concert6.png";
                EventCarte6.TicketCount = "2 loại vé";
                EventCarte6.DetailUrl = "/Page_User/Event_Detail.aspx?id=6";
                EventCarte6.TicketTypes = new List<TicketType>
                {
                    new TicketType { Name = "VIP", Price = 1200000 },
                    new TicketType { Name = "Regular", Price = 700000 }
                };
                EventCarte6.BindData();
            }
        }
    }
}