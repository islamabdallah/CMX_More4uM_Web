//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using MoreForYou.Models.Models;
//using MoreForYou.Service.Contracts.Auth;
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
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BenefitAPIController : ControllerBase
//    {
//        private readonly IBenefitService _BenefitService;
//        private readonly UserManager<AspNetUser> _userManager;
//        private readonly SignInManager<AspNetUser> _signInManager;
//        private readonly IEmployeeService _EmployeeService;
//        private readonly IBenefitRequestService _benefitRequestService;
//        private readonly IGroupEmployeeService _groupEmployeeService;
//        private readonly IRequestWorkflowService _requestWorkflowService;
//        private readonly IRoleService _roleService;
//        private readonly IDepartmentService _departmentService;
//        private readonly IUserNotificationService _userNotificationService;
//        private readonly IBenefitWorkflowService _benefitWorkflowService;
//        private readonly IRequestDocumentService _requestDocumentService;
//        private readonly IEmployeeService _employeeService;
//        private readonly IGroupService _groupService;
//        private readonly IBenefitMailService _benefitMailService;

//        public BenefitAPIController(IBenefitService BenefitService,
//            IBenefitWorkflowService BenefitWorkflowService,

//            UserManager<AspNetUser> userManager,
//             SignInManager<AspNetUser> signInManager,
//            IEmployeeService EmployeeService,
//            IBenefitRequestService benefitRequestService,
//            IGroupEmployeeService groupEmployeeService,
//            IRequestWorkflowService requestWorkflowService,
//            IRoleService roleService,
//            IDepartmentService departmentService,
//            IUserNotificationService userNotificationService,
//            IBenefitWorkflowService benefitWorkflowService,
//            IRequestDocumentService requestDocumentService,
//            IEmployeeService employeeService,
//            IFirebaseNotificationService firebaseNotificationService,
//            IGroupService groupService,
//            IBenefitMailService benefitMailService
//            )
//        {
//            _BenefitService = BenefitService;
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _EmployeeService = EmployeeService;
//            _benefitRequestService = benefitRequestService;
//            _groupEmployeeService = groupEmployeeService;
//            _requestWorkflowService = requestWorkflowService;
//            _roleService = roleService;
//            _departmentService = departmentService;
//            _userNotificationService = userNotificationService;
//            _benefitWorkflowService = benefitWorkflowService;
//            _requestDocumentService = requestDocumentService;
//            _employeeService = employeeService;
//            _groupService = groupService;
//            _benefitMailService = benefitMailService;
//        }

//        [HttpPost("GetBenefitDetails")]
//        public async Task<ActionResult> GetBenefitDetails(long employeeNumber, long benefitId, int languageId)
//        {
//            EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber).Result;
//            BenefitModel benefitModel = new BenefitModel();
//            if (employeeModel != null)
//            {
//                BenefitAPIModel benefitAPIModel = _BenefitService.GetBenefitDetails(benefitId, employeeModel, languageId);
//                if (benefitAPIModel != null)
//                {
//                    return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = benefitModel });
//                }
//                else
//                {
//                    return BadRequest(new { Message = "Failed Process, benefit not found", Data = 0 });

//                }
//            }
//            else
//            {
//                return BadRequest(new { Message = "Failed Process, wrong employee Data", Data = 0 });
//            }
//        }

//        [HttpPost("WhoCanRedeemThisGroupBenefit")]
//        public async Task<ActionResult> WhoCanRedeemThisGroupBenefit(long employeeNumber, long benefitId, int languageId)
//        {
//            List<Participant> participants = new List<Participant>();
//            participants = _BenefitService.GetEmployeesCanRedeemThisGroupBenefit(employeeNumber, benefitId);

//            if (participants != null)
//            {
//                return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = participants });

//            }
//            else
//            {
//                return BadRequest(new { Message = BenefitMessages.InvalidEmployeeNumber[languageId], Data = 0 });
//            }
//        }


//        [HttpPost("WhoCanIGiveThisBenefit")]
//        public async Task<ActionResult> WhoCanIGiveThisBenefit(long employeeNumber, long benefitId, int languageId)
//        {
//            List<Participant> participants = _BenefitService.GetEmployeesWhoCanIGiveThisBenefit(employeeNumber, benefitId);
//            if (participants != null)
//            {
//                return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = participants });
//            }
//            else
//            {
//                return BadRequest(new { Message = BenefitMessages.InvalidEmployeeNumber[languageId], Data = 0 });
//            }
//        }


//        [HttpPost("ShowMyBenefits")]
//        public async Task<ActionResult> ShowMyBenefits(int EmployeeNumber, int languageId)
//        {
//            try
//            {
//                //AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
//                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployee(EmployeeNumber).Result;
//                List<BenefitAPIModel> benefitModels = _BenefitService.GetMyBenefits(CurrentEmployee.EmployeeNumber, languageId);
//                if(benefitModels != null)
//                {
//                    if (benefitModels.Count > 0)
//                    {
//                        return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = benefitModels.ToList() });
//                    }
//                    else
//                    {
//                        return Ok(new { Message = BenefitMessages.NoBenefits[languageId], Data = new List<BenefitAPIModel> { } });
//                    }
//                }
//                else
//                {
//                    return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = 0 });
//                }
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = 0 });
//            }
//        }


//        [HttpPost("ShowMyBenefitRequests")]
//        public async Task<ActionResult> ShowMyBenefitRequests(long BenefitId, long EmployeeNumber, long requestNumber, int languageId)
//        {
//            try
//            {
//                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
//                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployee(EmployeeNumber).Result;
//                long benefitTypeId = _BenefitService.GetBenefit(BenefitId).BenefitTypeId;
//                List<Request> requests = _requestWorkflowService.GetMyBenefitRequests(CurrentEmployee.EmployeeNumber, BenefitId, benefitTypeId, languageId).ToList();
//                MyRequests myRequests = new MyRequests();
//                if (requests != null)
//                {
//                    requests = requests.OrderByDescending(r => r.Requestedat).ToList();

//                    if (requestNumber != -1)
//                    {
//                        List<Request> arrangedRequests = new List<Request>();
//                        var myrequest = requests.Where(r => r.RequestNumber == requestNumber).First();
//                        requests.Remove(myrequest);
//                        arrangedRequests.AddRange(requests);
//                        arrangedRequests.Insert(0, myrequest);
//                        return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = arrangedRequests });
//                    }
//                    else
//                    {
//                        return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = requests });
//                    }
//                }
//                else
//                {
//                    return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = 0 });
//                }
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = 0 });
//            }

//        }

//        [HttpPost("ShowRequestsDefault")]
//        public async Task<ActionResult> ShowRequests(long EmployeeNumber, long requestNumber, int languageId)
//        {
//            try
//            {
//                EmployeeModel employeeModel = _EmployeeService.GetEmployee(EmployeeNumber).Result;
//                AspNetUser CurrentUser = await _userManager.FindByIdAsync(employeeModel.UserId);
//                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
//                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
//                ManageRequest manageRequest = new ManageRequest();
//                if (userRoles != null)
//                {
//                    //if (userRoles.Contains("Admin"))
//                    //{
//                    //    //requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows().Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
//                    //    //List<DepartmentModel> departmentModels = _departmentService.GetAllDepartments().ToList();
//                    //    //manageRequest.DepartmentModels = new List<DepartmentAPI>();
//                    //    //foreach (var dept in departmentModels)
//                    //    //{
//                    //    //    DepartmentAPI departmentAPI = new DepartmentAPI();
//                    //    //    departmentAPI.Id = dept.Id;
//                    //    //    departmentAPI.Name = dept.Name;
//                    //    //    manageRequest.DepartmentModels.Add(departmentAPI);
//                    //    //}
//                    //    //manageRequest.IsAdmin = true;
//                    //}
//                    if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR") || userRoles.Contains("Payroll"))
//                    {
//                        requestWokflowModels = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber).Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();

//                    }
//                    //manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id).Result;
//                    manageRequest.SelectedDepartmentId = -1;
//                    manageRequest.SelectedRequestStatus = -1;
//                    manageRequest.SelectedTimingId = -1;
//                    manageRequest.SelectedBenefitType = -1;
//                    if (true)
//                    {
//                        if (requestNumber != -1&& requestWokflowModels.Count != 0)
//                        {
//                            var requiredRequest = requestWokflowModels.Where(w => w.BenefitRequestId == requestNumber);
//                            if (requiredRequest.Count() == 0)
//                            {
//                                RequestWokflowModel requiredRequestWokflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(EmployeeNumber, requestNumber);
//                                requestWokflowModels.Add(requiredRequestWokflowModel);
//                            }
//                        }
//                        else if (requestNumber != -1&&  requestWokflowModels.Count == 0)
//                        {
//                            RequestWokflowModel requiredRequestWokflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(EmployeeNumber, requestNumber);
//                            requestWokflowModels = new List<RequestWokflowModel>();
//                            requestWokflowModels.Add(requiredRequestWokflowModel);
//                        }
//                        else if (requestWokflowModels.Count == 0)
//                        {
//                            return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = manageRequest });
//                        }
//                        requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
//                        requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
//                        foreach (var requestWorkflow in requestWokflowModels)
//                        {
//                            var documents = _requestDocumentService.GetRequestDocuments(requestWorkflow.BenefitRequestId);
//                            if (documents.Count > 0)
//                            {
//                                requestWorkflow.HasDocuments = true;
//                            }
//                            else
//                            {
//                                requestWorkflow.HasDocuments = false;
//                            }
//                        }

//                        manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels, languageId);
//                        if (manageRequest.Requests != null)
//                        {
//                            manageRequest.Requests = manageRequest.Requests.OrderByDescending(r => r.Requestedat).ToList();
//                            if (requestNumber != -1)
//                            {
//                                var myRequest = manageRequest.Requests.Where(r => r.RequestNumber == requestNumber).First();
//                                List<Request> requests = new List<Request>();
//                                manageRequest.Requests.Remove(myRequest);
//                                requests.AddRange(manageRequest.Requests);
//                                requests.Insert(0, myRequest);
//                                manageRequest.Requests = requests;
//                            }
//                        }
//                    }
//                }
//                return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = manageRequest });

//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = 0 });
//            }

//        }

//        [HttpPost("ShowRequests")]
//        public async Task<ActionResult> ShowRequests(RequestSearch requestSearch)
//        {
//            try
//            {
//                ManageRequest manageRequest = _requestWorkflowService.CreateManageRequestModel(requestSearch);
//                EmployeeModel employeeModel = _EmployeeService.GetEmployee(requestSearch.userNumber).Result;
//                if (employeeModel != null)
//                {
//                    AspNetUser CurrentUser = _userManager.FindByIdAsync(employeeModel.UserId).Result;
//                    List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
//                    List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
//                    if (userRoles != null)
//                    {
//                        //if (userRoles.Contains("Admin"))
//                        //{
//                        //    //if (manageRequest.SelectedDepartmentId != -1)
//                        //    //{
//                        //    //    requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows().Where(rw => rw.BenefitRequest.Employee.DepartmentId == manageRequest.SelectedDepartmentId).ToList();
//                        //    //}
//                        //    //else
//                        //    //{
//                        //    //    requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows();
//                        //    //}
//                        //    //requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
//                        //    //List<DepartmentModel> departmentModels = _departmentService.GetAllDepartments().ToList();
//                        //    //manageRequest.DepartmentModels = new List<DepartmentAPI>();
//                        //    //foreach (var dept in departmentModels)
//                        //    //{
//                        //    //    DepartmentAPI departmentAPI = new DepartmentAPI();
//                        //    //    departmentAPI.Id = dept.Id;
//                        //    //    departmentAPI.Name = dept.Name;
//                        //    //    manageRequest.DepartmentModels.Add(departmentAPI);
//                        //    //}
//                        //    //manageRequest.IsAdmin = true;
//                        //}
//                        if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR")|| userRoles.Contains("Payroll"))
//                        {
//                            requestWokflowModels = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber).ToList();
//                            //requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();

//                        }
//                        if (requestWokflowModels.Count != 0)
//                        {

//                            if (manageRequest.employeeNumberSearch != -1)
//                            {
//                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.EmployeeId == manageRequest.employeeNumberSearch).ToList();
//                            }

//                            if (manageRequest.SelectedRequestStatus != -1)
//                            {
//                                requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId == manageRequest.SelectedRequestStatus).ToList();
//                            }
//                            if (manageRequest.SearchDateFrom.ToString("yyyy-MM-dd") != "0001-01-01" && manageRequest.SearchDateTo.ToString("yyyy-MM-dd") != "0001-01-01")
//                            {
//                                requestWokflowModels = requestWokflowModels.Where(rw => rw.CreatedDate.Date >= manageRequest.SearchDateFrom.Date && rw.CreatedDate.Date <= manageRequest.SearchDateTo.Date).ToList();
//                            }
//                            if (manageRequest.SelectedBenefitType != -1)
//                            {
//                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.Benefit.BenefitTypeId == manageRequest.SelectedBenefitType).ToList();
//                            }
//                            requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
//                            foreach (var requestWorkflow in requestWokflowModels)
//                            {
//                                var documents = _requestDocumentService.GetRequestDocuments(requestWorkflow.BenefitRequestId);
//                                if (documents.Count > 0)
//                                {
//                                    requestWorkflow.HasDocuments = true;
//                                }
//                                else
//                                {
//                                    requestWorkflow.HasDocuments = false;
//                                }
//                            }
//                            if (manageRequest.HasWarningMessage == true)
//                            {
//                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.WarningMessage != null || rw.HasDocuments == true).ToList();
//                                //requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.WarningMessage != null).ToList();

//                            }
//                            //List<long> uniquestRequestsId = requestWokflowModels.Select(r => r.BenefitRequestId).ToList();
//                            //List<RequestWokflowModel> SelectedRequestWorkflowModels = new List<RequestWokflowModel>();
//                            //foreach(long requestId in uniquestRequestsId)
//                            //{
//                            //    RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
//                            //    requestWokflowModel = _requestWorkflowService

//                            //}
//                            requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
//                            //manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id).Result;
//                            manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels, requestSearch.languageId);
//                            if (manageRequest.Requests != null)
//                            {
//                                manageRequest.Requests = manageRequest.Requests.OrderByDescending(r => r.Requestedat).ToList();
//                            }
//                        }
//                    }
//                    else
//                    {
//                        return Ok(new { Message = BenefitMessages.NoRequests[requestSearch.languageId], Data = 0 });

//                    }
//                    return Ok(new { Message = BenefitMessages.SuccessfulProcess[requestSearch.languageId], Data = manageRequest });

//                }
//                else
//                {
//                    return Ok(new { Message = BenefitMessages.InvalidEmployeeData[requestSearch.languageId], Data = 0 });
//                }


//            }
//            catch (Exception e)
//            {
//                return Ok(new { Message = BenefitMessages.FailedProcess[requestSearch.languageId], Data = 0 });

//            }
//        }

//        [HttpPost("ShowTenNotifications")]
//        public async Task<ActionResult> ShowTenNotifications(long employeeNumber, long index, int languageId)
//        {
//            try
//            {
//                List<NotificationAPIModel> notificationAPIModels = new List<NotificationAPIModel>();
//                EmployeeModel employee = _EmployeeService.GetEmployee(employeeNumber).Result;
//                List<UserNotificationModel> userNotificationModels = _userNotificationService.GetUserNotification(employee.UserId);
//                if (userNotificationModels != null)
//                {
//                    List<NotificationAPIModel> NotificationAPIModels = new List<NotificationAPIModel>();
//                    userNotificationModels = userNotificationModels.OrderByDescending(un => un.CreatedDate).ToList();
//                    notificationAPIModels = _userNotificationService.CreateNotificationAPIModel(userNotificationModels, languageId);
//                }
//                else
//                {
//                    notificationAPIModels = new List<NotificationAPIModel>();

//                }

//                return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = notificationAPIModels });
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = 0 });

//            }

//        }

//        [HttpPost("ShowFiftyNotifications")]
//        public async Task<ActionResult> ShowFiftyNotifications(long employeeNumber, int languageId)
//        {
//            try
//            {
//                List<NotificationAPIModel> notificationAPIModels = new List<NotificationAPIModel>();
//                EmployeeModel employee = _EmployeeService.GetEmployee(employeeNumber).Result;
//                List<UserNotificationModel> userNotificationModels = _userNotificationService.GetFiftyUserNotification(employee.UserId);
//                if (userNotificationModels != null)
//                {
//                    foreach(var notification in userNotificationModels)
//                    {
//                        notification.Seen = true;
//                        _userNotificationService.UpdateUserNotification(notification);
//                    }
//                    List<NotificationAPIModel> NotificationAPIModels = new List<NotificationAPIModel>();
//                    userNotificationModels = userNotificationModels.OrderByDescending(un => un.CreatedDate).ToList();
//                    notificationAPIModels = _userNotificationService.CreateNotificationAPIModel(userNotificationModels, languageId);
//                }
//                else
//                {
//                    notificationAPIModels = new List<NotificationAPIModel>();

//                }

//                return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = notificationAPIModels });
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = 0 });

//            }

//        }


//        [HttpPost("ConfirmRequest")]
//        public async Task<ActionResult> ConfirmRequest(RequestAPI request1)
//        {
//            try
//            {
//                string result = "";
//                BenefitModel benefitModel = _BenefitService.GetBenefit(request1.benefitId);
//                Request request = _benefitRequestService.CreateRequestModel(request1, benefitModel.BenefitTypeId, benefitModel.IsAgift);
//                if (request != null)
//                {
//                    DateTime requiredDate = Convert.ToDateTime(request.From);
//                    if (requiredDate.Date > DateTime.Today.Date)
//                    {
//                        EmployeeModel employeeModel = _EmployeeService.GetEmployee(request.UserNumber).Result;
//                        if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Individual)
//                        {
//                            result = _requestWorkflowService.AddIndividualRequest(request, employeeModel.UserId, benefitModel);

//                        }
//                        else if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Group)
//                        {
//                            result = _requestWorkflowService.ConfirmGroupRequest(request, employeeModel.UserId, benefitModel).Result;
//                        }
//                    }
//                    else
//                    {
//                        return BadRequest(new { Message = BenefitMessages.InvalidDate[request1.languageId], Data = 0 });
//                    }
//                }
//                else
//                {
//                    return BadRequest(new { Message = BenefitMessages.FailedProcess[request1.languageId], Data = 0 });
//                }

//                if (result.Contains("Success Process"))
//                {
//                    return Ok(new { Message = BenefitMessages.SuccessfulProcess[request1.languageId], Data = request });
//                }
//                else
//                {
//                    //return BadRequest(new { Message = result, Data = 0 });
//                    return BadRequest(new { Message = BenefitMessages.FailedProcess[request1.languageId], Data = 0 });

//                }
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[request1.languageId], Data = 0 });

//            }
//        }

//        [HttpPost("UploadRequestDocuments")]
//        public async Task<IActionResult> UploadRequestDocuments(long requestNumber, List<IFormFile> files, int languageId)
//        {
//            string message = await _requestWorkflowService.AddDocumentsToRequest(requestNumber, files);
//            if (message.Contains("Success Process"))
//            {
//                return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = true });
//            }
//            else
//            {
//                return BadRequest(new { Message = "Failed Process", Data = false });

//            }
//        }

//        [HttpPost("RequestCancel")]
//        public ActionResult RequestCancel(long id, long benefitId, long employeeNumber, int languageId)
//        {
//            string message = _requestWorkflowService.CancelMyRequest(id);
//            if (message == "Success Process")
//            {
//                return Ok(new { Message = BenefitMessages.RequestCancelDone[languageId], Data = true });
//                //BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
//                //RequestWokflowModel requestWokflowModel = _requestWorkflowService.GetRequestWorkflow(id).First();
//                //bool result = SendNotification(benefitRequestModel, requestWokflowModel, "RequestCancel").Result;

//            }
//            else
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = false });
//            }
//        }

//        [HttpPost("updateProfilePicture")]
//        public async Task<IActionResult> updateProfilePicture(long employeeNumber, UpdateProfile updateProfile, int languageId)
//        {
//            bool result = false;

//            if (updateProfile.Photo != "")
//            {
//                EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber).Result;
//                employeeModel.ProfilePicture = _requestWorkflowService.SaveImage(updateProfile.Photo, CommanData.ProfileFolder);
//                result = _EmployeeService.UpdateEmployee(employeeModel).Result;
//            }
//            else
//            {
//                return BadRequest(new { Message = BenefitMessages.InvalidImage[languageId], Data = false });
//            }
//            if (result == true)
//            {
//                LoginUser User = new LoginUser();
//                EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber).Result;
//                var requests = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber);
//                if (requests.Count != 0)
//                {
//                    employeeModel.hasRequests = true;
//                    employeeModel.PendingRequestsCount = requests.Where(r => r.RequestStatusId == (int)CommanData.BenefitStatus.Pending).Count();

//                }
//                else
//                {
//                    employeeModel.hasRequests = false;
//                    employeeModel.PendingRequestsCount = 0;

//                }
//                User = _employeeService.CreateLoginUser(employeeModel);
//                if (User != null)
//                {
//                    return Ok(new { Message = BenefitMessages.UpdateProfileDone[languageId], Data = User });
//                }
//                else
//                {
//                    return BadRequest(new { Message = BenefitMessages.UpdateProfileFailed[languageId], Data = true });

//                }
//            }
//            else
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = false });
//            }
//        }

//        [HttpPost("AddResponse")]
//        public async Task<IActionResult> AddResponse(long requestWorkflowId, int status, string message, long employeeNumber, int languageId)
//        {
//            try
//            {
//                bool result = false;
//                string type = "Response";
//                string replay = "";
//                int order2 = 0;
//                EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber).Result;
//                AspNetUser CurrentUser = await _userManager.FindByIdAsync(employeeModel.UserId);
//                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
//                RequestWokflowModel DBRequestWorkflowModel = _requestWorkflowService.GetRequestWorkflowById(requestWorkflowId);
//                var request =_benefitRequestService.GetBenefitRequest(DBRequestWorkflowModel.BenefitRequestId);
//                if(request.GroupId == null)
//                {
//                    if (DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending ||
//                   DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.InProgress)
//                    {
//                        //DBRequestWorkflowModel.IsVisible = true;
//                        //DBRequestWorkflowModel.IsDelted = false;
//                        //DBRequestWorkflowModel.UpdatedDate = DateTime.Now;
//                        DBRequestWorkflowModel.ReplayDate = DateTime.Now;
//                        DBRequestWorkflowModel.Notes = message;
//                        bool updateResult = false;
//                        BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(DBRequestWorkflowModel.BenefitRequestId);
//                        if (status == 2)
//                        {
//                            DBRequestWorkflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
//                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
//                            updateResult = _requestWorkflowService.UpdateRequestWorkflow(DBRequestWorkflowModel).Result;
//                            if (updateResult == true)
//                            {
//                                updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
//                                if (updateResult == true)
//                                {
//                                    result = true;
//                                    result = _requestWorkflowService.SendNotification(benefitRequestModel, DBRequestWorkflowModel, type, 0).Result;
//                                    if(userRoles.Contains("HR"))
//                                    {
//                                      result = _requestWorkflowService.AddSameResponseForAllHRRole(benefitRequestModel.Id, status, message, DBRequestWorkflowModel.EmployeeId);
//                                      if(result == true)
//                                        {
//                                            replay = BenefitMessages.ApprovedDone[languageId];
//                                        }
//                                        else
//                                        {
//                                            replay = BenefitMessages.RejectedDone[languageId];
//                                            result = true;
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    replay = BenefitMessages.ApprovalUpdateFailed[languageId];
//                                    result = false;
//                                }
//                            }
//                            else
//                            {
//                                replay = BenefitMessages.FailedProcess[languageId];
//                                result = false;
//                            }
//                        }
//                        else if (status == 1)
//                        {
//                            DBRequestWorkflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
//                            updateResult = _requestWorkflowService.UpdateRequestWorkflow(DBRequestWorkflowModel).Result;
                            
//                            if (updateResult == true)
//                            {
//                                if (userRoles.Contains("HR"))
//                                {
//                                 result = _requestWorkflowService.AddSameResponseForAllHRRole(benefitRequestModel.Id, status, message, DBRequestWorkflowModel.EmployeeId);
//                                }
//                                else
//                                {
//                                    result = true;
//                                }
//                                if (result == true)
//                                {
//                                    List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
//                                    int benefitWorflowsCount = benefitWorkflowModels.Count;
//                                    int Currentorder = benefitWorkflowModels.Where(bw => bw.RoleId == DBRequestWorkflowModel.RoleId).Select(bw => bw.Order).First();
//                                    if (Currentorder < benefitWorflowsCount)
//                                    {
//                                        order2 = _requestWorkflowService.CheckSameWorkflow(Currentorder, benefitRequestModel.BenefitId, DBRequestWorkflowModel, benefitRequestModel.EmployeeId);
//                                    }
//                                    else
//                                    {
//                                        order2 = Currentorder;
//                                    }
//                                    if (order2 != 0)
//                                    {
//                                        int order = order2 + 1;

//                                        if (order > benefitWorflowsCount)
//                                        {
//                                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
//                                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
//                                            if (updateResult == true)
//                                            {
//                                                replay = BenefitMessages.ApprovedDone[languageId];
//                                                result = true;
//                                                result = _requestWorkflowService.SendNotification(benefitRequestModel, DBRequestWorkflowModel, type, 0).Result;
//                                                ////////////////////////////////////Send mail to MailList//////////
//                                                if (benefitRequestModel.Benefit.HasMails == true)
//                                                {
//                                                  //  _benefitMailService.SendToMailList(benefitRequestModel);
//                                                }

//                                            }
//                                        }
//                                        else if (order <= benefitWorflowsCount)
//                                        {
//                                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.InProgress;
//                                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
//                                            if (updateResult == true)
//                                            {
//                                                updateResult = _requestWorkflowService.SendNotification(benefitRequestModel, DBRequestWorkflowModel, type, 0).Result;
//                                                //int nextOrder = order + 1;
//                                                string messageResult = _requestWorkflowService.SendReuestToWhoIsConcern(benefitRequestModel.Id, order).Result;
//                                                replay = BenefitMessages.ApprovedDone[languageId];
//                                                result = true;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        replay = BenefitMessages.RejectedDone[languageId];

//                                        return BadRequest(new { Message = replay, Data = false });
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                replay = BenefitMessages.ApprovalUpdateFailed[languageId];
//                                return BadRequest(new { Message = replay, Data = false });
//                            }
//                        }
//                        else
//                        {
//                            return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = false });
//                        }
//                        if (result == true)
//                        {
//                            return Ok(new { Message = BenefitMessages.ApprovedDone[languageId], Data = true });

//                        }
//                        else
//                        {
//                            return BadRequest(new { Message = replay, Data = false });
//                        }
//                    }
//                    else
//                    {
//                        return BadRequest(new { Message = BenefitMessages.WrongResponseStatus[languageId] + DBRequestWorkflowModel.RequestStatus.Name, Data = false });

//                    }
//                }
//                else
//                {
//                    if (DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending ||
//                   DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.InProgress)
//                    {
//                        var groupResult = _requestWorkflowService.GroupRequestResponse((long)request.GroupId, status, message, employeeNumber);
//                        if (groupResult.Result == true)
//                        {
//                            return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId] + DBRequestWorkflowModel.RequestStatus.Name, Data = true });

//                        }
//                        else
//                        {
//                            return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = false });

//                        }
//                    }
//                    else
//                    {
//                        return BadRequest(new { Message = BenefitMessages.WrongResponseStatus[languageId] + DBRequestWorkflowModel.RequestStatus.Name, Data = false });
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = false });
//            }

//        }

//        [HttpPost("ShowMyGifts")]
//        public async Task<IActionResult> ShowMyGifts(long employeeNumber, long requestNumber, int languageId)
//        {
//            try
//            {
//                List<Gift> myGifts = new List<Gift>();
//                List<Gift> gifts = _requestWorkflowService.GetMyGifts(employeeNumber, languageId);
//                if (gifts.Count > 0)
//                {
//                    if(requestNumber != -1)
//                    {
//                       var requiredGift = gifts.Where(g => g.RequestNumber == requestNumber).First();
//                        gifts.Remove(requiredGift);
//                        myGifts.AddRange(gifts);
//                        myGifts.Insert(0, requiredGift);
//                        return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = myGifts });
//                    }
//                    else
//                    {
//                        return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = gifts });
//                    }

//                }
//                else
//                {
//                    return Ok(new { Message = BenefitMessages.NoGiftsExist[languageId], Data = myGifts });
//                }
//            }
//            catch (Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = false });
//            }
//        }

//        [HttpPost("GetProfilePictureAndRequestDocuments")]
//        public async Task<IActionResult> GetProfilePictureAndRequestDocuments(long employeeNumber, long requestNumber, int languageId)
//        {
//            try
//            {
//               var request =  _benefitRequestService.GetBenefitRequest(requestNumber);
//                EmployeeModel employeeModel = _employeeService.GetEmployee(request.EmployeeId).Result;
//                if (employeeModel != null)
//                {
//                    ProfileAndDocuments profileAndDocuments = new ProfileAndDocuments();

//                    profileAndDocuments.ProfilePicture = employeeModel.ProfilePicture;

//                    var documents = _requestDocumentService.GetRequestDocuments(requestNumber);

//                    if (documents != null)
//                    {
//                        List<string> requestDocuments = documents.Select(d => d.fileName).ToList();
//                        profileAndDocuments.Documents = requestDocuments;
//                    }
//                    return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = profileAndDocuments });
//                }
//                else
//                {
//                    return BadRequest(new { Message = "Failed Process, Invalid employee data", Data = 0 });

//                }
//            }
//            catch(Exception e)
//            {
//                return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = 0 });
//            }

//        }
        
//        [HttpPost("GetEmployeeProfileData")]
//        public async Task<ActionResult> GetEmployeeProfileData(long employeeNumber, int languageId)
//        {
//          EmployeeModel employeeModel =  _employeeService.GetEmployee(employeeNumber).Result;
//            if(employeeModel != null)
//            {
//                LoginUser loginUser = new LoginUser();
//               loginUser = _employeeService.CreateLoginUser(employeeModel);
//               if(loginUser != null)
//                {
//                    return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = loginUser });
//                }
//                else
//                {
//                    return BadRequest(new { Message = BenefitMessages.FailedProcess[languageId], Data = false });
//                }
//            }
//            else
//            {
//                return BadRequest(new { Message = BenefitMessages.InvalidEmployeeData[languageId], Data = false });
//            }
//        }

//        [HttpPost("GetEmployeeProfilePicture")]
//        public async Task<ActionResult> GetEmployeeProfilePicture(long employeeNumber, int languageId)
//        {
//            EmployeeModel employeeModel = _employeeService.GetEmployee(employeeNumber).Result;
//            if (employeeModel != null)
//            {
//                if (employeeModel.ProfilePicture != null)
//                {
//                    return Ok(new { Message = BenefitMessages.SuccessfulProcess[languageId], Data = employeeModel.ProfilePicture});
//                }
//                else
//                {
//                    return BadRequest(new { Message = BenefitMessages.NoProfilePicture[languageId], Data = "" });
//                }
//            }
//            else
//            {
//                return BadRequest(new { Message = BenefitMessages.InvalidEmployeeData[languageId], Data = false });
//            }
//        }



//    }
//}
