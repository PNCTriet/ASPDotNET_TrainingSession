using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;

namespace Online_ticket_platform.Page_User.Controls
{
    public partial class Slidebar : System.Web.UI.UserControl
    {
        private List<SlideItem> slides;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserSlides();
                BindSlides();
            }
        }

        private void LoadUserSlides()
        {
            slides = new List<SlideItem>();
            var eventService = new BLL_EventService();
            var events = eventService.GetAllEvents();
            int maxSlides = 5;
            int count = 0;
            foreach (var evt in events)
            {
                slides.Add(new SlideItem
                {
                    ImageUrl = $"../../../Public/assets/images/events/concert{(count % 6) + 1}.png",
                    AltText = evt.Name
                });
                count++;
                if (count >= maxSlides) break;
            }
            // Nếu không có sự kiện, dùng ảnh mặc định
            if (slides.Count == 0)
            {
                slides.Add(new SlideItem { ImageUrl = "../../../Public/assets/images/logo.jpg", AltText = "No events" });
            }
        }

        private void BindSlides()
        {
            rptSlides.DataSource = slides;
            rptSlides.DataBind();

            rptDots.DataSource = slides;
            rptDots.DataBind();
        }
    }

    public class SlideItem
    {
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
    }
}