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
    public partial class Order_List : System.Web.UI.Page
    {
        private readonly BLL_IOrderService _orderService;

        public Order_List()
        {
            _orderService = new BLL_OrderService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            var orders = _orderService.GetAllOrders();
            gvOrders.DataSource = orders;
            gvOrders.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var order = new MOD_Order
                {
                    UserId = Convert.ToInt32(txtUserId.Text.Trim()),
                    Status = txtStatus.Text.Trim(),
                    PaymentMethod = txtPaymentMethod.Text.Trim(),
                    Amount = Convert.ToDecimal(txtAmount.Text.Trim()),
                    CreatedAt = DateTime.Now
                };

                if (_orderService.AddOrder(order))
                {
                    LoadOrders();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Thêm đơn hàng thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể thêm đơn hàng!');", true);
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
                int orderId = Convert.ToInt32(hdnEditId.Value);
                var order = _orderService.GetOrderById(orderId);
                if (order == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không tìm thấy đơn hàng!');", true);
                    return;
                }

                order.UserId = Convert.ToInt32(txtEditUserId.Text.Trim());
                order.Status = txtEditStatus.Text.Trim();
                order.PaymentMethod = txtEditPaymentMethod.Text.Trim();
                order.Amount = Convert.ToDecimal(txtEditAmount.Text.Trim());

                if (_orderService.UpdateOrder(order))
                {
                    LoadOrders();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "showSuccess('Cập nhật đơn hàng thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", "showError('Không thể cập nhật đơn hàng! Vui lòng kiểm tra lại thông tin.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error", $"showError('Có lỗi xảy ra: {ex.Message}');", true);
            }
        }

        [WebMethod]
        public static object CheckConstraints(int orderId)
        {
            try
            {
                var orderService = new BLL_OrderService();
                var constraints = orderService.GetRelatedDataInfo(orderId);
                return new { success = true, data = constraints };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditOrder")
            {
                int orderId = Convert.ToInt32(e.CommandArgument);
                var order = _orderService.GetOrderById(orderId);
                if (order != null)
                {
                    hdnEditId.Value = orderId.ToString();
                    txtEditUserId.Text = order.UserId.ToString();
                    txtEditStatus.Text = order.Status;
                    txtEditPaymentMethod.Text = order.PaymentMethod;
                    txtEditAmount.Text = order.Amount.ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", 
                        "$(document).ready(function() { $('#editModal').modal('show'); });", true);
                }
            }
            else if (e.CommandName == "DeleteOrder")
            {
                hdnDeleteId.Value = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "showDeleteModal", 
                    "$(document).ready(function() { $('#deleteModal').modal('show'); });", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int orderId = Convert.ToInt32(hdnDeleteId.Value);
                bool forceDelete = hdnForceDelete.Value == "true";

                if (!forceDelete && _orderService.HasRelatedData(orderId))
                {
                    var constraints = _orderService.GetRelatedDataInfo(orderId);
                    var constraintMessage = "Không thể xóa đơn hàng vì có dữ liệu liên quan:<br/>" + 
                        string.Join("<br/>", constraints);
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                        $"showError('{constraintMessage}');", true);
                    return;
                }

                if (forceDelete)
                {
                    if (!_orderService.DeleteRelatedData(orderId))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                            "showError('Không thể xóa dữ liệu liên quan!');", true);
                        return;
                    }
                }

                if (_orderService.DeleteOrder(orderId))
                {
                    LoadOrders();
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", 
                        "showSuccess('Xóa đơn hàng thành công!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", 
                        "showError('Không thể xóa đơn hàng!');", true);
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