using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;
using Online_ticket_platform_BLL.Interfaces;
using Online_ticket_platform_BLL.Services;

namespace Online_ticket_platform.Page_Admin.Organizer
{
    public partial class Ticket_List : System.Web.UI.Page
    {
        private readonly BLL_ITicketService _ticketService;
        private readonly BLL_IEventService _eventService;

        public Ticket_List()
        {
            _ticketService = new BLL_TicketService();
            _eventService = new BLL_EventService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTickets();
                LoadEvents();
            }
        }

        private void LoadTickets()
        {
            var tickets = _ticketService.GetAllTickets();
            gvTickets.DataSource = tickets;
            gvTickets.DataBind();
        }

        private void LoadEvents()
        {
            var events = _eventService.GetAllEvents();
            ddlEventId.DataSource = events;
            ddlEventId.DataTextField = "Name";
            ddlEventId.DataValueField = "Id";
            ddlEventId.DataBind();

            ddlEditEventId.DataSource = events;
            ddlEditEventId.DataTextField = "Name";
            ddlEditEventId.DataValueField = "Id";
            ddlEditEventId.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var ticket = new MOD_Ticket
                {
                    EventId = Convert.ToInt32(ddlEventId.SelectedValue),
                    Name = txtName.Text.Trim(),
                    Price = Convert.ToDecimal(txtPrice.Text.Trim()),
                    Type = txtType.Text.Trim(),
                    Total = Convert.ToInt32(txtTotal.Text.Trim()),
                    Sold = 0,
                    StartSaleDate = Convert.ToDateTime(txtStartSaleDate.Text),
                    EndSaleDate = Convert.ToDateTime(txtEndSaleDate.Text),
                    IsActive = chkIsActive.Checked
                };

                if (_ticketService.AddTicket(ticket))
                {
                    LoadTickets();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Thêm vé thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể thêm vé!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int ticketId = Convert.ToInt32(hdnEditId.Value);
                var ticket = _ticketService.GetTicketById(ticketId);
                if (ticket == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không tìm thấy vé!');", true);
                    return;
                }

                // Cập nhật thông tin
                ticket.EventId = Convert.ToInt32(ddlEditEventId.SelectedValue);
                ticket.Name = txtEditName.Text.Trim();
                ticket.Price = Convert.ToDecimal(txtEditPrice.Text.Trim());
                ticket.Type = txtEditType.Text.Trim();
                ticket.Total = Convert.ToInt32(txtEditTotal.Text.Trim());
                ticket.StartSaleDate = Convert.ToDateTime(txtEditStartSaleDate.Text);
                ticket.EndSaleDate = Convert.ToDateTime(txtEditEndSaleDate.Text);
                ticket.IsActive = chkEditIsActive.Checked;

                if (_ticketService.UpdateTicket(ticket))
                {
                    LoadTickets();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Cập nhật vé thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể cập nhật vé! Vui lòng kiểm tra lại thông tin.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        [WebMethod]
        public static object CheckConstraints(int ticketId)
        {
            try
            {
                var ticketService = new BLL_TicketService();
                var constraints = ticketService.GetRelatedDataInfo(ticketId);
                return new { success = true, data = constraints };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        protected void gvTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditTicket")
            {
                int ticketId = Convert.ToInt32(e.CommandArgument);
                var ticket = _ticketService.GetTicketById(ticketId);
                if (ticket != null)
                {
                    hdnEditId.Value = ticketId.ToString();
                    ddlEditEventId.SelectedValue = ticket.EventId.ToString();
                    txtEditName.Text = ticket.Name;
                    txtEditPrice.Text = ticket.Price.ToString();
                    txtEditType.Text = ticket.Type;
                    txtEditTotal.Text = ticket.Total.ToString();
                    txtEditStartSaleDate.Text = ticket.StartSaleDate.ToString("yyyy-MM-dd");
                    txtEditEndSaleDate.Text = ticket.EndSaleDate.ToString("yyyy-MM-dd");
                    chkEditIsActive.Checked = ticket.IsActive;

                    ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", 
                        "$(document).ready(function() { $('#editModal').modal('show'); });", true);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int ticketId = Convert.ToInt32(hdnDeleteId.Value);
                bool forceDelete = hdnForceDelete.Value == "true";

                // Kiểm tra ràng buộc nếu không force delete
                if (!forceDelete && _ticketService.HasRelatedData(ticketId))
                {
                    var constraints = _ticketService.GetRelatedDataInfo(ticketId);
                    var constraintMessage = "Không thể xóa vé vì có dữ liệu liên quan:<br/>" + 
                        string.Join("<br/>", constraints);
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                        $"showError('{constraintMessage}');", true);
                    return;
                }

                // Xóa dữ liệu liên quan nếu force delete
                if (forceDelete)
                {
                    if (!_ticketService.DeleteRelatedData(ticketId))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                            "showError('Không thể xóa dữ liệu liên quan!');", true);
                        return;
                    }
                }

                // Xóa vé
                if (_ticketService.DeleteTicket(ticketId))
                {
                    LoadTickets();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                        "showSuccess('Xóa vé thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                        "showError('Không thể xóa vé!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                    $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }
    }
}