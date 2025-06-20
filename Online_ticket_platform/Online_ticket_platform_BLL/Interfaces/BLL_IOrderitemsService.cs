using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_BLL
{
    public interface BLL_IOrderitemsService
    {
        List<MOD_OrderItem> GetAllOrderItems();
        MOD_OrderItem GetOrderItemById(int id);
        bool AddOrderItem(MOD_OrderItem item);
        bool UpdateOrderItem(MOD_OrderItem item);
        bool DeleteOrderItem(int id);
        bool HasRelatedData(int itemId);
        bool DeleteRelatedData(int itemId);
        List<string> GetRelatedDataInfo(int itemId);
    }
}
