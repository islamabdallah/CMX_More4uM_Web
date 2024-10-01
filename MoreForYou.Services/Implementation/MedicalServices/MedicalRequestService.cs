using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models.MasterModels.MedicalModels;
using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.hub;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using MoreForYou.Services.Models.Medical;
using MoreForYou.Services.Models;
using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoreForYou.Services.Implementation.MedicalServices
{
    public class MedicalRequestService : IMedicalRequestService
    {
        private readonly
#nullable disable
   IRepository<MedicalRequest, long> _repository;
        private readonly IRepository<MedicalRequestLog, long> _logrepository;
        private readonly IRepository<MedicalAttachment, long> _attachmentrepository;
        private readonly IRepository<MedicalRequestType, long> _typerepository;
        private readonly IRepository<Relative, long> _employeeRepository;
        private readonly IRepository<MedicalItem, long> _itemsRepository;
        private readonly ILogger<MedicalRequestService> _logger;
        private readonly IMapper _mapper;
        private readonly IRequestWorkflowService _requestWorkflowService;
        public readonly IEmployeeService _employeeService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public APPDBContext _context;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IMedicalCategoryService _medicalCategoryService;
        private readonly IMedicalSubCategoryService _medicalSubCategoryService;
        private readonly IMedicalDetailsService _medicalDetailsService;

        public MedicalRequestService(
          IRepository<MedicalRequest, long> RequestRepository,
          ILogger<MedicalRequestService> logger,
          IMapper mapper,
          IRepository<MedicalRequestLog, long> logrepository,
          IRepository<MedicalAttachment, long> attachmentrepository,
          IRepository<Relative, long> employeeRepository,
          IRequestWorkflowService requestWorkflowService,
          IRepository<MedicalRequestType, long> typerepository,
          IRepository<MedicalItem, long> itemsRepository,
          IEmployeeService employeeService,
          APPDBContext context,
          UserManager<AspNetUser> userManager,
          IUserService userService,
          IHubContext<NotificationHub> hub,
          INotificationService notificationService,
          IUserNotificationService userNotificationService,
          IUserConnectionManager userConnectionManager,
          IRoleService roleService,
          IMedicalCategoryService medicalCategoryService,
          IMedicalSubCategoryService medicalSubCategoryService,
          IMedicalDetailsService medicalDetailsService)
        {
           _repository = RequestRepository;
            _logger = logger;
            _mapper = mapper;
            _logrepository = logrepository;
            _itemsRepository = itemsRepository;
            _attachmentrepository = attachmentrepository;
            _requestWorkflowService = requestWorkflowService;
            _typerepository = typerepository;
            _employeeService = employeeService;
            _context = context;
            _roleService = roleService;
            _userManager = userManager;
            _userService = userService;
            _hub = hub;
            _notificationService = notificationService;
            _userConnectionManager = userConnectionManager;
            _userNotificationService = userNotificationService;
            _employeeRepository = employeeRepository;
            _medicalCategoryService = medicalCategoryService;
            _medicalSubCategoryService = medicalSubCategoryService;
            _medicalDetailsService = medicalDetailsService;
        }

        public async Task<MedicalRequest> CreateMedicalRequest(MedicalRequest model)
        {
            try
            {
                MedicalRequest medicalRequest = _repository.Add(model);
                return medicalRequest == null ? (MedicalRequest)null : medicalRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return (MedicalRequest)null;
        }

        public async Task<MedicalAttachment> CreateMedicalRequestAttachment(
          MedicalRequest requestAPI,
          List<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateMedicalRequestAttachmentt(
          MedicalRequest requestAPI,
          List<IFormFile> files)
        {
            string result = "";
            if (files.Count != 0)
            {
                int num = 0;
                foreach (IFormFile file in files)
                {
                    if (file.Length > 0L)
                    {
                        if (_attachmentrepository.Add(new MedicalAttachment()
                        {
                            Name = _requestWorkflowService.UploadedImageAsync(file, "MedicalRequestFiles").Result,
                            Type = file.ContentType,
                            MedicalRequestId = requestAPI.Id
                        }) != null)
                            ++num;
                    }
                }
                result = num != files.Count ? "failed Process" : "Success Process, you upload " + num.ToString() + nameof(files);
            }
            return Task.FromResult<string>(result);
        }

        public async Task<MedicalRequestLog> CreateMedicalRequestLog(MedicalRequest requestAPI)
        {
            MedicalRequestLog medicalRequestLog1 = new MedicalRequestLog();
            medicalRequestLog1.MedicalRequestId = requestAPI.Id;
            medicalRequestLog1.Status = "Pending";
            medicalRequestLog1.CreatedBy = requestAPI.CreatedBy;
            medicalRequestLog1.IsActive = true;
            medicalRequestLog1.CreatedDate = DateTime.Now;
            medicalRequestLog1.UpdatedDate = DateTime.Now;
            medicalRequestLog1.IsVisible = true;
            medicalRequestLog1.IsDelted = false;
            try
            {
                MedicalRequestLog medicalRequestLog2 = _logrepository.Add(medicalRequestLog1);
                return medicalRequestLog2 == null ? (MedicalRequestLog)null : medicalRequestLog2;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return (MedicalRequestLog)null;
        }

        public async Task<MedicalRequest> CreateMedicalRequestModelAsync(
          MedicalRequestApiModel requestAPI)
        {
            MedicalRequest requestModel = new MedicalRequest();
            requestModel.MedicalRequestTypeId = requestAPI.requestType;
            requestModel.CreatedBy = (await _employeeService.GetEmployeeBySapNumber(Convert.ToInt64(requestAPI.createdBy))).UserId;
            requestModel.RequestedBy = (long)requestAPI.requestedBy;
            requestModel.RequestedFor = requestAPI.requestedFor;
            if (requestAPI.selfRequest)
            {
                Relative relative = _employeeRepository.Find((Expression<Func<Relative, bool>>)(x => x.EmployeeNumber == (long)requestAPI.requestedBy)).FirstOrDefault<Relative>();
                if (relative != null)
                    requestModel.RequestedFor = relative.Id;
            }
            if (requestAPI.requestType==1)
            {
                requestModel.MedicalPurpose = "Medication";
            }
            else
            {
                requestModel.MedicalPurpose = requestAPI.medicalPurpose;
            }
            requestModel.RequestDate = requestAPI.requestDate;
            requestModel.RequestMedicalEntity = requestAPI.medicalEntityId;
            
            requestModel.RequestComment = requestAPI.comment;
            requestModel.MonthlyMedication = requestAPI.monthlyMedication;
            requestModel.CreatedDate = DateTime.Now;
            requestModel.UpdatedDate = DateTime.Now;
            requestModel.IsVisible = true;
            requestModel.IsDelted = false;
            MedicalRequest requestModelAsync = requestModel;
            requestModel = (MedicalRequest)null;
            return requestModelAsync;
        }

        public PendingRequestApiModel GetAllMedicalRequestsByType(int TypeId, int langCode)
        {
            PendingRequestApiModel medicalRequestsByType = new PendingRequestApiModel();
            List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
            PendingRequestCount pendingRequestCount1 = new PendingRequestCount();
            List<MedicalRequestLog> list = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(i => i.Status == "Pending" && i.IsActive == true), false, (Expression<Func<MedicalRequestLog, object>>)(i => i.MedicalRequest)).ToList<MedicalRequestLog>();
            if (list == null || list.Count <= 0)
                return (PendingRequestApiModel)null;
            PendingRequestCount pendingRequestCount2 = pendingRequestCount1;
            int num1 = list.Count<MedicalRequestLog>();
            string str1 = num1.ToString();
            pendingRequestCount2.totalRequest = str1;
            PendingRequestCount pendingRequestCount3 = pendingRequestCount1;
            num1 = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(t => t.MedicalRequest.MedicalRequestTypeId == 1)).Count<MedicalRequestLog>();
            string str2 = num1.ToString();
            pendingRequestCount3.medications = str2;
            PendingRequestCount pendingRequestCount4 = pendingRequestCount1;
            num1 = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(t => t.MedicalRequest.MedicalRequestTypeId == 2)).Count<MedicalRequestLog>();
            string str3 = num1.ToString();
            pendingRequestCount4.checkups = str3;
            PendingRequestCount pendingRequestCount5 = pendingRequestCount1;
            num1 = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(t => t.MedicalRequest.MedicalRequestTypeId == 3)).Count<MedicalRequestLog>();
            string str4 = num1.ToString();
            pendingRequestCount5.sickleave = str4;
            if (TypeId == 1 || TypeId == 2 || TypeId == 3)
                list = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(t => t.MedicalRequest.MedicalRequestTypeId == TypeId)).ToList<MedicalRequestLog>();
            if (list != null && list.Count > 0)
            {
                foreach (MedicalRequestLog medicalRequestLog in list)
                {
                    MedicalRequestLog pendingRequest = medicalRequestLog;
                    PendingRequestSummeyModel requestSummeyModel1 = new PendingRequestSummeyModel();
                    EmployeeModel result = _employeeService.GetEmployeeBySapNumber(pendingRequest.MedicalRequest.RequestedBy).Result;
                    PendingRequestSummeyModel requestSummeyModel2 = requestSummeyModel1;
                    long num2 = pendingRequest.MedicalRequestId;
                    string str5 = num2.ToString();
                    requestSummeyModel2.requestID = str5;
                    requestSummeyModel1.employeeName = result.FullName;
                    PendingRequestSummeyModel requestSummeyModel3 = requestSummeyModel1;
                    num2 = pendingRequest.MedicalRequest.RequestedBy;
                    string str6 = num2.ToString();
                    requestSummeyModel3.employeeNumber = str6;
                    requestSummeyModel1.employeeImageUrl = CommanData.Url + CommanData.ProfileFolder + result.ProfilePicture;
                    requestSummeyModel1.requestDate = pendingRequest.MedicalRequest.RequestDate;
                    requestSummeyModel1.createdBy = _employeeService.GetEmployeeByUserId(pendingRequest.MedicalRequest.CreatedBy).Result.FullName;
                    PendingRequestSummeyModel requestSummeyModel4 = requestSummeyModel1;
                    num1 = pendingRequest.MedicalRequest.MedicalRequestTypeId;
                    string str7 = num1.ToString();
                    requestSummeyModel4.requestTypeID = str7;
                    requestSummeyModel1.requestStatus = pendingRequest.Status;
                    requestSummeyModel1.requestComment = pendingRequest.MedicalRequest.RequestComment;
                    Relative relative = _employeeRepository.Find((Expression<Func<Relative, bool>>)(i => i.Id == pendingRequest.MedicalRequest.RequestedFor)).FirstOrDefault<Relative>();
                    requestSummeyModel1.selfRequest = relative != null && relative.Relation == "Self";
                    requestSummeyModelList.Add(requestSummeyModel1);
                }
                medicalRequestsByType.requestsCount = pendingRequestCount1;
                medicalRequestsByType.requests = requestSummeyModelList;
                return medicalRequestsByType;
            }
            medicalRequestsByType.requestsCount = pendingRequestCount1;
            medicalRequestsByType.requests = new List<PendingRequestSummeyModel>();
            return medicalRequestsByType;
        }

        public bool SendRequestToMedicalAdminRole(MedicalRequest model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendRequestToDoctorRoleAsync(MedicalRequest model)
        {
            try
            {
                EmployeeModel result1 = _employeeService.GetEmployee(model.RequestedBy).Result;
                List<AspNetUser> list = _userManager.GetUsersInRoleAsync("Doctor").Result.ToList<AspNetUser>();
                RoleModel result2 = _roleService.GetRoleByName("Doctor").Result;
                RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
                foreach (IdentityUser<string> identityUser in list)
                {
                   
                        EmployeeModel result3 = _employeeService.GetEmployeeByUserId(identityUser.Id).Result;
                        MedicalRequestType result4 = _typerepository.Find(i => i.Id == model.MedicalRequestTypeId).FirstOrDefaultAsync().Result;
                        NotificationModel model1 = new NotificationModel();
                        string str1 = "Request";
                        if (str1 == "Request")
                        {
                            string str2 = result1.FullName + " added new request for " + result4.Name;
                            string str3 = result1.FullName + "  اضاف طلب جديد " + result4.Name;
                            model1.IsDelted = false;
                            model1.IsVisible = true;
                            model1.UpdatedDate = DateTime.Now;
                            model1.CreatedDate = DateTime.Now;
                            model1.MedicalRequestId = new long?(model.Id);
                            model1.Type = str1;
                            model1.Message = str2;
                            model1.ArabicMessage = str3;
                            NotificationModel notification = _notificationService.CreateNotification(model1);
                            if (notification != null)
                                _userNotificationService.CreateUserNotification(new UserNotificationModel()
                                {
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now,
                                    EmployeeId = result3.EmployeeNumber,
                                    NotificationId = notification.Id,
                                    Seen = false
                                });
                            _userConnectionManager.GetUserConnections(result3.UserId);
                            List<string> stringList = str1 == "Request" || str1 == "RequestCancel" ? _userConnectionManager.GetUserConnections(result3.UserId) : _userConnectionManager.GetUserConnections(result1.UserId);
                            if (stringList != null && stringList.Count > 0)
                            {
                                foreach (string connectionId in stringList)
                                {
                                    if (str1 == "Request")
                                    {
                                      //  IClientProxy clientProxy = _hub.Clients.Client(connectionId);
                                        string str4 = str1;
                                        DateTime dateTime = model.CreatedDate.Date;
                                        string str5 = dateTime.ToString("dd-MM-yyyy");
                                        dateTime = model.CreatedDate;
                                        string shortTimeString = dateTime.ToShortTimeString();
                                        // ISSUE: variable of a boxed type
                                        long id = model.Id;
                                        string str6 = str2;
                                        string fullName = result3.FullName;
                                        string userId = result3.UserId;
                                       // CancellationToken cancellationToken = new CancellationToken();
                                        await _hub.Clients.Client(connectionId).SendAsync("sendToUser", (object)str4, (object)str5, (object)shortTimeString, (object)id, (object)str6, (object)fullName, (object)userId);
                                    }
                                }
                            }
                        }
                        return true;
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
            throw new NotImplementedException();
        }

        public MedicalRequest addMedicalRequest(MedicalRequest model, List<IFormFile> files)
        {
            using (IDbContextTransaction contextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    MedicalRequest model1 = _repository.Add(model);
                    if (files != null && files.Count != 0)
                    {
                        int num = 0;
                        foreach (IFormFile file in files)
                        {
                            if (file.Length > 0L)
                            {
                                if (_attachmentrepository.Add(new MedicalAttachment()
                                {
                                    Name = _requestWorkflowService.UploadedImageAsync(file, "MedicalRequestFiles").Result,
                                    Type = file.ContentType,
                                    MedicalRequestId = model1.Id,
                                    Status = "Request"
                                }) != null)
                                    ++num;
                            }
                        }
                    }
                    MedicalRequestLog medicalRequestLog = new MedicalRequestLog();
                    medicalRequestLog.MedicalRequestId = model1.Id;
                    medicalRequestLog.Status = "Pending";
                    medicalRequestLog.CreatedBy = model1.CreatedBy;
                    medicalRequestLog.IsActive = true;
                    medicalRequestLog.CreatedDate = DateTime.Now;
                    medicalRequestLog.UpdatedDate = DateTime.Now;
                    medicalRequestLog.IsVisible = true;
                    medicalRequestLog.IsDelted = false;
                    _logrepository.Add(medicalRequestLog);
                    contextTransaction.Commit();
                    SendRequestToDoctorRoleAsync(model1);
                    return model1;
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    return (MedicalRequest)null;
                }
            }
        }

        public MedicalRequestModel GetMedicalRequestsDetailsById(long MedicalRequestId, int langCode)
        {
            MedicalRequestLog Request = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(i => i.MedicalRequestId == MedicalRequestId && i.IsActive == true), false, (Expression<Func<MedicalRequestLog, object>>)(i => i.MedicalRequest)).FirstOrDefault<MedicalRequestLog>();
            if (Request == null)
                return (MedicalRequestModel)null;
            MedicalRequestModel requestsDetailsById = new MedicalRequestModel();
            requestsDetailsById.createdBy = _employeeService.GetEmployeeByUserId(Request.MedicalRequest.CreatedBy).Result.FullName;
            requestsDetailsById.requestedBy = _employeeService.GetEmployeeBySapNumber(Request.MedicalRequest.RequestedBy).Result.FullName;
            requestsDetailsById.requestedByNumber = _employeeService.GetEmployeeBySapNumber(Request.MedicalRequest.RequestedBy).Result.EmployeeNumber.ToString();
            Relative relative = _employeeRepository.Find((Expression<Func<Relative, bool>>)(i => i.Id == Request.MedicalRequest.RequestedFor)).FirstOrDefault<Relative>();
            requestsDetailsById.requestedFor = relative.Relatives;
            requestsDetailsById.requestType = Request.MedicalRequest.MedicalRequestTypeId;
            requestsDetailsById.requestDate = Request.MedicalRequest.RequestDate;
            requestsDetailsById.monthlyMedication = Request.MedicalRequest.MonthlyMedication;
            requestsDetailsById.attachment = GetRequestAttachmentsByStatus(MedicalRequestId, "Request");
            if (Request.MedicalRequest.RequestMedicalEntity.HasValue)
            {
                MedicalDetails medicalDetails = _context.MedicalDetails.Where<MedicalDetails>((Expression<Func<MedicalDetails, bool>>)(t => (long?)t.Id == (long?)Request.MedicalRequest.RequestMedicalEntity)).FirstOrDefault<MedicalDetails>();
                requestsDetailsById.MedicalEntity = medicalDetails.Name_EN;
                requestsDetailsById.MedicalEntityId = medicalDetails.Id.ToString();
            }
            requestsDetailsById.medicalPurpose = Request.MedicalRequest.MedicalPurpose;
            requestsDetailsById.comment = Request.MedicalRequest.RequestComment;
            if (relative != null)
            {
                if (relative.Relation == "Self")
                {
                    requestsDetailsById.selfRequest = true;
                    requestsDetailsById.employeeCoverage = relative.CoverPercentage;
                    requestsDetailsById.relativeCoverage = relative.CoverPercentage;
                    requestsDetailsById.relation = relative.Relation;
                    requestsDetailsById.order = relative.Order.ToString();
                }
                else
                {
                    requestsDetailsById.employeeCoverage = _employeeRepository.Find((Expression<Func<Relative, bool>>)(i => i.EmployeeNumber == Request.MedicalRequest.RequestedBy)).FirstOrDefault<Relative>().CoverPercentage;
                    requestsDetailsById.selfRequest = false;
                    requestsDetailsById.relativeCoverage = relative.CoverPercentage;
                    requestsDetailsById.relation = relative.Relation;
                    requestsDetailsById.order = relative.Order.ToString();
                }
            }
            else
                requestsDetailsById.selfRequest = false;
            return requestsDetailsById;
        }

        public async Task<MedicalRequestDetailsModel> GetMedicalRequestsDetailsAsync(
          long MedicalRequestId,
          long EmployeeNumber,
          int langCode)
        {
            MedicalRequestLog Request = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(i => i.MedicalRequestId == MedicalRequestId && i.IsActive == true)).FirstOrDefault<MedicalRequestLog>();
            if (Request == null)
                return (MedicalRequestDetailsModel)null;
            MedicalRequestDetailsModel medicalRequestDetailsModel = new MedicalRequestDetailsModel();
            medicalRequestDetailsModel.MedicalRequestId = MedicalRequestId.ToString();
            medicalRequestDetailsModel.RequestStatus = Request.Status;
            medicalRequestDetailsModel.MedicalRequest = GetMedicalRequestsDetailsById(MedicalRequestId, langCode);
            EmployeeModel result = _employeeService.GetEmployee(EmployeeNumber).Result;
            if (result != null)
            {
                 if (_userManager.GetRolesAsync(await _userManager.FindByIdAsync(result.UserId)).Result.ToList<string>().Contains("Doctor"))
                    medicalRequestDetailsModel.MedicalResponse = GetMedicalResponseForDoctor(MedicalRequestId, langCode);
                else if (Request.Status != "Pending")
                    medicalRequestDetailsModel.MedicalResponse = GetMedicalResponseForEmployee(MedicalRequestId, langCode);
                
               
            }
            return medicalRequestDetailsModel;
        }

        public MedicalResponseModel GetMedicalResponseForEmployee(long MedicalRequestId, int langCode)
        {
            MedicalRequestLog Request = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(i => i.MedicalRequestId == MedicalRequestId && i.IsActive == true), false, (Expression<Func<MedicalRequestLog, object>>)(i => i.MedicalRequest)).FirstOrDefault<MedicalRequestLog>();
            _context.MedicalResponses.Where<MedicalResponse>((Expression<Func<MedicalResponse, bool>>)(t => t.MedicalRequestId == MedicalRequestId)).ToList<MedicalResponse>();
            if (Request == null)
                return (MedicalResponseModel)null;
            MedicalResponseModel responseForEmployee = new MedicalResponseModel();
            responseForEmployee.createdBy = _employeeService.GetEmployeeByUserId(Request.CreatedBy).Result.FullName;
            responseForEmployee.responseDate = Request.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            responseForEmployee.attachment = GetRequestAttachmentsByStatus(MedicalRequestId, "Response");
            responseForEmployee.feedback = Request.MedicalRequest.ResponseFeedback;
            responseForEmployee.responseComment = Request.MedicalRequest.ResponseReason;
            responseForEmployee.medicalItems = new List<MedicalItemsAPIModel>();
            var responseList = _context.MedicalResponses.Where<MedicalResponse>((Expression<Func<MedicalResponse, bool>>)(t => t.MedicalRequestId == MedicalRequestId)).ToList<MedicalResponse>();

            if (responseList != null)
            {
                if(responseList.Count > 0)
                {
                    foreach (var item in responseList)
                    {
                        var medicin = _context.MedicalItems.Where(t => t.Id == item.MedicalItemId).FirstOrDefault();
                        if(medicin != null)
                        {
                            MedicalItemsAPIModel medicalItems = new MedicalItemsAPIModel()
                            {
                                itemId = medicin.Id.ToString(),
                                itemName = medicin.DrugName,
                                itemType = medicin.Type,
                                itemQuantity = item.Quantity.ToString(),
                                itemDose = medicin.Dose,
                                itemImage = CommanData.Url + CommanData.MedicalRequestFolder + "capsules_10895948.PNG"
                            };
                            responseForEmployee.medicalItems.Add(medicalItems);
                        }
                        
                    }

                }
            }
          
            if (Request.MedicalRequest.ResponseMedicalEntity.HasValue && Request.MedicalRequest.ResponseMedicalEntity!=0)
            {
                responseForEmployee.medicalEntity = _context.MedicalDetails.Where<MedicalDetails>((Expression<Func<MedicalDetails, bool>>)(t => (long?)t.Id == (long?)Request.MedicalRequest.ResponseMedicalEntity)).FirstOrDefault<MedicalDetails>().Name_EN;
                List<MedicalDetailsModel> list = _medicalDetailsService.GetMedicalDetailsForCountry("Assuit").Result.Where<MedicalDetailsModel>((Func<MedicalDetailsModel, bool>)(t =>
                {
                    long id = t.Id;
                    int? responseMedicalEntity = Request.MedicalRequest.ResponseMedicalEntity;
                    long? nullable = responseMedicalEntity.HasValue ? new long?((long)responseMedicalEntity.GetValueOrDefault()) : new long?();
                    long valueOrDefault = nullable.GetValueOrDefault();
                    return id == valueOrDefault & nullable.HasValue;
                })).ToList<MedicalDetailsModel>();
                if (list != null && list.Count > 0)
                    responseForEmployee.medicalEntities = _medicalDetailsService.ConvertMedicalDetailsModelToMedicalDetailsAPIModelWithId(list, langCode);
            }
      //      responseForEmployee.medicalItems = new List<MedicalItemsAPIModel>()
      //{
      //  new MedicalItemsAPIModel()
      //  {
      //    itemId = "1",
      //    itemName = "panadol",
      //    itemType = "taplet",
      //    itemQuantity = "1"
      //  },
      //  new MedicalItemsAPIModel()
      //  {
      //    itemId = "2",
      //    itemName = "kaphsid",
      //    itemType = "taplet",
      //    itemQuantity = "1"
      //  },
      //  new MedicalItemsAPIModel()
      //  {
      //    itemId = "3",
      //    itemName = "vitamin c",
      //    itemType = "taplet",
      //    itemQuantity = "1"
      //  },
      //  new MedicalItemsAPIModel()
      //  {
      //    itemId = "4",
      //    itemName = "otrivin",
      //    itemType = "drops",
      //    itemQuantity = "1"
      //  },
      //  new MedicalItemsAPIModel()
      //  {
      //    itemId = "5",
      //    itemName = "Ivyrospan",
      //    itemType = "syrup",
      //    itemQuantity = "1"
      //  },
      //  new MedicalItemsAPIModel()
      //  {
      //    itemId = "6",
      //    itemName = "adol",
      //    itemType = "syrup",
      //    itemQuantity = "1"
      //  }
      //};
            return responseForEmployee;
        }

        public MedicalResponseModel GetMedicalResponseForDoctor(long MedicalRequestId, int langCode)
        {
            MedicalRequestLog Request = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(i => i.MedicalRequestId == MedicalRequestId && i.IsActive == true), false, (Expression<Func<MedicalRequestLog, object>>)(i => i.MedicalRequest)).FirstOrDefault<MedicalRequestLog>();
            _context.MedicalResponses.Where<MedicalResponse>((Expression<Func<MedicalResponse, bool>>)(t => t.MedicalRequestId == MedicalRequestId)).ToList<MedicalResponse>();
            if (Request == null)
                return (MedicalResponseModel)null;
            MedicalResponseModel responseForDoctor = new MedicalResponseModel();
            if (Request.Status == "Pending")
            {
                List<MedicalDetailsModel> result = _medicalDetailsService.GetMedicalDetailsForCountry("Assiut").Result;
                long lineID = _medicalDetailsService.GetMedicalEntityById(Request.MedicalRequest.RequestMedicalEntity).Result.MedicalSubCategory.Id;
                List<MedicalDetailsModel> list = result.Where<MedicalDetailsModel>((Func<MedicalDetailsModel, bool>)(t => t.MedicalSubCategory.Id == lineID)).ToList<MedicalDetailsModel>();
                if (list != null && list.Count > 0)
                    responseForDoctor.medicalEntities = _medicalDetailsService.ConvertMedicalDetailsModelToMedicalDetailsAPIModelWithId(list, langCode);
                List<string> stringList = new List<string>()
        {
          "Any",
          "Not Applicable",
          "Empty",
          "Finished"
        };
                responseForDoctor.feedbackCollection = stringList;
                if (Request.MedicalRequest.MedicalRequestTypeId == 1)
                {
                    var itemList = _itemsRepository.Find(t => t.IsVisible == true).ToList();
                    responseForDoctor.medicalItems = new List<MedicalItemsAPIModel>();
                    if (itemList.Count > 0)
                    {
                        foreach (var item in itemList)
                        {
                            MedicalItemsAPIModel medical = new MedicalItemsAPIModel()
                            {
                                itemId = item.Id.ToString(),
                                itemName = item.DrugName,
                                itemType = item.Type,
                                itemQuantity ="0", //item.Quantity,
                                itemDose = item.Dose
                            };
                            responseForDoctor.medicalItems.Add(medical);
                        }
                    }
                }
          //          responseForDoctor.medicalItems = new List<MedicalItemsAPIModel>()
          //{
          //  new MedicalItemsAPIModel()
          //  {
          //    itemId = "1",
          //    itemName = "panadol",
          //    itemType = "taplet",
          //    itemQuantity = "1",
          //    itemDose="50mg/ml"
          //  },
          //  new MedicalItemsAPIModel()
          //  {
          //    itemId = "2",
          //    itemName = "kaphsid",
          //    itemType = "taplet",
          //    itemQuantity = "1",
          //    itemDose="50mg/ml"
          //  },
          //  new MedicalItemsAPIModel()
          //  {
          //    itemId = "6",
          //    itemName = "adol",
          //    itemType = "syrup",
          //    itemQuantity = "1",
          //    itemDose="50mg/ml"
          //  }
          //};
             
            }
            else
            {
                responseForDoctor.createdBy = _employeeService.GetEmployeeByUserId(Request.CreatedBy).Result.FullName;
                responseForDoctor.responseDate = Request.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                responseForDoctor.feedback = Request.MedicalRequest.ResponseFeedback;
                responseForDoctor.responseComment = Request.MedicalRequest.ResponseReason;
                responseForDoctor.attachment = GetRequestAttachmentsByStatus(MedicalRequestId, "Response");
                if (Request.MedicalRequest.ResponseMedicalEntity.HasValue && Request.MedicalRequest.ResponseMedicalEntity > 0)
                {
                    responseForDoctor.medicalEntity = _context.MedicalDetails.Where<MedicalDetails>((Expression<Func<MedicalDetails, bool>>)(t => (long?)t.Id == (long?)Request.MedicalRequest.ResponseMedicalEntity)).FirstOrDefault<MedicalDetails>().Name_EN;
                }
                    if (Request.MedicalRequest.ResponseMedicalEntity.HasValue && Request.MedicalRequest.ResponseMedicalEntity>0)
                {
                    responseForDoctor.medicalEntity = _context.MedicalDetails.Where<MedicalDetails>((Expression<Func<MedicalDetails, bool>>)(t => (long?)t.Id == (long?)Request.MedicalRequest.ResponseMedicalEntity)).FirstOrDefault<MedicalDetails>().Name_EN;
                    List<MedicalDetailsModel> list = _medicalDetailsService.GetMedicalDetailsForCountry("Assuit").Result.Where<MedicalDetailsModel>((Func<MedicalDetailsModel, bool>)(t =>
                    {
                        long id = t.Id;
                        int? responseMedicalEntity = Request.MedicalRequest.ResponseMedicalEntity;
                        long? nullable = responseMedicalEntity.HasValue ? new long?((long)responseMedicalEntity.GetValueOrDefault()) : new long?();
                        long valueOrDefault = nullable.GetValueOrDefault();
                        return id == valueOrDefault & nullable.HasValue;
                    })).ToList<MedicalDetailsModel>();
                    if (list != null && list.Count > 0)
                        responseForDoctor.medicalEntities = _medicalDetailsService.ConvertMedicalDetailsModelToMedicalDetailsAPIModelWithId(list, langCode);
                }
               // responseForDoctor.medicalItems = new List<MedicalItemsAPIModel>();
                if (Request.MedicalRequest.MedicalRequestTypeId == 1)
                {
                    //var itemList = _itemsRepository.Find(t => t.IsVisible == true).ToList();
                    responseForDoctor.medicalItems = new List<MedicalItemsAPIModel>();
                    var responseList = _context.MedicalResponses.Where<MedicalResponse>((Expression<Func<MedicalResponse, bool>>)(t => t.MedicalRequestId == MedicalRequestId)).ToList<MedicalResponse>();

                    if (responseList != null)
                    {
                        if (responseList.Count > 0)
                        {
                            foreach (var item in responseList)
                            {
                                var medicin = _context.MedicalItems.Where(t => t.Id == item.MedicalItemId).FirstOrDefault();
                                if (medicin != null)
                                {
                                    MedicalItemsAPIModel medicalItems = new MedicalItemsAPIModel()
                                    {
                                        itemId = medicin.Id.ToString(),
                                        itemName = medicin.DrugName,
                                        itemType = medicin.Type,
                                        itemQuantity = item.Quantity.ToString(),
                                        itemDose = medicin.Dose,
                                        itemImage = CommanData.Url + CommanData.MedicalRequestFolder + "capsules_10895948.PNG"
                                    };
                                    responseForDoctor.medicalItems.Add(medicalItems);
                                }

                            }

                        }
                    }

                }
            }
            return responseForDoctor;
        }

        public List<string> GetRequestAttachmentsByStatus(long requestId, string status)
        {
            List<string> attachmentsByStatus = new List<string>();
            List<MedicalAttachment> list = _context.MedicalAttachments.Where<MedicalAttachment>((Expression<Func<MedicalAttachment, bool>>)(t => t.MedicalRequestId == requestId && t.Status == status)).ToList<MedicalAttachment>();
            if (list == null || list.Count <= 0)
                return (List<string>)null;
            foreach (MedicalAttachment medicalAttachment in list)
            {
                string str = CommanData.Url + CommanData.MedicalRequestFolder + medicalAttachment.Name;
                attachmentsByStatus.Add(str);
            }
            return attachmentsByStatus;
        }

        public PendingRequestApiModel GetEmployeeMedicalRequestsBy(long EmployeeNumber, int langCode)
        {
            PendingRequestApiModel medicalRequestsBy = new PendingRequestApiModel();
            List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
            PendingRequestCount pendingRequestCount1 = new PendingRequestCount();
            List<MedicalRequestLog> list = _logrepository.Find((i => i.IsActive == true), false, (Expression<Func<MedicalRequestLog, object>>)(i => i.MedicalRequest)).ToList<MedicalRequestLog>().Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(t => t.MedicalRequest.RequestedBy == EmployeeNumber && t.Status == "Pending")).ToList<MedicalRequestLog>();
            if (list == null || list.Count <= 0)
                return (PendingRequestApiModel)null;
            PendingRequestCount pendingRequestCount2 = pendingRequestCount1;
            int num1 = list.Count<MedicalRequestLog>();
            string str1 = num1.ToString();
            pendingRequestCount2.totalRequest = str1;
            PendingRequestCount pendingRequestCount3 = pendingRequestCount1;
            num1 = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(t => t.MedicalRequest.MedicalRequestTypeId == 1)).Count<MedicalRequestLog>();
            string str2 = num1.ToString();
            pendingRequestCount3.medications = str2;
            PendingRequestCount pendingRequestCount4 = pendingRequestCount1;
            num1 = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(t => t.MedicalRequest.MedicalRequestTypeId == 2)).Count<MedicalRequestLog>();
            string str3 = num1.ToString();
            pendingRequestCount4.checkups = str3;
            PendingRequestCount pendingRequestCount5 = pendingRequestCount1;
            num1 = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(t => t.MedicalRequest.MedicalRequestTypeId == 3)).Count<MedicalRequestLog>();
            string str4 = num1.ToString();
            pendingRequestCount5.sickleave = str4;
            if (list != null && list.Count > 0)
            {
                foreach (MedicalRequestLog medicalRequestLog in list)
                {
                    MedicalRequestLog pendingRequest = medicalRequestLog;
                    PendingRequestSummeyModel requestSummeyModel1 = new PendingRequestSummeyModel();
                    EmployeeModel result1 = _employeeService.GetEmployeeBySapNumber(pendingRequest.MedicalRequest.RequestedBy).Result;
                    PendingRequestSummeyModel requestSummeyModel2 = requestSummeyModel1;
                    long num2 = pendingRequest.MedicalRequestId;
                    string str5 = num2.ToString();
                    requestSummeyModel2.requestID = str5;
                    requestSummeyModel1.employeeName = result1.FullName;
                    PendingRequestSummeyModel requestSummeyModel3 = requestSummeyModel1;
                    num2 = pendingRequest.MedicalRequest.RequestedBy;
                    string str6 = num2.ToString();
                    requestSummeyModel3.employeeNumber = str6;
                    requestSummeyModel1.employeeImageUrl = CommanData.Url + CommanData.ProfileFolder + result1.ProfilePicture;
                    requestSummeyModel1.requestDate = pendingRequest.MedicalRequest.RequestDate;
                    requestSummeyModel1.createdBy = _employeeService.GetEmployeeByUserId(pendingRequest.MedicalRequest.CreatedBy).Result.FullName;
                    PendingRequestSummeyModel requestSummeyModel4 = requestSummeyModel1;
                    num1 = pendingRequest.MedicalRequest.MedicalRequestTypeId;
                    string str7 = num1.ToString();
                    requestSummeyModel4.requestTypeID = str7;
                    requestSummeyModel1.requestComment = pendingRequest.MedicalRequest.RequestComment;
                    requestSummeyModel1.requestStatus = pendingRequest.Status;
                    Relative relative = _employeeRepository.Find((Expression<Func<Relative, bool>>)(i => i.Id == pendingRequest.MedicalRequest.RequestedFor)).FirstOrDefault<Relative>();
                    requestSummeyModel1.selfRequest = relative != null && relative.Relation == "Self";
                    MedicalDetailsModel result2 = _medicalDetailsService.GetMedicalEntityById(pendingRequest.MedicalRequest.RequestMedicalEntity).Result;
                    requestSummeyModel1.requestMedicalEntity = langCode != 1 ? result2.Name_AR : result2.Name_EN;
                    requestSummeyModelList.Add(requestSummeyModel1);
                }
                medicalRequestsBy.requestsCount = pendingRequestCount1;
                medicalRequestsBy.requests = requestSummeyModelList;
                return medicalRequestsBy;
            }
            medicalRequestsBy.requestsCount = pendingRequestCount1;
            medicalRequestsBy.requests = new List<PendingRequestSummeyModel>();
            return medicalRequestsBy;
        }

        public PendingRequestApiModel GetAllEmployeeMedicalRequests(MedicalRequestSearch searchModel)
        {
            PendingRequestApiModel employeeMedicalRequests = new PendingRequestApiModel();
            List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
            PendingRequestCount pendingRequestCount = new PendingRequestCount();
            List<MedicalRequestLog> medicalRequestLogList = new List<MedicalRequestLog>();
            List<MedicalRequestLog> list;
            if (searchModel.userNumberSearch != "" && searchModel.requestId != "")
                list = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(t => t.IsActive == true && t.MedicalRequest.RequestedBy == Convert.ToInt64(searchModel.userNumberSearch) && t.MedicalRequestId == Convert.ToInt64(searchModel.requestId)), false, (Expression<Func<MedicalRequestLog, object>>)(t => t.MedicalRequest)).ToList<MedicalRequestLog>();
            else if (searchModel.requestId != "")
                list = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(t => t.IsActive == true && t.MedicalRequestId == Convert.ToInt64(searchModel.requestId)), false, (Expression<Func<MedicalRequestLog, object>>)(t => t.MedicalRequest)).ToList<MedicalRequestLog>();
            else if (searchModel.userNumberSearch != "")
                list = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(t => t.IsActive == true && t.MedicalRequest.RequestedBy == Convert.ToInt64(searchModel.userNumberSearch)), false, (Expression<Func<MedicalRequestLog, object>>)(t => t.MedicalRequest)).ToList<MedicalRequestLog>();
            else
            return (PendingRequestApiModel)null;
            if (list == null || list.Count <= 0)
                return (PendingRequestApiModel)null;
            if (searchModel.selectedRequestType != "")
                list = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(a => a.MedicalRequest.MedicalRequestTypeId == Convert.ToInt32(searchModel.selectedRequestType))).ToList<MedicalRequestLog>();
            if (searchModel.selectedRequestStatus != "")
            {
                string statusStr = "";
                if (searchModel.selectedRequestStatus == "1")
                    statusStr = "Pending";
                else if (searchModel.selectedRequestStatus == "3")
                    statusStr = "Approved";
                else if (searchModel.selectedRequestStatus == "4")
                    statusStr = "Rejected";
                list = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(a => a.Status == statusStr)).ToList<MedicalRequestLog>();
            }
            if (searchModel.relativeId != "")
                list = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(a => (long)a.MedicalRequest.RequestedFor == Convert.ToInt64(searchModel.relativeId))).ToList<MedicalRequestLog>();
            if (searchModel.searchDateFrom != "")
                list = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(a =>
                {
                    DateTime dateTime = a.MedicalRequest.RequestDate;
                    DateTime date1 = dateTime.Date;
                    dateTime = Convert.ToDateTime(searchModel.searchDateFrom);
                    DateTime date2 = dateTime.Date;
                    return date1 >= date2;
                })).ToList<MedicalRequestLog>();
            if (searchModel.searchDateTo != "")
                list = list.Where<MedicalRequestLog>((Func<MedicalRequestLog, bool>)(a =>
                {
                    DateTime dateTime = a.MedicalRequest.RequestDate;
                    DateTime date3 = dateTime.Date;
                    dateTime = Convert.ToDateTime(searchModel.searchDateTo);
                    DateTime date4 = dateTime.Date;
                    return date3 <= date4;
                })).ToList<MedicalRequestLog>();
            pendingRequestCount.totalRequest = "0";
            pendingRequestCount.medications = "0";
            pendingRequestCount.checkups = "0";
            pendingRequestCount.sickleave = "0";
            if (list == null || list.Count <= 0)
                return (PendingRequestApiModel)null;
            if (list != null && list.Count > 20)
                list = list.Take<MedicalRequestLog>(20).ToList<MedicalRequestLog>();
            foreach (MedicalRequestLog medicalRequestLog in list)
            {
                MedicalRequestLog pendingRequest = medicalRequestLog;
                PendingRequestSummeyModel requestSummeyModel = new PendingRequestSummeyModel();
                EmployeeModel result1 = _employeeService.GetEmployeeBySapNumber(pendingRequest.MedicalRequest.RequestedBy).Result;
                requestSummeyModel.requestID = pendingRequest.MedicalRequestId.ToString();
                requestSummeyModel.employeeName = result1.FullName;
                requestSummeyModel.employeeNumber = pendingRequest.MedicalRequest.RequestedBy.ToString();
                requestSummeyModel.employeeImageUrl = CommanData.Url + CommanData.ProfileFolder + result1.ProfilePicture;
                requestSummeyModel.requestDate = pendingRequest.MedicalRequest.RequestDate;
                requestSummeyModel.createdBy = _employeeService.GetEmployeeByUserId(pendingRequest.MedicalRequest.CreatedBy).Result.FullName;
                requestSummeyModel.requestTypeID = pendingRequest.MedicalRequest.MedicalRequestTypeId.ToString();
                requestSummeyModel.requestComment = pendingRequest.MedicalRequest.RequestComment;
                requestSummeyModel.requestStatus = pendingRequest.Status;
                Relative relative = _employeeRepository.Find((Expression<Func<Relative, bool>>)(i => i.Id == pendingRequest.MedicalRequest.RequestedFor)).FirstOrDefault<Relative>();
                requestSummeyModel.selfRequest = relative != null && relative.Relation == "Self";
                MedicalDetailsModel result2 = _medicalDetailsService.GetMedicalEntityById(pendingRequest.MedicalRequest.RequestMedicalEntity).Result;
                requestSummeyModel.requestMedicalEntity = Convert.ToInt32(searchModel.languageId) != 1 ? result2.Name_AR : result2.Name_EN;
                requestSummeyModelList.Add(requestSummeyModel);
            }
            employeeMedicalRequests.requestsCount = pendingRequestCount;
            employeeMedicalRequests.requests = requestSummeyModelList;
            return employeeMedicalRequests;
        }

        public PendingRequestApiModel GetAllMedicalRequestsByAdmin(long EmployeeNumber, int langCode)
        {
            throw new NotImplementedException();
        }
    }
}
