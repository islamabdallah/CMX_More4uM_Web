using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Models.Models;
using MoreForYou.Services.Contracts;
//using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models.MaterModels;
using MoreForYou.Services.Models.Message;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Contracts.Medical;

namespace More4UWebAPI.Controllers.MedicalRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IBenefitMailService _benefitMailService;
        private readonly IBenefitService _BenefitService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IPrivilegeService _privilegeService;
        private readonly IRelativeService _relativeService;

        public EmployeeApiController(
          IEmployeeService employeeService,
          UserManager<AspNetUser> userManager,
          IBenefitMailService benefitMailService,
          IBenefitService benefitService,
          IUserNotificationService userNotificationService,
          IPrivilegeService privilegeService,
          IRelativeService relativeService)
        {
            this._employeeService = employeeService;
            this._userManager = userManager;
            this._benefitMailService = benefitMailService;
            this._BenefitService = benefitService;
            this._userNotificationService = userNotificationService;
            this._privilegeService = privilegeService;
            this._relativeService = relativeService;
        }

        [HttpPost("EmployeeRelatives")]
        public async Task<ActionResult> EmployeeRelatives(
          string userNumber,
          string languageCode,
          string type)
        {
            EmployeeApiController employeeApiController = this;
            EmployeeModel result1 = employeeApiController._employeeService.GetEmployee(Convert.ToInt64(userNumber)).Result;
            int int32_1 = Convert.ToInt32(languageCode);
            int int32_2 = Convert.ToInt32(type);
            if (!result1.IsDirectEmployee)
                return (ActionResult)employeeApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = UserMessage.InvalidEmployeeData[int32_1],
                    Data = 0
                });
            if (result1 == null)
                return (ActionResult)employeeApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = UserMessage.InvalidEmployeeData[int32_1],
                    Data = 0
                });
            switch (int32_2)
            {
                case 1:
                case 2:
                    EmployeeRelativesApi result2 = employeeApiController._relativeService.GetEmployeeRelativesApi(Convert.ToInt64(userNumber), int32_1, int32_2, result1.Country).Result;
                    if (result2 == null)
                        return (ActionResult)employeeApiController.BadRequest((object)new
                        {
                            Flag = false,
                            Message = UserMessage.InvalidEmployeeData[int32_1],
                            Data = 0
                        });
                    result2.RelativesApiModel.ProfilePicture = CommanData.Url + CommanData.ProfileFolder + result1.ProfilePicture;
                    result2.RelativesApiModel.EmployeeDepartment = result1.Department.Name;
                    return (ActionResult)employeeApiController.Ok((object)new
                    {
                        Flag = true,
                        Message = UserMessage.SuccessfulProcess[int32_1],
                        employeeRelativesApiModel = result2.RelativesApiModel,
                        Category = result2.medicalCategoryAPIModels,
                        SubCategory = result2.medicalSubCategoryAPIModels,
                        MedicalDetails = result2.medicalDetailsAPIModels
                    });
                case 3:
                    EmployeeRelativesApiModel result3 = employeeApiController._relativeService.GetEmployeeRelativesApiModel(Convert.ToInt64(userNumber), int32_1).Result;
                    if (result3 == null)
                        return (ActionResult)employeeApiController.BadRequest((object)new
                        {
                            Flag = false,
                            Message = UserMessage.InvalidEmployeeData[int32_1],
                            Data = 0
                        });
                    result3.ProfilePicture = CommanData.Url + CommanData.ProfileFolder + result1.ProfilePicture;
                    result3.EmployeeDepartment = result1.Department.Name;
                    return (ActionResult)employeeApiController.Ok((object)new
                    {
                        Flag = true,
                        Message = UserMessage.SuccessfulProcess[int32_1],
                        employeeRelativesApiModel = result3,
                        Category = 0,
                        SubCategory = 0,
                        MedicalDetails = 0
                    });
                default:
                    return (ActionResult)employeeApiController.BadRequest((object)new
                    {
                        Flag = false,
                        Message = "Invalid Type",
                        Data = 0
                    });
            }
        }

        [HttpPost("AllEmployee")]
        public async Task<ActionResult> AllEmployee(int userNumber, int languageCode)
        {
            EmployeeApiController employeeApiController = this;
            EmployeeModel result = employeeApiController._employeeService.GetEmployee((long)userNumber).Result;
            if (result == null)
                return (ActionResult)employeeApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = UserMessage.InvalidEmployeeData[languageCode],
                    Data = 0
                });
            AspNetUser byIdAsync = await employeeApiController._userManager.FindByIdAsync(result.UserId);
            if (!employeeApiController._userManager.GetRolesAsync(byIdAsync).Result.ToList<string>().Contains("MedicalAdmin"))
                return (ActionResult)employeeApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = UserMessage.InvalidEmployeeData[languageCode],
                    Data = 0
                });
            List<EmployeeApiModel> participants = new List<EmployeeApiModel>();
            foreach (EmployeeModel allDirectEmployee in await employeeApiController._employeeService.GetAllDirectEmployees())
                participants.Add(new EmployeeApiModel()
                {
                    name = allDirectEmployee.FullName,
                    employeeNumber = allDirectEmployee.EmployeeNumber.ToString()
                });
            return participants == null || participants.Count<EmployeeApiModel>() <= 0 ? (ActionResult)employeeApiController.BadRequest((object)new
            {
                Flag = false,
                Message = UserMessage.InvalidEmployeeData[languageCode],
                Data = 0
            }) : (ActionResult)employeeApiController.Ok((object)new
            {
                Flag = true,
                Message = UserMessage.SuccessfulProcess[languageCode],
                Data = participants
            });
        }

        [HttpGet("test")]
        public ActionResult Test()
        {
            return (ActionResult)this.Ok((object)new
            {
                Message = "Done",
                Data = "heellllllllllllllllllllllllllllllo"
            });
        }
    }
}
