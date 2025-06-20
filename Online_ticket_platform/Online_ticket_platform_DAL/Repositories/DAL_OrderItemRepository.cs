using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Online_ticket_platform_Model;
using Online_ticket_platform_DAL.Interfaces;

namespace Online_ticket_platform_DAL.Repositories
{
    public class DAL_OrderItemRepository : DAL_IOrderItemRepository
    {
        private readonly string _connectionString;

        public DAL_OrderItemRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public List<MOD_OrderItem> GetAllOrderItems()
        {
            var items = new List<MOD_OrderItem>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM order_items ORDER BY id DESC", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(MapOrderItemFromReader(reader));
                        }
                    }
                }
            }
            return items;
        }

        public MOD_OrderItem GetOrderItemById(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM order_items WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapOrderItemFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public bool AddOrderItem(MOD_OrderItem item)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int nextId;
                        using (var command = new OracleCommand("SELECT seq_order_items.NEXTVAL FROM DUAL", connection))
                        {
                            command.Transaction = transaction;
                            nextId = Convert.ToInt32(command.ExecuteScalar());
                        }
                        using (var command = new OracleCommand(@"
                            INSERT INTO order_items (id, order_id, ticket_id, quantity, price_snapshot)
                            VALUES (:id, :orderId, :ticketId, :quantity, :priceSnapshot)", connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.Add(":id", OracleDbType.Int32).Value = nextId;
                            command.Parameters.Add(":orderId", OracleDbType.Int32).Value = item.OrderId;
                            command.Parameters.Add(":ticketId", OracleDbType.Int32).Value = item.TicketId;
                            command.Parameters.Add(":quantity", OracleDbType.Int32).Value = item.Quantity;
                            command.Parameters.Add(":priceSnapshot", OracleDbType.Decimal).Value = item.PriceSnapshot;
                            command.ExecuteNonQuery();

                            int result = command.ExecuteNonQuery();
                            transaction.Commit();
                            return result > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[UpdateImageLink] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public bool UpdateOrderItem(MOD_OrderItem item)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE order_items SET
                                order_id = :orderId,
                                ticket_id = :ticketId,
                                quantity = :quantity,
                                price_snapshot = :priceSnapshot
                            WHERE id = :id";
                        using (var command = new OracleCommand(query, conn))
                        {
                            command.Transaction = transaction;
                            command.Parameters.Add(":id", OracleDbType.Int32).Value = item.Id;
                            command.Parameters.Add(":orderId", OracleDbType.Int32).Value = item.OrderId;
                            command.Parameters.Add(":ticketId", OracleDbType.Int32).Value = item.TicketId;
                            command.Parameters.Add(":quantity", OracleDbType.Int32).Value = item.Quantity;
                            command.Parameters.Add(":priceSnapshot", OracleDbType.Decimal).Value = item.PriceSnapshot;
                            command.ExecuteNonQuery();
                            int result = command.ExecuteNonQuery();
                            transaction.Commit();
                            return result > 0;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[UpdateOrderItem] Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public bool DeleteOrderItem(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("DELETE FROM order_items WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool HasRelatedData(int itemId)
        {
            // Không có bảng con liên quan trực tiếp đến order_items nên luôn trả về false
            return false;
        }

        public bool DeleteRelatedData(int itemId)
        {
            // Không có bảng con liên quan trực tiếp đến order_items nên luôn trả về true
            return true;
        }

        public List<string> GetRelatedDataInfo(int itemId)
        {
            // Không có bảng con liên quan trực tiếp đến order_items
            return new List<string>();
        }

        private MOD_OrderItem MapOrderItemFromReader(OracleDataReader reader)
        {
            return new MOD_OrderItem
            {
                Id = Convert.ToInt32(reader["id"]),
                OrderId = Convert.ToInt32(reader["order_id"]),
                TicketId = Convert.ToInt32(reader["ticket_id"]),
                Quantity = Convert.ToInt32(reader["quantity"]),
                PriceSnapshot = Convert.ToDecimal(reader["price_snapshot"])
            };
        }
    }
}
