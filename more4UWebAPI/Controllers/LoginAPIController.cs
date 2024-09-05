using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Irony.Parsing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.TermsConditions;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using MoreForYou.Services.Models.Message;
using MoreForYou.Services.Models.TermsConditionsModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tavis.UriTemplates;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace More4UWebAPI.APIController
{
    [Route("api/LoginAPI")]
    [ApiController]
    [Produces("application/json")]
    public class LoginAPIController : ControllerBase
    {
        private readonly IBenefitService _BenefitService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IEmployeeService _EmployeeService;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IBenefitWorkflowService _benefitWorkflowService;
        private readonly IRoleService _roleService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IPrivilegeService _privilegeService;
        private readonly IMobileVersionService _mobileVersionService;
        private readonly IConfiguration _configuration;
        private readonly ITermsConditionsService _termsConditionsService;
        public LoginAPIController(IBenefitService BenefitService,
            IBenefitWorkflowService BenefitWorkflowService,
            UserManager<AspNetUser> userManager,
             SignInManager<AspNetUser> signInManager,
            IEmployeeService EmployeeService,
            IBenefitRequestService benefitRequestService,
            IBenefitWorkflowService benefitWorkflowService,
            IRoleService roleService,
            IUserNotificationService userNotificationService,
            IPrivilegeService privilegeService,
            IMobileVersionService mobileVersionService,
            IConfiguration configuration,
            ITermsConditionsService termsConditionsService
            )
        {
            _BenefitService = BenefitService;
            _userManager = userManager;
            _signInManager = signInManager;
            _EmployeeService = EmployeeService;
            _benefitRequestService = benefitRequestService;
            _benefitWorkflowService = benefitWorkflowService;
            _roleService = roleService;
            _userNotificationService = userNotificationService;
            _privilegeService = privilegeService;
            _mobileVersionService = mobileVersionService;
            _configuration = configuration;
            _termsConditionsService = termsConditionsService;
        }
        //[HttpGet]
        //[Route("All")]
        //public EmployeeModel test()
        //{
        //    EmployeeModel employee = _EmployeeService.GetEmployee(100);
        //    return employee;
        //}

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("getHomeData")]
        public async Task<ActionResult> getHomeData(long userNumber, int languageId)
        {
            EmployeeModel employee = _EmployeeService.GetEmployee(userNumber).Result;
            if (employee != null)
            {
                if (employee.IsDirectEmployee == true)
                {
                
                EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(employee.UserId);
                    if (employeeModel != null)
                    {
                        AspNetUser aspNetUser = await _userManager.FindByIdAsync(employeeModel.UserId);
                    if (aspNetUser != null)
                        {
                            HomeModel homeModel = await _BenefitService.ShowAllBenefits(employeeModel, languageId);
                            homeModel.UserUnSeenNotificationCount = _userNotificationService.GetUserUnseenNotificationCount(employeeModel.EmployeeNumber);
                            homeModel.user.Email = aspNetUser.Email;
                            var priviliges = _privilegeService.GetAllPrivileges();
                            if (priviliges != null)
                            {
                                homeModel.PriviligesCount = priviliges.Result.Count;
                            }
                            else
                            {
                                homeModel.PriviligesCount = 0;
                            }
                            List<string> userRoles = _userManager.GetRolesAsync(aspNetUser).Result.ToList();
                            List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                            if (userRoles.Count != 0)
                            {

                                homeModel.user.HasRoles = true;
                            }
                            else
                            {
                                homeModel.user.HasRoles = false;
                            }
                            return Ok(new { Message = UserMessage.Done[languageId], Data = homeModel });
                        }
                    else
                        {
                            return BadRequest(new { Message = UserMessage.InValidData[languageId], Data = 0 }); // FailedAccount
                        }

                    }
                    else
                    {
                        return BadRequest(new { Message = UserMessage.InValidData[languageId], Data = 0 }); // FailedAccount
                    }
                        }
                        else
                        {
                            return BadRequest(new { Message = UserMessage.InValidData[languageId], Data = 0 }); // FailedAccount
                        }
                }
                else
                {
                    return BadRequest(new { Message = UserMessage.LoginIndirect[languageId], Data = 0 });
                }
            }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

        [HttpPost("mobileToken")]
        public async Task<ActionResult> UpdateToken(long userNumber, string newToken, int languageId)
        {
            EmployeeModel employeeModel = await _EmployeeService.GetEmployee(userNumber);
            bool result = false;
            if (employeeModel != null)
            {
                //EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(userId).Result;
                employeeModel.UserToken = newToken;
                result = _EmployeeService.UpdateEmployee(employeeModel).Result;
            }
            if (result == true)
            {
                return Ok(new { Message = UserMessage.SuccessfulProcess[languageId], Data = true });
            }
            else
            {
                return BadRequest(new { Message = UserMessage.InvalidEmployeeData[languageId], Data = false });
            }
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(UserSetting userSetting)
        {
            EmployeeModel employee = await _EmployeeService.GetEmployee(userSetting.userNumber);
            if (employee != null)
            {
                AspNetUser aspNetUser = await _userManager.FindByIdAsync(employee.UserId);
                if (aspNetUser != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(aspNetUser, userSetting.oldPassword, true);
                    if (result.Succeeded)
                    {
                        if (userSetting.oldPassword != userSetting.newPassword)
                        {
                            var result2 = await _userManager.ChangePasswordAsync(aspNetUser, userSetting.oldPassword, userSetting.newPassword);
                            if (result2.Succeeded)
                            {
                                return Ok(new { Message = UserMessage.SuccessfulPasswordChange[userSetting.LanguageId], Data = true });
                            }
                            else
                            {
                                return BadRequest(new { Message = UserMessage.FailedProcess[userSetting.LanguageId], Data = false });
                            }
                        }
                        else
                        {
                            return BadRequest(new { Message = UserMessage.ChangePasswordFailed[userSetting.LanguageId], Data = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = UserMessage.ChangePasswordFailed[userSetting.LanguageId], Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = UserMessage.InValidData[userSetting.LanguageId], Data = false });
                }
            }
            else
            {
                return BadRequest(new { Message = UserMessage.InValidData[userSetting.LanguageId], Data = false });
            }

        }

        [AllowAnonymous]
        [HttpGet("GetLastMobileVersion")]
        public async Task<ActionResult> GetLastMobileVersion()
        {
            try
            {
                var model = await _mobileVersionService.GetLastMobileVersion();
                if (model != null)
                {
                    MobileVersionAPIModel mobileVersionAPIModel = new MobileVersionAPIModel();
                    mobileVersionAPIModel.IOSVersion = model.IOSVersion;
                    mobileVersionAPIModel.AndroidVersion = model.AndroidVersion;
                    mobileVersionAPIModel.IOSLink = model.IOSLink;
                    mobileVersionAPIModel.AndroidLink = model.AndroidLink;
                    mobileVersionAPIModel.CreatedDate = model.CreatedDate;
                    return Ok(new { Message = "Done", data = mobileVersionAPIModel });
                }
                else
                {
                    return BadRequest(new { Message = "No Data", Data = 0 });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = 0 });
            }
        }

        [Authorize]
        [HttpGet("GetAllMobileVersions")]
        public async Task<ActionResult> GetAllMobileVersions()
        {
            try
            {
                var model = _mobileVersionService.GetAllMobileVersion().Result;
                if (model != null)
                {
                    List<MobileVersionAPIModel> mobileVersionAPIModels = new List<MobileVersionAPIModel>();
                    foreach (var version in model)
                    {
                        MobileVersionAPIModel mobileVersionAPIModel = new MobileVersionAPIModel();
                        mobileVersionAPIModel.IOSVersion = version.IOSVersion;
                        mobileVersionAPIModel.AndroidVersion = version.AndroidVersion;
                        mobileVersionAPIModel.IOSLink = version.IOSLink;
                        mobileVersionAPIModel.AndroidLink = version.AndroidLink;
                        mobileVersionAPIModel.CreatedDate = version.CreatedDate;
                        mobileVersionAPIModels.Add(mobileVersionAPIModel);
                    }

                    return Ok(new { Message = "Done", data = mobileVersionAPIModels });
                }
                else
                {
                    return BadRequest(new { Message = "No Data", Data = 0 });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = 0 });
            }
        }

        //[HttpGet("CheckToken")]
        //public async Task<ActionResult> CheckToken()
        //{
        //    if (HttpContext.Request.Headers.TryGetValue("Authorization", out var headerAuth))
        //    {
        //        var jwtToken = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];

        //        return Ok(new { flag = true, token = jwtToken });
        //    }
        //    else
        //    {
        //        return BadRequest(new { flag = false, token = "" });
        //    }

        //}


        [AllowAnonymous]
        [HttpGet("GetTermsOfConditions")]
        public async Task<ActionResult> GetTermsOfConditions(int languageId)
        {
            try
            {
                TermsOfConditionsModel termsOfConditionsModel = new TermsOfConditionsModel();
                if ((int)CommanData.Languages.English == languageId)
                {
                    termsOfConditionsModel = _termsConditionsService.LoadEnglishTerms().Result;
                }
                else if((int)CommanData.Languages.Arabic == languageId)
                {
                    termsOfConditionsModel = _termsConditionsService.LoadArabicTerms().Result;
                }
                if(termsOfConditionsModel != null)
                {
                    return Ok(new { flag = true, Data = termsOfConditionsModel });
                }
                else
                {
                    return BadRequest(new { flag = false, Data = 0 });
                }
            }
            catch(Exception e) 
            {
                return BadRequest(new { flag = false, Data = 0});
            }
        }


        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel, int languageId)
        {
            if (tokenModel is null)
            {
                return Unauthorized(new { Message = UserMessage.WrongToken[languageId], Data = 0 });
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = _EmployeeService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return Unauthorized(new { Message = UserMessage.WrongToken[languageId], Data = 0 });
            }
            else
            {
                if (principal.Result != null)
                {
                    var principalIdentity = principal.Result.Identity;
                    if (principalIdentity != null)
                    {
                        string username = principalIdentity.Name;

                        var user = await _userManager.FindByNameAsync(username);

                        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                        {
                            return Unauthorized(new { Message = UserMessage.WrongToken[languageId], Data = 0 }); ;
                        }

                        var newAccessToken = _EmployeeService.GenerateAccessToken(principal.Result.Claims.ToList());
                        //var newRefreshToken = GenerateRefreshToken();

                        //user.RefreshToken = newRefreshToken;
                        //await _userManager.UpdateAsync(user);

                        //var userUpdateRefreshTokenResult = await _userManager.UpdateAsync(user);
                        //End Token 

                        if (newAccessToken != null)
                        {
                            TokenModel newTokenModel = new TokenModel()
                            {
                                RefreshToken = tokenModel.RefreshToken,
                                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken.Result),
                            };
                            return Ok(new { Message = UserMessage.Done[languageId], Data = newTokenModel });
                        }
                        else
                        {
                            return Unauthorized(new { Message = UserMessage.FailedProcess[languageId], Data = 0 });
                        }
                    }
                    else
                    {
                        return Unauthorized(new { Message = UserMessage.FailedProcess[languageId], Data = 0 });
                    }
                }
                else
                {
                    return Unauthorized(new { Message = UserMessage.FailedProcess[languageId], Data = 0 });
                }

            }


        }

        [HttpPost("userLogin")]
        public async Task<ActionResult> UserLogin(LoginModel loginModel)
        {
            TokenModel tokenModel = new TokenModel();
            UserData userData = new UserData();
            EmployeeModel employee = await _EmployeeService.GetEmployee(loginModel.UserNumber);
            if (employee != null)
            {
                if (employee.IsDirectEmployee == true)
                {
                    AspNetUser aspNetUser = await _userManager.FindByIdAsync(employee.UserId);
                    if (aspNetUser != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(aspNetUser.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            var authClaims = new List<Claim>
                            {
                    new Claim(ClaimTypes.Name, aspNetUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                            var refreshToken =await _EmployeeService.GenerateRefreshToken();
                           
                 var Accesstoken = await _EmployeeService.GenerateAccessToken(authClaims);
                            if (Accesstoken != null)
                            {
                                tokenModel = new TokenModel()
                                {
                                    AccessToken = new   JwtSecurityTokenHandler().WriteToken(Accesstoken),
                                    RefreshToken = refreshToken,
                                };
                            }
                            else
                            {
                                return BadRequest(new { Message = UserMessage.FailedProcess[loginModel.LanguageId], Data = 0 });
                            }
                            aspNetUser.RefreshToken = refreshToken;
                            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                            aspNetUser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInDays);   //expiration date
                            var userUpdateRefreshTokenResult = await _userManager.UpdateAsync(aspNetUser);
                            //End Token 
                            if (userUpdateRefreshTokenResult.Succeeded)
                            {
                                userData.UserNumber = employee.EmployeeNumber;
                                userData.TokenModel = tokenModel;
                                return Ok(new { Message = UserMessage.Done[loginModel.LanguageId], Data = userData});
                            }
                            else
                            {
                                return BadRequest(new { Message = UserMessage.FailedProcess[loginModel.LanguageId], Data = 0 });
                            }
                        }
                        else
                        {
                            return BadRequest(new { Message = UserMessage.LoginFailed[loginModel.LanguageId], Data = 0 }); // FailedAccount
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = UserMessage.EmailNotFound[loginModel.LanguageId], Data = 0 });
                    }
                }
                else
                {
                    return BadRequest(new { Message = UserMessage.LoginIndirect[loginModel.LanguageId], Data = 0 });
                }
            }
            else
            {
                return BadRequest(new { Message = UserMessage.LoginInvalidNumber[loginModel.LanguageId], Data = 0 });
            }

        }

    }
}
