using Microsoft.AspNetCore.Identity;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Service.Contracts.Auth
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel> CreateUser(UserModel model, bool hasRole);
        Task<bool> UpdateUser(UserModel model);
        Task<bool> DeleteUser(string id);
        Task<UserModel> GetUser(string id);
        Task<UserModel> GetUserByUserName(string userName);
        public List<string> GetUserRoles(UserModel model);
        Task<List<UserModel>> GetUsersByRole(RoleModel role);

        Task<UserModel> SearchEmail(string email);
        Task<AspNetUser> UserLogin(long EmployeeNumber, string Email, bool RememberMe);

   







    }
}
