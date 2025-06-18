using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_Model;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace Online_ticket_platform_DAL.Repositories
{
    public class DAL_OrganizationRepository : DAL_IOrganizationRepository
    {
        private readonly string _connectionString;

        public DAL_OrganizationRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public List<MOD_Organization> GetAllOrganizations()
        {
            List<MOD_Organization> organizations = new List<MOD_Organization>();
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        id,
                        name,
                        contact_email,
                        phone,
                        address,
                        created_at
                    FROM organizations 
                    ORDER BY created_at DESC";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                organizations.Add(new MOD_Organization
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Name = reader["name"].ToString(),
                                    ContactEmail = reader["contact_email"].ToString(),
                                    Phone = reader["phone"].ToString(),
                                    Address = reader["address"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi truy vấn danh sách tổ chức: " + ex.Message);
                    }
                }
            }
            return organizations;
        }

        public MOD_Organization GetOrganizationById(int id)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        id,
                        name,
                        contact_email,
                        phone,
                        address,
                        created_at
                    FROM organizations 
                    WHERE id = :id";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new MOD_Organization
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Name = reader["name"].ToString(),
                                    ContactEmail = reader["contact_email"].ToString(),
                                    Phone = reader["phone"].ToString(),
                                    Address = reader["address"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi truy vấn thông tin tổ chức: " + ex.Message);
                    }
                }
            }
            return null;
        }

        public MOD_Organization GetOrganizationByEmail(string email)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        id,
                        name,
                        contact_email,
                        phone,
                        address,
                        created_at
                    FROM organizations 
                    WHERE contact_email = :email";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new MOD_Organization
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Name = reader["name"].ToString(),
                                    ContactEmail = reader["contact_email"].ToString(),
                                    Phone = reader["phone"].ToString(),
                                    Address = reader["address"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi truy vấn tổ chức theo email: " + ex.Message);
                    }
                }
            }
            return null;
        }

        public bool AddOrganization(MOD_Organization organization)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO organizations (
                        id,
                        name,
                        contact_email,
                        phone,
                        address,
                        created_at
                    ) VALUES (
                        seq_organizations.NEXTVAL,
                        :name,
                        :contact_email,
                        :phone,
                        :address,
                        SYSDATE
                    )";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Add(":name", OracleDbType.Varchar2).Value = organization.Name;
                        command.Parameters.Add(":contact_email", OracleDbType.Varchar2).Value = organization.ContactEmail;
                        command.Parameters.Add(":phone", OracleDbType.Varchar2).Value = organization.Phone;
                        command.Parameters.Add(":address", OracleDbType.Varchar2).Value = organization.Address;

                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi thêm tổ chức mới: " + ex.Message);
                    }
                }
            }
        }

        public bool UpdateOrganization(MOD_Organization organization)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                string query = @"
                    UPDATE organizations 
                    SET name = :name,
                        contact_email = :contact_email,
                        phone = :phone,
                        address = :address
                    WHERE id = :id";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Add(":name", OracleDbType.Varchar2).Value = organization.Name;
                        command.Parameters.Add(":contact_email", OracleDbType.Varchar2).Value = organization.ContactEmail;
                        command.Parameters.Add(":phone", OracleDbType.Varchar2).Value = organization.Phone;
                        command.Parameters.Add(":address", OracleDbType.Varchar2).Value = organization.Address;
                        command.Parameters.Add(":id", OracleDbType.Int32).Value = organization.Id;

                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi cập nhật thông tin tổ chức: " + ex.Message);
                    }
                }
            }
        }

        public bool DeleteOrganization(int id)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                string query = "DELETE FROM organizations WHERE id = :id";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi xóa tổ chức: " + ex.Message);
                    }
                }
            }
        }

        public bool HasRelatedData(int organizationId)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        (SELECT COUNT(*) FROM events WHERE organization_id = :id) as event_count,
                        (SELECT COUNT(*) FROM user_organizations WHERE organization_id = :id) as user_count,
                        (SELECT COUNT(*) FROM webhook_subscriptions WHERE organization_id = :id) as webhook_count,
                        (SELECT COUNT(*) FROM image_links WHERE organization_id = :id) as image_count
                    FROM dual";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Add(":id", OracleDbType.Int32).Value = organizationId;
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int eventCount = Convert.ToInt32(reader["event_count"]);
                                int userCount = Convert.ToInt32(reader["user_count"]);
                                int webhookCount = Convert.ToInt32(reader["webhook_count"]);
                                int imageCount = Convert.ToInt32(reader["image_count"]);

                                return (eventCount + userCount + webhookCount + imageCount) > 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi kiểm tra dữ liệu liên quan: " + ex.Message);
                    }
                }
            }
            return false;
        }

        public void DeleteRelatedData(int organizationId)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string[] deleteQueries = new string[]
                        {
                            "DELETE FROM events WHERE organization_id = :id",
                            "DELETE FROM user_organizations WHERE organization_id = :id",
                            "DELETE FROM webhook_subscriptions WHERE organization_id = :id",
                            "DELETE FROM image_links WHERE organization_id = :id"
                        };

                        foreach (string query in deleteQueries)
                        {
                            using (OracleCommand command = new OracleCommand())
                            {
                                command.Connection = connection;
                                command.Transaction = transaction;
                                command.CommandText = query;
                                command.Parameters.Add(":id", OracleDbType.Int32).Value = organizationId;
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public Dictionary<string, int> GetRelatedDataInfo(int organizationId)
        {
            var result = new Dictionary<string, int>();
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        (SELECT COUNT(*) FROM events WHERE organization_id = :id) as event_count,
                        (SELECT COUNT(*) FROM user_organizations WHERE organization_id = :id) as user_count,
                        (SELECT COUNT(*) FROM webhook_subscriptions WHERE organization_id = :id) as webhook_count,
                        (SELECT COUNT(*) FROM image_links WHERE organization_id = :id) as image_count
                    FROM dual";

                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Add(":id", OracleDbType.Int32).Value = organizationId;
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int eventCount = Convert.ToInt32(reader["event_count"]);
                                int userCount = Convert.ToInt32(reader["user_count"]);
                                int webhookCount = Convert.ToInt32(reader["webhook_count"]);
                                int imageCount = Convert.ToInt32(reader["image_count"]);

                                if (eventCount > 0) result.Add("events", eventCount);
                                if (userCount > 0) result.Add("user_organizations", userCount);
                                if (webhookCount > 0) result.Add("webhook_subscriptions", webhookCount);
                                if (imageCount > 0) result.Add("image_links", imageCount);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi lấy thông tin dữ liệu liên quan: " + ex.Message);
                    }
                }
            }
            return result;
        }
    }
}
