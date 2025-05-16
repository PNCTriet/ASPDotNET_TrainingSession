using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;
using DataAccessLayer;
using BCrypt.Net;

namespace BussinessLayer
{
    public class UserBL
    {
        private readonly UserDAL _userDAL;

        public UserBL()
        {
            _userDAL = new UserDAL();
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return UserDAL.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business layer while getting users: " + ex.Message);
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                // Validate user data
                if (string.IsNullOrEmpty(user.Username))
                    throw new Exception("Username is required.");

                if (string.IsNullOrEmpty(user.Email))
                    throw new Exception("Email is required.");

                // If password is being updated, hash it
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    user.PasswordHash = HashPassword(user.PasswordHash);
                }

                return UserDAL.UpdateUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business layer while updating user: " + ex.Message);
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new Exception("Invalid user ID.");

                return UserDAL.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business layer while deleting user: " + ex.Message);
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new Exception("Invalid user ID.");

                return UserDAL.GetUserById(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business layer while getting user: " + ex.Message);
            }
        }

        public List<Role> GetAllRoles()
        {
            try
            {
                return UserDAL.GetAllRoles();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business layer while getting roles: " + ex.Message);
            }
        }

        public bool CreateUser(User user)
        {
            try
            {
                // Kiểm tra username đã tồn tại chưa
                var existingUser = GetAllUsers().FirstOrDefault(u => u.Username.ToLower() == user.Username.ToLower());
                if (existingUser != null)
                {
                    throw new Exception("Username đã tồn tại");
                }

                // Kiểm tra email đã tồn tại chưa
                existingUser = GetAllUsers().FirstOrDefault(u => u.Email.ToLower() == user.Email.ToLower());
                if (existingUser != null)
                {
                    throw new Exception("Email đã tồn tại");
                }

                // Hash the password before storing
                user.PasswordHash = HashPassword(user.PasswordHash);

                return UserDAL.CreateUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business layer: " + ex.Message);
            }
        }

        public bool ValidateUser(string username, string password)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[Validation] Starting validation for user: {username}");
                
                User user = UserDAL.GetUserByUsername(username);
                System.Diagnostics.Debug.WriteLine($"[Validation] User lookup result: {(user != null ? "Found" : "Not found")}");
                
                if (user != null)
                {
                    System.Diagnostics.Debug.WriteLine("[Validation] Verifying password...");
                    bool isPasswordValid = VerifyPassword(password, user.PasswordHash);
                    System.Diagnostics.Debug.WriteLine($"[Validation] Password verification result: {isPasswordValid}");
                    
                    if (isPasswordValid)
                    {
                        System.Diagnostics.Debug.WriteLine("[Validation] User authenticated successfully");
                        return true;
                    }
                }
                
                System.Diagnostics.Debug.WriteLine("[Validation] Authentication failed");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Validation] Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[Validation] Stack trace: {ex.StackTrace}");
                throw new Exception("Error validating user: " + ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[Password Debug] Starting password verification");
                System.Diagnostics.Debug.WriteLine($"[Password Debug] Input password: {password}");
                System.Diagnostics.Debug.WriteLine($"[Password Debug] Stored hash: {hashedPassword}");
                
                bool result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                
                System.Diagnostics.Debug.WriteLine($"[Password Debug] Verification result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Password Debug] Error during verification: {ex.Message}");
                throw;
            }
        }

        public static List<User> GetAllUsersStatic()
        {
            return UserDAL.GetAllUsers();
        }

        public static bool UpdateUserStatic(User user)
        {
            return UserDAL.UpdateUser(user);
        }

        public static bool DeleteUserStatic(int userId)
        {
            return UserDAL.DeleteUser(userId);
        }

        public static User GetUserByIdStatic(int userId)
        {
            return UserDAL.GetUserById(userId);
        }

        public static List<Role> GetAllRolesStatic()
        {
            return UserDAL.GetAllRoles();
        }

        public static bool CreateUserStatic(User user)
        {
            return UserDAL.CreateUser(user);
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    throw new Exception("Username is required.");

                return UserDAL.GetUserByUsername(username);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user by username: " + ex.Message);
            }
        }
    }
}