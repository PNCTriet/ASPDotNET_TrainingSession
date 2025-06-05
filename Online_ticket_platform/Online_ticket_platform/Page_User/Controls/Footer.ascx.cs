using System;
using System.Web.UI;

namespace Online_ticket_platform.Page_User.Controls
{
    public partial class Footer : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set copyright year
                CopyrightYear.Text = DateTime.Now.Year.ToString();
            }
        }
    }
}