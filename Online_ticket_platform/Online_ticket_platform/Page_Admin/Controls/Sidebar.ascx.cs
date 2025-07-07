using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Online_ticket_platform.Page_Admin.Controls
{
    public partial class sidebar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string currentPage = GetCurrentPage();
                UpdateActiveMenuItem(currentPage);
            }
        }

        private string GetCurrentPage()
        {
            string path = Request.Path.ToLower();
            string[] pathParts = path.Split('/');
            string pageName = pathParts[pathParts.Length - 1];

            // Map page names to menu items
            Dictionary<string, string> pageMapping = new Dictionary<string, string>
            {
                { "admin_home.aspx", "menu-home" },
                { "user_list.aspx", "menu-users" },
                { "organization_list.aspx", "menu-organizations" },
                { "event_list.aspx", "menu-events" },
                { "eventsetting_list.aspx", "menu-event-settings" },
                { "ticket_list.aspx", "menu-tickets" },
                { "order_list.aspx", "menu-orders" },
                { "orderitem_list.aspx", "menu-order-items" },
                { "payment_list.aspx", "menu-payments" },
                { "checkinlog_list.aspx", "menu-checkin-logs" },
                { "promocode_list.aspx", "menu-promo-codes" },
                { "orderpromo_list.aspx", "menu-order-promos" },
                { "referralcode_list.aspx", "menu-referral-codes" },
                { "emaillog_list.aspx", "menu-email-logs" },
                { "webhooklog_list.aspx", "menu-webhook-logs" },
                { "webhooksubscription_list.aspx", "menu-webhook-subscriptions" },
                { "trackingvisit_list.aspx", "menu-tracking-visits" },
                { "image_list.aspx", "menu-images" },
                { "imagelink_list.aspx", "menu-image-links" },
                { "userorganization_list.aspx", "menu-user-organizations" }
            };

            return pageMapping.ContainsKey(pageName) ? pageMapping[pageName] : "menu-home";
        }

        private void UpdateActiveMenuItem(string menuItemId)
        {
            // Remove active class from all menu items
            foreach (Control control in this.Controls)
            {
                if (control is HtmlGenericControl)
                {
                    HtmlGenericControl li = control as HtmlGenericControl;
                    if (li.Attributes["id"] != null)
                    {
                        li.Attributes["class"] = li.Attributes["class"].Replace(" active", "");
                    }
                }
            }

            // Add active class to current menu item
            Control activeItem = FindControl(menuItemId);
            if (activeItem != null && activeItem is HtmlGenericControl)
            {
                HtmlGenericControl li = activeItem as HtmlGenericControl;
                li.Attributes["class"] = li.Attributes["class"] + " active";
            }
        }
    }
}