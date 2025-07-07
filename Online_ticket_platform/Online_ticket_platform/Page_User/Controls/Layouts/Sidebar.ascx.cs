using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;

namespace Online_ticket_platform.Page_User.Controls
{
    public partial class Sidebar : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserInfo();
            }
        }

        private void LoadUserInfo()
        {
            if (Session["UserID"] != null)
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                var userService = new BLL_UserService();
                MOD_User user = userService.GetUserById(userId);
                if (user != null)
                {
                    UserName.Text = user.Name;
                    UserEmail.Text = user.Email;
                    // Nếu có avatar trong DB, set UserAvatar.Src = ...
                }
            }
        }
    }
}