using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Models.Models;
using Microsoft.AspNetCore.Identity;
using MoreForYou.Models.Auth;
using MoreForYou.Service.Contracts.Auth;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Tavis.UriTemplates;
using Microsoft.EntityFrameworkCore;

namespace MoreForYou.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee, long> _repository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IRoleService _roleService;

        private readonly IConfiguration _configuration;
        public EmployeeService(IRepository<Employee, long> employeeRepository,
          ILogger<EmployeeService> logger, IMapper mapper,
          UserManager<AspNetUser> userManager,
          IRoleService roleService,
          IConfiguration configuration
)
        {
            _repository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleService = roleService;
            _configuration = configuration;
        }
        public Task<bool> CreateEmployee(EmployeeModel model)
        {
            var employee = _mapper.Map<Employee>(model);
            try
            {
                var addedEmployee = _repository.Add(employee);
                if(addedEmployee != null)
                {
                    return Task<bool>.FromResult<bool>(true);
                }
                else
                {
                    return Task<bool>.FromResult<bool>(false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public bool DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeModel>> GetAllEmployees()
        {
            try
            {
                var models = new List<EmployeeModel>();
                var employees = await _repository.Find(i => i.IsVisible == true, false, i => i.Department, i => i.Position, i => i.Nationality, i => i.Supervisor, i => i.
                            Company).ToListAsync();
             if(employees != null)
                {
                    models = _mapper.Map<List<EmployeeModel>>(employees);
                }
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<List<EmployeeModel>> GetAllDirectEmployees()
        {
            try
            {
                var models = new List<EmployeeModel>();
                var employees =  await _repository.Find(i => i.IsVisible == true && i.IsDirectEmployee == true, false, i => i.Department, i => i.Position, i => i.Nationality, i => i.Supervisor, i => i.
                            Company).ToListAsync();
                if( employees != null)
                {
                    models = _mapper.Map<List<EmployeeModel>>(employees);

                }
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<List<EmployeeModel>> GetAllDirectEmployeesSameCountry(long employeeNumber)
        {
            try
            {
                var employee =  GetEmployee(employeeNumber);
                string employeeCountry = employee.Result.Country;
                var employees = await _repository.Find(i => i.IsVisible == true && i.IsDirectEmployee == true && i.Country == employeeCountry, false, i => i.Department, i => i.Position, i => i.Nationality, i => i.Supervisor, i => i.
                            Company).ToListAsync();
                var models = new List<EmployeeModel>();
                if(employees != null)
                {
                    models = _mapper.Map<List<EmployeeModel>>(employees);
                }
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<List<EmployeeModel>> GetAllEmployeeWhoCanIGive()
        {
            try
            {
                var employees = await _repository.Find(i => i.IsVisible == true).ToListAsync();
                if(employees != null)
                {
                    var models = new List<EmployeeModel>();
                    models = _mapper.Map<List<EmployeeModel>>(employees);
                    return models;
                }
                else
                {
                    return null;
                    
                }
               
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public EmployeeModel GetDepartmentManager(long departmentId)
        {
            try
            {
                Employee employee = _repository.Find(e => e.IsVisible == true && e.isDeptManager == true && e.DepartmentId == departmentId, false, e => e.Department).FirstOrDefault();
                if (employee != null)
                {
                    EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                    return employeeModel;
                }
                else
                {
                    return null;
                }
                

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public async Task<EmployeeModel> GetEmployee(long employeeNumber)
        {
            try
            {
                Employee employee =  _repository.Find(e => e.EmployeeNumber == employeeNumber && e.IsVisible == true, false, e => e.Department, e => e.Position, e => e.Company, e => e.Nationality, e => e.Supervisor).FirstOrDefault();
                if (employee != null)
                {
                    EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                    return employeeModel;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public string GetSupervisorEmailOfEmployee(long employeeNumber)
        {
            try
            {
                var employee = _repository.Find(e => e.EmployeeNumber == employeeNumber && e.IsVisible == true,false, e=>e.Supervisor).FirstOrDefault();
                if(employee != null)
                {
                    Employee supervisor = employee.Supervisor;
                    string supervisorEmail = _userManager.FindByIdAsync(supervisor.UserId).Result.Email;
                    return supervisorEmail;
                }
                else
                {
                    return null;
                }
               
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public string GetManagerEmailOfEmployee(long employeeNumber)
        {
            try
            {
                var employee = _repository.Find(e => e.EmployeeNumber == employeeNumber && e.IsVisible == true, false, e => e.Supervisor).FirstOrDefault();
                if(employee != null)
                {
                    Employee supervisor = employee.Supervisor;
                    var SupervisorEmployee = _repository.Find(e => e.EmployeeNumber == supervisor.EmployeeNumber && e.IsVisible == true, false, e => e.Supervisor).FirstOrDefault();
                    if(SupervisorEmployee != null)
                    {
                        Employee manager = SupervisorEmployee.Supervisor;
                        var managerUser = _userManager.FindByIdAsync(manager.UserId);
                        if(managerUser != null)
                        {
                            string managerEmail = managerUser.Result.Email;
                            return managerEmail;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<EmployeeModel> GetEmployeeByName(string name)
        {
            try
            {
                var employee = await _repository.Find(e => e.FullName == name).FirstOrDefaultAsync();
                if(employee != null)
                {
                    EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                    return employeeModel;
                }
                else
                {
                    return null;
                }
               
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<EmployeeModel> GetEmployeeByUserId(string userId)
        {
            try
            {
                Employee employee = await _repository.Find(i => i.IsVisible == true && i.UserId == userId, false, i => i.Department, i => i.Position, i => i.Company, i => i.Nationality, i => i.Supervisor).FirstOrDefaultAsync();
                if(employee != null)
                {
                    EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                    return employeeModel;
                }
                else
                {
                    return null;
                }
            
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public EmployeeModel GetEmployeeByUserId2(string userId)
        {
            try
            {
                Employee employee =  _repository.Find(i => i.IsVisible == true && i.UserId == userId, false,  i => i.Company, i => i.Supervisor).FirstOrDefault();
                if (employee != null)
                {
                    EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                    return employeeModel;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<List<EmployeeModel>> GetEmployeesDataByDepartmentId(long DeptId)
        {
            try
            {
                var departments = await _repository.Find(i => i.IsVisible == true && i.DepartmentId == DeptId).ToListAsync();
                var models = new List<EmployeeModel>();
                if(departments != null)
                {
                    models = _mapper.Map<List<EmployeeModel>>(departments);
                }
                return models;

            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public async Task<EmployeeModel> GetSystemEmployee()
        {
            try
            {
                RoleModel roleModel = _roleService.GetRoleByName("System").Result;
                if(roleModel != null)
                {
                    var systemUser = await _userManager.GetUsersInRoleAsync("System");
                    if (systemUser != null)
                    {
                        EmployeeModel whoIsConcern = await GetEmployeeByUserId(systemUser.FirstOrDefault().Id);
                        return whoIsConcern;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }  
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
         
        public Task<bool> UpdateEmployee(EmployeeModel model)
        {
            var employee = _mapper.Map<Employee>(model);
            bool result = false;
            try
            {
                employee.UpdatedDate = DateTime.Now;
                result = _repository.Update(employee);

                return Task<bool>.FromResult<bool>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public async Task<EmployeeModel> GetEmployeeBySapNumber(long sapNumber)
        {
            try
            {
                Employee employee = await _repository.Find(e => e.SapNumber == sapNumber).FirstOrDefaultAsync();
                if(employee != null)
                {
                    EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                    return employeeModel;
                }
                else
                {
                    return null;
                }
              
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public EmployeeModel GetEmployeeByPhoneNumber(string PhoneNumber)
        {
            try
            {
                Employee employee = _repository.Find(e => e.PhoneNumber == PhoneNumber).FirstOrDefault();
                if(employee != null)
                {
                    EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                    return employeeModel;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public EmployeeModel GetEmployeeById(string Id)
        {
            try
            {
                Employee employee = _repository.Find(e => e.Id == Id).FirstOrDefault();
                if(employee != null)
                {
                    EmployeeModel employeeModel = _mapper.Map<EmployeeModel>(employee);
                    return employeeModel;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public LoginUser CreateLoginUser(EmployeeModel employeeModel)
        {
            LoginUser user = new LoginUser();
            user.UserName = employeeModel.FullName;
            user.PositionName = employeeModel.Position.Name;
            user.DepartmentName = employeeModel.Department.Name;
            user.UserNumber = employeeModel.EmployeeNumber;
            user.BirthDate = employeeModel.BirthDate.ToString("yyyy-MM-dd");
            user.JoinDate = employeeModel.JoiningDate.ToString("yyyy-MM-dd");
            user.Address = employeeModel.Address;
            user.PhoneNumber = employeeModel.PhoneNumber;
            user.MaritalStatus = Enum.GetName(typeof(CommanData.MaritialStatus), employeeModel.MaritalStatus);
            user.Collar = Enum.GetName(typeof(CommanData.CollarTypes), employeeModel.Collar);
            user.Gender = Enum.GetName(typeof(CommanData.Gender), employeeModel.Gender);
            user.Entity = employeeModel.Company.Code;
            user.Nationality = employeeModel.Nationality.Name;
            user.Id = employeeModel.Id;
            user.SupervisorName = employeeModel.Supervisor.FullName;
            user.SapNumber = employeeModel.SapNumber;
            user.ProfilePicture = employeeModel.ProfilePicture;
            user.PendingRequestsCount = employeeModel.PendingRequestsCount;
            user.hasRequests = employeeModel.hasRequests;
            user.Email = _userManager.FindByIdAsync(employeeModel.UserId).Result.Email;
            user.WorkDuration = CalculateWorkDuration(Convert.ToDateTime(employeeModel.JoiningDate));
            user.ProfilePictureAPI = CommanData.Url + CommanData.ProfileFolder + employeeModel.ProfilePicture;
            var roles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(employeeModel.UserId).Result).Result;
            if(roles != null)
            {
                if(roles.Count > 0)
                {
                    user.HasRoles = true;
                }
            }
                return user;
        }

        public List<EmployeeModel> EmployeesSearch(FilterModel filterModel)
        {
            try
            {
                List<EmployeeModel> employeeModels = GetAllDirectEmployees().Result;
                if (filterModel.DirectEmployee == true)
                {
                    employeeModels = employeeModels.Where(e => e.IsDirectEmployee == true).ToList();
                }
                if (filterModel.EmployeeName != null)
                {
                    employeeModels = employeeModels.Where(e => e.FullName.Contains(filterModel.EmployeeName)).ToList();
                }
                if (filterModel.EmployeeNumber != 0)
                {
                    employeeModels = employeeModels.Where(e => e.EmployeeNumber == filterModel.EmployeeNumber).ToList();
                }
                if (filterModel.SapNumber != 0)
                {
                    employeeModels = employeeModels.Where(e => e.SapNumber == filterModel.SapNumber).ToList();
                }
                if (filterModel.Id != null)
                {
                    employeeModels = employeeModels.Where(e => e.Id == filterModel.Id).ToList();
                }
               
                //if (filterModel.Email != null)
                //{
                //    employeeModels = employeeModels.Where(e => e.Email.Contains(filterModel.Email)).ToList();
                //}
                if (filterModel.BirthDate != DateTime.Today)
                {
                    employeeModels = employeeModels.Where(e => e.BirthDate == filterModel.BirthDate).ToList();
                }
                if (filterModel.JoinDate != DateTime.Today)
                {
                    employeeModels = employeeModels.Where(e => e.JoiningDate == filterModel.JoinDate).ToList();
                }
                if (filterModel.SelectedGenderId != -1)
                {
                    employeeModels = employeeModels.Where(e => e.Gender == filterModel.SelectedGenderId).ToList();
                }
                if (filterModel.SelectedDepartmentId != -1)
                {
                    employeeModels = employeeModels.Where(e => e.DepartmentId == filterModel.SelectedDepartmentId).ToList();
                }
                if (filterModel.SelectedPositionId != -1)
                {
                    employeeModels = employeeModels.Where(e => e.PositionId == filterModel.SelectedPositionId).ToList();
                }
                if (filterModel.SelectedNationalityId != -1)
                {
                    employeeModels = employeeModels.Where(e => e.NationalityId == filterModel.SelectedNationalityId).ToList();
                }
                if(employeeModels.Count >0)
                {
                    employeeModels.ForEach(e => e.Email = _userManager.FindByIdAsync(e.UserId).Result.Email);
                }
                return employeeModels;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public string CalculateWorkDuration(DateTime joinDate)
        {
            string workDuration = "";
            TimeSpan diff = DateTime.Today - joinDate;
            int days = diff.Days;
            int months = days / 30;
            int years = (int)(days / (356.25));
            if (years < 1)
            {
                if (months < 1)
                {
                    workDuration = days + " days";
                }
                else
                {
                    workDuration = months + " Months";
                }

            }
            else
            {
                workDuration = years + " years";

            }
            return workDuration;
        }

        public EmployeeModel GetPayrollEmployee(long employeeNumber)
        {
            try
            {
                EmployeeModel CurrentEmployee = GetEmployee(employeeNumber).Result;
                var payrollUsers = _userManager.GetUsersInRoleAsync("Payroll").Result;
                List<EmployeeModel> payrollEmployees = new List<EmployeeModel>();
               foreach(var user in payrollUsers)
                {
                    EmployeeModel employee1 = GetEmployeeByUserId(user.Id).Result;
                    payrollEmployees.Add(employee1);
                }

               EmployeeModel payrollEmployee = payrollEmployees.Where(p => p.Country == CurrentEmployee.Country).FirstOrDefault();
                return payrollEmployee;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length - 3;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            chars[4] = 'M';
            chars[5] = 'u';
            chars[6] = '$';
            return new string(chars);
        }

        public string GetEmailOfEmployee(long employeeNumber)
        {
            try
            {
                Employee employee = _repository.Find(e => e.EmployeeNumber == employeeNumber && e.IsVisible == true, false, e => e.Supervisor).FirstOrDefault();
                string employeeEmail = _userManager.FindByIdAsync(employee.UserId).Result.Email;
                return employeeEmail;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public async Task<ClaimsPrincipal>? GetPrincipalFromExpiredToken(string? token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                //throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch(Exception e)
            {
                return null;
            }

        }


        public async Task<JwtSecurityToken> GenerateAccessToken(List<Claim> authClaims)
        {
            try
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return token; 
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
