using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;

namespace Online_ticket_platform.Page_Admin.Organizer
{
    public partial class Event_List : System.Web.UI.Page
    {
        private readonly BLL_IEventService _eventService;

        public Event_List()
        {
            _eventService = new BLL_EventService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEvents();
            }
        }

        private void LoadEvents()
        {
            var events = _eventService.GetAllEvents();
            gvEvents.DataSource = events;
            gvEvents.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var evt = new MOD_Event
                {
                    Name = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    EventDate = Convert.ToDateTime(txtEventDate.Text),
                    Location = txtLocation.Text.Trim(),
                    OrganizationId = 1 // TODO: Lấy từ session hoặc context
                };

                if (_eventService.AddEvent(evt))
                {
                    LoadEvents();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Thêm sự kiện thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể thêm sự kiện!');", true);
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
                int eventId = Convert.ToInt32(hdnEditId.Value);
                var evt = _eventService.GetEventById(eventId);
                if (evt == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không tìm thấy sự kiện!');", true);
                    return;
                }

                // Cập nhật thông tin
                evt.Name = txtEditName.Text.Trim();
                evt.Description = txtEditDescription.Text.Trim();
                evt.EventDate = Convert.ToDateTime(txtEditEventDate.Text);
                evt.Location = txtEditLocation.Text.Trim();
                evt.OrganizationId = Convert.ToInt32(txtEditOrganizationId.Text.Trim());

                if (_eventService.UpdateEvent(evt))
                {
                    LoadEvents();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Cập nhật sự kiện thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể cập nhật sự kiện! Vui lòng kiểm tra lại thông tin.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        [WebMethod]
        public static object CheckConstraints(int eventId)
        {
            try
            {
                var eventService = new BLL_EventService();
                var constraints = eventService.GetRelatedDataInfo(eventId);
                return new { success = true, data = constraints };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteEvent(int eventId, bool forceDelete)
        {
            try
            {
                var eventService = new BLL_EventService();
                
                // Kiểm tra ràng buộc trước khi xóa
                if (!forceDelete && eventService.HasRelatedData(eventId))
                {
                    var constraints = eventService.GetRelatedDataInfo(eventId);
                    return new { 
                        success = false, 
                        message = "Không thể xóa sự kiện vì có dữ liệu liên quan!",
                        constraints = constraints
                    };
                }

                // Nếu force delete, xóa dữ liệu liên quan trước
                if (forceDelete)
                {
                    if (!eventService.DeleteRelatedData(eventId))
                    {
                        return new { success = false, message = "Không thể xóa dữ liệu liên quan!" };
                    }
                }

                // Xóa sự kiện
                if (eventService.DeleteEvent(eventId))
                {
                    return new { success = true, message = "Xóa sự kiện thành công!" };
                }
                else
                {
                    return new { success = false, message = "Không thể xóa sự kiện!" };
                }
            }
            catch (Exception ex)
            {
                return new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" };
            }
        }

        protected void gvEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditEvent")
            {
                int eventId = Convert.ToInt32(e.CommandArgument);
                var evt = _eventService.GetEventById(eventId);
                if (evt != null)
                {
                    hdnEditId.Value = eventId.ToString();
                    txtEditName.Text = evt.Name;
                    txtEditDescription.Text = evt.Description;
                    txtEditEventDate.Text = evt.EventDate.ToString("yyyy-MM-dd");
                    txtEditLocation.Text = evt.Location;
                    txtEditOrganizationId.Text = evt.OrganizationId.ToString();

                    ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", 
                        "$(document).ready(function() { $('#editModal').modal('show'); });", true);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int eventId = Convert.ToInt32(hdnDeleteId.Value);
                bool forceDelete = hdnForceDelete.Value == "true";

                // Kiểm tra ràng buộc nếu không force delete
                if (!forceDelete && _eventService.HasRelatedData(eventId))
                {
                    var constraints = _eventService.GetRelatedDataInfo(eventId);
                    var constraintMessage = "Không thể xóa sự kiện vì có dữ liệu liên quan:<br/>" + 
                        string.Join("<br/>", constraints);
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                        $"showError('{constraintMessage}');", true);
                    return;
                }

                // Xóa dữ liệu liên quan nếu force delete
                if (forceDelete)
                {
                    if (!_eventService.DeleteRelatedData(eventId))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                            "showError('Không thể xóa dữ liệu liên quan!');", true);
                        return;
                    }
                }

                // Xóa sự kiện
                if (_eventService.DeleteEvent(eventId))
                {
                    LoadEvents();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                        "showSuccess('Xóa sự kiện thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                        "showError('Không thể xóa sự kiện!');", true);
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