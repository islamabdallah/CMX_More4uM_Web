using MoreForYou.Models.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetAllEmployees();
        Task<bool> CreateEmployee(EmployeeModel model);
        Task<bool> UpdateEmployee(EmployeeModel model);
        bool DeleteEmployee(int id);
        Task<EmployeeModel> GetEmployee(long employeeNumber);
        Task<EmployeeModel> GetEmployeeByName(string name);

        Task<List<EmployeeModel>> GetEmployeesDataByDepartmentId(long DeptId);
        Task<EmployeeModel> GetEmployeeByUserId(string userId);

        EmployeeModel GetDepartmentManager(long departmentId);

        //Task<List<EmployeeModel>> GetEmployeeAuthority(long employeeNumber, String authorityName);
        Task<EmployeeModel> GetEmployeeBySapNumber(long sapNumber);

        EmployeeModel GetEmployeeById(string Id);

        Task<List<EmployeeModel>> GetAllEmployeeWhoCanIGive();

        LoginUser CreateLoginUser(EmployeeModel employeeModel);

        List<EmployeeModel> EmployeesSearch(FilterModel filterModel);

        string CalculateWorkDuration(DateTime joinDate);
        EmployeeModel GetEmployeeByPhoneNumber(string PhoneNumber);

        Task<List<EmployeeModel>> GetAllDirectEmployees();
        EmployeeModel GetPayrollEmployee(long employeeNumber);

        Task<EmployeeModel> GetSystemEmployee();

        Task<List<EmployeeModel>> GetAllDirectEmployeesSameCountry(long employeeNumber);

        public string CreateRandomPassword(int PasswordLength);

        public string GetSupervisorEmailOfEmployee(long employeeNumber);

        public string GetEmailOfEmployee(long employeeNumber);

        public string GetManagerEmailOfEmployee(long employeeNumber);

        public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string? token);

        public  Task<JwtSecurityToken> GenerateAccessToken(List<Claim> authClaims);

        public  Task<string> GenerateRefreshToken();

        EmployeeModel GetEmployeeByUserId2(string userId);

    }
}
