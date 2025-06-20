using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;
using Online_ticket_platform_BLL;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_DAL.Repositories;

namespace Online_ticket_platform_BLL.Services
{
    public class BLL_OrderService : BLL_IOrderService
    {
        private readonly DAL_IOrderRepository _orderRepository;

        public BLL_OrderService()
        {
            _orderRepository = new DAL_OrderRepository();
        }

        public List<MOD_Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public MOD_Order GetOrderById(int id)
        {
            return _orderRepository.GetOrderById(id);
        }

        public bool AddOrder(MOD_Order order)
        {
            if (order == null) return false;
            if (order.UserId <= 0) return false;
            if (string.IsNullOrWhiteSpace(order.Status)) return false;
            if (string.IsNullOrWhiteSpace(order.PaymentMethod)) return false;
            if (order.Amount < 0) return false;
            return _orderRepository.AddOrder(order);
        }

        public bool UpdateOrder(MOD_Order order)
        {
            if (order == null) return false;
            if (order.UserId <= 0) return false;
            if (string.IsNullOrWhiteSpace(order.Status)) return false;
            if (string.IsNullOrWhiteSpace(order.PaymentMethod)) return false;
            if (order.Amount < 0) return false;
            return _orderRepository.UpdateOrder(order);
        }

        public bool DeleteOrder(int id)
        {
            return _orderRepository.DeleteOrder(id);
        }

        public bool HasRelatedData(int orderId)
        {
            return _orderRepository.HasRelatedData(orderId);
        }

        public bool DeleteRelatedData(int orderId)
        {
            return _orderRepository.DeleteRelatedData(orderId);
        }

        public List<string> GetRelatedDataInfo(int orderId)
        {
            return _orderRepository.GetRelatedDataInfo(orderId);
        }
    }
}
