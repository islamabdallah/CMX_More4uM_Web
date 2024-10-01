using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Models.Models;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.MaterModels;
using MoreForYou.Services.Models.Message;


namespace More4UWebAPI.Controllers.MedicalRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRequestApiController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IBenefitMailService _benefitMailService;
        private readonly IBenefitService _BenefitService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IPrivilegeService _privilegeService;
        private readonly IRelativeService _relativeService;
        private readonly IMedicalRequestService _medicalRequest;
        private readonly IMedicalResponseService _medicalResponse;

        public MedicalRequestApiController(
          IEmployeeService employeeService,
          UserManager<AspNetUser> userManager,
          IBenefitMailService benefitMailService,
          IBenefitService benefitService,
          IUserNotificationService userNotificationService,
          IPrivilegeService privilegeService,
          IRelativeService relativeService,
          IMedicalRequestService medicalRequest,
          IMedicalResponseService medicalResponse)
        {
            _employeeService = employeeService;
            _userManager = userManager;
            _benefitMailService = benefitMailService;
            _BenefitService = benefitService;
            _userNotificationService = userNotificationService;
            _privilegeService = privilegeService;
            _relativeService = relativeService;
            _medicalRequest = medicalRequest;
            _medicalResponse = medicalResponse;
        }

        [HttpPost("MedicalRequests")]
        public async Task<ActionResult> MedicalRequests([FromForm] MedicalRequestApiModel model)
        {
            MedicalRequestApiController requestApiController = this;
            int languageCode = Convert.ToInt32(model.LanguageId);
            if (model == null)
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "failed3",
                    Data = 0
                });
            if (await requestApiController._employeeService.GetEmployeeBySapNumber(Convert.ToInt64(model.createdBy)) == null)
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "failed2",
                    Data = 0
                });
            var result = _medicalRequest.CreateMedicalRequestModelAsync(model).Result;
            if (result == null)
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "failed1",
                    Data = 0
                });
            if (model.attachment == null)
                model.attachment = new List<IFormFile>();
            var medicalRequest = requestApiController._medicalRequest.addMedicalRequest(result, model.attachment);
            return medicalRequest == null ? (ActionResult)requestApiController.BadRequest((object)new
            {
                Flag = false,
                Message = MediccalMessage.FailedProcess[languageCode],
                Data = 0
            }) : (ActionResult)requestApiController.Ok((object)new
            {
                Flag = true,
                Message = MediccalMessage.SuccessfulProcess[languageCode],
                Data = medicalRequest.Id.ToString()
            });
        }

        [HttpPost("PendingRequestSummey")]
        public async Task<ActionResult> PendingRequestSummey(string RequestTypeID, string LanguageId)
        {
            MedicalRequestApiController requestApiController = this;
            int int32_1 = Convert.ToInt32(LanguageId);
            int int32_2 = Convert.ToInt32(RequestTypeID);
            if (!(RequestTypeID != ""))
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "failed3",
                    Data = 0
                });
            if (LanguageId == null)
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "WrongLanguage",
                    Data = 0
                });
            PendingRequestApiModel medicalRequestsByType = requestApiController._medicalRequest.GetAllMedicalRequestsByType(int32_2, int32_1);
            if (medicalRequestsByType != null)
                return (ActionResult)requestApiController.Ok((object)new
                {
                    Flag = true,
                    Message = UserMessage.SuccessfulProcess[int32_1],
                    Data = medicalRequestsByType
                });
            PendingRequestApiModel pendingRequestApiModel = new PendingRequestApiModel();
            PendingRequestCount pendingRequestCount = new PendingRequestCount();
            pendingRequestCount.totalRequest = "0";
            pendingRequestCount.sickleave = "0";
            pendingRequestCount.checkups = "0";
            pendingRequestCount.medications = "0";
            List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
            pendingRequestApiModel.requestsCount = pendingRequestCount;
            pendingRequestApiModel.requests = requestSummeyModelList;
            return (ActionResult)requestApiController.Ok((object)new
            {
                Flag = true,
                Message = UserMessage.SuccessfulProcess[int32_1],
                Data = pendingRequestApiModel
            });
        }

        [HttpPost("MyMedicalRequests")]
        public async Task<ActionResult> MyMedicalRequests(string employeeNumber, string LanguageId)
        {
            MedicalRequestApiController requestApiController = this;
            int int32 = Convert.ToInt32(LanguageId);
            long int64 = Convert.ToInt64(employeeNumber);
            if (!(employeeNumber != ""))
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "failed3",
                    Data = 0
                });
            if (LanguageId == null)
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "WrongLanguage",
                    Data = 0
                });
            PendingRequestApiModel medicalRequestsBy = requestApiController._medicalRequest.GetEmployeeMedicalRequestsBy(int64, int32);
            if (medicalRequestsBy != null)
                return (ActionResult)requestApiController.Ok((object)new
                {
                    Flag = true,
                    Message = UserMessage.SuccessfulProcess[int32],
                    Data = medicalRequestsBy
                });
            PendingRequestApiModel pendingRequestApiModel = new PendingRequestApiModel();
            PendingRequestCount pendingRequestCount = new PendingRequestCount();
            pendingRequestCount.totalRequest = "0";
            pendingRequestCount.sickleave = "0";
            pendingRequestCount.checkups = "0";
            pendingRequestCount.medications = "0";
            List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
            pendingRequestApiModel.requestsCount = pendingRequestCount;
            pendingRequestApiModel.requests = requestSummeyModelList;
            return (ActionResult)requestApiController.Ok((object)new
            {
                Flag = true,
                Message = UserMessage.SuccessfulProcess[int32],
                Data = pendingRequestApiModel
            });
        }

        [HttpPost("MedicalRequestsSearch")]
        public async Task<ActionResult> MedicalRequestsSearch(MedicalRequestSearch requestSearch)
        {
            MedicalRequestApiController requestApiController = this;
            try
            {
                EmployeeModel employee = await requestApiController._employeeService.GetEmployee(Convert.ToInt64(requestSearch.userNumber));
                if (employee == null)
                    return (ActionResult)requestApiController.BadRequest((object)new
                    {
                        Flag = false,
                        Message = "failed3",
                        Data = 0
                    });
                PendingRequestApiModel pendingRequestApiModel1 = new PendingRequestApiModel();
                AspNetUser byIdAsync = await requestApiController._userManager.FindByIdAsync(employee.UserId);
                IList<string> rolesAsync = await requestApiController._userManager.GetRolesAsync(byIdAsync);
                PendingRequestApiModel employeeMedicalRequests;
                if (rolesAsync.Contains("Doctor") || rolesAsync.Contains("MedicalAdmin"))
                {
                    if (string.IsNullOrEmpty(requestSearch.userNumberSearch) && string.IsNullOrEmpty(requestSearch.requestId))
                        return (ActionResult)requestApiController.BadRequest((object)new
                        {
                            Flag = false,
                            Message = "faild employee ",
                            Data = 0
                        });
                    employeeMedicalRequests = requestApiController._medicalRequest.GetAllEmployeeMedicalRequests(requestSearch);
                }
                else
                {
                    if (requestSearch.selectedRequestType != "" || requestSearch.selectedRequestStatus != "" || requestSearch.requestId != "" || requestSearch.relativeId != "" || requestSearch.searchDateFrom != "" || requestSearch.searchDateTo != "")
                    {
                        requestSearch.userNumberSearch = requestSearch.userNumber;
                    }                  
                    employeeMedicalRequests = requestApiController._medicalRequest.GetAllEmployeeMedicalRequests(requestSearch);
                }
                if (employeeMedicalRequests != null)
                    return (ActionResult)requestApiController.Ok((object)new
                    {
                        Flag = true,
                        Message = UserMessage.SuccessfulProcess[Convert.ToInt32(requestSearch.languageId)],
                        Data = employeeMedicalRequests
                    });
                PendingRequestApiModel pendingRequestApiModel2 = new PendingRequestApiModel();
                PendingRequestCount pendingRequestCount = new PendingRequestCount();
                pendingRequestCount.totalRequest = "0";
                pendingRequestCount.sickleave = "0";
                pendingRequestCount.checkups = "0";
                pendingRequestCount.medications = "0";
                List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
                pendingRequestApiModel2.requestsCount = pendingRequestCount;
                pendingRequestApiModel2.requests = requestSummeyModelList;
                return (ActionResult)requestApiController.Ok((object)new
                {
                    Flag = true,
                    Message = UserMessage.SuccessfulProcess[Convert.ToInt32(requestSearch.languageId)],
                    Data = pendingRequestApiModel2
                });
            }
            catch (Exception ex)
            {
                return (ActionResult)requestApiController.Ok((object)new
                {
                    Message = BenefitMessages.FailedProcess[Convert.ToInt32(requestSearch.languageId)],
                    Data = 0
                });
            }
        }

        [HttpPost("MedicalRequestDetails")]
        public async Task<ActionResult> MedicalRequestDetails(string MedicalRequestId, string employeeNumber, string LanguageId)
        {
            MedicalRequestApiController requestApiController = this;
            int int32_1 = Convert.ToInt32(LanguageId);
            int int32_2 = Convert.ToInt32(MedicalRequestId);
            long int64 = Convert.ToInt64(employeeNumber);
            if (!(MedicalRequestId != ""))
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "failed3",
                    Data = 0
                });
            if (LanguageId == null)
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "WrongLanguage",
                    Data = 0
                });
            MedicalRequestDetailsModel result = requestApiController._medicalRequest.GetMedicalRequestsDetailsAsync((long)int32_2, int64, int32_1).Result;
            return result == null ? (ActionResult)requestApiController.BadRequest((object)new
            {
                Flag = false,
                Message = "failed",
                Data = 0
            }) : (ActionResult)requestApiController.Ok((object)new
            {
                Flag = true,
                Message = UserMessage.SuccessfulProcess[int32_1],
                Data = result
            });
        }

        [HttpPost("MedicalResponse")]
        public async Task<ActionResult> MedicalResponse([FromForm] MedicalResponseApiModel model)
        {
            MedicalRequestApiController requestApiController = this;
            int languageCode = Convert.ToInt32(model.LanguageId);
            if (model == null)
                return (ActionResult)requestApiController.BadRequest((object)new
                {
                    Flag = false,
                    Message = "Invalid Data",
                    Data = 0
                });
            EmployeeModel employeeBySapNumber = await requestApiController._employeeService.GetEmployeeBySapNumber(Convert.ToInt64(model.createdBy));
            return employeeBySapNumber != null ? (await requestApiController._userManager.FindByIdAsync(employeeBySapNumber.UserId) == null ? (ActionResult)requestApiController.BadRequest((object)new
            {
                Flag = false,
                Message = "Invalid User",
                Data = 0
            }) : (requestApiController._medicalResponse.addMedicalResponseAsync(model) == null ? (ActionResult)requestApiController.BadRequest((object)new
            {
                Flag = false,
                Message = MediccalMessage.FailedProcess[languageCode],
                Data = 0
            }) : (ActionResult)requestApiController.Ok((object)new
            {
                Flag = true,
                Message = MediccalMessage.SuccessfulProcess[languageCode],
                Data = model.requestId.ToString()
            }))) : (ActionResult)requestApiController.BadRequest((object)new
            {
                Flag = false,
                Message = "Invalid User",
                Data = 0
            });
        }
    }
}
