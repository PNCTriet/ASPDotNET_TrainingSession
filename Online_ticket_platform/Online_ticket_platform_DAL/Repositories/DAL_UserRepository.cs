using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_DAL.Repositories
{
    public class DAL_UserRepository : DAL_IUserRepository
    {
        private readonly string _connectionString;

        public DAL_UserRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
        }

        public List<MOD_User> GetAllUsers()
        {
            var users = new List<MOD_User>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM users ORDER BY created_at DESC", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUserFromReader(reader));
                        }
                    }
                }
            }
            return users;
        }

        public MOD_User GetUserById(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM users WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUserFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public MOD_User GetUserByEmail(string email)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("SELECT * FROM users WHERE email = :email", connection))
                {
                    command.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUserFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public bool AddUser(MOD_User user)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(@"
                    INSERT INTO users (id, email, password_hash, name, phone, role, is_verified, registered_at, created_at)
                    VALUES (seq_users.NEXTVAL, :email, :password_hash, :name, :phone, :role, :is_verified, :registered_at, :created_at)
                    RETURNING id INTO :id", connection))
                {
                    command.Parameters.Add(":email", OracleDbType.Varchar2).Value = user.Email;
                    command.Parameters.Add(":password_hash", OracleDbType.Varchar2).Value = user.PasswordHash;
                    command.Parameters.Add(":name", OracleDbType.Varchar2).Value = (object)user.Name ?? DBNull.Value;
                    command.Parameters.Add(":phone", OracleDbType.Varchar2).Value = (object)user.Phone ?? DBNull.Value;
                    command.Parameters.Add(":role", OracleDbType.Varchar2).Value = user.Role;
                    command.Parameters.Add(":is_verified", OracleDbType.Int32).Value = user.IsVerified ? 1 : 0;
                    command.Parameters.Add(":registered_at", OracleDbType.TimeStamp).Value = (object)user.RegisteredAt ?? DBNull.Value;
                    command.Parameters.Add(":created_at", OracleDbType.TimeStamp).Value = user.CreatedAt;

                    var idParam = new OracleParameter(":id", OracleDbType.Int32);
                    idParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(idParam);

                    try
                    {
                        command.ExecuteNonQuery();
                        user.Id = Convert.ToInt32(idParam.Value.ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public bool UpdateUser(MOD_User user)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(@"
            UPDATE users 
            SET email = :email,
                name = :name,
                phone = :phone,
                role = :role,
                is_verified = :is_verified
            WHERE id = :id", connection))
                {
                    command.BindByName = true;

                    command.Parameters.Add(":id", OracleDbType.Int32).Value = user.Id;
                    command.Parameters.Add(":email", OracleDbType.Varchar2).Value = user.Email;
                    command.Parameters.Add(":name", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(user.Name) ? (object)DBNull.Value : user.Name;
                    command.Parameters.Add(":phone", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(user.Phone) ? (object)DBNull.Value : user.Phone;
                    command.Parameters.Add(":role", OracleDbType.Varchar2).Value = user.Role;
                    command.Parameters.Add(":is_verified", OracleDbType.Int32).Value = user.IsVerified ? 1 : 0;

                    try
                    {
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("UpdateUser Error: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public bool DeleteUser(int id)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand("DELETE FROM users WHERE id = :id", connection))
                {
                    command.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    try
                    {
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public bool HasRelatedData(int userId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(@"
                    SELECT COUNT(*) FROM (
                        SELECT user_id FROM user_organizations WHERE user_id = :userId
                        UNION ALL
                        SELECT user_id FROM orders WHERE user_id = :userId
                        UNION ALL
                        SELECT user_id FROM checkin_logs WHERE user_id = :userId
                        UNION ALL
                        SELECT user_id FROM email_logs WHERE user_id = :userId
                        UNION ALL
                        SELECT user_id FROM webhook_logs WHERE user_id = :userId
                        UNION ALL
                        SELECT user_id FROM tracking_visits WHERE user_id = :userId
                        UNION ALL
                        SELECT uploaded_by FROM images WHERE uploaded_by = :userId
                    )", connection))
                {
                    command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        public List<string> GetRelatedDataInfo(int userId)
        {
            var constraints = new List<string>();
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                // Check user_organizations
                using (var command = new OracleCommand("SELECT COUNT(*) FROM user_organizations WHERE user_id = :userId", connection))
                {
                    command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} tổ chức liên quan");
                }

                // Check orders
                using (var command = new OracleCommand("SELECT COUNT(*) FROM orders WHERE user_id = :userId", connection))
                {
                    command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} đơn hàng");
                }

                // Check checkin_logs
                using (var command = new OracleCommand("SELECT COUNT(*) FROM checkin_logs WHERE user_id = :userId", connection))
                {
                    command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử check-in");
                }

                // Check email_logs
                using (var command = new OracleCommand("SELECT COUNT(*) FROM email_logs WHERE user_id = :userId", connection))
                {
                    command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử email");
                }

                // Check webhook_logs
                using (var command = new OracleCommand("SELECT COUNT(*) FROM webhook_logs WHERE user_id = :userId", connection))
                {
                    command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử webhook");
                }

                // Check tracking_visits
                using (var command = new OracleCommand("SELECT COUNT(*) FROM tracking_visits WHERE user_id = :userId", connection))
                {
                    command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} lịch sử truy cập");
                }

                // Check images
                using (var command = new OracleCommand("SELECT COUNT(*) FROM images WHERE uploaded_by = :userId", connection))
                {
                    command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                        constraints.Add($"Có {count} hình ảnh đã tải lên");
                }
            }
            return constraints;
        }

        public void DeleteRelatedData(int userId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Delete from user_organizations
                        using (var command = new OracleCommand("DELETE FROM user_organizations WHERE user_id = :userId", connection))
                        {
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                            command.ExecuteNonQuery();
                        }

                        // Delete from orders
                        using (var command = new OracleCommand("DELETE FROM orders WHERE user_id = :userId", connection))
                        {
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                            command.ExecuteNonQuery();
                        }

                        // Delete from checkin_logs
                        using (var command = new OracleCommand("DELETE FROM checkin_logs WHERE user_id = :userId", connection))
                        {
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                            command.ExecuteNonQuery();
                        }

                        // Delete from email_logs
                        using (var command = new OracleCommand("DELETE FROM email_logs WHERE user_id = :userId", connection))
                        {
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                            command.ExecuteNonQuery();
                        }

                        // Delete from webhook_logs
                        using (var command = new OracleCommand("DELETE FROM webhook_logs WHERE user_id = :userId", connection))
                        {
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                            command.ExecuteNonQuery();
                        }

                        // Delete from tracking_visits
                        using (var command = new OracleCommand("DELETE FROM tracking_visits WHERE user_id = :userId", connection))
                        {
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                            command.ExecuteNonQuery();
                        }

                        // Delete from images
                        using (var command = new OracleCommand("DELETE FROM images WHERE uploaded_by = :userId", connection))
                        {
                            command.Parameters.Add(":userId", OracleDbType.Int32).Value = userId;
                            command.ExecuteNonQuery();
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

        private MOD_User MapUserFromReader(OracleDataReader reader)
        {
            return new MOD_User
            {
                Id = Convert.ToInt32(reader["id"]),
                Email = reader["email"].ToString(),
                PasswordHash = reader["password_hash"].ToString(),
                Name = reader["name"] != DBNull.Value ? reader["name"].ToString() : null,
                Phone = reader["phone"] != DBNull.Value ? reader["phone"].ToString() : null,
                Role = reader["role"].ToString(),
                IsVerified = Convert.ToBoolean(reader["is_verified"]),
                RegisteredAt = reader["registered_at"] != DBNull.Value ? Convert.ToDateTime(reader["registered_at"]) : (DateTime?)null,
                CreatedAt = Convert.ToDateTime(reader["created_at"])
            };
        }
    }
}
