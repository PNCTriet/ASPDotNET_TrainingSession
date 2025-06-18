using System;
using System.Collections.Generic;
using Online_ticket_platform_DAL.Interfaces;
using Online_ticket_platform_DAL.Repositories;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_BLL
{
    public class BLL_UserService : BLL_IUserService
    {
        private readonly DAL_IUserRepository _userRepository;

        public BLL_UserService()
        {
            _userRepository = new DAL_UserRepository();
        }

        public List<MOD_User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public MOD_User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public MOD_User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public bool AddUser(MOD_User user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
                return false;

            user.CreatedAt = DateTime.Now;
            return _userRepository.AddUser(user);
        }

        public bool UpdateUser(MOD_User user)
        {
            if (string.IsNullOrEmpty(user.Email))
                return false;

            return _userRepository.UpdateUser(user);
        }

        public bool DeleteUser(int id)
        {
            if (_userRepository.HasRelatedData(id))
                return false;

            return _userRepository.DeleteUser(id);
        }

        public bool HasRelatedData(int userId)
        {
            return _userRepository.HasRelatedData(userId);
        }

        public List<string> GetRelatedDataInfo(int userId)
        {
            return _userRepository.GetRelatedDataInfo(userId);
        }

        public void DeleteRelatedData(int userId)
        {
            _userRepository.DeleteRelatedData(userId);
        }
    }
}
