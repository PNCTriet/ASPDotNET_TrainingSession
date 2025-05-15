using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntity;
using DataAccessLayer;

namespace BussinessLayer
{
    public class UserBL
    {
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

                // Mã hóa mật khẩu
                user.PasswordHash = HashPassword(user.PasswordHash);

                return UserDAL.CreateUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business layer: " + ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}