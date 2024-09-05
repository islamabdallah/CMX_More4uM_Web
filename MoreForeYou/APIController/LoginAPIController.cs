//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using MoreForYou.Models.Models;
//using MoreForYou.Service.Contracts.Auth;
//using MoreForYou.Services;
//using MoreForYou.Services.Contracts;
//using MoreForYou.Services.Models;
//using MoreForYou.Services.Models.API;
//using MoreForYou.Services.Models.MasterModels;
//using MoreForYou.Services.Models.MaterModels;
//using MoreForYou.Services.Models.Message;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace MoreForYou.APIController
//{
//    [Route("api/LoginAPI")]
//    [ApiController]
//    [Produces("application/json")]
//    public class LoginAPIController : ControllerBase
//    {
//        private readonly IBenefitService _BenefitService;
//        private readonly UserManager<AspNetUser> _userManager;
//        private readonly SignInManager<AspNetUser> _signInManager;
//        private readonly IEmployeeService _EmployeeService;
//        private readonly IBenefitRequestService _benefitRequestService;
//        private readonly IBenefitWorkflowService _benefitWorkflowService;
//        private readonly IRoleService _roleService;
//        private readonly IUserNotificationService _userNotificationService;
//        private readonly IPrivilegeService _privilegeService;
//        private readonly IMobileVersionService _mobileVersionService;
//        public LoginAPIController(IBenefitService BenefitService,
//            IBenefitWorkflowService BenefitWorkflowService,
//            UserManager<AspNetUser> userManager,
//             SignInManager<AspNetUser> signInManager,
//            IEmployeeService EmployeeService,
//            IBenefitRequestService benefitRequestService,
//            IBenefitWorkflowService benefitWorkflowService,
//            IRoleService roleService,
//            IUserNotificationService userNotificationService,
//            IPrivilegeService privilegeService,
//            IMobileVersionService mobileVersionService
//            )
//        {
//            _BenefitService = BenefitService;
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _EmployeeService = EmployeeService;
//            _benefitRequestService = benefitRequestService;
//            _benefitWorkflowService = benefitWorkflowService;
//            _roleService = roleService;
//            _userNotificationService = userNotificationService;
//            _privilegeService = privilegeService;
//            _mobileVersionService = mobileVersionService;
//        }
//        [HttpGet]
//        [Route("All")]
//        public EmployeeModel test()
//        {
//            EmployeeModel employee = _EmployeeService.GetEmployee(100).Result;
//            return employee;
//        }

//        [HttpPost("userLogin")]
//        public async Task<ActionResult> UserLogin(LoginModel loginModel)
//        {
//            EmployeeModel employee = _EmployeeService.GetEmployee(loginModel.UserNumber).Result;
//            if (employee != null)
//            {
//                if(employee.IsDirectEmployee == true)
//                {
//                    AspNetUser aspNetUser = await _userManager.FindByIdAsync(employee.UserId);
//                    if (aspNetUser != null)
//                    {
//                        var result = await _signInManager.PasswordSignInAsync(aspNetUser.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
//                        if (result.Succeeded)
//                        {
//                            EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(aspNetUser.Id);
//                            HomeModel homeModel = _BenefitService.ShowAllBenefits(employeeModel, loginModel.LanguageId);
//                            homeModel.UserUnSeenNotificationCount = _userNotificationService.GetUserUnseenNotificationCount(employeeModel.EmployeeNumber);
//                            homeModel.user.Email = aspNetUser.Email;
//                            var priviliges = _privilegeService.GetAllPrivileges();
//                            if (priviliges != null)
//                            {
//                                homeModel.PriviligesCount = priviliges.Result.Count;
//                            }
//                            else
//                            {
//                                homeModel.PriviligesCount = 0;
//                            }
//                            List<string> userRoles = _userManager.GetRolesAsync(aspNetUser).Result.ToList();
//                            List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
//                            if (userRoles.Count != 0)
//                            {
//                                //if (userRoles.Contains("Admin"))
//                                //{
//                                //    homeModel.user.IsAdmin = true;
//                                //}
//                                //else
//                                //{
//                                //    homeModel.user.IsAdmin = false;
//                                //}
//                                homeModel.user.HasRoles = true;
//                            }
//                            else
//                            {
//                                homeModel.user.HasRoles = false;
//                            }

//                            return Ok(new { Message = UserMessage.Done[loginModel.LanguageId], Data = homeModel });

//                        }
//                        else
//                        {
//                            return BadRequest(new { Message = UserMessage.LoginFailed[loginModel.LanguageId], Data = 0 }); // FailedAccount

//                        }
//                    }
//                    else
//                    {
//                        return BadRequest(new { Message = UserMessage.EmailNotFound[loginModel.LanguageId], Data = 0 });
//                    }
//                }
//                else
//                {
//                    return BadRequest(new { Message = UserMessage.LoginIndirect[loginModel.LanguageId], Data = 0 });
//                }
//            }
//            else
//            {
//                return BadRequest(new { Message = UserMessage.LoginInvalidNumber[loginModel.LanguageId], Data = 0 });
//            }

//        }

//        [HttpPost("refreshToken")]
//        public async Task<ActionResult> UpdateToken(long employeeNumber, string newToken, int languageId)
//        {
//            EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber).Result;
//            bool result = false;
//            if (employeeModel != null)
//            {
//                //EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(userId).Result;
//                employeeModel.UserToken = newToken;
//                result = _EmployeeService.UpdateEmployee(employeeModel).Result;
//            }
//            if (result == true)
//            {
//                return Ok(new { Message = UserMessage.SuccessfulProcess[languageId], Data = true });
//            }
//            else
//            {
//                return BadRequest(new { Message = UserMessage.InvalidEmployeeData[languageId], Data = false });
//            }
//        }


//        [HttpPost("ChangePassword")]
//        public async Task<ActionResult> ChangePassword(UserSetting userSetting)
//        {
//            EmployeeModel employee = _EmployeeService.GetEmployee(userSetting.userNumber).Result;
//            if (employee != null)
//            {
//                AspNetUser aspNetUser = await _userManager.FindByIdAsync(employee.UserId);
//                if (aspNetUser != null)
//                {
//                    var result = await _signInManager.CheckPasswordSignInAsync(aspNetUser, userSetting.oldPassword, true);
//                    if (result.Succeeded)
//                    {
//                        if(userSetting.oldPassword != userSetting.newPassword)
//                        {
//                            var result2 = await _userManager.ChangePasswordAsync(aspNetUser, userSetting.oldPassword, userSetting.newPassword);
//                            if (result2.Succeeded)
//                            {
//                                return Ok(new { Message = UserMessage.SuccessfulPasswordChange[userSetting.LanguageId], Data = true });
//                            }
//                            else
//                            {
//                                return BadRequest(new { Message = UserMessage.FailedProcess[userSetting.LanguageId], Data = false });
//                            }
//                        }
//                        else
//                        {
//                            return BadRequest(new { Message = UserMessage.ChangePasswordFailed[userSetting.LanguageId], Data = false });
//                        }
//                    }
//                    else
//                    {
//                        return BadRequest(new { Message = UserMessage.ChangePasswordFailed[userSetting.LanguageId], Data = false });
//                    }
//                }
//                else
//                {
//                    return BadRequest(new { Message = UserMessage.InValidData[userSetting.LanguageId], Data = false });
//                }
//            }
//            else
//            {
//                return BadRequest(new { Message = UserMessage.InValidData[userSetting.LanguageId], Data = false });
//            }

//        }


//        [HttpGet("GetLastMobileVersion")]
//        public async Task<ActionResult> GetLastMobileVersion()
//        {
//            try
//            {
//                var model = _mobileVersionService.GetLastMobileVersion().Result;
//                if(model != null)
//                {
//                    MobileVersionAPIModel mobileVersionAPIModel = new MobileVersionAPIModel();
//                    mobileVersionAPIModel.IOSVersion = model.IOSVersion;
//                    mobileVersionAPIModel.AndroidVersion = model.AndroidVersion;
//                    mobileVersionAPIModel.IOSLink = model.IOSLink;
//                    mobileVersionAPIModel.AndroidLink = model.AndroidLink;
//                    mobileVersionAPIModel.CreatedDate = model.CreatedDate;
//                    return Ok(new { Message = "Done", data = mobileVersionAPIModel });
//                }
//                else
//                {
//                    return BadRequest(new { Message = "No Data", Data = 0 });
//                }
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = "Failed Process", Data = 0 });
//            }
//        }


//        [HttpGet("GetAllMobileVersions")]
//        public async Task<ActionResult> GetAllMobileVersions()
//        {
//            try
//            {
//                var model = _mobileVersionService.GetAllMobileVersion().Result;
//                if (model != null)
//                {
//                    List<MobileVersionAPIModel> mobileVersionAPIModels = new List<MobileVersionAPIModel>();
//                    foreach(var version in model)
//                    {
//                        MobileVersionAPIModel mobileVersionAPIModel = new MobileVersionAPIModel();
//                        mobileVersionAPIModel.IOSVersion = version.IOSVersion;
//                        mobileVersionAPIModel.AndroidVersion = version.AndroidVersion;
//                        mobileVersionAPIModel.IOSLink = version.IOSLink;
//                        mobileVersionAPIModel.AndroidLink = version.AndroidLink;
//                        mobileVersionAPIModel.CreatedDate = version.CreatedDate;
//                        mobileVersionAPIModels.Add(mobileVersionAPIModel);
//                    }
                    
//                    return Ok(new { Message = "Done", data = mobileVersionAPIModels });
//                }
//                else
//                {
//                    return BadRequest(new { Message = "No Data", Data = 0 });
//                }
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = "Failed Process", Data = 0 });
//            }
//        }






//    }
//}
