using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;
using Online_ticket_platform_BLL;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_DAL.Repositories;

namespace Online_ticket_platform_BLL.Services
{
    public class BLL_OrderitemsService : BLL_IOrderitemsService
    {
        private readonly DAL_IOrderItemRepository _orderItemRepository;

        public BLL_OrderitemsService()
        {
            _orderItemRepository = new DAL_OrderItemRepository();
        }

        public List<MOD_OrderItem> GetAllOrderItems()
        {
            return _orderItemRepository.GetAllOrderItems();
        }

        public MOD_OrderItem GetOrderItemById(int id)
        {
            return _orderItemRepository.GetOrderItemById(id);
        }

        public bool AddOrderItem(MOD_OrderItem item)
        {
            if (item == null) return false;
            if (item.OrderId <= 0) return false;
            if (item.TicketId <= 0) return false;
            if (item.Quantity <= 0) return false;
            if (item.PriceSnapshot < 0) return false;
            return _orderItemRepository.AddOrderItem(item);
        }

        public bool UpdateOrderItem(MOD_OrderItem item)
        {
            if (item == null) return false;
            if (item.OrderId <= 0) return false;
            if (item.TicketId <= 0) return false;
            if (item.Quantity <= 0) return false;
            if (item.PriceSnapshot < 0) return false;
            return _orderItemRepository.UpdateOrderItem(item);
        }

        public bool DeleteOrderItem(int id)
        {
            return _orderItemRepository.DeleteOrderItem(id);
        }

        public bool HasRelatedData(int itemId)
        {
            return _orderItemRepository.HasRelatedData(itemId);
        }

        public bool DeleteRelatedData(int itemId)
        {
            return _orderItemRepository.DeleteRelatedData(itemId);
        }

        public List<string> GetRelatedDataInfo(int itemId)
        {
            return _orderItemRepository.GetRelatedDataInfo(itemId);
        }
    }
}
