using System;
using System.Web.UI;

namespace Online_ticket_platform.Page_User.Controls
{
    public partial class Header : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null && Session["UserName"] != null)
                {
                    pnlUserInfo.Visible = true;
                    pnlLogin.Visible = false;
                    litUserName.Text = Session["UserName"].ToString();
                }
                else
                {
                    pnlUserInfo.Visible = false;
                    pnlLogin.Visible = true;
                }
            }
        }
    }
}