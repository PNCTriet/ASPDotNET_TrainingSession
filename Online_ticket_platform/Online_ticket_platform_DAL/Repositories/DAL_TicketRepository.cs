using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Online_ticket_platform_Model;
using Online_ticket_platform_DAL.Interfaces;
using System.Diagnostics;

namespace Online_ticket_platform_DAL.Repositories
{
    public class DAL_TicketRepository : DAL_ITicketRepository
    {
        private readonly string _connectionString;

        public DAL_TicketRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public List<MOD_Ticket> GetAllTickets()
        {
            var tickets = new List<MOD_Ticket>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM tickets", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tickets.Add(MapTicketFromReader(reader));
                        }
                    }
                }
            }
            return tickets;
        }

        public MOD_Ticket GetTicketById(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM tickets WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapTicketFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public bool AddTicket(MOD_Ticket ticket)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Lấy ID tiếp theo từ sequence
                        int nextId;
                        using (var command = new OracleCommand("SELECT seq_tickets.NEXTVAL FROM DUAL", connection))
                        {
                            command.Transaction = transaction;
                            nextId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        // Insert ticket với ID đã lấy
                        using (var command = new OracleCommand(@"
                            INSERT INTO tickets (id, event_id, name, price, type, total, sold, start_sale_date, end_sale_date, is_active)
                            VALUES (:id, :eventId, :name, :price, :type, :total, :sold, TO_TIMESTAMP(:startSaleDate, 'YYYY-MM-DD HH24:MI:SS'), TO_TIMESTAMP(:endSaleDate, 'YYYY-MM-DD HH24:MI:SS'), :isActive)", connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.Add(":id", OracleDbType.Int32).Value = nextId;
                            command.Parameters.Add(":eventId", OracleDbType.Int32).Value = ticket.EventId;
                            command.Parameters.Add(":name", OracleDbType.Varchar2).Value = ticket.Name;
                            command.Parameters.Add(":price", OracleDbType.Decimal).Value = ticket.Price;
                            command.Parameters.Add(":type", OracleDbType.Varchar2).Value = ticket.Type;
                            command.Parameters.Add(":total", OracleDbType.Int32).Value = ticket.Total;
                            command.Parameters.Add(":sold", OracleDbType.Int32).Value = ticket.Sold;
                            command.Parameters.Add(":startSaleDate", OracleDbType.Varchar2).Value = ticket.StartSaleDate.ToString("yyyy-MM-dd HH:mm:ss");
                            command.Parameters.Add(":endSaleDate", OracleDbType.Varchar2).Value = ticket.EndSaleDate.ToString("yyyy-MM-dd HH:mm:ss");
                            command.Parameters.Add(":isActive", OracleDbType.Int32).Value = ticket.IsActive ? 1 : 0;

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool UpdateTicket(MOD_Ticket ticket)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Log ticket data
                        Debug.WriteLine($"Updating ticket ID: {ticket.Id}");
                        Debug.WriteLine($"StartSaleDate: {ticket.StartSaleDate} (Type: {ticket.StartSaleDate.GetType()})");
                        Debug.WriteLine($"EndSaleDate: {ticket.EndSaleDate} (Type: {ticket.EndSaleDate.GetType()})");

                        using (var command = new OracleCommand(@"
                            UPDATE tickets 
                            SET event_id = :eventId,
                                name = :name,
                                price = :price,
                                type = :type,
                                total = :total,
                                sold = :sold,
                                start_sale_date = :startSaleDate,
                                end_sale_date = :endSaleDate,
                                is_active = :isActive
                            WHERE id = :id", connection))
                        {
                            command.Transaction = transaction;

                            //// Log parameter values before adding
                            //Debug.WriteLine("Adding parameters:");
                            //Debug.WriteLine($"id: {ticket.Id} (Type: Int32)");
                            //Debug.WriteLine($"eventId: {ticket.EventId} (Type: Int32)");
                            //Debug.WriteLine($"name: {ticket.Name} (Type: Varchar2)");
                            //Debug.WriteLine($"price: {ticket.Price} (Type: Decimal)");
                            //Debug.WriteLine($"type: {ticket.Type} (Type: Varchar2)");
                            //Debug.WriteLine($"total: {ticket.Total} (Type: Int32)");
                            //Debug.WriteLine($"sold: {ticket.Sold} (Type: Int32)");
                            //Debug.WriteLine($"startSaleDate: {ticket.StartSaleDate} (Type: TimeStamp)");
                            //Debug.WriteLine($"endSaleDate: {ticket.EndSaleDate} (Type: TimeStamp)");
                            //Debug.WriteLine($"isActive: {(ticket.IsActive ? 1 : 0)} (Type: Int32)");
                            command.BindByName = true;
                            command.Parameters.Add(":id", OracleDbType.Int32).Value = ticket.Id;
                            command.Parameters.Add(":eventId", OracleDbType.Int32).Value = ticket.EventId;
                            command.Parameters.Add(":name", OracleDbType.Varchar2).Value = ticket.Name;
                            command.Parameters.Add(":price", OracleDbType.Decimal).Value = ticket.Price;
                            command.Parameters.Add(":type", OracleDbType.Varchar2).Value = ticket.Type;
                            command.Parameters.Add(":total", OracleDbType.Int32).Value = ticket.Total;
                            command.Parameters.Add(":sold", OracleDbType.Int32).Value = ticket.Sold;
                            command.Parameters.Add(":startSaleDate", OracleDbType.TimeStamp).Value = ticket.StartSaleDate;
                            command.Parameters.Add(":endSaleDate", OracleDbType.TimeStamp).Value = ticket.EndSaleDate;
                            //command.Parameters.Add(":startSaleDate", OracleDbType.Varchar2).Value = ticket.StartSaleDate.ToString("yyyy-MM-dd HH:mm:ss");
                            //command.Parameters.Add(":endSaleDate", OracleDbType.Varchar2).Value = ticket.EndSaleDate.ToString("yyyy-MM-dd HH:mm:ss");
                            command.Parameters.Add(":isActive", OracleDbType.Int32).Value = ticket.IsActive ? 1 : 0;

                            // Log the final SQL command
                            //Debug.WriteLine("Executing SQL:");
                            //Debug.WriteLine(command.CommandText);
                            //foreach (OracleParameter param in command.Parameters)
                            //{
                            //    Debug.WriteLine($"Parameter {param.ParameterName}: {param.Value} (Type: {param.OracleDbType})");
                            //}

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error in UpdateTicket: {ex.Message}");
                        Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool DeleteTicket(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("DELETE FROM tickets WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool HasRelatedData(int ticketId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(@"
                    SELECT COUNT(*) FROM (
                        SELECT id FROM order_items WHERE ticket_id = :ticketId
                        UNION ALL
                        SELECT id FROM checkin_logs WHERE ticket_id = :ticketId
                    )", connection))
                {
                    command.Parameters.Add(":ticketId", OracleDbType.Int32).Value = ticketId;
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        public bool DeleteRelatedData(int ticketId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Xóa các bản ghi liên quan từ bảng order_items
                        using (var command = new OracleCommand(@"
                            DELETE FROM order_items 
                            WHERE ticket_id = :ticketId", connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.Add(":ticketId", OracleDbType.Int32).Value = ticketId;
                            command.ExecuteNonQuery();
                        }

                        // Xóa các bản ghi liên quan từ bảng checkin_logs
                        using (var command = new OracleCommand(@"
                            DELETE FROM checkin_logs 
                            WHERE ticket_id = :ticketId", connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.Add(":ticketId", OracleDbType.Int32).Value = ticketId;
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

        public List<string> GetRelatedDataInfo(int ticketId)
        {
            var constraints = new List<string>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                // Check order_items
                using (var command = new OracleCommand("SELECT COUNT(*) FROM order_items WHERE ticket_id = :ticketId", connection))
                {
                    command.Parameters.Add(":ticketId", OracleDbType.Int32).Value = ticketId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} đơn hàng");
                }

                // Check checkin_logs
                using (var command = new OracleCommand("SELECT COUNT(*) FROM checkin_logs WHERE ticket_id = :ticketId", connection))
                {
                    command.Parameters.Add(":ticketId", OracleDbType.Int32).Value = ticketId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử check-in");
                }
            }
            return constraints;
        }

        private MOD_Ticket MapTicketFromReader(OracleDataReader reader)
        {
            return new MOD_Ticket
            {
                Id = Convert.ToInt32(reader["id"]),
                EventId = Convert.ToInt32(reader["event_id"]),
                Name = reader["name"].ToString(),
                Price = Convert.ToDecimal(reader["price"]),
                Type = reader["type"].ToString(),
                Total = Convert.ToInt32(reader["total"]),
                Sold = Convert.ToInt32(reader["sold"]),
                StartSaleDate = Convert.ToDateTime(reader["start_sale_date"]),
                EndSaleDate = Convert.ToDateTime(reader["end_sale_date"]),
                IsActive = Convert.ToInt32(reader["is_active"]) == 1
            };
        }
    }
}
