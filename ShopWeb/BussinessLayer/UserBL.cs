using System;
using System.Data;
using BusinessEntity;
using DataAccessLayer;

namespace BussinessLayer
{
    public class UserBL
    {
        private readonly UserDAL _userDAL;

        public UserBL()
        {
            _userDAL = new UserDAL();
        }

        public DataTable GetAllUsers()
        {
            try
            {
                DataTable dt = _userDAL.GetAllUsers();
                Console.WriteLine($"[BL] Number of users from DAL: {dt.Rows.Count}");
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BL] Error in business layer while getting users: {ex.Message}");
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

                return _userDAL.UpdateUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BL] Error in business layer while updating user: {ex.Message}");
                throw new Exception("Error in business layer while updating user: " + ex.Message);
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new Exception("Invalid user ID.");

                return _userDAL.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BL] Error in business layer while deleting user: {ex.Message}");
                throw new Exception("Error in business layer while deleting user: " + ex.Message);
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                if (userId <= 0)
                    throw new Exception("Invalid user ID.");

                return _userDAL.GetUserById(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BL] Error in business layer while getting user: {ex.Message}");
                throw new Exception("Error in business layer while getting user: " + ex.Message);
            }
        }

        public DataTable GetAllRoles()
        {
            try
            {
                return _userDAL.GetAllRoles();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BL] Error in business layer while getting roles: {ex.Message}");
                throw new Exception("Error in business layer while getting roles: " + ex.Message);
            }
        }
    }
}