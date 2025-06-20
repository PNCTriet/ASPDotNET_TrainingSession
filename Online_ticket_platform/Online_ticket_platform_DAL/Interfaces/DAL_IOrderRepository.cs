using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_DAL.Interfaces
{
    public interface DAL_IOrderRepository
    {
        List<MOD_Order> GetAllOrders();
        MOD_Order GetOrderById(int id);
        bool AddOrder(MOD_Order order);
        bool UpdateOrder(MOD_Order order);
        bool DeleteOrder(int id);
        bool HasRelatedData(int orderId);
        bool DeleteRelatedData(int orderId);
        List<string> GetRelatedDataInfo(int orderId);
    }
}