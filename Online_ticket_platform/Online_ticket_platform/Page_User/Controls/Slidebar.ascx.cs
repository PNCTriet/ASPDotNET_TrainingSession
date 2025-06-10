using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_ticket_platform.Page_User.Controls
{
    public partial class Slidebar : System.Web.UI.UserControl
    {
        private List<SlideItem> slides;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeSlides();
                BindSlides();
            }
        }

        private void InitializeSlides()
        {
            slides = new List<SlideItem>
            {
                new SlideItem { ImageUrl = "https://flip.vn/_next/image?url=https%3A%2F%2Fflivnewbucket.s3.ap-southeast-1.amazonaws.com%2Fuploads%2Forganizations%2F3e14cc7d-e559-4a95-8106-b35cbd80a55c%2F8d192c3e-0515-4fe1-a2b9-37aaf324cd40.png&w=1920&q=75", AltText = "Slide 1" },
                new SlideItem { ImageUrl = "https://flip.vn/_next/image?url=https%3A%2F%2Fflivnewbucket.s3.ap-southeast-1.amazonaws.com%2Fuploads%2Forganizations%2Ff795e606-7bab-4fe4-8843-031450a22333%2F1cf281f7-9c68-4612-8681-c5b0981f27da.webp&w=1920&q=75", AltText = "Slide 2" },
                new SlideItem { ImageUrl = "https://flip.vn/_next/image?url=https%3A%2F%2Fflivnewbucket.s3.ap-southeast-1.amazonaws.com%2Fuploads%2Forganizations%2Ff6554ab4-a5b8-4838-bbab-3f305c56d624%2Feb58c96f-7a5e-4f53-ba86-6c2b2f14e368.png&w=1920&q=75", AltText = "Slide 3" },
                new SlideItem { ImageUrl = "https://flip.vn/_next/image?url=https%3A%2F%2Fflivnewbucket.s3.ap-southeast-1.amazonaws.com%2Fuploads%2Forganizations%2F3e14cc7d-e559-4a95-8106-b35cbd80a55c%2F8d192c3e-0515-4fe1-a2b9-37aaf324cd40.png&w=1920&q=75", AltText = "Slide 4" }
            };
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