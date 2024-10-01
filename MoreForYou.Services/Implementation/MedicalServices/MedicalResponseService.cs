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
          IRoleService roleService)
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
                    medicalRequest.ResponseMedicalEntity = new int?(Convert.ToInt32(model.medicalEntity));
                    medicalRequest.ResponseFeedback = model.feedback;
                    medicalRequest.ResponseReason = model.responseComment;
                    _repository.Update(medicalRequest);
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
                    contextTransaction.Commit();
                    return model.requestId;
                }
                catch (Exception ex)
                {
                    contextTransaction.Rollback();
                    return (string)null;
                }
            }
        }
    }
}
