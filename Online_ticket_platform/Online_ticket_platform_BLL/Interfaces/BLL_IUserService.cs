using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_BLL
{
    public interface BLL_IUserService
    {
        List<MOD_User> GetAllUsers();
        MOD_User GetUserById(int id);
        MOD_User GetUserByEmail(string email);
        bool AddUser(MOD_User user);
        bool UpdateUser(MOD_User user);
        bool DeleteUser(int id);
        bool HasRelatedData(int userId);
        List<string> GetRelatedDataInfo(int userId);
        void DeleteRelatedData(int userId);
    }
}
