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
                // Set active menu item based on current page
                string currentPage = Request.Url.Segments.Last().ToLower();
                SetActiveMenuItem(currentPage);
            }
        }

        private void SetActiveMenuItem(string currentPage)
        {
            // Remove active class from all menu items
            foreach (Control control in this.Controls)
            {
                if (control is HtmlGenericControl li && li.Attributes["class"] != null)
                {
                    li.Attributes["class"] = li.Attributes["class"].Replace(" active", "");
                }
            }

            // Set active class for current page
            string menuItemId = GetMenuItemId(currentPage);
            if (!string.IsNullOrEmpty(menuItemId))
            {
                Control menuItem = FindControl(menuItemId);
                if (menuItem != null && menuItem is HtmlGenericControl li)
                {
                    li.Attributes["class"] = li.Attributes["class"] + " active";
                }
            }
        }

        private string GetMenuItemId(string currentPage)
        {
            // Map current page to menu item ID
            switch (currentPage)
            {
                case "admin_home.aspx":
                    return "dashboardMenuItem";
                case "user_list.aspx":
                    return "usersMenuItem";
                case "organization_list.aspx":
                    return "organizationsMenuItem";
                case "event_list.aspx":
                    return "eventsMenuItem";
                case "eventsetting_list.aspx":
                    return "eventSettingsMenuItem";
                case "ticket_list.aspx":
                    return "ticketsMenuItem";
                case "order_list.aspx":
                    return "ordersMenuItem";
                case "orderitem_list.aspx":
                    return "orderItemsMenuItem";
                case "payment_list.aspx":
                    return "paymentsMenuItem";
                case "checkinlog_list.aspx":
                    return "checkinLogsMenuItem";
                case "promocode_list.aspx":
                    return "promoCodesMenuItem";
                case "orderpromo_list.aspx":
                    return "orderPromosMenuItem";
                case "referralcode_list.aspx":
                    return "referralCodesMenuItem";
                case "emaillog_list.aspx":
                    return "emailLogsMenuItem";
                case "webhooklog_list.aspx":
                    return "webhookLogsMenuItem";
                case "webhooksubscription_list.aspx":
                    return "webhookSubscriptionsMenuItem";
                case "trackingvisit_list.aspx":
                    return "trackingVisitsMenuItem";
                case "image_list.aspx":
                    return "imagesMenuItem";
                case "imagelink_list.aspx":
                    return "imageLinksMenuItem";
                case "userorganization_list.aspx":
                    return "userOrganizationsMenuItem";
                default:
                    return string.Empty;
            }
        }
    }
}