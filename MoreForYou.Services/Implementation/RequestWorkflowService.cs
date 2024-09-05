using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Service.Implementation.Email;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.hub;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class RequestWorkflowService : IRequestWorkflowService
    {
        private readonly IRepository<RequestWorkflow, long> _repository;
        private readonly ILogger<RequestWorkflowService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _EmployeeService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        //private readonly IBenefitService _benefitService;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IBenefitTypeService _benefitTypeService;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IDepartmentService _departmentService;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private readonly IGroupService _groupService;
        private readonly IBenefitWorkflowService _benefitWorkflowService;
        private readonly IRequestDocumentService _requestDocumentService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IFirebaseNotificationService _firebaseNotificationService;
        private readonly IBenefitMailService _benefitMailService;
        private readonly IEmployeeService _employeeService;
        private readonly IOutlookSenderService _outlookSenderService;

        public RequestWorkflowService(IRepository<RequestWorkflow, long> requestWorkflowRepository,
          ILogger<RequestWorkflowService> logger,
          IMapper mapper,
          UserManager<AspNetUser> userManager,
          IEmployeeService EmployeeService,
          IUserService userService,
          IRoleService roleService,
          //IBenefitService benefitService,
          IRequestStatusService requestStatusService,
          IBenefitTypeService benefitTypeService,
          IBenefitRequestService benefitRequestService,
          IDepartmentService departmentService,
          IGroupEmployeeService groupEmployeeService,
          IGroupService groupService,
          IBenefitWorkflowService benefitWorkflowService,
          IRequestDocumentService requestDocumentService,
          IWebHostEnvironment hostEnvironment,
           IHubContext<NotificationHub> hub,
            INotificationService notificationService,
            IUserNotificationService userNotificationService,
            IUserConnectionManager userConnectionManager,
            IFirebaseNotificationService firebaseNotificationService,
            IBenefitMailService benefitMailService,
            IEmployeeService employeeService,
            IOutlookSenderService outlookSenderService
          )
        {
            _repository = requestWorkflowRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _EmployeeService = EmployeeService;
            _userService = userService;
            _roleService = roleService;
            _requestStatusService = requestStatusService;
            _benefitTypeService = benefitTypeService;
            _benefitRequestService = benefitRequestService;
            _departmentService = departmentService;
            _groupEmployeeService = groupEmployeeService;
            _groupService = groupService;
            _benefitWorkflowService = benefitWorkflowService;
            _requestDocumentService = requestDocumentService;
            _hostEnvironment = hostEnvironment;
            _hub = hub;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _userConnectionManager = userConnectionManager;
            _firebaseNotificationService = firebaseNotificationService;
            _benefitMailService = benefitMailService;
            _employeeService = employeeService;
            _outlookSenderService = outlookSenderService;
        }
        public async Task<RequestWokflowModel> CreateRequestWorkflow(RequestWokflowModel model)
        {
            var requestWorkflow = _mapper.Map<RequestWorkflow>(model);

            try
            {
                var addedReuestWorkflow = _repository.Add(requestWorkflow);
                if (addedReuestWorkflow != null)
                {
                    RequestWokflowModel addedRequestWokflowModel = new RequestWokflowModel();
                    addedRequestWokflowModel = _mapper.Map<RequestWokflowModel>(addedReuestWorkflow);
                    return addedRequestWokflowModel;
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

        public bool DeleteRequestWorkflow(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RequestWokflowModel>> GetAllRequestWorkflows()
        {
            List<RequestWorkflow> requestWorkflows = await _repository.Find(rw => rw.IsVisible == true, false, RW => RW.Employee, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company, RW => RW.BenefitRequest.Employee.Nationality).ToListAsync();
            List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
            return requestWokflowModels;
        }

        public async Task<List<RequestWokflowModel>> GetRequestWorkflow(long requestId)
        {
            try
            {
                List<RequestWorkflow> requestWorkflows = await _repository.Find(RW => RW.BenefitRequestId == requestId, false, RW => RW.Employee, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company).ToListAsync();
                List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<bool> UpdateRequestWorkflow(RequestWokflowModel model)
        {
            bool result = false;
            try
            {
                RequestWorkflow requestWorkflow = _mapper.Map<RequestWorkflow>(model);
                result = _repository.Update(requestWorkflow);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return result;

            }
        }
        public List<RequestWokflowModel> GetRequestWorkflow2(long requestId)
        {
            try
            {
                List<RequestWorkflow> requestWorkflows = _repository.Find(RW => RW.BenefitRequestId == requestId, false, RW => RW.Employee, RW => RW.RequestStatus).ToList();
                List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public Task<bool> CancelRequestWorkflow(RequestWokflowModel model)
        {
            bool result = false;
            try
            {
                RequestWorkflow requestWorkflow = _mapper.Map<RequestWorkflow>(model);
                result = _repository.Update(requestWorkflow);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(result);
        }

        public async Task<List<RequestWokflowModel>> GetRequestWorkflowByEmployeeNumber(long employeeNumber)
        {
            try
            {
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                //List<RequestWorkflow> requestWorkflows = _repository.Find(RW => RW.EmployeeId == employeeNumber && RW.IsVisible == true && RW.BenefitRequest.RequestStatusId !=(int) CommanData.BenefitStatus.Cancelled, false, RW => RW.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Group, RW => RW.BenefitRequest.Group.RequestStatus, RW => RW.BenefitRequest.Employee, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company, RW => RW.BenefitRequest.Employee.Nationality, RequestWorkflow=>RequestWorkflow.BenefitRequest.Employee.Supervisor).ToList();
                List<RequestWorkflow> requestWorkflows = await _repository.Find(RW => RW.EmployeeId == employeeNumber && RW.IsVisible == true, false, RW => RW.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Group, RW => RW.BenefitRequest.Group.RequestStatus, RW => RW.BenefitRequest.Employee, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company, RW => RW.BenefitRequest.Employee.Nationality, RequestWorkflow => RequestWorkflow.BenefitRequest.Employee.Supervisor).ToListAsync();
                if (requestWorkflows != null)
                {
                    requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
                }
                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public RequestWokflowModel GetRequestWorkflowByEmployeeNumber(long employeeNumber, long requestId)
        {
            try
            {
                RequestWorkflow requestWorkflow = _repository.Find(RW => RW.EmployeeId == employeeNumber && RW.BenefitRequestId == requestId, false, RW => RW.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Group, RW => RW.BenefitRequest.Group.RequestStatus, RW => RW.BenefitRequest.Employee, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company, RW => RW.BenefitRequest.Employee.Nationality, RequestWorkflow => RequestWorkflow.BenefitRequest.Employee.Supervisor).First();
                RequestWokflowModel requestWokflowModel = _mapper.Map<RequestWokflowModel>(requestWorkflow);
                return requestWokflowModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public RequestWokflowModel GetRequestWorkflowById(long requestWorflowId)
        {
            try
            {
                RequestWorkflow requestWorkflow = _repository.Find(RW => RW.Id == requestWorflowId, false, RW => RW.BenefitRequest, RW => RW.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest.Employee, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType).First();
                RequestWokflowModel requestWokflowModel = _mapper.Map<RequestWokflowModel>(requestWorkflow);
                return requestWokflowModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<RequestWokflowModel> GetGroupRequestWorkflowByEmployeeNumber(long employeeNumber)
        {
            try
            {
                List<RequestWorkflow> requestWorkflows = _repository.Find(RW => RW.EmployeeId == employeeNumber && RW.IsVisible == true, false, RW => RW.Employee, RW => RW.RequestStatus, RW => RW.BenefitRequest, RW => RW.BenefitRequest.Benefit, RW => RW.BenefitRequest.Benefit.BenefitType, RW => RW.BenefitRequest.Group, RW => RW.BenefitRequest.Group.RequestStatus, RW => RW.BenefitRequest.Employee, RW => RW.BenefitRequest.Employee.Department, RW => RW.BenefitRequest.Employee.Position, RW => RW.BenefitRequest.Employee.Company).ToList();
                List<RequestWokflowModel> requestWokflowModels = _mapper.Map<List<RequestWokflowModel>>(requestWorkflows);
                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }



        public async Task<ManageRequest> CreateManageRequestFilter(string userId, ManageRequest manageRequest)
        {
            try
            {
                //ManageRequest manageRequest = new ManageRequest();
                manageRequest.DepartmentModels = new List<DepartmentAPI>();
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(userId).Result;
                UserModel userModel = await _userService.GetUser(userId);
                AspNetUser user = _mapper.Map<AspNetUser>(userModel);
                List<string> userRoles = _userManager.GetRolesAsync(user).Result.ToList();
                RequestFilterModel requestFilterModel = new RequestFilterModel();
                if (userRoles != null)
                {
                    manageRequest.BenefitTypeModels = CommanData.BenefitTypeModels;
                    manageRequest.RequestStatusModels = CommanData.whoIsConcernRequestStatusModels;
                    //manageRequest.TimingModels = CommanData.timingModels;
                    manageRequest.SelectedDepartmentId = -1;
                    manageRequest.SelectedRequestStatus = -1;
                    manageRequest.SelectedTimingId = -1;
                    manageRequest.SelectedBenefitType = -1;
                    manageRequest.employeeNumberSearch = 0;
                    //manageRequest.SearchDateFrom = DateTime.Today;
                    //manageRequest.SearchDateTo = DateTime.Today;
                    //manageRequest.BenefitTypeModels.Insert(0, new BenefitTypeModel { Id = -2, Name = "Select Type" });
                    if (userRoles.Contains("Admin"))
                    {
                        List<DepartmentModel> departmentModels = _departmentService.GetAllDepartments().ToList();
                        foreach (var dept in departmentModels)
                        {
                            DepartmentAPI departmentAPI = new DepartmentAPI();
                            departmentAPI.Id = dept.Id;
                            departmentAPI.Name = dept.Name;
                            manageRequest.DepartmentModels.Add(departmentAPI);
                        }
                        manageRequest.DepartmentModels.Insert(0, new DepartmentAPI { Id = -1, Name = "Department" });

                        //manageRequest.DepartmentModels = _departmentService.GetAllDepartments();
                        //manageRequest.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Department" });
                    }

                }
                return manageRequest;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<Request> CreateRequestToApprove(List<RequestWokflowModel> requestWokflowModels, int languageId)
        {
            try
            {
                int ListCount = requestWokflowModels.Count;
                List<Request> requestsToApprove = new List<Request>();
                string filePath = "";
                string document64 = "";
                string base64ImageRepresentation = "";
                byte[] imageArray;
                string uploadsFolder = "";
                string[] docs;
                List<string> docs64 = new List<string>();
                if (requestWokflowModels.Count != 0)
                {
                    for (int index = 0; index < requestWokflowModels.Count; index++)
                    {
                        Request requestToApprove1 = new Request();
                        switch (languageId)
                        {
                            case (int)CommanData.Languages.Arabic:
                                requestToApprove1.BenefitName = requestWokflowModels[index].BenefitRequest.Benefit.ArabicName;
                                requestToApprove1.BenefitType = requestWokflowModels[index].BenefitRequest.Benefit.BenefitType.ArabicName;

                                break;
                            case (int)CommanData.Languages.English:
                                requestToApprove1.BenefitName = requestWokflowModels[index].BenefitRequest.Benefit.Name;
                                requestToApprove1.BenefitType = requestWokflowModels[index].BenefitRequest.Benefit.BenefitType.Name;
                                break;
                        }
                        requestToApprove1.benefitId = requestWokflowModels[index].BenefitRequest.BenefitId;
                        requestToApprove1.RequestNumber = requestWokflowModels[index].BenefitRequestId;
                        requestToApprove1.From = requestWokflowModels[index].BenefitRequest.ExpectedDateFrom.ToString("yyyy-MM-dd");
                        requestToApprove1.To = requestWokflowModels[index].BenefitRequest.ExpectedDateTo.ToString("yyyy-MM-dd");
                        requestToApprove1.Requestedat = requestWokflowModels[index].BenefitRequest.CreatedDate;
                        requestToApprove1.Message = requestWokflowModels[index].BenefitRequest.Message;
                        requestToApprove1.status = CommanData.RequestStatusModels.Where(s => s.Id == requestWokflowModels[index].BenefitRequest.RequestStatusId).First().Name;
                        requestToApprove1.BenefitCard = requestWokflowModels[index].BenefitRequest.Benefit.BenefitCard;
                        requestToApprove1.BenefitCardAPI = CommanData.Url + CommanData.CardsFolder + requestWokflowModels[index].BenefitRequest.Benefit.BenefitCard;
                        requestToApprove1.HasDocuments = requestWokflowModels[index].HasDocuments;
                        requestToApprove1.UserCanResponse = requestWokflowModels[index].canResponse;
                        requestToApprove1.RequestWorkflowId = requestWokflowModels[index].Id;
                        if (requestWokflowModels[index].BenefitRequest.WarningMessage != null)
                        {
                            requestToApprove1.WarningMessage = requestWokflowModels[index].BenefitRequest.WarningMessage;
                        }
                        var documents = _requestDocumentService.GetRequestDocuments(requestWokflowModels[index].BenefitRequestId);
                        if (documents.Count > 0)
                        {
                            //requestToApprove1.Documents = documents.Select(d => d.fileName).ToArray();
                            requestToApprove1.HasDocuments = true;
                            //}
                            //if (requestWokflowModels[index].Documents != null)
                            //{
                            requestToApprove1.DocumentsPath = documents.Select(d => d.fileName).ToArray();

                            for (int DocumentsPathIndex = 0; DocumentsPathIndex < requestToApprove1.DocumentsPath.Length; DocumentsPathIndex++)
                            {
                                requestToApprove1.DocumentsPath[DocumentsPathIndex] = CommanData.Url + CommanData.DocumentsFolder + requestToApprove1.DocumentsPath[DocumentsPathIndex];
                            }
                            //uploadsFolder = Path.Combine(CommanData.UploadMainFolder, CommanData.DocumentsFolder);

                            //for(int documentIndex =0; documentIndex< documents.Count; documentIndex++)
                            //{
                            //    filePath = Path.Combine(uploadsFolder, documents[documentIndex].fileName);
                            //    imageArray = System.IO.File.ReadAllBytes(filePath);
                            //    base64ImageRepresentation = Convert.ToBase64String(imageArray);
                            //    if (base64ImageRepresentation != "")
                            //    {
                            //        docs64.Add(base64ImageRepresentation);
                            //    }

                            //}
                            //requestToApprove1.Documents = docs64.ToArray();
                            requestToApprove1.Documents = null;
                        }

                        List<EmployeeModel> employeeModels1 = new List<EmployeeModel>();
                        employeeModels1.Add(requestWokflowModels[index].BenefitRequest.Employee);
                        requestToApprove1.CreatedBy = CreateEmployeeData(employeeModels1).First();
                        if (requestWokflowModels[index].BenefitRequest.Group != null)
                        {
                            requestToApprove1.GroupName = requestWokflowModels[index].BenefitRequest.Group.Name;
                            List<EmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupParticipants((long)requestWokflowModels[index].BenefitRequest.GroupId).Result.Select(eg => eg.Employee).ToList();
                            if (groupEmployeeModels != null)
                            {
                                List<LoginUser> employeesData = CreateEmployeeData(groupEmployeeModels);
                                //List<LoginUser> employeesData = new List<LoginUser>();
                                //foreach (var groupEmployeeModel in groupEmployeeModels)
                                //{
                                //    LoginUser employeeData = new LoginUser();
                                //    employeeData.EmployeeNumber = groupEmployeeModel.EmployeeNumber;
                                //    employeeData.EmployeeName = groupEmployeeModel.FullName;
                                //    employeeData.DepartmentName = groupEmployeeModel.Department.Name;
                                //    employeeData.PositionName = groupEmployeeModel.Position.Name;
                                //    employeeData.SapNumber = groupEmployeeModel.SapNumber;
                                //    employeeData.PhoneNumber = groupEmployeeModel.PhoneNumber;
                                //    employeeData.JoinDate = groupEmployeeModel.JoiningDate.ToString("yyyy-MM-dd");
                                //    employeeData.BirthDate = groupEmployeeModel.BirthDate.ToString("yyyy-MM-dd");
                                //    employeeData.Email = groupEmployeeModel.Email;
                                //    employeeData.Collar = CommanData.Collars.Where(c => c.Id == groupEmployeeModel.Collar).First().Name;
                                //    employeeData.Company = groupEmployeeModel.Company.Code;
                                //    employeeData.WorkDuration = CalculateWorkDuration(Convert.ToDateTime(employeeData.JoinDate));
                                //    employeesData.Add(employeeData);
                                //}
                                requestToApprove1.FullParticipantsData = employeesData;
                            }
                        }
                        if (requestWokflowModels[index].BenefitRequest.SendTo != 0)
                        {
                            requestToApprove1.SendToId = requestWokflowModels[index].BenefitRequest.SendTo;
                            EmployeeModel employeeModel = _EmployeeService.GetEmployee(requestWokflowModels[index].BenefitRequest.SendTo).Result;
                            List<EmployeeModel> employeeModels = new List<EmployeeModel>();
                            employeeModels.Add(employeeModel);
                            requestToApprove1.SendToModel = CreateEmployeeData(employeeModels).First();
                        }

                        if (requestWokflowModels[index].canResponse == false && (requestWokflowModels[index].RequestStatusId == (int)CommanData.BenefitStatus.Approved || requestWokflowModels[index].RequestStatusId == (int)CommanData.BenefitStatus.Rejected))
                        {
                            MyAction myAction = new MyAction();
                            myAction.action = Enum.GetName(typeof(CommanData.BenefitStatus), requestWokflowModels[index].RequestStatusId);
                            if (requestWokflowModels[index].Notes != null)
                            {
                                myAction.Notes = requestWokflowModels[index].Notes;
                            }
                            if (requestWokflowModels[index].WhoResponseId != null)
                            {
                                myAction.whoIsResponseName = _EmployeeService.GetEmployee((long)requestWokflowModels[index].WhoResponseId).Result.FullName;
                            }
                            myAction.ReplayDate = requestWokflowModels[index].ReplayDate;
                            requestToApprove1.MyAction = myAction;
                        }
                        requestsToApprove.Add(requestToApprove1);
                    }
                }
                requestsToApprove = requestsToApprove.OrderByDescending(r => r.Requestedat).ToList();
                return requestsToApprove;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<LoginUser> CreateEmployeeData(List<EmployeeModel> groupEmployeeModels)
        {
            try
            {
                List<LoginUser> employeesData = new List<LoginUser>();
                groupEmployeeModels.ForEach(eg => eg.Email = _userManager.FindByIdAsync(eg.UserId).Result.Email);
                foreach (var groupEmployeeModel in groupEmployeeModels)
                {
                    LoginUser employeeData = new LoginUser();
                    employeeData.UserNumber = groupEmployeeModel.EmployeeNumber;
                    employeeData.UserName = groupEmployeeModel.FullName;
                    employeeData.DepartmentName = groupEmployeeModel.Department.Name;
                    employeeData.PositionName = groupEmployeeModel.Position.Name;
                    employeeData.SapNumber = groupEmployeeModel.SapNumber;
                    employeeData.PhoneNumber = groupEmployeeModel.PhoneNumber;
                    employeeData.JoinDate = groupEmployeeModel.JoiningDate.ToString("yyyy-MM-dd");
                    employeeData.BirthDate = groupEmployeeModel.BirthDate.ToString("yyyy-MM-dd");
                    employeeData.Email = groupEmployeeModel.Email;
                    employeeData.Collar = CommanData.Collars.Where(c => c.Id == groupEmployeeModel.Collar).First().Name;
                    employeeData.Entity = groupEmployeeModel.Company.Code;
                    employeeData.WorkDuration = CalculateWorkDuration(Convert.ToDateTime(employeeData.JoinDate));
                    employeeData.Email = groupEmployeeModel.Email;
                    employeeData.SupervisorName = groupEmployeeModel.Supervisor.FullName;
                    employeeData.Address = groupEmployeeModel.Address;
                    employeeData.MaritalStatus = Enum.GetName(typeof(CommanData.MaritialStatus), groupEmployeeModel.MaritalStatus);
                    employeeData.Gender = Enum.GetName(typeof(CommanData.Gender), groupEmployeeModel.Gender);
                    employeeData.Nationality = groupEmployeeModel.Nationality.Name;
                    employeeData.ProfilePicture = groupEmployeeModel.ProfilePicture;
                    employeeData.ProfilePictureAPI = CommanData.Url + CommanData.ProfileFolder + groupEmployeeModel.ProfilePicture;

                    employeesData.Add(employeeData);
                }

                return employeesData;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        //public EmployeeData CreateEmployeeData(GroupEmployeeModel groupEmployeeModel)
        //{
        //    try
        //    {
        //        EmployeeData employeeData = new EmployeeData();
        //        employeeData.EmployeeNumber = groupEmployeeModel.EmployeeId;
        //        employeeData.EmployeeName = groupEmployeeModel.Employee.FullName;
        //        employeeData.DepartmentName = groupEmployeeModel.Employee.Department.Name;
        //        employeeData.PositionName = groupEmployeeModel.Employee.Position.Name;
        //        employeeData.SapNumber = groupEmployeeModel.Employee.SapNumber;
        //        employeeData.PhoneNumber = groupEmployeeModel.Employee.PhoneNumber;
        //        employeeData.JoinDate = groupEmployeeModel.Employee.JoiningDate.ToString("yyyy-MM-dd");
        //        employeeData.BirthDate = groupEmployeeModel.Employee.BirthDate.ToString("yyyy-MM-dd");
        //        employeeData.Email = groupEmployeeModel.Employee.Email;
        //        employeeData.Collar = CommanData.Collars[groupEmployeeModel.Employee.Collar].Name;
        //        employeeData.Company = groupEmployeeModel.Employee.Company.Code;

        //        return employeeData;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.ToString());
        //        return null;
        //    }
        //}

        public List<RequestWokflowModel> CreateWarningMessage(List<RequestWokflowModel> requestWokflowModels)
        {
            try
            {
                foreach (var request in requestWokflowModels)
                {
                    if (request.BenefitRequest.Benefit.DateToMatch != null)
                    {
                        if (request.BenefitRequest.Benefit.DateToMatch == "Birth Date")
                        {
                            if (request.BenefitRequest.ExpectedDateFrom.Day != request.BenefitRequest.Employee.BirthDate.Day || request.BenefitRequest.ExpectedDateFrom.Month != request.BenefitRequest.Employee.BirthDate.Month)
                            {
                                request.BenefitRequest.WarningMessage = "Employee Birth Date does not match requird Date";

                            }
                        }
                        if (request.BenefitRequest.Benefit.DateToMatch == "Join Date")
                        {
                            if (request.BenefitRequest.ExpectedDateFrom.Day != request.BenefitRequest.Employee.JoiningDate.Day || request.BenefitRequest.ExpectedDateFrom.Month != request.BenefitRequest.Employee.JoiningDate.Month)
                            {
                                request.BenefitRequest.WarningMessage = "Employee Join Date does not match requird Date";
                            }
                        }
                    }
                    else if (request.BenefitRequest.Benefit.CertainDate.HasValue)
                    {
                        if (request.BenefitRequest.ExpectedDateFrom.Day != request.BenefitRequest.Benefit.CertainDate.Value.Day || request.BenefitRequest.ExpectedDateFrom.Month != request.BenefitRequest.Benefit.CertainDate.Value.Month)
                        {
                            request.BenefitRequest.WarningMessage = "Required date does not match benefit date";
                        }
                    }
                }

                return requestWokflowModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<RequestWokflowModel> EmployeeCanResponse(List<RequestWokflowModel> requestWokflowModels)
        {
            bool canResponse = false;
            foreach (RequestWokflowModel employeeRequestWokflowModel in requestWokflowModels)
            {
                //if (employeeRequestWokflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending && (employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Today && employeeRequestWokflowModel.BenefitRequest.RequestStatusId == (int)CommanData.BenefitStatus.Pending || employeeRequestWokflowModel.BenefitRequest.RequestStatusId == (int)CommanData.BenefitStatus.InProgress))
                if (employeeRequestWokflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending)
                {
                    canResponse = true;
                }
                else
                {
                    canResponse = false;

                }

                employeeRequestWokflowModel.canResponse = canResponse;
            }
            return requestWokflowModels;
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

        public async Task<string> CancelMyRequest(long id)
        {
            bool cancelResult = false;
            string message = "";
            BenefitRequestModel DBBenefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
            if (DBBenefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending)
            {
                List<RequestWokflowModel> requestWokflowModel = await GetRequestWorkflow(id);
                if (requestWokflowModel.Count > 0)
                {
                    //RequestWokflowModel requestWokflowModel1 = requestWokflowModel.First();
                    DBBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                    foreach (var workflow in requestWokflowModel)
                    {
                        workflow.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                        cancelResult = _benefitRequestService.CancelBenefitRequest(DBBenefitRequestModel, workflow);
                    }
                }
                else
                {
                    DBBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                    cancelResult = _benefitRequestService.UpdateBenefitRequest(DBBenefitRequestModel).Result;
                }

                if (cancelResult == true)
                {
                    if (DBBenefitRequestModel.Benefit.BenefitTypeId != (int)CommanData.BenefitTypes.Group)
                    {
                        message = "Success Process";
                    }
                    else
                    {
                        var group = _groupService.GetGroup((long)DBBenefitRequestModel.GroupId);
                        group.RequestStatusId = (int)CommanData.BenefitStatus.Cancelled;
                        cancelResult = _groupService.UpdateGroup(group);
                        if (cancelResult == true)
                        {
                            message = "Success Process";
                        }
                        else
                        {
                            message = "Problem in Cancelling your Group";
                        }
                    }
                }
                else
                {
                    message = "Problem in Cancelling your request";
                }
            }
            else
            {
                message = "your request can not be Cancelled";
            }
            return message;
        }


        public async Task<List<Request>> GetMyBenefitRequests(long employeeNumber, long BenefitId, long benefitTypeId, int languageId)
        {
            try
            {
                List<BenefitRequestModel> benefitRequestModels = new List<BenefitRequestModel>();
                if (benefitTypeId == (int)CommanData.BenefitTypes.Individual)
                {
                    benefitRequestModels = _benefitRequestService.GetBenefitRequestByEmployeeId(employeeNumber, BenefitId).Result;

                }
                else if (benefitTypeId == (int)CommanData.BenefitTypes.Group)
                {
                    List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupsByEmployeeId(employeeNumber).Result;
                    List<GroupModel> groupModels = groupEmployeeModels.Where(g => g.Group.BenefitId == BenefitId).Select(g => g.Group).ToList();
                    foreach (GroupModel group in groupModels)
                    {
                        BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
                        benefitRequestModel = _benefitRequestService.GetBenefitRequestByGroupId(group.Id);
                        benefitRequestModel.CurrentMember = _EmployeeService.GetEmployee(employeeNumber).Result;
                        benefitRequestModels.Add(benefitRequestModel);
                    }
                }
                benefitRequestModels = benefitRequestModels.OrderByDescending(r => r.CreatedDate).ToList();
                List<Request> myRequests = new List<Request>();
                if (benefitRequestModels != null)
                {
                    string HRRoleId = _roleService.GetRoleByName("HR").Result.Id;
                    for (int index = 0; index < benefitRequestModels.Count; index++)
                    {
                        //if (model.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled)
                        //{
                        benefitRequestModels[index].RequestWokflowModels = await GetRequestWorkflow(benefitRequestModels[index].Id);
                        var HRWorkflow = benefitRequestModels[index].RequestWokflowModels.Where(rw => rw.RoleId == HRRoleId);
                        if (HRWorkflow.Any() && benefitRequestModels[index].RequestStatusId != (int)CommanData.BenefitStatus.Cancelled)
                        {
                            var HRmodel = benefitRequestModels[index].RequestWokflowModels.Where(rw => rw.RoleId.Contains(HRRoleId)).First();
                            HRmodel.Employee.FullName = "HR";
                            benefitRequestModels[index].RequestWokflowModels.RemoveAll(rw => rw.RoleId == HRRoleId);
                            benefitRequestModels[index].RequestWokflowModels.Insert(0, HRmodel);
                        }
                        //}
                        if (benefitRequestModels[index].RequestStatusId == (int)CommanData.BenefitStatus.Pending)
                        {
                            benefitRequestModels[index].CanCancel = true;
                            benefitRequestModels[index].CanEdit = true;
                        }
                        else
                        {
                            benefitRequestModels[index].CanCancel = false;
                            benefitRequestModels[index].CanEdit = false;
                        }
                    }
                    myRequests = CreateRequestToApprove(benefitRequestModels, employeeNumber, languageId);
                }
                return myRequests;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<Request> CreateRequestToApprove(List<BenefitRequestModel> benefitRequestModels, long employeeNumber, int languageId)
        {
            try
            {
                int ListCount = benefitRequestModels.Count;
                List<Request> requestsToApprove = new List<Request>();

                if (benefitRequestModels.Count != 0)
                {
                    for (int index = 0; index < benefitRequestModels.Count; index++)
                    {
                        Request requestToApprove1 = new Request();
                        requestToApprove1.benefitId = benefitRequestModels[index].BenefitId;
                        switch (languageId)
                        {
                            case (int)CommanData.Languages.Arabic:
                                requestToApprove1.BenefitName = benefitRequestModels[index].Benefit.ArabicName;
                                requestToApprove1.BenefitType = benefitRequestModels[index].Benefit.BenefitType.ArabicName;
                                break;
                            case (int)CommanData.Languages.English:
                                requestToApprove1.BenefitName = benefitRequestModels[index].Benefit.Name;
                                requestToApprove1.BenefitType = benefitRequestModels[index].Benefit.BenefitType.Name;
                                break;
                        }
                        //requestToApprove1.BenefitName = benefitRequestModels[index].Benefit.Name;
                        requestToApprove1.CanCancel = benefitRequestModels[index].CanCancel;
                        requestToApprove1.CanEdit = benefitRequestModels[index].CanEdit;
                        requestToApprove1.RequestNumber = benefitRequestModels[index].Id;
                        requestToApprove1.From = benefitRequestModels[index].ExpectedDateFrom.ToString("yyyy-MM-dd");
                        requestToApprove1.To = benefitRequestModels[index].ExpectedDateTo.ToString("yyyy-MM-dd");
                        requestToApprove1.Requestedat = benefitRequestModels[index].CreatedDate;
                        requestToApprove1.Message = benefitRequestModels[index].Message;
                        requestToApprove1.status = CommanData.RequestStatusModels.Where(s => s.Id == benefitRequestModels[index].RequestStatusId).First().Name;
                        requestToApprove1.BenefitCard = benefitRequestModels[index].Benefit.BenefitCard;
                        requestToApprove1.BenefitCardAPI = CommanData.Url + CommanData.CardsFolder + benefitRequestModels[index].Benefit.BenefitCard;

                        if (benefitRequestModels[index].WarningMessage != null)
                        {
                            requestToApprove1.WarningMessage = benefitRequestModels[index].WarningMessage;
                        }
                        List<EmployeeModel> employeeModels1 = new List<EmployeeModel>();
                        employeeModels1.Add(benefitRequestModels[index].Employee);
                        requestToApprove1.CreatedBy = CreateEmployeeData(employeeModels1).First();
                        if (benefitRequestModels[index].Group != null)
                        {
                            requestToApprove1.GroupName = benefitRequestModels[index].Group.Name;
                            List<EmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupParticipants((long)benefitRequestModels[index].GroupId).Result.Select(eg => eg.Employee).ToList();
                            groupEmployeeModels.Where(ge => ge.EmployeeNumber == benefitRequestModels[index].EmployeeId).First().IsTheGroupCreator = true;
                            if (groupEmployeeModels != null)
                            {
                                List<LoginUser> employeesData = CreateEmployeeData(groupEmployeeModels);
                                requestToApprove1.FullParticipantsData = employeesData;
                            }
                            List<EmployeeModel> employeeModels2 = new List<EmployeeModel>();
                            employeeModels2.Add(_EmployeeService.GetEmployee(employeeNumber).Result);
                            requestToApprove1.CreatedBy = CreateEmployeeData(employeeModels2).FirstOrDefault();
                        }
                        if (benefitRequestModels[index].SendTo != 0)
                        {
                            EmployeeModel employeeModel = _EmployeeService.GetEmployee(benefitRequestModels[index].SendTo).Result;
                            List<EmployeeModel> employeeModels = new List<EmployeeModel>();
                            employeeModels.Add(employeeModel);
                            requestToApprove1.SendToModel = CreateEmployeeData(employeeModels).First();
                        }
                        if (requestToApprove1.status != "Cancelled")
                        {
                            List<RequestWorkFlowAPI> requestWorkFlowAPIs = new List<RequestWorkFlowAPI>();
                            requestWorkFlowAPIs = AddWorkflowToRequest(benefitRequestModels[index]);
                            requestToApprove1.RequestWorkFlowAPIs = requestWorkFlowAPIs;
                        }

                        requestsToApprove.Add(requestToApprove1);
                    }
                    return requestsToApprove;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public RequestWorkFlowAPI CreateRequestWorkFlowAPI(RequestWokflowModel requestWokflowModel)
        {
            try
            {
                RequestWorkFlowAPI requestWorkFlowAPI = new RequestWorkFlowAPI();
                requestWorkFlowAPI.UserNumber = requestWokflowModel.EmployeeId;
                requestWorkFlowAPI.UserName = requestWokflowModel.Employee.FullName;
                requestWorkFlowAPI.UserCanResponse = requestWokflowModel.canResponse;
                if (requestWokflowModel.Notes != null)
                {
                    requestWorkFlowAPI.Notes = requestWokflowModel.Notes;
                }
                requestWorkFlowAPI.ReplayDate = requestWokflowModel.ReplayDate;
                requestWorkFlowAPI.Status = Enum.GetName(typeof(CommanData.BenefitStatus), requestWokflowModel.RequestStatusId);
                return requestWorkFlowAPI;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public RequestWorkFlowAPI CreateRequestWorkFlowAPIBasic(EmployeeModel employeeModel, string status)
        {
            try
            {

                RequestWorkFlowAPI requestWorkFlowAPI = new RequestWorkFlowAPI();
                requestWorkFlowAPI.UserNumber = employeeModel.EmployeeNumber;
                requestWorkFlowAPI.UserName = employeeModel.FullName;
                requestWorkFlowAPI.Status = status;
                requestWorkFlowAPI.ReplayDate = DateTime.Today.AddDays(1);
                return requestWorkFlowAPI;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public List<RequestWorkFlowAPI> AddWorkflowToRequest(BenefitRequestModel benefitRequestModel)
        {
            List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId).OrderBy(w => w.Order).ToList();
            List<RequestWorkFlowAPI> requestWorkFlowAPIs = new List<RequestWorkFlowAPI>();
            EmployeeModel employeeModel = new EmployeeModel();

            int start = 0;
            int index = 0;
            int length = 0;
            bool rearrange = false;
            int actualWorkflowCount = benefitWorkflowModels.Count;
            int requestWorkflowCount = benefitRequestModel.RequestWokflowModels.Count;

            if (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending ||
                benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.InProgress ||
                (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Approved) ||
                (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Rejected))
            {
                length = benefitRequestModel.RequestWokflowModels.Count;

            }
            else if (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Cancelled)
            {
                length = benefitRequestModel.RequestWokflowModels.Count - 1;
            }
            //else if (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Approved ||
            //    (benefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Rejected))
            //{
            //    length = benefitRequestModel.RequestWokflowModels.Count;
            //}

            for (index = 0; index < length; index++)
            {
                RequestWorkFlowAPI requestWorkFlowAPI = CreateRequestWorkFlowAPI(benefitRequestModel.RequestWokflowModels[index]);

                requestWorkFlowAPIs.Add(requestWorkFlowAPI);
            }
            start = index;

            for (int index2 = start; index2 < benefitWorkflowModels.Count; index2++)
            {
                string status = "Not Yet";
                string roleName = "";

                roleName = _roleService.GetRole(benefitWorkflowModels[index2].RoleId).Result.Name;
                if (roleName == "HR")
                {
                    employeeModel.EmployeeNumber = 0;
                    employeeModel.FullName = "HR";

                }
                else if (roleName == "Supervisor")
                {
                    employeeModel = _EmployeeService.GetEmployee((long)benefitRequestModel.Employee.SupervisorId).Result;
                }
                else if (roleName == "Department Manager")
                {
                    employeeModel = _EmployeeService.GetDepartmentManager(benefitRequestModel.Employee.DepartmentId);
                }
                else if (roleName == "Payroll")
                {
                    employeeModel = _EmployeeService.GetPayrollEmployee(benefitRequestModel.Employee.EmployeeNumber);
                }

                RequestWorkFlowAPI requestWorkFlowAPI = CreateRequestWorkFlowAPIBasic(employeeModel, status);
                requestWorkFlowAPIs.Add(requestWorkFlowAPI);
                rearrange = true;
            }
            if (rearrange == true)
            {
                requestWorkFlowAPIs = requestWorkFlowAPIs.OrderBy(r => r.ReplayDate).ToList();
            }
            return requestWorkFlowAPIs;
        }

        public async Task<string> AddIndividualRequest(Request request, string userId, BenefitModel benefitModel)
        {
            BenefitRequestModel NewBenefitRequestModel = new BenefitRequestModel();
            List<RequestDocumentModel> requestDocumentModels = new List<RequestDocumentModel>();
            EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(userId).Result;
            string result = "";
            if (benefitModel.IsAgift == true && request.SendToId != 0 || benefitModel.IsAgift == false && request.SendToId == 0)
            {
                bool canRedeem = _benefitRequestService.EmployeeCanRedeemthisbenefit(benefitModel, employeeModel);
                if (canRedeem == true)
                {
                    NewBenefitRequestModel.EmployeeId = employeeModel.EmployeeNumber;
                    NewBenefitRequestModel.Message = request.Message;
                    NewBenefitRequestModel.CreatedDate = DateTime.Now;
                    NewBenefitRequestModel.RequestDate = DateTime.Now;
                    NewBenefitRequestModel.UpdatedDate = DateTime.Now;
                    NewBenefitRequestModel.IsVisible = true;
                    NewBenefitRequestModel.IsDelted = false;
                    NewBenefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                    NewBenefitRequestModel.BenefitId = request.benefitId;
                    if (benefitModel.MustMatch == true)
                    {
                        RequestDates requestDates = CalculateRequestExactDates(benefitModel, employeeModel);
                        NewBenefitRequestModel.ExpectedDateFrom = Convert.ToDateTime(requestDates.From);
                        NewBenefitRequestModel.ExpectedDateTo = Convert.ToDateTime(requestDates.To);
                        request.To = requestDates.To;
                    }
                    else
                    {
                        NewBenefitRequestModel.ExpectedDateFrom = Convert.ToDateTime(request.From);
                    }

                    if (request.To == null)
                    {
                        NewBenefitRequestModel.ExpectedDateTo = Convert.ToDateTime(request.From);
                    }
                    else
                    {
                        NewBenefitRequestModel.ExpectedDateTo = Convert.ToDateTime(request.To);
                    }
                    if (benefitModel.RequiredDocuments != null)
                    {
                        if (request.Documents != null)
                        {
                            string filePath = "";
                            foreach (var file in request.Documents)
                            {
                                RequestDocumentModel requestDocumentModel = new RequestDocumentModel();

                                filePath = await SaveImage(file, CommanData.DocumentsFolder);
                                requestDocumentModel.fileName = filePath;
                                requestDocumentModel.FileType = "image";
                                requestDocumentModels.Add(requestDocumentModel);
                            }
                        }
                        else if (request.docs != null)
                        {
                            string filePath = "";
                            foreach (var file in request.docs)
                            {
                                RequestDocumentModel requestDocumentModel = new RequestDocumentModel();
                                //filePath = UploadedImageAsync(file, @"C:\inetpub\wwwroot\_more4u\wwwroot\BenefitRequestFiles").Result;
                                requestDocumentModel.fileName = file;
                                requestDocumentModel.FileType = "image";
                                requestDocumentModels.Add(requestDocumentModel);
                            }
                        }
                        else
                        {
                            requestDocumentModels = null;
                            result = "invalid data, you must add documents";
                            return result;
                        }
                    }
                    else
                    {
                        requestDocumentModels = null;
                    }

                    if (request.SendToId != 0)
                    {
                        NewBenefitRequestModel.SendTo = request.SendToId;
                    }
                    BenefitRequestModel addedRequest = await _benefitRequestService.CreateBenefitRequest(NewBenefitRequestModel);
                    if (addedRequest != null)
                    {
                        if (requestDocumentModels != null)
                        {
                            foreach (var document in requestDocumentModels)
                            {
                                document.BenefitRequestId = addedRequest.Id;
                                _requestDocumentService.CreateRequestDocument(document);
                            }
                        }

                        BenefitRequestModel newBenefitRequestModel = new BenefitRequestModel();
                        newBenefitRequestModel.Benefit = benefitModel;
                        if (newBenefitRequestModel.Benefit.HasWorkflow)
                        {
                            int orderNumber = 1;
                            result = await SendReuestToWhoIsConcern(addedRequest.Id, orderNumber);
                            if (result.Contains("successful Process"))
                            {
                                result = "Success Process, you added new request for " + benefitModel.Name
                                    + "your requested date from " + addedRequest.ExpectedDateFrom.ToString("yyyy-MM-dd")
                                    + "To " + addedRequest.ExpectedDateTo.ToString("yyyy-MM-dd");

                            }
                            else
                            {
                                result = "There is problem in workflow";
                            }
                        }
                        else
                        {
                            RequestWokflowModel approvedBenefitRequestWorkflow = CreateRequestWorkflowForSystem(addedRequest);
                            if (approvedBenefitRequestWorkflow != null)
                            {
                                approvedBenefitRequestWorkflow = GetRequestWorkflowById(approvedBenefitRequestWorkflow.Id);
                                if (newBenefitRequestModel.Benefit.HasMails == true)
                                {
                                    BenefitRequestModel benefitRequest = _benefitRequestService.GetBenefitRequest(addedRequest.Id);
                                    _benefitMailService.SendToMailList(benefitRequest);
                                }
                                EmployeeModel systemEmployee = _EmployeeService.GetSystemEmployee().Result;
                                bool sendNotfyResult = SendNotification(addedRequest, approvedBenefitRequestWorkflow, "Response", systemEmployee.EmployeeNumber).Result;
                                result = "Success Process, you Got " + benefitModel.Name
                                    + " from " + addedRequest.ExpectedDateFrom.ToString("yyyy-MM-dd")
                                    + "To " + addedRequest.ExpectedDateTo.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                    else
                    {
                        result = "Sorry, your request can't be created";

                    }
                }
                else
                {
                    result = "Sorry, can not redeem to this benefit";

                }
            }
            else
            {
                result = "invalid Data";
            }
            return result;
        }

        public async Task<string> SaveImage(string strm, string uploadFolder)
        {

            //this is a simple white background image
            var myfilename = string.Format(@"{0}", Guid.NewGuid());

            //Generate unique filename
            string uploadsFolder = Path.Combine(CommanData.UploadMainFolder, uploadFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + "newImg.jpg";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var bytess = Convert.FromBase64String(strm);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.WriteAsync(bytess, 0, bytess.Length);
                fileStream.Flush();
            }

            return uniqueFileName;
        }



        public async Task<string> UploadedImageAsync(IFormFile ImageName, string path)
        {
            string uniqueFileName = null;
            string filePath = null;

            if (ImageName != null)
            {
                string uploadsFolder = Path.Combine(CommanData.UploadMainFolder, path);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageName.FileName;
                uniqueFileName = uniqueFileName.Replace(" ", "");
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageName.CopyToAsync(fileStream);
                }
            }
            return uniqueFileName;
        }

        public async Task<string> SendReuestToWhoIsConcern(long benefitRequetId, int orderNumber)
        {
            try
            {
                string message = "";
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequetId);
                benefitRequestModel.Benefit.BenefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.Benefit.Id);
                if (benefitRequestModel.Benefit.BenefitWorkflowModels != null)
                {
                    BenefitWorkflowModel benefitWorkflowModel = benefitRequestModel.Benefit.BenefitWorkflowModels.Where(w => w.Order == orderNumber).First();
                    string roleName = _roleService.GetRole(benefitWorkflowModel.RoleId).Result.Name;
                    //EmployeeModel employeeWhoRequest = _EmployeeService.GetEmployee(benefitRequestModel.Employee.EmployeeNumber);
                    EmployeeModel whoIsConcern = new EmployeeModel();
                    if (roleName != null)
                    {
                        if (roleName == "Supervisor")
                        {
                            whoIsConcern = await _EmployeeService.GetEmployee((long)benefitRequestModel.Employee.SupervisorId);
                        }
                        if (roleName == "Payroll")
                        {
                            whoIsConcern = _EmployeeService.GetPayrollEmployee((long)benefitRequestModel.Employee.EmployeeNumber);
                        }
                        else if (roleName == "HR")
                        {
                            bool result = SendRequestToHRRole(benefitRequestModel);
                            if (result == true)
                            {
                                return "successful Process, your request will be proceed";
                            }
                            else
                            {
                                return "Problem in HR workflow";
                            }

                        }
                        else if (roleName == "Timing")
                        {
                            bool result = SendRequestToTimingRole(benefitRequestModel);
                            if (result == true)
                            {
                                return "successful Process, your request will be proceed";
                            }
                            else
                            {
                                return "Problem in HR workflow";
                            }

                        }
                        else if (roleName == "Department Manager")
                        {
                            DepartmentModel departmentModel = _departmentService.GetDepartment(benefitRequestModel.Employee.DepartmentId);
                            if (departmentModel != null)
                            {
                                whoIsConcern = _EmployeeService.GetDepartmentManager(departmentModel.Id);
                            }
                        }
                        if (whoIsConcern != null)
                        {
                            RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
                            requestWokflowModel.EmployeeId = whoIsConcern.EmployeeNumber;
                            requestWokflowModel.BenefitRequestId = benefitRequestModel.Id;
                            requestWokflowModel.RoleId = benefitWorkflowModel.RoleId;
                            requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                            requestWokflowModel.CreatedDate = DateTime.Now;
                            requestWokflowModel.UpdatedDate = DateTime.Now;
                            requestWokflowModel.IsDelted = false;
                            requestWokflowModel.IsVisible = true;
                            var requestWorkflow = CreateRequestWorkflow(requestWokflowModel);
                            if (requestWorkflow != null)
                            {
                                RequestWokflowModel requestWokflowModel1 = GetRequestWorkflowById(requestWorkflow.Result.Id);
                                message = "successful Process, your request will be proceed";
                                bool result = await SendMailToWhoIsConcern(benefitRequestModel, requestWokflowModel1);
                                result = SendNotification(benefitRequestModel, requestWokflowModel1, "Request", 0).Result;


                                // NotificationModel notificationModel = CreateNotification("Request", requestWokflowModel1);
                                //await SendToSpecificUser(requestWokflowModel1, "Request");
                            }
                            else
                            {
                                message = "Failed Process, failed to send it";
                            }
                        }
                        else
                        {
                            message = "Failed Process, There is a problem in this benefit";
                        }
                    }
                    else
                    {
                        message = "Failed Process, There is a problem in this benefit";
                    }
                }
                else
                {
                    message = "Failed Process, There is a problem in this benefit";
                }
                return message;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return "There is a problem, please contact with your Technical support";
            }
        }


        public async Task<string> ConfirmGroupRequest(Request request, string userId, BenefitModel benefitModel)
        {
            EmployeeModel CurrentEmployee = await _EmployeeService.GetEmployeeByUserId(userId);
            string[] insertedEmployeeNumbersString = new string[] { };
            if (request.SelectedUserNumbers != null)
            {
                string[] insertedEmployeeNumbersString2 = request.SelectedUserNumbers.Split(";");
                if (insertedEmployeeNumbersString2[insertedEmployeeNumbersString2.Length - 1] == "")
                {
                    insertedEmployeeNumbersString2[insertedEmployeeNumbersString2.Length - 1] = CurrentEmployee.EmployeeNumber.ToString();          //BenefitModel benefitModel = _BenefitService.GetBenefit(groupModel.BenefitRequestModel.Benefit.Id);
                    insertedEmployeeNumbersString = insertedEmployeeNumbersString2.ToArray();
                }
                else
                {
                    insertedEmployeeNumbersString = insertedEmployeeNumbersString2.Append(CurrentEmployee.EmployeeNumber.ToString()).ToArray();            //BenefitModel benefitModel = _BenefitService.GetBenefit(groupModel.BenefitRequestModel.Benefit.Id);
                }
            }
            else
            {
                string[] insertedEmployeeNumbersString2 = { CurrentEmployee.EmployeeNumber.ToString() };
                insertedEmployeeNumbersString = insertedEmployeeNumbersString2;
            }

            int GroupMembersCount = insertedEmployeeNumbersString.Length;
            bool result = true;
            string Message = "";
            GroupModel groupModel = new GroupModel();
            if (GroupMembersCount <= benefitModel.MaxParticipant && GroupMembersCount >= benefitModel.MinParticipant)
            {
                for (int index = 0; index < insertedEmployeeNumbersString.Length; index++)
                {
                    long candidateNumber = Convert.ToInt32(insertedEmployeeNumbersString[index]);
                    EmployeeModel candidateMember = await _EmployeeService.GetEmployee(candidateNumber);
                    result = _benefitRequestService.EmployeeCanRedeemthisbenefit(benefitModel, candidateMember);
                    if (result == false)
                    {
                        return "Can not create group, employee with number " + candidateNumber + "can not redeem";
                    }
                }
                groupModel.BenefitId = request.benefitId;
                groupModel.CreatedDate = DateTime.Now;
                groupModel.UpdatedDate = DateTime.Now;
                groupModel.IsDelted = false;
                groupModel.IsVisible = true;
                groupModel.CreatedBy = CurrentEmployee.Id;
                groupModel.ExpectedDateFrom = Convert.ToDateTime(request.From);
                groupModel.ExpectedDateTo = Convert.ToDateTime(request.To);
                groupModel.Message = request.Message;
                if (request.GroupName != null)
                {
                    groupModel.Name = request.GroupName;
                }
                else
                {
                    groupModel.Name = benefitModel.Name + " Group";
                }
                if (request.To == null)
                {
                    groupModel.ExpectedDateTo = Convert.ToDateTime(request.From);
                }
                if (GroupMembersCount == benefitModel.MaxParticipant)
                {
                    groupModel.GroupStatus = "Closed";
                    groupModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                }
                else if (GroupMembersCount >= benefitModel.MinParticipant)
                {
                    groupModel.GroupStatus = "Open";
                    groupModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                }
                groupModel.Benefit = null;
                groupModel.BenefitRequestModel = null;
                GroupModel newGroupModel = _groupService.CreateGroup(groupModel);
                if (newGroupModel != null)
                {
                    newGroupModel.Code = "G-" + DateTime.Today.ToString("yyyyMMdd") + newGroupModel.Id;
                    result = _groupService.UpdateGroup(newGroupModel);
                    GroupEmployeeModel groupEmployeeModel = new GroupEmployeeModel();
                    GroupEmployeeModel newGroupEmployeeModel = new GroupEmployeeModel();
                    if (result == true)
                    {
                        for (int index = 0; index < insertedEmployeeNumbersString.Length; index++)
                        {
                            long employeeNumber = long.Parse(insertedEmployeeNumbersString[index]);
                            EmployeeModel employeeMember = _EmployeeService.GetEmployee(employeeNumber).Result;
                            groupEmployeeModel.EmployeeId = employeeMember.EmployeeNumber;
                            groupEmployeeModel.GroupId = newGroupModel.Id;
                            groupEmployeeModel.JoinDate = DateTime.Now;
                            newGroupEmployeeModel = _groupEmployeeService.CreateGroupEmployee(groupEmployeeModel);
                        }
                    }
                    BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
                    benefitRequestModel.RequestDate = DateTime.Now;
                    benefitRequestModel.CreatedDate = DateTime.Now;
                    benefitRequestModel.UpdatedDate = DateTime.Now;
                    benefitRequestModel.IsDelted = false;
                    benefitRequestModel.IsVisible = true;
                    benefitRequestModel.Message = groupModel.Message;
                    benefitRequestModel.ExpectedDateFrom = groupModel.ExpectedDateFrom;
                    benefitRequestModel.ExpectedDateTo = groupModel.ExpectedDateTo;
                    benefitRequestModel.BenefitId = benefitModel.Id;
                    benefitRequestModel.GroupId = newGroupModel.Id;
                    benefitRequestModel.EmployeeId = CurrentEmployee.EmployeeNumber;
                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                    BenefitRequestModel newBenefitRequestModel = await _benefitRequestService.CreateBenefitRequest(benefitRequestModel);

                    if (newBenefitRequestModel != null)
                    {
                        BenefitRequestModel newAddedBenefitRequestModel = _benefitRequestService.GetBenefitRequest(newBenefitRequestModel.Id);
                        if (newAddedBenefitRequestModel.Benefit.HasWorkflow)
                        {
                            result = SendRequestToHRRole(newAddedBenefitRequestModel);
                            if (result == true)
                            {
                                Message = "Success Process, your request has been sent";

                            }
                            else
                            {
                                Message = "Problem in Workflow";
                            }
                        }
                        else
                        {
                            RequestWokflowModel approvedBenefitRequestWorkflow = CreateRequestWorkflowForSystem(newAddedBenefitRequestModel);
                            if (approvedBenefitRequestWorkflow != null)
                            {
                                List<string> groupMails = new List<string>();
                                approvedBenefitRequestWorkflow = GetRequestWorkflowById(approvedBenefitRequestWorkflow.Id);
                                newGroupModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                                bool updateGroupResult = _groupService.UpdateGroup(newGroupModel);
                                result = SendNotification(newAddedBenefitRequestModel, approvedBenefitRequestWorkflow, "CreateGroup", 0).Result;

                                result = SendNotification(newAddedBenefitRequestModel, approvedBenefitRequestWorkflow, "Response", 0).Result;

                                if (newAddedBenefitRequestModel.Benefit.HasMails == true)
                                {
                                    groupMails = _groupEmployeeService.GetGroupParticipantsMails((int)newAddedBenefitRequestModel.GroupId).Result;
                                    List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupParticipantsBasicData((int)newAddedBenefitRequestModel.GroupId).Result;
                                    _benefitMailService.SendToMailList(newAddedBenefitRequestModel, groupMails, groupEmployeeModels);
                                }
                                Message = "Success Process, you Got " + benefitModel.Name
                                    + " From " + newBenefitRequestModel.ExpectedDateFrom.ToString("yyyy-MM-dd")
                                    + "To " + newBenefitRequestModel.ExpectedDateTo.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                    else
                    {
                        Message = "Can not send Request";
                    }
                }
                else
                {
                    Message = "Can not create group";
                }
            }
            else
            {
                Message = "Failed Process, Group Members does not match";
            }


            return Message;
        }

        public bool SendRequestToHRRole(BenefitRequestModel benefitRequestModel)
        {
            try
            {
                EmployeeModel requesterEmployee = _EmployeeService.GetEmployee(benefitRequestModel.EmployeeId).Result;
                List<AspNetUser> HRUsers = _userManager.GetUsersInRoleAsync("HR").Result.ToList();
                RoleModel roleModel = _roleService.GetRoleByName("HR").Result;
                RequestWokflowModel newRequestWokflowModel = new RequestWokflowModel();
                bool result = false;
                foreach (AspNetUser user in HRUsers)
                {
                    EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(user.Id).Result;
                    if (employeeModel.Country == requesterEmployee.Country)
                    {
                        RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
                        requestWokflowModel.EmployeeId = employeeModel.EmployeeNumber;
                        requestWokflowModel.BenefitRequestId = benefitRequestModel.Id;
                        requestWokflowModel.RoleId = roleModel.Id;
                        requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
                        requestWokflowModel.CreatedDate = DateTime.Today;
                        requestWokflowModel.UpdatedDate = DateTime.Today;
                        requestWokflowModel.IsDelted = false;
                        requestWokflowModel.IsVisible = true;
                        newRequestWokflowModel = CreateRequestWorkflow(requestWokflowModel).Result;
                        benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                        if (newRequestWokflowModel != null)
                        {
                            newRequestWokflowModel = GetRequestWorkflowByEmployeeNumber(newRequestWokflowModel.EmployeeId, newRequestWokflowModel.BenefitRequestId);
                            // result = SendMailToWhoIsConcern(benefitRequestModel, newRequestWokflowModel);
                            result = SendNotification(benefitRequestModel, newRequestWokflowModel, "Request", 0).Result;

                        }
                    }
                }
                if (benefitRequestModel.Benefit.BenefitTypeId == (int)CommanData.BenefitTypes.Group)
                {
                    //result = SendMailToWhoIsConcern(benefitRequestModel, newRequestWokflowModel);
                    result = SendNotification(benefitRequestModel, newRequestWokflowModel, "CreateGroup", 0).Result;
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }

        public async Task<bool> SendNotification(BenefitRequestModel benefitRequestModel, RequestWokflowModel DBRequestWorkflowModel, string type, long responserId)
        {
            try
            {
                NotificationModel notificationModel = new NotificationModel();
                string notificationMessage = "";
                string arabicNotificationMessage = "";
                benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                if (type == "CreateGroup")
                {
                    var groupEmployeeModels = _groupEmployeeService.GetGroupParticipants((long)DBRequestWorkflowModel.BenefitRequest.GroupId).Result;
                    if (groupEmployeeModels.Count > 0)
                    {
                        notificationMessage = benefitRequestModel.Employee.FullName + " added you to new group for " + benefitRequestModel.Benefit.Name + " benefit";
                        arabicNotificationMessage = " اضافك " + benefitRequestModel.Employee.FullName + " في مجموعة جديدة لميزة " + benefitRequestModel.Benefit.ArabicName;
                        await SendNotificationToGroupMembers(groupEmployeeModels, DBRequestWorkflowModel, benefitRequestModel, notificationMessage, arabicNotificationMessage, type);
                    }

                }
                else
                {
                    DBRequestWorkflowModel.BenefitRequest = benefitRequestModel;

                    //if (benefitRequestModel.Benefit.BenefitTypeId == (int)CommanData.BenefitTypes.Individual)
                    //{
                    var token = "fSPSMCuzQ_au9r3unW_Xyz:APA91bENMQnmO1T_pK2Wjtx2IzIHP9KQGjSY7PUnoECoKao51pB8el-tukSWYQ0kl-yxiQwo1aTybsDJuOaL-Ml-dkvUVY244UxhYFEg4hTLfIiDFgbBzGC9STe213GLvsoxdZT_vTqn";
                    if (type == "Request")
                    {
                        notificationMessage = benefitRequestModel.Employee.FullName + " added new request for " + benefitRequestModel.Benefit.Name + " benefit";
                        arabicNotificationMessage = benefitRequestModel.Employee.FullName + "  اضاف طلب جديد لميزة" + benefitRequestModel.Benefit.ArabicName;
                        notificationModel = CreateNotification(type, DBRequestWorkflowModel.EmployeeId, benefitRequestModel.Id, notificationMessage, arabicNotificationMessage, 0, DBRequestWorkflowModel.Id);
                        await SendToSpecificUser(notificationMessage, DBRequestWorkflowModel, type, benefitRequestModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);
                        //var token = _EmployeeService.GetEmployee(DBRequestWorkflowModel.EmployeeId).Result.UserToken;
                        // var token = "fC5Zdfv-FExHg9eStk0oE2:APA91bEnZ4IXH5mcqz9A070LqQw5QP6E5KqKRbPsTUSEZ2EAmUFpN2rPHMkbAmr0sHlCdad3Aw5IliogqYLHQq_mXVBP81hbKJEtihbkh_kIGWUF8XsACUgPGjL7EqIgLkks7ymRuLMP";
                      //  await _firebaseNotificationService.SendNotification(token, "Request", notificationMessage);
                    }
                    if (type == "RequestCancel")
                    {
                        notificationMessage = benefitRequestModel.Employee.FullName + " cancelled his request for " + benefitRequestModel.Benefit.Name + " benefit";
                        arabicNotificationMessage = benefitRequestModel.Employee.FullName + " الغي طلبه لميزة " + benefitRequestModel.Benefit.ArabicName;
                        notificationModel = CreateNotification(type, DBRequestWorkflowModel.EmployeeId, benefitRequestModel.Id, notificationMessage, arabicNotificationMessage, 0, DBRequestWorkflowModel.Id);
                        await SendToSpecificUser(notificationMessage, DBRequestWorkflowModel, type, benefitRequestModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);
                        //var token = _EmployeeService.GetEmployee(DBRequestWorkflowModel.EmployeeId).Result.UserToken;
                        // var token = "eSpRbAdTR-W9meL0cKj-0i:APA91bFtNK3vz81OR9J0F4FOlp9q3jORkdhntx33aHi8NpEeSxMoAJ76_RngXKuH9klGEM9VHuNYl9khdOxFFkXLyCA8imZXCWwPglGhGvnio6gE_8fsHxpRK_-PqvjwQ3PaPRpn-oWB";
                       // await _firebaseNotificationService.SendNotification(token, "RequestCancel", notificationMessage);
                    }
                    else if (type == "Response")
                    {
                        benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);

                        if (DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Approved)
                        {
                            notificationMessage = DBRequestWorkflowModel.Employee.FullName + " Approved your request for " + benefitRequestModel.Benefit.Name + " benefit";
                            arabicNotificationMessage = " وافق " + DBRequestWorkflowModel.Employee.FullName + " علي طلبك لميزة " + benefitRequestModel.Benefit.ArabicName;
                            if (benefitRequestModel.SendTo != 0)
                            {
                                EmployeeModel employee = _EmployeeService.GetEmployee(benefitRequestModel.SendTo).Result;
                                RequestWokflowModel newRequestWokflowModel = DBRequestWorkflowModel;
                                newRequestWokflowModel.BenefitRequest.Employee = employee;
                                benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                                string giftMessage = benefitRequestModel.Employee.FullName + " Send a new gift to you from " + benefitRequestModel.Benefit.Name + " benefit";
                                string arabicGiftMessage = " أرسل " + benefitRequestModel.Employee.FullName + " اليك هدية من ميزة " + benefitRequestModel.Benefit.ArabicName;
                                notificationModel = CreateNotification("Gift", newRequestWokflowModel.BenefitRequest.SendTo, benefitRequestModel.Id, giftMessage, arabicGiftMessage, DBRequestWorkflowModel.EmployeeId, DBRequestWorkflowModel.Id);
                                await SendToSpecificUser(giftMessage, DBRequestWorkflowModel, "Gift", benefitRequestModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);
                                // var token = _EmployeeService.GetEmployee(newRequestWokflowModel.BenefitRequest.SendTo).Result.UserToken;
                               // await _firebaseNotificationService.SendNotification(token, "Gift", notificationMessage);
                            }
                        }
                        else
                        {
                            notificationMessage = DBRequestWorkflowModel.Employee.FullName + " Rejected your request for " + benefitRequestModel.Benefit.Name + " benefit";
                            arabicNotificationMessage = " رفض " + DBRequestWorkflowModel.Employee.FullName + " طلبك لميزة " + benefitRequestModel.Benefit.ArabicName;
                        }
                        if (benefitRequestModel.GroupId == null)
                        {
                            notificationModel = CreateNotification(type, benefitRequestModel.EmployeeId, benefitRequestModel.Id, notificationMessage, arabicNotificationMessage, DBRequestWorkflowModel.EmployeeId, DBRequestWorkflowModel.Id);
                            await SendToSpecificUser(notificationMessage, DBRequestWorkflowModel, type, DBRequestWorkflowModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);
                            //var token = _EmployeeService.GetEmployee(benefitRequestModel.EmployeeId).Result.UserToken;
                           // await _firebaseNotificationService.SendNotification(token, "Response", notificationMessage);
                        }
                        else
                        {
                            List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupParticipants((long)benefitRequestModel.GroupId).Result.ToList();

                            await SendNotificationToGroupMembers(groupEmployeeModels, DBRequestWorkflowModel, benefitRequestModel, notificationMessage, arabicNotificationMessage, type);
                        }
                    }
                    else if (type == "Take Action")
                    {
                        EmployeeModel responser = _EmployeeService.GetEmployee(responserId).Result;
                        if (DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Approved)
                        {
                            notificationMessage = responser.FullName + " Approved " + benefitRequestModel.Employee.FullName + "request for " + benefitRequestModel.Benefit.Name + " benefit";
                            arabicNotificationMessage = responser.FullName + " وافق علي الطلب المقدم من " + benefitRequestModel.Employee.FullName + " لميزة " + benefitRequestModel.Benefit.ArabicName;
                        }
                        else
                        {
                            notificationMessage = responser.FullName + " Rejected " + benefitRequestModel.Employee.FullName + "request for " + benefitRequestModel.Benefit.Name + " benefit";
                            arabicNotificationMessage = responser.FullName + " رفض الطلب المقدم من " + benefitRequestModel.Employee.FullName + " لميزة " + benefitRequestModel.Benefit.ArabicName;
                        }
                        notificationModel = CreateNotification(type, DBRequestWorkflowModel.EmployeeId, benefitRequestModel.Id, notificationMessage, arabicNotificationMessage, responser.EmployeeNumber, DBRequestWorkflowModel.Id);
                        await SendToSpecificUser(notificationMessage, DBRequestWorkflowModel, type, DBRequestWorkflowModel.Employee.FullName, DBRequestWorkflowModel.BenefitRequest.Employee.UserId);
                        //var token = _EmployeeService.GetEmployee(benefitRequestModel.EmployeeId).Result.UserToken;
                      //  await _firebaseNotificationService.SendNotification(token, "Response", notificationMessage);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }

        public async Task<bool> SendNotificationToGroupMembers(List<GroupEmployeeModel> groupEmployeeModels, RequestWokflowModel DBRequestWorkflowModel, BenefitRequestModel benefitRequestModel, string notificationMessage, string arabicNotificationMessage, string type)
        {
            try
            {
                NotificationModel notificationModel = null;
                string userId = benefitRequestModel.Employee.UserId;
                string token = "";
                foreach (GroupEmployeeModel groupEmployeeModel in groupEmployeeModels)
                {
                    token = _EmployeeService.GetEmployee(groupEmployeeModel.EmployeeId).Result.UserToken;
                    RequestWokflowModel newRequestWokflowModel = DBRequestWorkflowModel;
                    newRequestWokflowModel.BenefitRequest.Employee = groupEmployeeModel.Employee;
                    notificationModel = CreateNotification(type, groupEmployeeModel.Employee.EmployeeNumber, benefitRequestModel.Id, notificationMessage, arabicNotificationMessage, DBRequestWorkflowModel.Employee.EmployeeNumber, DBRequestWorkflowModel.Id);
                    await SendToSpecificUser(notificationMessage, newRequestWokflowModel, type, benefitRequestModel.Employee.FullName, userId);
                    await _firebaseNotificationService.SendNotification(type, notificationMessage, token);

                }
                if (notificationModel != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }

        }

        public NotificationModel CreateNotification(string Type, long employeeNumber, long BenefitRequestId, string message, string arabicMessage, long responsedBy, long requestWorkflowId)
        {
            NotificationModel notificationModel = new NotificationModel();
            notificationModel.IsDelted = false;
            notificationModel.IsVisible = true;
            notificationModel.UpdatedDate = DateTime.Now;
            notificationModel.CreatedDate = DateTime.Now;
            notificationModel.BenefitRequestId = BenefitRequestId;
            notificationModel.Type = Type;
            notificationModel.Message = message;
            notificationModel.ArabicMessage = arabicMessage;
            notificationModel.RequestWorkflowId = requestWorkflowId;
            if (Type == "Response" || Type == "Take Action")
            {
                notificationModel.ResponsedBy = responsedBy;
            }
            NotificationModel newNotificationModel = _notificationService.CreateNotification(notificationModel);

            if (newNotificationModel != null)
            {
                UserNotificationModel userNotificationModel = new UserNotificationModel();
                userNotificationModel.CreatedDate = DateTime.Now;
                userNotificationModel.UpdatedDate = DateTime.Now;
                userNotificationModel.EmployeeId = employeeNumber;

                //if (Type == "Response")
                //{
                //    userNotificationModel.EmployeeId = employeeNumber;
                //}
                //else if (Type == "Request")
                //{
                //    userNotificationModel.EmployeeId = employeeNumber;
                //}
                //else if(Type =="CreateGroup")
                //{
                //    userNotificationModel.EmployeeId = employeeNumber;
                //}
                userNotificationModel.NotificationId = newNotificationModel.Id;
                userNotificationModel.Seen = false;
                UserNotificationModel newUserotificationModel = _userNotificationService.CreateUserNotification(userNotificationModel);
            }
            return newNotificationModel;
        }


        public async Task<bool> SendToSpecificUser(string message, RequestWokflowModel model, string requestType, string employeeName, string userId)
        {

            var connections = _userConnectionManager.GetUserConnections(model.Employee.UserId);
            if (requestType == "Request" || requestType == "RequestCancel")
            {
                connections = _userConnectionManager.GetUserConnections(model.Employee.UserId);
            }
            else
            {
                connections = _userConnectionManager.GetUserConnections(model.BenefitRequest.Employee.UserId);
            }
            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    if (requestType == "Request")
                    {
                        await _hub.Clients.Client(connectionId).SendAsync("sendToUser", requestType, model.CreatedDate.Date.ToString("dd-MM-yyyy"), model.CreatedDate.ToShortTimeString(), model.BenefitRequest.Benefit.Id, message, employeeName, userId);
                    }
                    else if (requestType == "Response")
                    {
                        await _hub.Clients.Client(connectionId).SendAsync("sendToUser", requestType, model.CreatedDate.Date.ToString("dd-MM-yyyy"), model.CreatedDate.ToShortTimeString(), model.BenefitRequest.Benefit.Id, message, employeeName, userId);
                    }
                    else if (requestType == "CreateGroup")
                    {
                        await _hub.Clients.Client(connectionId).SendAsync("sendToUser", requestType, model.CreatedDate.Date.ToString("dd-MM-yyyy"), model.CreatedDate.ToShortTimeString(), model.BenefitRequest.Benefit.Id, message, employeeName, userId);
                    }
                }
            }
            return true;
        }

        public Task<string> AddDocumentsToRequest(long requestNumber, List<IFormFile> files)
        {
            string message = "";
            if (files.Count != 0)
            {

                string filePath = "";
                int count = 0;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        RequestDocumentModel requestDocumentModel = new RequestDocumentModel();
                        filePath = UploadedImageAsync(file, "~/BenefitRequestFiles").Result;
                        requestDocumentModel.fileName = filePath;
                        requestDocumentModel.FileType = file.ContentType;
                        requestDocumentModel.BenefitRequestId = requestNumber;
                        RequestDocumentModel requestDocument = _requestDocumentService.CreateRequestDocument(requestDocumentModel);
                        if (requestDocument != null)
                        {
                            count++;
                        }
                    }

                }
                if (count == files.Count)
                {
                    message = "Success Process, you upload " + count + "files";
                }
                else
                {
                    message = "failed Process";
                }

            }
            return Task<string>.FromResult(message);
        }


        public async Task<List<Gift>> GetMyGifts(long employeeNumber, int languageId)
        {
            List<Gift> myGifts = new List<Gift>();
            List<BenefitRequestModel> benefitRequestModels = _benefitRequestService.GetRequestsSendToMe(employeeNumber);
            if (benefitRequestModels.Count > 0)
            {
                foreach (var request in benefitRequestModels)
                {
                    List<RequestWokflowModel> requestWokflowModels = await GetRequestWorkflow(request.Id);
                    requestWokflowModels = requestWokflowModels.OrderByDescending(w => w.ReplayDate).ToList();
                    Gift gift = new Gift();
                    gift.RequestNumber = request.Id;
                    gift.BenefitCard = CommanData.Url + CommanData.CardsFolder + request.Benefit.BenefitCard;
                    switch (languageId)
                    {
                        case (int)CommanData.Languages.Arabic:
                            gift.BenefitName = request.Benefit.ArabicName;
                            break;
                        case (int)CommanData.Languages.English:
                            gift.BenefitName = request.Benefit.Name;
                            break;
                    }
                    //  gift.BenefitName = request.BenefitName;
                    gift.UserName = request.Employee.FullName;
                    gift.UserNumber = request.EmployeeId;
                    gift.UserDepartment = request.Employee.Department.Name;
                    //gift.EmployeeEmail = request.Employee.Email;
                    gift.Date = requestWokflowModels[0].ReplayDate;
                    myGifts.Add(gift);
                }
            }
            return myGifts;
        }


        public ManageRequest CreateManageRequestModel(RequestSearch requestSearch)
        {
            ManageRequest manageRequest = new ManageRequest();

            manageRequest.employeeNumberSearch = requestSearch.userNumberSearch;
            manageRequest.SelectedAll = requestSearch.SelectedAll;
            manageRequest.SelectedBenefitType = requestSearch.SelectedBenefitType;
            manageRequest.SelectedDepartmentId = requestSearch.SelectedDepartmentId;
            manageRequest.SelectedRequestStatus = requestSearch.SelectedRequestStatus;
            manageRequest.SelectedTimingId = requestSearch.SelectedTimingId;
            manageRequest.SearchDateFrom = requestSearch.SearchDateFrom;
            manageRequest.SearchDateTo = requestSearch.SearchDateTo;
            manageRequest.HasWarningMessage = requestSearch.HasWarningMessage;
            return manageRequest;
        }
        public async Task<bool> GroupRequestResponse(long groupId, int status, string message, long employeeNumber)
        {
            try
            {
                bool updateResult = false;
                bool requestWorkflowResult = false;
                EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber).Result;
                AspNetUser CurrentUser = await _userManager.FindByIdAsync(employeeModel.UserId);
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                GroupModel groupModel1 = _groupService.GetGroup(groupId);
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequestByGroupId((int)groupId);
                RequestWokflowModel requestWokflowModel = GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber, benefitRequestModel.Id);
                groupModel1.GroupStatus = "Closed";
                if (status == 2)
                {
                    groupModel1.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                    requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                }
                else if (status == 1)
                {
                    groupModel1.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                    requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                }
                requestWokflowModel.ReplayDate = DateTime.Now;
                requestWokflowModel.Notes = message;
                updateResult = await UpdateRequestWorkflow(requestWokflowModel);

                if (updateResult == true)
                {
                    updateResult = await _benefitRequestService.UpdateBenefitRequest(benefitRequestModel);

                    if (updateResult == true)
                    {
                        updateResult = _groupService.UpdateGroup(groupModel1);

                        if (updateResult == true)
                        {
                            List<RequestWokflowModel> requestWorkflowModels = await GetRequestWorkflow(benefitRequestModel.Id);
                            requestWorkflowModels = requestWorkflowModels.Where(rw => rw.EmployeeId != employeeModel.EmployeeNumber).ToList();
                            foreach (var requestWorkflow in requestWorkflowModels)
                            {
                                if (status == 2)
                                {
                                    requestWorkflow.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                                }
                                else if (status == 1)
                                {
                                    requestWorkflow.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                                }
                                requestWorkflow.ReplayDate = DateTime.Now;
                                requestWorkflow.Notes = message;
                                requestWorkflow.WhoResponseId = employeeNumber;
                                await UpdateRequestWorkflow(requestWorkflow);
                                updateResult = SendNotification(benefitRequestModel, requestWokflowModel, "Take Action", employeeNumber).Result;
                            }
                            updateResult = SendNotification(benefitRequestModel, requestWokflowModel, "Response", 0).Result;
                            List<string> groupMails = _groupEmployeeService.GetGroupParticipantsMails((int)benefitRequestModel.GroupId).Result;
                            _benefitMailService.SendToMailList(benefitRequestModel, groupMails);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return true;

        }

        public int CheckSameWorkflow(int Currentorder, long benefitId, RequestWokflowModel requestWokflowModel, long employeeNumber)
        {
            try
            {
                bool createResult = false;
                bool matched = false;
                int index = 0;
                var benefitWorkflows = _benefitWorkflowService.GetBenefitWorkflowS(benefitId);

                for (index = Currentorder; index <= benefitWorkflows.Count; index++)
                {
                    var CurrentRole = benefitWorkflows.Where(w => w.Order == index);
                    int nextOrder = index + 1;
                    if (nextOrder <= benefitWorkflows.Count)
                    {
                        var nextRoleId = benefitWorkflows.Where(w => w.Order == nextOrder).Select(w => w.RoleId).First();
                        string nextRoleName = _roleService.GetRole(nextRoleId).Result.Name;
                        EmployeeModel requestOwner = _EmployeeService.GetEmployee(employeeNumber).Result;
                        EmployeeModel currentWhoIsConcern = _EmployeeService.GetEmployee(requestWokflowModel.EmployeeId).Result;
                        long currentWhoIsConcernNumber = requestWokflowModel.EmployeeId;
                        EmployeeModel NextWhoIsConcern = new EmployeeModel();
                        if (nextRoleName != null)
                        {
                            if (nextRoleName == "Supervisor")
                            {
                                NextWhoIsConcern = requestOwner.Supervisor;
                            }
                            else if (nextRoleName == "HR")
                            {
                                List<AspNetUser> aspNetUsers = _userManager.GetUsersInRoleAsync("HR").Result.ToList();
                                List<EmployeeModel> HREmployeeModels = new List<EmployeeModel>();
                                foreach (var HRuser in aspNetUsers)
                                {
                                    EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(HRuser.Id).Result;
                                    if (employeeModel.Country == requestOwner.Country)
                                    {
                                        HREmployeeModels.Add(employeeModel);
                                    }
                                }
                                foreach (var HRemployee in HREmployeeModels)
                                {
                                    if (HRemployee.EmployeeNumber == currentWhoIsConcernNumber)
                                    {
                                        matched = true;
                                        createResult = CreateAutoRequestWorkflowForRole(nextRoleId, requestWokflowModel, HREmployeeModels);
                                        break;
                                    }

                                }
                            }
                            else if (nextRoleName == "Department Manager")
                            {
                                NextWhoIsConcern = _EmployeeService.GetDepartmentManager(requestOwner.Department.Id);
                            }
                            if (matched == false)
                            {
                                if (NextWhoIsConcern.EmployeeNumber == currentWhoIsConcernNumber)
                                {
                                    List<EmployeeModel> employeeModels = new List<EmployeeModel>();
                                    employeeModels.Add(NextWhoIsConcern);
                                    createResult = CreateAutoRequestWorkflowForRole(nextRoleId, requestWokflowModel, employeeModels);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                return index;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public bool CreateAutoRequestWorkflowForRole(string nextRoleId, RequestWokflowModel requestWokflowModel, List<EmployeeModel> employeeModels)
        {
            try
            {
                RequestWokflowModel addedRequestWokflowModel = new RequestWokflowModel();
                foreach (var employee in employeeModels)
                {
                    RequestWokflowModel newRequestWokflowModel = new RequestWokflowModel();
                    newRequestWokflowModel.EmployeeId = employee.EmployeeNumber;
                    newRequestWokflowModel.BenefitRequestId = requestWokflowModel.BenefitRequestId;
                    newRequestWokflowModel.RoleId = nextRoleId;
                    newRequestWokflowModel.RequestStatusId = requestWokflowModel.RequestStatusId;
                    newRequestWokflowModel.Notes = requestWokflowModel.Notes;
                    newRequestWokflowModel.ReplayDate = requestWokflowModel.ReplayDate;
                    newRequestWokflowModel.IsVisible = true;
                    newRequestWokflowModel.IsDelted = false;
                    newRequestWokflowModel.CreatedDate = DateTime.Now;
                    newRequestWokflowModel.UpdatedDate = DateTime.Now;
                    addedRequestWokflowModel = CreateRequestWorkflow(newRequestWokflowModel).Result;
                }
                if (addedRequestWokflowModel != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;

            }

        }

        public async Task<bool> AddSameResponseForAllHRRole(long requestNumber, int status, string message, long responsedBy)
        {
            try
            {
                bool updateResult = false;
                var requestWorkflowModels = await GetRequestWorkflow(requestNumber);
                string roleId = _roleService.GetRoleByName("HR").Result.Id;
                var HRPendingWorkflowRequests = requestWorkflowModels.Where(w => w.RoleId == roleId && w.RequestStatusId == (int)CommanData.BenefitStatus.Pending);
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(requestNumber);
                if (HRPendingWorkflowRequests != null)
                {
                    foreach (var HRPendingFlow in HRPendingWorkflowRequests.ToList())
                    {
                        if (status == 1)
                        {
                            HRPendingFlow.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                        }
                        else
                        {
                            HRPendingFlow.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;

                        }
                        HRPendingFlow.UpdatedDate = DateTime.Now;
                        HRPendingFlow.ReplayDate = DateTime.Now;
                        HRPendingFlow.Notes = message;
                        HRPendingFlow.WhoResponseId = responsedBy;
                        updateResult = UpdateRequestWorkflow(HRPendingFlow).Result;
                        bool result = SendNotification(benefitRequestModel, HRPendingFlow, "Take Action", responsedBy).Result;

                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public RequestDates CalculateRequestExactDates(BenefitModel benefitModel, EmployeeModel CurrentEmployee)
        {
            RequestDates requestDates = new RequestDates();
            if (benefitModel.DateToMatch == "Birth Date")
            {
                requestDates.From = new DateTime(DateTime.Today.Year, CurrentEmployee.BirthDate.Month, CurrentEmployee.BirthDate.Day).ToString("yyyy-MM-dd");
                requestDates.To = new DateTime(DateTime.Today.Year, CurrentEmployee.BirthDate.Month, CurrentEmployee.BirthDate.Day).ToString("yyyy-MM-dd");
            }
            else if (benefitModel.DateToMatch == "Join Date")
            {
                requestDates.From = new DateTime(DateTime.Today.Year, CurrentEmployee.JoiningDate.Month, CurrentEmployee.JoiningDate.Day).ToString("yyyy-MM-dd");
                requestDates.To = new DateTime(DateTime.Today.Year, CurrentEmployee.JoiningDate.Month, CurrentEmployee.JoiningDate.Day).ToString("yyyy-MM-dd");
            }
            else
            {
                requestDates.From = new DateTime(DateTime.Today.Year, benefitModel.CertainDate.Value.Month, benefitModel.CertainDate.Value.Day).ToString("yyyy-MM-dd");
                requestDates.To = new DateTime(DateTime.Today.Year, benefitModel.CertainDate.Value.Month, benefitModel.CertainDate.Value.Day).ToString("yyyy-MM-dd");
            }
            return requestDates;
        }


        public RequestWokflowModel CreateRequestWorkflowForSystem(BenefitRequestModel benefitRequestModel)
        {
            bool updateResult = false;
            RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
            RoleModel roleModel = _roleService.GetRoleByName("System").Result;
            // var systemUser = _userManager.GetUsersInRoleAsync("System").Result.FirstOrDefault();
            // EmployeeModel whoIsConcern = _EmployeeService.GetEmployeeByUserId(systemUser.Id).Result;
            EmployeeModel whoIsConcern = _EmployeeService.GetSystemEmployee().Result;
            requestWokflowModel.EmployeeId = whoIsConcern.EmployeeNumber;
            requestWokflowModel.BenefitRequestId = benefitRequestModel.Id;
            requestWokflowModel.RoleId = roleModel.Id;
            requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
            requestWokflowModel.CreatedDate = DateTime.Now;
            requestWokflowModel.UpdatedDate = DateTime.Now;
            requestWokflowModel.IsDelted = false;
            requestWokflowModel.IsVisible = true;
            requestWokflowModel.ReplayDate = DateTime.Now;
            var requestWorkflow = CreateRequestWorkflow(requestWokflowModel);
            if (requestWorkflow != null)
            {
                BenefitRequestModel benefitRequestModelToApprove = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                benefitRequestModelToApprove.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModelToApprove).Result;
                return requestWorkflow.Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SendMailToWhoIsConcern(BenefitRequestModel benefitRequestModel, RequestWokflowModel requestWokflowModel)
        {
            try
            {
                string whoIsConcernEmail = _employeeService.GetEmailOfEmployee(requestWokflowModel.Employee.EmployeeNumber);
                string employeeMail = _employeeService.GetEmailOfEmployee(benefitRequestModel.EmployeeId);
                bool result = false;
                if (!whoIsConcernEmail.Contains("NotProvided"))
                {
                    if (!employeeMail.Contains("NotProvided"))
                    {
                        result = await _outlookSenderService.SendMailForApproval(whoIsConcernEmail, benefitRequestModel.Benefit, benefitRequestModel.Employee, benefitRequestModel.Benefit.BenefitTypeId, employeeMail);
                    }
                    else
                    {
                        result = await _outlookSenderService.SendMailForApproval(whoIsConcernEmail, benefitRequestModel.Benefit, benefitRequestModel.Employee, benefitRequestModel.Benefit.BenefitTypeId, null);
                    }
                    return result;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Timing Role
        /// </summary>
        /// <param name="benefitRequestModel"></param>
        /// <returns></returns>
        public bool SendRequestToTimingRole(BenefitRequestModel benefitRequestModel)
        {
            try
            {
                EmployeeModel result1 = _EmployeeService.GetEmployee(benefitRequestModel.EmployeeId).Result;
                List<AspNetUser> list = _userManager.GetUsersInRoleAsync("Timing").Result.ToList<AspNetUser>();
                RoleModel result2 = _roleService.GetRoleByName("Timing").Result;
                RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
                foreach (IdentityUser<string> identityUser in list)
                {
                    EmployeeModel result3 = _EmployeeService.GetEmployeeByUserId(identityUser.Id).Result;
                    requestWokflowModel =  CreateRequestWorkflow(new RequestWokflowModel()
                    {
                        EmployeeId = result3.EmployeeNumber,
                        BenefitRequestId = benefitRequestModel.Id,
                        RoleId = result2.Id,
                        RequestStatusId = 1,
                        CreatedDate = DateTime.Today,
                        UpdatedDate = DateTime.Today,
                        IsDelted = false,
                        IsVisible = true
                    }).Result;
                    benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                    if (requestWokflowModel != null)
                    {
                        requestWokflowModel = this.GetRequestWorkflowByEmployeeNumber(requestWokflowModel.EmployeeId, requestWokflowModel.BenefitRequestId);
                        this.SendMailToWhoIsConcern(benefitRequestModel, requestWokflowModel);
                        int num = this.SendNotification(benefitRequestModel, requestWokflowModel, "Request", 0L).Result ? 1 : 0;
                    }
                }
                if (benefitRequestModel.Benefit.BenefitTypeId == 3L)
                {
                    SendMailToWhoIsConcern(benefitRequestModel, requestWokflowModel);
                    int num = SendNotification(benefitRequestModel, requestWokflowModel, "CreateGroup", 0L).Result ? 1 : 0;
                }
                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
                return false;
            }
        }

        public async Task<bool> AddSameResponseForAllTimingRole(long requestNumber, int status, string message, long responsedBy)
        {
            try
            {
                List<RequestWokflowModel> requestWorkflow = await GetRequestWorkflow(requestNumber);
                string roleId = _roleService.GetRoleByName("Timing").Result.Id;
                Func<RequestWokflowModel, bool> predicate = (Func<RequestWokflowModel, bool>)(w => w.RoleId == roleId && w.RequestStatusId == 1);
                IEnumerable<RequestWokflowModel> source = requestWorkflow.Where<RequestWokflowModel>(predicate);
                BenefitRequestModel benefitRequest = _benefitRequestService.GetBenefitRequest(requestNumber);
                if (source != null)
                {
                    foreach (RequestWokflowModel requestWokflowModel in source.ToList<RequestWokflowModel>())
                    {
                        requestWokflowModel.RequestStatusId = status != 1 ? 4 : 3;
                        requestWokflowModel.UpdatedDate = DateTime.Now;
                        requestWokflowModel.ReplayDate = DateTime.Now;
                        requestWokflowModel.Notes = message;
                        requestWokflowModel.WhoResponseId = new long?(responsedBy);
                        int num1 = UpdateRequestWorkflow(requestWokflowModel).Result ? 1 : 0;
                        int num2 = SendNotification(benefitRequest, requestWokflowModel, "Take Action", responsedBy).Result ? 1 : 0;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
