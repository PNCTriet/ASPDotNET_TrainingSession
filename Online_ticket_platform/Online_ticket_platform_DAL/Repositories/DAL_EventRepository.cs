using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_DAL.Repositories
{
    public class DAL_EventRepository : DAL_IEventRepository
    {
        private readonly string _connectionString;

        public DAL_EventRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public List<MOD_Event> GetAllEvents()
        {
            var events = new List<MOD_Event>();

            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT 
                id,
                organization_id,
                name,
                description,
                event_date,
                location
            FROM 
                events
            ORDER BY 
                event_date DESC";

                using (var command = new OracleCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(MapEventFromReader(reader));
                    }
                }
            }

            return events;
        }

        public MOD_Event GetEventById(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM events WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapEventFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public List<MOD_Event> GetEventsByOrganizationId(int organizationId)
        {
            var events = new List<MOD_Event>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM events WHERE organization_id = :orgId ORDER BY event_date DESC", connection))
                {
                    command.Parameters.Add(":orgId", OracleDbType.Int32).Value = organizationId;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(MapEventFromReader(reader));
                        }
                    }
                }
            }
            return events;
        }

        public bool AddEvent(MOD_Event evt)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Lấy ID mới từ sequence
                        var cmdGetId = new OracleCommand("SELECT seq_events.NEXTVAL FROM DUAL", connection);
                        cmdGetId.Transaction = transaction;
                        evt.Id = Convert.ToInt32(cmdGetId.ExecuteScalar());

                        // Insert dữ liệu với ID đã lấy
                        var command = new OracleCommand(@"
                            INSERT INTO events (id, organization_id, name, description, event_date, location)
                            VALUES (:id, :orgId, :name, :description, :eventDate, :location)", connection);
                        command.Transaction = transaction;

                        command.Parameters.Add(":id", OracleDbType.Int32).Value = evt.Id;
                        command.Parameters.Add(":orgId", OracleDbType.Int32).Value = evt.OrganizationId;
                        command.Parameters.Add(":name", OracleDbType.Varchar2).Value = evt.Name;
                        command.Parameters.Add(":description", OracleDbType.Clob).Value = evt.Description;
                        command.Parameters.Add(":eventDate", OracleDbType.Date).Value = evt.EventDate;
                        command.Parameters.Add(":location", OracleDbType.Varchar2).Value = evt.Location;

                        command.ExecuteNonQuery();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Log error
                        System.Diagnostics.Debug.WriteLine($"Error adding event: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        public bool UpdateEvent(MOD_Event evt)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(@"
                    UPDATE events 
                    SET name = :name,
                        description = :description,
                        event_date = :eventDate,
                        location = :location,
                        organization_id = :organizationId
                    WHERE id = :id", connection))
                {
                    command.BindByName = true;
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = evt.Id;
                    command.Parameters.Add(":name", OracleDbType.Varchar2).Value = (object)evt.Name ?? DBNull.Value;
                    command.Parameters.Add(":description", OracleDbType.Clob).Value = (object)evt.Description ?? DBNull.Value;
                    // Không dùng ToUniversalTime(), truyền trực tiếp EventDate
                    command.Parameters.Add(":eventDate", OracleDbType.Date).Value = evt.EventDate;
                    command.Parameters.Add(":location", OracleDbType.Varchar2).Value = (object)evt.Location ?? DBNull.Value;
                    command.Parameters.Add(":organizationId", OracleDbType.Int32).Value = evt.OrganizationId;

                    try
                    {
                        System.Diagnostics.Debug.WriteLine("ID: " + evt.Id);
                        System.Diagnostics.Debug.WriteLine("Name: " + evt.Name);
                        System.Diagnostics.Debug.WriteLine("Description: " + evt.Description);
                        System.Diagnostics.Debug.WriteLine("EventDate: " + evt.EventDate.ToString("yyyy-MM-dd"));
                        System.Diagnostics.Debug.WriteLine("Location: " + evt.Location);
                        System.Diagnostics.Debug.WriteLine("OrganizationId: " + evt.OrganizationId);

                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        // Ghi log chi tiết lỗi để dễ kiểm tra
                        System.Diagnostics.Debug.WriteLine("UpdateEvent Error: " + ex);
                        return false;
                    }
                }
            }
        }

        public bool DeleteEvent(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(@"
                DELETE FROM events 
                WHERE id = :id", connection))
                {
                    command.BindByName = true;
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;

                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Deleting event with ID: " + id);
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("DeleteEvent Error: " + ex);
                        return false;
                    }
                }
            }
        }

        public bool HasRelatedData(int eventId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(@"
                    SELECT COUNT(*) FROM (
                        SELECT event_id FROM tickets WHERE event_id = :eventId
                        UNION ALL
                        SELECT event_id FROM orders WHERE event_id = :eventId
                        UNION ALL
                        SELECT event_id FROM checkin_logs WHERE event_id = :eventId
                        UNION ALL
                        SELECT event_id FROM email_logs WHERE event_id = :eventId
                        UNION ALL
                        SELECT event_id FROM webhook_logs WHERE event_id = :eventId
                        UNION ALL
                        SELECT event_id FROM tracking_visits WHERE event_id = :eventId
                        UNION ALL
                        SELECT event_id FROM image_links WHERE event_id = :eventId
                    )", connection))
                {
                    command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        public List<string> GetRelatedDataInfo(int eventId)
        {
            var constraints = new List<string>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                // Check tickets
                using (var command = new OracleCommand("SELECT COUNT(*) FROM tickets WHERE event_id = :eventId", connection))
                {
                    command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} vé");
                }

                // Check orders
                using (var command = new OracleCommand("SELECT COUNT(*) FROM orders WHERE event_id = :eventId", connection))
                {
                    command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} đơn hàng");
                }

                // Check checkin_logs
                using (var command = new OracleCommand("SELECT COUNT(*) FROM checkin_logs WHERE event_id = :eventId", connection))
                {
                    command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử check-in");
                }

                // Check email_logs
                using (var command = new OracleCommand("SELECT COUNT(*) FROM email_logs WHERE event_id = :eventId", connection))
                {
                    command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử email");
                }

                // Check webhook_logs
                using (var command = new OracleCommand("SELECT COUNT(*) FROM webhook_logs WHERE event_id = :eventId", connection))
                {
                    command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử webhook");
                }

                // Check tracking_visits
                using (var command = new OracleCommand("SELECT COUNT(*) FROM tracking_visits WHERE event_id = :eventId", connection))
                {
                    command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử truy cập");
                }

                // Check image_links
                using (var command = new OracleCommand("SELECT COUNT(*) FROM image_links WHERE event_id = :eventId", connection))
                {
                    command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} hình ảnh liên kết");
                }
            }
            return constraints;
        }

        public bool DeleteRelatedData(int eventId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Xóa các bản ghi liên quan từ bảng tickets
                        using (var command = new OracleCommand(@"
                        DELETE FROM tickets 
                        WHERE event_id = :eventId", connection))
                        {
                            command.Transaction = transaction;
                            command.BindByName = true;
                            command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                            command.ExecuteNonQuery();
                        }

                        // Xóa các bản ghi liên quan từ bảng event_images
                        using (var command = new OracleCommand(@"
                        DELETE FROM event_images 
                        WHERE event_id = :eventId", connection))
                        {
                            command.Transaction = transaction;
                            command.BindByName = true;
                            command.Parameters.Add(":eventId", OracleDbType.Int32).Value = eventId;
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        System.Diagnostics.Debug.WriteLine($"Successfully deleted related data for event {eventId}");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        System.Diagnostics.Debug.WriteLine($"DeleteRelatedData Error for event {eventId}: {ex}");
                        return false;
                    }
                }
            }
        }

        private MOD_Event MapEventFromReader(OracleDataReader reader)
        {
            return new MOD_Event
            {
                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                OrganizationId = reader["organization_id"] != DBNull.Value ? Convert.ToInt32(reader["organization_id"]) : 0,
                Name = reader["name"] != DBNull.Value ? reader["name"].ToString() : string.Empty,
                Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : null,
                EventDate = reader["event_date"] != DBNull.Value ? Convert.ToDateTime(reader["event_date"]) : DateTime.MinValue,
                Location = reader["location"] != DBNull.Value ? reader["location"].ToString() : string.Empty
            };
        }
    }
}
