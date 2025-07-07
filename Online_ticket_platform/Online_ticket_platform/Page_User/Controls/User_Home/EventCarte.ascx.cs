using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Online_ticket_platform.Page_User.Controls.User_Home
{
    public partial class EventCarte : System.Web.UI.UserControl
    {
        private string _eventTitle;
        private string _eventDate;
        private string _eventLocation;
        private string _bannerUrl;
        private string _ticketCount;
        private string _detailUrl;
        private string _eventId;
        private List<TicketType> _ticketTypes;

        public string EventTitle
        {
            get { return _eventTitle; }
            set { _eventTitle = value; }
        }

        public string EventDate
        {
            get { return _eventDate; }
            set { _eventDate = value; }
        }

        public string EventLocation
        {
            get { return _eventLocation; }
            set { _eventLocation = value; }
        }

        public string BannerUrl
        {
            get { return _bannerUrl; }
            set { _bannerUrl = value; }
        }

        public string TicketCount
        {
            get { return _ticketCount; }
            set { _ticketCount = value; }
        }

        public string DetailUrl
        {
            get { return _detailUrl; }
            set { _detailUrl = value; }
        }

        public string EventId
        {
            get { return _eventId; }
            set { _eventId = value; }
        }

        public List<TicketType> TicketTypes
        {
            get { return _ticketTypes; }
            set { _ticketTypes = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            if (lblEventTitle != null) lblEventTitle.Text = _eventTitle;
            if (lblEventDate != null) lblEventDate.Text = _eventDate;
            if (lblEventLocation != null) lblEventLocation.Text = _eventLocation;
            if (lblTicketCount != null) lblTicketCount.Text = _ticketCount;
            if (lnkDetail != null) lnkDetail.NavigateUrl = $"/Page_User/User_Event.aspx?eventId={_eventId}";

            // Set image URL for both main image and background
            if (imgEventBanner != null) imgEventBanner.Src = _bannerUrl;
            if (imgEventBannerBg != null) imgEventBannerBg.Src = _bannerUrl;

            if (TicketTypesRepeater != null && _ticketTypes != null)
            {
                TicketTypesRepeater.DataSource = _ticketTypes;
                TicketTypesRepeater.DataBind();
            }
        }
    }

    public class TicketType
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}