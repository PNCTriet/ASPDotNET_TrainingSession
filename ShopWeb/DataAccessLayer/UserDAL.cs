using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
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
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = @"
                    SELECT u.UserID, u.Username, u.Email, u.IsActive, u.CreatedAt, u.UpdatedAt,
                            e.FirstName, e.LastName, e.Title, e.HomePhone,
                            r.RoleID, r.RoleName
                    FROM Users u
                    LEFT JOIN Employees e ON u.EmployeeID = e.EmployeeID
                    LEFT JOIN Roles r ON u.RoleID = r.RoleID";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
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
                using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
                {
                    string query = @"
                        UPDATE Users 
                        SET Username = :Username,
                            Email = :Email,
                            IsActive = :IsActive,
                            UpdatedAt = SYSDATE
                        WHERE UserID = :UserID";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        // Add parameters in the same order as they appear in the SQL
                        cmd.Parameters.Add(":Username", OracleDbType.Varchar2).Value = user.Username;
                        cmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = user.Email;
                        cmd.Parameters.Add(":IsActive", OracleDbType.Int32).Value = user.IsActive ? 1 : 0;
                        cmd.Parameters.Add(":UserID", OracleDbType.Int32).Value = user.UserID;

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
                using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
                {
                    conn.Open();
                    string query = "DELETE FROM Users WHERE UserID = :UserID";
                    
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(":UserID", OracleDbType.Int32).Value = userId;
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
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = @"
                    SELECT u.UserID, u.Username, u.Email, u.IsActive, u.CreatedAt, u.UpdatedAt,
                           e.FirstName, e.LastName, e.Title, e.HomePhone,
                           r.RoleID, r.RoleName
                    FROM Users u
                    LEFT JOIN Employees e ON u.EmployeeID = e.EmployeeID
                    LEFT JOIN Roles r ON u.RoleID = r.RoleID
                    WHERE u.UserID = :UserID";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(":UserID", OracleDbType.Int32).Value = userId;
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
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
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = "SELECT RoleID, RoleName FROM Roles";
                
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
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
                using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
                {
                    string query = @"
                        SELECT UserID 
                        FROM Users 
                        WHERE ROWNUM = 1
                        ORDER BY UserID DESC";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        
                        if (result == null || result == DBNull.Value)
                        {
                            return $"USR{DateTime.Now:yy}0001";
                        }
                        else
                        {
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
                using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
                {
                    // Tạo UserID mới
                    string newUserID = GenerateNextUserID();
                    user.UserID = int.Parse(newUserID.Substring(5));

                    // Tạo Employee trước
                    string employeeQuery = @"
                        DECLARE
                            v_NewEmployeeID NUMBER;
                        BEGIN
                            SELECT NVL(MAX(EmployeeID), 0) + 1 INTO v_NewEmployeeID FROM Employees;
                            
                            INSERT INTO Employees (EmployeeID, FirstName, LastName)
                            VALUES (v_NewEmployeeID, :FirstName, :LastName);
                            
                            :NewEmployeeID := v_NewEmployeeID;
                        END;";

                    int employeeId;
                    using (OracleCommand empCmd = new OracleCommand(employeeQuery, conn))
                    {
                        empCmd.Parameters.Add(":FirstName", OracleDbType.Varchar2).Value = user.FirstName;
                        empCmd.Parameters.Add(":LastName", OracleDbType.Varchar2).Value = user.LastName;
                        empCmd.Parameters.Add(":NewEmployeeID", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        conn.Open();
                        empCmd.ExecuteNonQuery();
                        employeeId = Convert.ToInt32(empCmd.Parameters[":NewEmployeeID"].Value.ToString());
                    }

                    if (employeeId > 0)
                    {
                        // Tạo User với EmployeeID đã có
                        string userQuery = @"
                            INSERT INTO Users (UserID, RoleID, EmployeeID, Username, PasswordHash, Email, IsActive, CreatedAt, UpdatedAt)
                            VALUES (:UserID, :RoleID, :EmployeeID, :Username, :PasswordHash, :Email, :IsActive, SYSDATE, SYSDATE)";

                        using (OracleCommand cmd = new OracleCommand(userQuery, conn))
                        {
                            cmd.Parameters.Add(":UserID", OracleDbType.Int32).Value = user.UserID;
                            cmd.Parameters.Add(":RoleID", OracleDbType.Int32).Value = 2; // Mặc định là role Admin (2)
                            cmd.Parameters.Add(":EmployeeID", OracleDbType.Int32).Value = employeeId;
                            cmd.Parameters.Add(":Username", OracleDbType.Varchar2).Value = user.Username;
                            cmd.Parameters.Add(":PasswordHash", OracleDbType.Varchar2).Value = user.PasswordHash;
                            cmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = user.Email;
                            cmd.Parameters.Add(":IsActive", OracleDbType.Int32).Value = 1;

                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
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
            using (OracleConnection conn = new OracleConnection(Connection.GetConnectionString()))
            {
                string query = @"
                    SELECT u.UserID, u.Username, u.Email, u.PasswordHash, u.IsActive, u.CreatedAt, u.UpdatedAt,
                           e.FirstName, e.LastName, e.Title,
                           r.RoleID, r.RoleName
                    FROM Users u
                    LEFT JOIN Employees e ON u.EmployeeID = e.EmployeeID
                    LEFT JOIN Roles r ON u.RoleID = r.RoleID
                    WHERE u.Username = :Username OR u.Email = :Username";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.Add(":Username", OracleDbType.Varchar2).Value = username;
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
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