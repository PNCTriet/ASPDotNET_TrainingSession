using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Online_ticket_platform_BLL;
using Online_ticket_platform_Model;
using Online_ticket_platform_BLL.Services;

namespace Online_ticket_platform.Page_Admin.Organizer
{
    public partial class OrderItem_List : System.Web.UI.Page
    {
        private readonly BLL_IOrderitemsService _orderItemService;

        public OrderItem_List()
        {
            _orderItemService = new BLL_OrderitemsService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrderItems();
            }
        }

        private void LoadOrderItems()
        {
            var items = _orderItemService.GetAllOrderItems();
            gvOrderItems.DataSource = items;
            gvOrderItems.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var item = new MOD_OrderItem
                {
                    OrderId = Convert.ToInt32(txtOrderId.Text.Trim()),
                    TicketId = Convert.ToInt32(txtTicketId.Text.Trim()),
                    Quantity = Convert.ToInt32(txtQuantity.Text.Trim()),
                    PriceSnapshot = Convert.ToDecimal(txtPriceSnapshot.Text.Trim())
                };

                if (_orderItemService.AddOrderItem(item))
                {
                    LoadOrderItems();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Thêm chi tiết thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể thêm chi tiết!');", true);
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
                int itemId = Convert.ToInt32(hdnEditId.Value);
                var item = _orderItemService.GetOrderItemById(itemId);
                if (item == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không tìm thấy chi tiết!');", true);
                    return;
                }

                item.OrderId = Convert.ToInt32(txtEditOrderId.Text.Trim());
                item.TicketId = Convert.ToInt32(txtEditTicketId.Text.Trim());
                item.Quantity = Convert.ToInt32(txtEditQuantity.Text.Trim());
                item.PriceSnapshot = Convert.ToDecimal(txtEditPriceSnapshot.Text.Trim());

                if (_orderItemService.UpdateOrderItem(item))
                {
                    LoadOrderItems();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Cập nhật chi tiết thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể cập nhật chi tiết! Vui lòng kiểm tra lại thông tin.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        [WebMethod]
        public static object CheckConstraints(int itemId)
        {
            try
            {
                var orderItemService = new BLL_OrderitemsService();
                var constraints = orderItemService.GetRelatedDataInfo(itemId);
                return new { success = true, data = constraints };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        protected void gvOrderItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditOrderItem")
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                var item = _orderItemService.GetOrderItemById(itemId);
                if (item != null)
                {
                    hdnEditId.Value = itemId.ToString();
                    txtEditOrderId.Text = item.OrderId.ToString();
                    txtEditTicketId.Text = item.TicketId.ToString();
                    txtEditQuantity.Text = item.Quantity.ToString();
                    txtEditPriceSnapshot.Text = item.PriceSnapshot.ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", 
                        "$(document).ready(function() { $('#editModal').modal('show'); });", true);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int itemId = Convert.ToInt32(hdnDeleteId.Value);
                bool forceDelete = hdnForceDelete.Value == "true";

                if (!forceDelete && _orderItemService.HasRelatedData(itemId))
                {
                    var constraints = _orderItemService.GetRelatedDataInfo(itemId);
                    var constraintMessage = "Không thể xóa chi tiết vì có dữ liệu liên quan:<br/>" + 
                        string.Join("<br/>", constraints);
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                        $"showError('{constraintMessage}');", true);
                    return;
                }

                if (forceDelete)
                {
                    if (!_orderItemService.DeleteRelatedData(itemId))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                            "showError('Không thể xóa dữ liệu liên quan!');", true);
                        return;
                    }
                }

                if (_orderItemService.DeleteOrderItem(itemId))
                {
                    LoadOrderItems();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                        "showSuccess('Xóa chi tiết thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                        "showError('Không thể xóa chi tiết!');", true);
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