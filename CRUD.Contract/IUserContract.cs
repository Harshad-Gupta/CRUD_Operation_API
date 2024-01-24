using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CRUD.Model;

namespace CRUD.Contract
{
    public interface IUserContract
    {
        public Task<LoggedInUser> AuthenticateUser(LoginData data);
        public Task<ResultResponse> SaveUserDetails(User data);
        public Task<Dictionary<string, object>> GetUserDetailsByID(int userID);

        public Task<ResultResponse> DeleteUser(int userID);
        public Task<UserList> GetUserList();
    }
}
