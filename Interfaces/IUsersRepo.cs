using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH_Academy_of_Programming.Controllers;
using TECH_Academy_of_Programming.Models;
using User = TECH_Academy_of_Programming.Models.User;

namespace TECH_Academy_of_Programming.Interfaces
{
    public interface IUsersRepo
    {
        Task<RegisterResult> register(UserRegisterModel userRegisterModel);
        Task<AuthModel> login(UserLoginModel userLoginModel);
        Task<User> getProfile (string ID);
        Task<UpdateResult> updateProfile (string ID, UpdatedUserModel updatedUserModel);
        Task<DeleteResult> delete (string ID);
        Task<IEnumerable<User>> getUsers(int page);
    }
}