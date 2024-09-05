using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MaterModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MoreForYou.Service.Implementation.Auth
{
   public class UserService : IUserService
    {
        private readonly UserManager<AspNetUser> _userManager;

        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;


        public UserService(UserManager<AspNetUser> userManager,
        ILogger<UserService> logger, 
        IMapper mapper,
        IEmployeeService employeeService,
        IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _employeeService = employeeService;
            _configuration = configuration;
        }
        async Task<List<UserModel>> IUserService.GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var models = new List<UserModel>();
             models = _mapper.Map<List<UserModel>>(users);
            return models;
        }

     public   async Task<UserModel> CreateUser(UserModel model, bool hasRole)
        {
            var user = new AspNetUser();
            user.UserName = model.Email;
            user.Email = model.Email;
        
            try
            {
                var result=  await _userManager.CreateAsync(user, model.Password);
                if(hasRole && result == IdentityResult.Success)
                {
                    result = await _userManager.AddToRolesAsync(user, model.AsignedRolesNames);
                }
                if(result == IdentityResult.Success)
                {
                     AspNetUser addedUser = await _userManager.FindByEmailAsync(model.Email);
                    UserModel userModel = _mapper.Map<UserModel>(addedUser);

                    return userModel;
                }
                else
                {
                    return null;
                }

            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());

            }
            return (null);

        }

        async Task<bool> IUserService.DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);               
                var response = await _userManager.DeleteAsync(user);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
    
        }

        async Task<UserModel> IUserService.GetUser(string id)
        {
            AspNetUser user = await _userManager.FindByIdAsync(id);
            var model = _mapper.Map<UserModel>(user);
            return model;
        }

        async Task <UserModel> IUserService.GetUserByUserName(string userName)
        {
            try
            {
                AspNetUser user = await _userManager.FindByNameAsync(userName);
                var model = _mapper.Map<UserModel>(user);
                return model;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        async Task<bool> IUserService.UpdateUser(UserModel model)
        {
            try
            {
                AspNetUser user = await _userManager.FindByIdAsync(model.id);
                user.Email = model.Email;
                var response = await _userManager.UpdateAsync(user);
                response = await _userManager.AddToRolesAsync(user, model.AsignedRolesNames);
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return false;
        }

       public   List<string> GetUserRoles(UserModel model)
        {
            AspNetUser user = new AspNetUser();
            user = _mapper.Map<AspNetUser>(model);
            List<string> rolesName =  (List<string>)_userManager.GetRolesAsync(user).Result;
            return rolesName;
        }

        Task<List<UserModel>> GetUsersByRole(RoleModel role)
        {
            try
            {
                
            }
            catch
            {

            }
            return null;
        }

        Task<List<UserModel>> IUserService.GetUsersByRole(RoleModel role)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> SearchEmail(string email)
        {
            try
            {
               AspNetUser aspNetUser = await _userManager.FindByEmailAsync(email);
                UserModel userModel = _mapper.Map<UserModel>(aspNetUser);

                return userModel;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<AspNetUser> UserLogin(long EmployeeNumber, string Password, bool RememberMe)
        {
            try
            {
               EmployeeModel employee =  await _employeeService.GetEmployee(EmployeeNumber);
                if(employee != null && employee.IsDirectEmployee == true)
                {
                   AspNetUser user = await _userManager.FindByIdAsync(employee.UserId);
                   //bool result = _userManager.CheckPasswordAsync(user, Password).Result;
                    if(user != null)
                    {
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch(Exception e)
            {
            }
            return null;

        }

   
    }
}
