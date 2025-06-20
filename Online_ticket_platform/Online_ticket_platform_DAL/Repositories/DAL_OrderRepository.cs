using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Online_ticket_platform_Model;
using Online_ticket_platform_DAL.Interfaces;

namespace Online_ticket_platform_DAL.Repositories
{
    public class DAL_OrderRepository : DAL_IOrderRepository
    {
        private readonly string _connectionString;

        public DAL_OrderRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public List<MOD_Order> GetAllOrders()
        {
            var orders = new List<MOD_Order>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM orders ORDER BY id DESC", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(MapOrderFromReader(reader));
                        }
                    }
                }
            }
            return orders;
        }

        public MOD_Order GetOrderById(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM orders WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapOrderFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public bool AddOrder(MOD_Order order)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int nextId;
                        using (var command = new OracleCommand("SELECT seq_orders.NEXTVAL FROM DUAL", connection))
                        {
                            command.Transaction = transaction;
                            nextId = Convert.ToInt32(command.ExecuteScalar());
                        }
                        using (var command = new OracleCommand(@"
                            INSERT INTO orders (id, user_id, status, payment_method, amount, created_at)
                            VALUES (:id, :userId, :status, :paymentMethod, :amount, :createdAt)", connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.Add(":id", OracleDbType.Int32).Value = nextId;
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = order.UserId;
                            command.Parameters.Add(":status", OracleDbType.Varchar2).Value = order.Status;
                            command.Parameters.Add(":paymentMethod", OracleDbType.Varchar2).Value = order.PaymentMethod;
                            command.Parameters.Add(":amount", OracleDbType.Decimal).Value = order.Amount;
                            command.Parameters.Add(":createdAt", OracleDbType.TimeStamp).Value = order.CreatedAt;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool UpdateOrder(MOD_Order order)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new OracleCommand(@"
                            UPDATE orders SET
                                user_id = :userId,
                                status = :status,
                                payment_method = :paymentMethod,
                                amount = :amount
                            WHERE id = :id", connection))
                        {
                            command.Transaction = transaction;
                            command.BindByName = true;
                            command.Parameters.Add(":id", OracleDbType.Int32).Value = order.Id;
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = order.UserId;
                            command.Parameters.Add(":status", OracleDbType.Varchar2).Value = order.Status;
                            command.Parameters.Add(":paymentMethod", OracleDbType.Varchar2).Value = order.PaymentMethod;
                            command.Parameters.Add(":amount", OracleDbType.Decimal).Value = order.Amount;
                            command.ExecuteNonQuery();
                            int rowsAffected = command.ExecuteNonQuery();
                            System.Diagnostics.Debug.WriteLine($"Rows affected: {rowsAffected}");


                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool DeleteOrder(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("DELETE FROM orders WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool HasRelatedData(int orderId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(@"
                    SELECT COUNT(*) FROM order_items WHERE order_id = :orderId", connection))
                {
                    command.Parameters.Add(":orderId", OracleDbType.Int32).Value = orderId;
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        public bool DeleteRelatedData(int orderId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new OracleCommand("DELETE FROM order_items WHERE order_id = :orderId", connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.Add(":orderId", OracleDbType.Int32).Value = orderId;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public List<string> GetRelatedDataInfo(int orderId)
        {
            var constraints = new List<string>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT COUNT(*) FROM order_items WHERE order_id = :orderId", connection))
                {
                    command.Parameters.Add(":orderId", OracleDbType.Int32).Value = orderId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} order_items liên quan");
                }
            }
            return constraints;
        }

        private MOD_Order MapOrderFromReader(OracleDataReader reader)
        {
            return new MOD_Order
            {
                Id = Convert.ToInt32(reader["id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Status = reader["status"].ToString(),
                PaymentMethod = reader["payment_method"].ToString(),
                Amount = Convert.ToDecimal(reader["amount"]),
                CreatedAt = reader["created_at"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["created_at"])
            };
        }
    }
}