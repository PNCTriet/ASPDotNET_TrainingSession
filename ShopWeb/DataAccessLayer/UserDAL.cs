using System;
using System.Data;
using System.Data.SqlClient;
using BusinessEntity;
using System.Collections.Generic;
using DataAccessLayer.DataConnection;

namespace DataAccessLayer
{
    public class UserDAL
    {
        private readonly string connectionString = Connection.GetConnectionString();

        public DataTable GetAllUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT u.UserID, u.Username, u.Email, u.IsActive, u.CreatedAt, u.UpdatedAt,
                           e.FirstName, e.LastName, e.Title, e.HomePhone,
                           r.RoleID, r.RoleName
                    FROM Users u
                    LEFT JOIN Employees e ON u.EmployeeID = e.EmployeeID
                    LEFT JOIN Roles r ON u.RoleID = r.RoleID
                    ORDER BY u.UserID DESC", conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            
                            // Debug: Log số lượng records
                            System.Diagnostics.Debug.WriteLine($"Number of users found: {dt.Rows.Count}");
                            foreach (DataRow row in dt.Rows)
                            {
                                System.Diagnostics.Debug.WriteLine($"User: {row["Username"]}, Role: {row["RoleName"]}, Active: {row["IsActive"]}");
                            }
                            
                            return dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in GetAllUsers: {ex.Message}");
                        throw new Exception("Error getting users: " + ex.Message);
                    }
                }
            }
        }

        public bool UpdateUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE Users 
                    SET Username = @Username,
                        Email = @Email,
                        IsActive = @IsActive,
                        UpdatedAt = GETDATE()
                    WHERE UserID = @UserID", conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@UserID", user.UserID);
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error updating user: " + ex.Message);
                    }
                }
            }
        }

        public bool DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE UserID = @UserID", conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error deleting user: " + ex.Message);
                    }
                }
            }
        }

        public User GetUserById(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT u.UserID, u.Username, u.Email, u.IsActive, u.CreatedAt, u.UpdatedAt,
                           e.FirstName, e.LastName, e.Title, e.HomePhone,
                           r.RoleID, r.RoleName
                    FROM Users u
                    LEFT JOIN Employees e ON u.EmployeeID = e.EmployeeID
                    LEFT JOIN Roles r ON u.RoleID = r.RoleID
                    WHERE u.UserID = @UserID", conn))
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

        public bool UpdateUserRole(int userId, int roleId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE Users 
                    SET RoleID = @RoleID,
                        UpdatedAt = GETDATE()
                    WHERE UserID = @UserID", conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@RoleID", roleId);

                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error updating user role: " + ex.Message);
                    }
                }
            }
        }

        public DataTable GetAllRoles()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT RoleID, RoleName FROM Roles", conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error getting roles: " + ex.Message);
                    }
                }
            }
        }
    }
}