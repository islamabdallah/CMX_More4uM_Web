using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.hub;
using MoreForYou.Services.Models.MaterModels;
using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MoreForYou.Models.Auth;
using MoreForYou.Services.Models.MasterModels;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.VariantTypes;

namespace MoreForYou.Services.Implementation.MedicalServices
{
    public class MedicalResponseService : IMedicalResponseService
    {
        private readonly IRepository<MedicalRequest, long> _repository;
        private readonly IRepository<MedicalRequestLog, long> _logrepository;
        private readonly IRepository<MedicalAttachment, long> _attachmentrepository;
        private readonly IRepository<MedicalRequestType, long> _typerepository;
        private readonly IRepository<MedicalResponse, long> _responserepository;
        private readonly IRepository<Relative, long> _employeeRepository;
        private readonly ILogger<MedicalRequestService> _logger;
        private readonly IMapper _mapper;
        public readonly IEmployeeService _employeeService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public APPDBContext _context;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IRequestWorkflowService _requestWorkflowService;
        private readonly IMedicalMailService _medicalMailService;
        private readonly IMedicalRequestService _medicalRequest;
        public MedicalResponseService(
          IRepository<MedicalRequest, long> RequestRepository,
          ILogger<MedicalRequestService> logger,
          IMapper mapper,
          IRepository<MedicalRequestLog, long> logrepository,
          IRepository<MedicalResponse, long> responserepository,
          IRepository<MedicalAttachment, long> attachmentrepository,
          IRepository<Relative, long> employeeRepository,
          IRepository<MedicalRequestType, long> typerepository,
          IEmployeeService employeeService,
          APPDBContext context,
          UserManager<AspNetUser> userManager,
          IUserService userService,
          IHubContext<NotificationHub> hub,
          IRequestWorkflowService requestWorkflowService,
          INotificationService notificationService,
          IUserNotificationService userNotificationService,
          IUserConnectionManager userConnectionManager,
          IRoleService roleService,
          IMedicalMailService medicalMailService,
          IMedicalRequestService medicalRequest)
        {
            _repository = RequestRepository;
            _logger = logger;
            _mapper = mapper;
            _logrepository = logrepository;
            _attachmentrepository = attachmentrepository;
            _responserepository = responserepository;
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
            _requestWorkflowService = requestWorkflowService;
            _medicalMailService = medicalMailService;
            _medicalRequest = medicalRequest;
        }

        public string addMedicalResponseAsync(MedicalResponseApiModel model)
        {
            using (IDbContextTransaction contextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    long RequestId = Convert.ToInt64(model.requestId);
                    int int32 = Convert.ToInt32(model.status);
                    string str1 = "Approved";
                    if (int32 == 4)
                    {
                         str1 = "Rejected";
                    }
                    MedicalRequestLog medicalRequestLog1 = _logrepository.Find((Expression<Func<MedicalRequestLog, bool>>)(i => i.MedicalRequestId == RequestId && i.IsActive == true), false, (Expression<Func<MedicalRequestLog, object>>)(i => i.MedicalRequest)).FirstOrDefault<MedicalRequestLog>();
                    if (medicalRequestLog1 == null || !(medicalRequestLog1.Status == "Pending"))
                        return (string)null;
                    medicalRequestLog1.IsActive = false;
                    medicalRequestLog1.UpdatedDate = DateTime.Now;
                    _logrepository.Update(medicalRequestLog1);
                    MedicalRequestLog medicalRequestLog2 = new MedicalRequestLog();
                    medicalRequestLog2.MedicalRequestId = RequestId;
                    medicalRequestLog2.Status = int32 != 3 ? "Rejected" : "Approved";
                    EmployeeModel result = _employeeService.GetEmployeeBySapNumber(Convert.ToInt64(model.createdBy)).Result;
                    medicalRequestLog2.CreatedBy = result.UserId;
                    medicalRequestLog2.IsActive = true;
                    medicalRequestLog2.CreatedDate = DateTime.Now;
                    medicalRequestLog2.UpdatedDate = DateTime.Now;
                    medicalRequestLog2.IsVisible = true;
                    medicalRequestLog2.IsDelted = false;
                    _logrepository.Add(medicalRequestLog2);
                    MedicalRequest medicalRequest = _repository.Find((Expression<Func<MedicalRequest, bool>>)(i => i.Id == RequestId && i.IsVisible == true)).FirstOrDefault<MedicalRequest>();
                    if (int32 != 4)
                    {
                        medicalRequest.ResponseMedicalEntity = new int?(Convert.ToInt32(model.medicalEntity));
                        medicalRequest.ResponseFeedback = model.feedback;
                    }
                    medicalRequest.ResponseReason = model.responseComment;
                    _repository.Update(medicalRequest);
                    if (int32 != 4)
                    {
                        if (model.attachment != null && model.attachment.Count != 0)
                        {
                            int num = 0;
                            foreach (IFormFile ImageName in model.attachment)
                            {
                                if (ImageName.Length > 0L)
                                {
                                    if (_attachmentrepository.Add(new MedicalAttachment()
                                    {
                                        Name = _requestWorkflowService.UploadedImageAsync(ImageName, "MedicalRequestFiles").Result,
                                        Type = ImageName.ContentType,
                                        MedicalRequestId = RequestId,
                                        Status = "Response"
                                    }) != null)
                                        ++num;
                                }
                            }
                        }
                        if (model.medicalItems != null && model.medicalItems.Count != 0)
                        {
                            foreach (MedicalItemsAPIModel medicalItem in model.medicalItems)
                            {
                                MedicalResponse medicalResponse = new MedicalResponse()
                                {
                                    MedicalRequestId = RequestId,
                                    MedicalItemId = new long?(Convert.ToInt64(medicalItem.itemId)),
                                    Quantity = new int?(Convert.ToInt32(medicalItem.itemQuantity)),
                                    DateFrom = new DateTime?(Convert.ToDateTime(medicalItem.itemDateFrom))
                                };
                                medicalResponse.DateFrom = new DateTime?(Convert.ToDateTime(medicalItem.itemDateTo));
                                _responserepository.Add(medicalResponse);
                            }
                        }
                    }
                    contextTransaction.Commit();

                    MedicalRequest medicalRequest2 = _repository.Find((Expression<Func<MedicalRequest, bool>>)(i => i.Id == RequestId && i.IsVisible == true)).FirstOrDefault<MedicalRequest>();
                    var testNotify = SendDoctorResponseToEmployeeAsync(medicalRequest2, result.EmployeeNumber,str1);
                    if (int32 != 4)
                    {
                        MedicalRequestDetailsModel detailsModel = _medicalRequest.GetMedicalRequestsDetailsAsync(medicalRequest2.Id, medicalRequest2.RequestedBy, 2).Result;
                        var testMail = _medicalMailService.SendToMailList(detailsModel);
                    }
                    return model.requestId;
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    return (string)null;
                }
            }
        }

        public async Task<bool> SendDoctorResponseToEmployeeAsync(MedicalRequest model, long empNumber, string status)
        {
            try
            {
                EmployeeModel result1 = _employeeService.GetEmployee(model.RequestedBy).Result;
                //EmployeeModel result1 = _employeeService.GetEmployee(empNumber).Result;
              //  List<AspNetUser> list = _userManager.GetUsersInRoleAsync("Doctor").Result.ToList<AspNetUser>();
              //  RoleModel result2 = _roleService.GetRoleByName("Doctor").Result;
                RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
               // foreach (IdentityUser<string> identityUser in list)
               // {

                    EmployeeModel result3 = _employeeService.GetEmployee(empNumber).Result;// _employeeService.GetEmployeeByUserId(identityUser.Id).Result;
                    MedicalRequestType result4 = _typerepository.Find(i => i.Id == model.MedicalRequestTypeId).FirstOrDefaultAsync().Result;
                   // MedicalRequestType result4 = _typerepository.Find(i => i.Id == 2).FirstOrDefaultAsync().Result;
                    NotificationModel model1 = new NotificationModel();
                    string str1 = "MedicalView";
                    if (str1 == "MedicalView")
                    {
                        string str2 = result3.FullName +" "+status+ " your request for " + result4.Name;
                        string str3 = result3.FullName +" "+status+"  علي طلبك ل " + result4.Name;
                        model1.IsDelted = false;
                        model1.IsVisible = true;
                        model1.UpdatedDate = DateTime.Now;
                        model1.CreatedDate = DateTime.Now;
                        model1.MedicalRequestId = Convert.ToInt64(model.Id);
                        model1.Type = str1;
                        model1.Message = str2;
                        model1.ArabicMessage = str3;
                        NotificationModel notification = _notificationService.CreateNotification(model1);
                        if (notification != null)
                            _userNotificationService.CreateUserNotification(new UserNotificationModel()
                            {
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                EmployeeId = result1.EmployeeNumber,
                                NotificationId = notification.Id,
                                Seen = false
                            });
                        var connectionsTest = _userConnectionManager.GetUserConnections(result3.UserId);
                        List<string> stringList = str1 == "MedicalView" || str1 == "RequestCancel" ? _userConnectionManager.GetUserConnections(result3.UserId) : _userConnectionManager.GetUserConnections(result1.UserId);
                        if (stringList != null && stringList.Count > 0)
                        {
                            foreach (string connectionId in stringList)
                            {
                                if (str1 == "Response")
                                {
                                    //  IClientProxy clientProxy = _hub.Clients.Client(connectionId);
                                    string str4 = str1;
                                    DateTime dateTime = DateTime.Now.Date; //model.CreatedDate.Date;
                                    string str5 = dateTime.ToString("dd-MM-yyyy");
                                    dateTime = DateTime.Now;//model.CreatedDate;
                                    string shortTimeString = dateTime.ToShortTimeString();
                                    // ISSUE: variable of a boxed type
                                    long id = Convert.ToInt64(model.Id);
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

                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
