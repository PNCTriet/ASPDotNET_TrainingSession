using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BusinessEntity;
using DataAccessLayer.DataConnection;
using BCrypt.Net;

namespace DataAccessLayer
{
    public class UserDAL
    {
        private static string FormatDisplayUserID(int userId)
        {
            return $"USR{DateTime.Now:yy}{userId:D4}";
        }

        public static List<User> GetAllUsers()
        {
            var list = new List<User>();
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                string query = @"
                    SELECT u.UserID, u.Username, u.Email, u.IsActive, u.CreatedAt, u.UpdatedAt,
                            e.FirstName, e.LastName, e.Title, e.HomePhone,
                            r.RoleID, r.RoleName
                    FROM Users u
                    LEFT JOIN Employees e ON u.EmployeeID = e.EmployeeID
                    LEFT JOIN Roles r ON u.RoleID = r.RoleID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["UserID"]);
                                list.Add(new User
                                {
                                    UserID = userId,
                                    DisplayUserID = FormatDisplayUserID(userId),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Title = reader["Title"].ToString(),
                                    Phone = reader["HomePhone"].ToString(),
                                    RoleName = reader["RoleName"].ToString(),
                                    RoleID = Convert.ToInt32(reader["RoleID"])
                                });
                            }
                        }
                        return list;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error getting users: " + ex.Message);
                    }
                }
            }
        }

        public static bool UpdateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
                {
                    string query = @"
                        UPDATE Users 
                        SET Username = @Username,
                            Email = @Email,
                            IsActive = @IsActive,
                            UpdatedAt = GETDATE()
                        WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", user.UserID);
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user: " + ex.Message);
            }
        }

        public static bool DeleteUser(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
                {
                    conn.Open();
                    string query = "DELETE FROM Users WHERE UserID = @UserID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting user: " + ex.Message);
            }
        }

        public static User GetUserById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                string query = @"
                    SELECT u.UserID, u.Username, u.Email, u.IsActive, u.CreatedAt, u.UpdatedAt,
                           e.FirstName, e.LastName, e.Title, e.HomePhone,
                           r.RoleID, r.RoleName
                    FROM Users u
                    LEFT JOIN Employees e ON u.EmployeeID = e.EmployeeID
                    LEFT JOIN Roles r ON u.RoleID = r.RoleID
                    WHERE u.UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    DisplayUserID = FormatDisplayUserID(userId),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Title = reader["Title"].ToString(),
                                    Phone = reader["HomePhone"].ToString(),
                                    RoleName = reader["RoleName"].ToString(),
                                    RoleID = Convert.ToInt32(reader["RoleID"])
                                };
                            }
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error getting user: " + ex.Message);
                    }
                }
            }
        }

        public static List<Role> GetAllRoles()
        {
            var list = new List<Role>();
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                string query = "SELECT RoleID, RoleName FROM Roles";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Role
                                {
                                    RoleID = Convert.ToInt32(reader["RoleID"]),
                                    RoleName = reader["RoleName"].ToString()
                                });
                            }
                        }
                        return list;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error getting roles: " + ex.Message);
                    }
                }
            }
        }

        private static string GenerateNextUserID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
                {
                    string query = @"
                        SELECT TOP 1 UserID 
                        FROM Users 
                        ORDER BY UserID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        
                        if (result == null || result == DBNull.Value)
                        {
                            // Nếu chưa có UserID nào
                            return $"USR{DateTime.Now:yy}0001";
                        }
                        else
                        {
                            // Lấy số thứ tự từ UserID hiện tại
                            int currentId = Convert.ToInt32(result);
                            int sequence = currentId + 1;
                            return $"USR{DateTime.Now:yy}{sequence:D4}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating UserID: " + ex.Message);
            }
        }

        public static bool CreateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
                {
                    // Tạo UserID mới
                    string newUserID = GenerateNextUserID();
                    user.UserID = int.Parse(newUserID.Substring(5)); // Lấy phần số từ USRyyxxxx

                    // Tạo Employee trước
                    string employeeQuery = @"
                        DECLARE @NewEmployeeID INT;
                        SELECT @NewEmployeeID = ISNULL(MAX(EmployeeID), 0) + 1 FROM Employees;
                        
                        INSERT INTO Employees (EmployeeID, FirstName, LastName)
                        VALUES (@NewEmployeeID, @FirstName, @LastName);
                        
                        SELECT @NewEmployeeID;";

                    int employeeId;
                    using (SqlCommand empCmd = new SqlCommand(employeeQuery, conn))
                    {
                        empCmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        empCmd.Parameters.AddWithValue("@LastName", user.LastName);

                        conn.Open();
                        object result = empCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            employeeId = Convert.ToInt32(result);
                            System.Diagnostics.Debug.WriteLine($"Created Employee with ID: {employeeId}");
                        }
                        else
                        {
                            throw new Exception("Failed to create Employee - no ID returned");
                        }
                    }

                    if (employeeId > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Proceeding to create User with EmployeeID: {employeeId}");
                        // Tạo User với EmployeeID đã có
                        string userQuery = @"
                            INSERT INTO Users (UserID, RoleID, EmployeeID, Username, PasswordHash, Email, IsActive, CreatedAt, UpdatedAt)
                            VALUES (@UserID, @RoleID, @EmployeeID, @Username, @PasswordHash, @Email, @IsActive, GETDATE(), GETDATE())";

                        using (SqlCommand cmd = new SqlCommand(userQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", user.UserID);
                            cmd.Parameters.AddWithValue("@RoleID", 2); // Mặc định là role Admin (2)
                            cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                            cmd.Parameters.AddWithValue("@Username", user.Username);
                            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                            cmd.Parameters.AddWithValue("@Email", user.Email);
                            cmd.Parameters.AddWithValue("@IsActive", true);

                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                // Cập nhật DisplayUserID cho user object
                                user.DisplayUserID = newUserID;
                                user.EmployeeID = employeeId;
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating user: " + ex.Message);
            }
        }

        public static User GetUserByUsername(string username)
        {
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                string query = @"
                    SELECT u.UserID, u.Username, u.Email, u.PasswordHash, u.IsActive, u.CreatedAt, u.UpdatedAt,
                           e.FirstName, e.LastName, e.Title,
                           r.RoleID, r.RoleName
                    FROM Users u
                    LEFT JOIN Employees e ON u.EmployeeID = e.EmployeeID
                    LEFT JOIN Roles r ON u.RoleID = r.RoleID
                    WHERE u.Username = @Username or u.Email = @Username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["UserID"]);
                                return new User
                                {
                                    UserID = userId,
                                    DisplayUserID = FormatDisplayUserID(userId),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Title = reader["Title"].ToString(),
                                    RoleName = reader["RoleName"].ToString(),
                                    RoleID = Convert.ToInt32(reader["RoleID"])
                                };
                            }
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error getting user by username: " + ex.Message);
                    }
                }
            }
        }
    }
}