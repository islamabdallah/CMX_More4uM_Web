//// Decompiled with JetBrains decompiler
//// Type: MoreForYou.Controllers.ReportController
//// Assembly: MoreForYou, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//// MVID: 0EBCA31A-8817-4355-82DC-811CC6A35CD4
//// Assembly location: D:\projects\publish_backup\_more4u\MoreForYou.dll

//using AutoMapper;
//using Data.Repository;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.CSharp.RuntimeBinder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using MoreForYou.Controllers.hub;
//using MoreForYou.Models.Models;
//using MoreForYou.Models.Models.MasterModels;
//using MoreForYou.Service.Contracts.Auth;
//using MoreForYou.Services.Contracts;
//using MoreForYou.Services.Contracts.Email;
//using MoreForYou.Services.Contracts.Medical;
//using MoreForYou.Services.Models;
//using MoreForYou.Services.Models.MasterModels;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Metrics;
//using System.IO;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Runtime.CompilerServices;
//using System.Text.Json;
//using System.Threading.Tasks;

//#nullable enable
//namespace MoreForYou.Controllers
//{
//    public class ReportController : Controller
//    {
//        private readonly IBenefitService _BenefitService;
//        private readonly IBenefitWorkflowService _benefitWorkflowService;
//        private readonly IUserService _userService;
//        private readonly IRoleService _roleService;
//        private readonly ILogger<ReportController> _logger;
//        private readonly UserManager<AspNetUser> _userManager;
//        private readonly IEmployeeService _EmployeeService;
//        private readonly IBenefitRequestService _benefitRequestService;
//        private readonly IRequestWorkflowService _requestWorkflowService;
//        private readonly IDepartmentService _departmentService;
//        private readonly IBenefitTypeService _benefitTypeService;
//        private readonly IRequestStatusService _requestStatusService;
//        private readonly IGroupService _groupService;
//        private readonly IWebHostEnvironment _hostEnvironment;
//        private readonly IGroupEmployeeService _groupEmployeeService;
//        private readonly IHubContext<NotificationHub> _hub;
//        private readonly INotificationService _notificationService;
//        private readonly IUserNotificationService _userNotificationService;
//        private readonly IUserConnectionManager _userConnectionManager;
//        private readonly IRequestDocumentService _requestDocumentService;
//        private readonly IFirebaseNotificationService _firebaseNotificationService;
//        private readonly SignInManager<AspNetUser> _signInManager;
//        private readonly IBenefitMailService _benefitMailService;
//        private readonly IMGraphMailService _mGraphMailService;
//        private readonly IMedicalCategoryService _medicalCategoryService;
//        private readonly IMedicalSubCategoryService _medicalSubCategoryService;
//        private readonly IMedicalDetailsService _medicalDetailsService;
//        private readonly IRepository<Benefit, long> _repository;
//        private readonly IMapper _mapper;
//        private readonly IWebHostEnvironment _environment;
//        public List<Country> CollarsSearch = new List<Country>()
//        {
//      new Country() { Id = 1, Name = "Assiut" },
//      new Country() { Id = 2, Name = "Cairo" }
//    };

//        public ReportController(
//          IBenefitService BenefitService,
//          IRepository<Benefit, long> repository,
//          IMapper mapper,
//          IBenefitWorkflowService BenefitWorkflowService,
//          IUserService userService,
//          IRoleService roleService,
//          ILogger<ReportController> logger,
//          UserManager<AspNetUser> userManager,
//          IEmployeeService EmployeeService,
//          IBenefitRequestService benefitRequestService,
//          IRequestWorkflowService requestWorkflowService,
//          IDepartmentService departmentService,
//          IBenefitTypeService benefitTypeService,
//          IRequestStatusService requestStatusService,
//          IGroupService groupService,
//          IWebHostEnvironment hostEnvironment,
//          IGroupEmployeeService groupEmployeeService,
//          IHubContext<NotificationHub> hub,
//          INotificationService notificationService,
//          IUserNotificationService userNotificationService,
//          IUserConnectionManager userConnectionManager,
//          IRequestDocumentService requestDocumentService,
//          IFirebaseNotificationService firebaseNotificationService,
//          IBenefitMailService benefitMailService,
//          SignInManager<AspNetUser> signInManager,
//          IMGraphMailService mGraphMailService,
//          IMedicalCategoryService medicalCategoryService,
//          IMedicalSubCategoryService medicalSubCategoryService,
//          IMedicalDetailsService medicalDetailsService,
//          IWebHostEnvironment environment)
//        {
//            this._repository = repository;
//            this._mapper = mapper;
//            this._BenefitService = BenefitService;
//            this._benefitWorkflowService = BenefitWorkflowService;
//            this._userService = userService;
//            this._roleService = roleService;
//            this._logger = logger;
//            this._userManager = userManager;
//            this._EmployeeService = EmployeeService;
//            this._benefitRequestService = benefitRequestService;
//            this._requestWorkflowService = requestWorkflowService;
//            this._departmentService = departmentService;
//            this._benefitTypeService = benefitTypeService;
//            this._requestStatusService = requestStatusService;
//            this._groupService = groupService;
//            this._hostEnvironment = hostEnvironment;
//            this._groupEmployeeService = groupEmployeeService;
//            this._hub = hub;
//            this._notificationService = notificationService;
//            this._userNotificationService = userNotificationService;
//            this._userConnectionManager = userConnectionManager;
//            this._requestDocumentService = requestDocumentService;
//            this._firebaseNotificationService = firebaseNotificationService;
//            this._signInManager = signInManager;
//            this._benefitMailService = benefitMailService;
//            this._mGraphMailService = mGraphMailService;
//            this._medicalCategoryService = medicalCategoryService;
//            this._medicalSubCategoryService = medicalSubCategoryService;
//            this._medicalDetailsService = medicalDetailsService;
//            this._environment = environment;
//        }

//        public IActionResult Index()
//        {
//            try
//            {
//                BenefitFilterModel filterModel = new BenefitFilterModel();
//                // ISSUE: reference to a compiler-generated field
//                if (ReportController.\u003C\u003Eo__32.\u003C\u003Ep__0 == null)
//        {
//                    // ISSUE: reference to a compiler-generated field
//                    ReportController.\u003C\u003Eo__32.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "benefitt", typeof(ReportController), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[2]
//                    {
//            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
//            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
//                    }));
//                }
//                // ISSUE: reference to a compiler-generated field
//                // ISSUE: reference to a compiler-generated field
//                object obj1 = ReportController.\u003C\u003Eo__32.\u003C\u003Ep__0.Target((CallSite)ReportController.\u003C\u003Eo__32.\u003C\u003Ep__0, this.ViewBag, "");
//                List <\u003C\u003Ef__AnonymousType14<long, string> > list = this._repository.Find((Expression<Func<Benefit, bool>>)(i => i.IsVisible == true), false, (Expression<Func<Benefit, object>>)(b => b.BenefitType)).Where<Benefit>((Expression<Func<Benefit, bool>>)(t => t.Country == filterModel.SelectedCountry)).Select(s => new
//                {
//                    Id = s.Id,
//                    VesselName = s.Name
//                }).ToList();
//                // ISSUE: reference to a compiler-generated field
//                if (ReportController.\u003C\u003Eo__32.\u003C\u003Ep__1 == null)
//        {
//                    // ISSUE: reference to a compiler-generated field
//                    ReportController.\u003C\u003Eo__32.\u003C\u003Ep__1 = CallSite < Func < CallSite, object, List<\u003C\u003Ef__AnonymousType14<long, string> >, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Vessel", typeof(ReportController), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[2]
//                    {
//            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
//            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
//                    }));
//                }
//                // ISSUE: reference to a compiler-generated field
//                // ISSUE: reference to a compiler-generated field
//                object obj2 = ReportController.\u003C\u003Eo__32.\u003C\u003Ep__1.Target((CallSite)ReportController.\u003C\u003Eo__32.\u003C\u003Ep__1, this.ViewBag, list);
//                return (IActionResult)this.View((object)filterModel);
//            }
//            catch (Exception ex)
//            {
//                return (IActionResult)this.RedirectToAction("ERROR404");
//            }
//        }

//        [HttpPost]
//        public async Task<ActionResult> Index(BenefitFilterModel filterModel)
//        {
//            ReportController reportController = this;
//            try
//            {
//                if (filterModel.SelectedCountry == null)
//                    return (ActionResult)reportController.View((object)filterModel);
//                BenefitFilterModel benefitFilterModel = filterModel;
//                benefitFilterModel.BenefitRequests = await reportController._BenefitService.BenefitSearch(filterModel);
//                benefitFilterModel = (BenefitFilterModel)null;
//                Benefit benefit = await reportController._repository.Find((Expression<Func<Benefit, bool>>)(b => (long?)b.Id == filterModel.SelectedBenefit && b.IsVisible == true), false, (Expression<Func<Benefit, object>>)(b => b.BenefitType)).FirstOrDefaultAsync<Benefit>();
//                List <\u003C\u003Ef__AnonymousType14<long, string> > list = reportController._repository.Find((Expression<Func<Benefit, bool>>)(i => i.IsVisible == true), false, (Expression<Func<Benefit, object>>)(b => b.BenefitType)).Where<Benefit>((Expression<Func<Benefit, bool>>)(t => t.Country == filterModel.SelectedCountry)).Select(s => new
//                {
//                    Id = s.Id,
//                    VesselName = s.Name
//                }).ToList();
//                // ISSUE: reference to a compiler-generated field
//                if (ReportController.\u003C\u003Eo__33.\u003C\u003Ep__0 == null)
//        {
//                    // ISSUE: reference to a compiler-generated field
//                    ReportController.\u003C\u003Eo__33.\u003C\u003Ep__0 = CallSite < Func < CallSite, object, List<\u003C\u003Ef__AnonymousType14<long, string> >, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Vessel", typeof(ReportController), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[2]
//                    {
//            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
//            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
//                    }));
//                }
//                // ISSUE: reference to a compiler-generated field
//                // ISSUE: reference to a compiler-generated field
//                object obj = ReportController.\u003C\u003Eo__33.\u003C\u003Ep__0.Target((CallSite)ReportController.\u003C\u003Eo__33.\u003C\u003Ep__0, reportController.ViewBag, list);
//                return (ActionResult)reportController.View((object)filterModel);
//            }
//            catch (Exception ex)
//            {
//                return (ActionResult)reportController.RedirectToAction("ERROR404");
//            }
//        }

//        [HttpPost]
//        public async Task<ActionResult> ExportExcel(BenefitFilterModel filterModel)
//        {
//            ReportController reportController = this;
//            try
//            {
//                if (string.IsNullOrEmpty(filterModel.SelectedCountry))
//                    return (ActionResult)reportController.RedirectToAction("Index", (object)filterModel);
//                BenefitFilterModel benefitFilterModel = filterModel;
//                benefitFilterModel.BenefitRequests = await reportController._BenefitService.BenefitSearch(filterModel);
//                benefitFilterModel = (BenefitFilterModel)null;
//                if (filterModel.BenefitRequests != null)
//                {
//                    MemoryStream excel = reportController._BenefitService.ExportBenefitsToExcel(filterModel.BenefitRequests);
//                    return (ActionResult)reportController.File(excel.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BenefitsRequests.xlsx");
//                }
//                reportController.TempData["Error"] = (object)"Error in file exporting, There is no vessels";
//                return (ActionResult)reportController.RedirectToAction("Index", (object)filterModel);
//            }
//            catch
//            {
//                return (ActionResult)reportController.RedirectToAction("ERROR404");
//            }
//        }

//        [HttpPost]
//        public async Task<string> GetBenefitValues(string id)
//        {
//            List<Benefit> listAsync = await this._repository.Find((Expression<Func<Benefit, bool>>)(i => i.IsVisible == true), false, (Expression<Func<Benefit, object>>)(b => b.BenefitType)).Where<Benefit>((Expression<Func<Benefit, bool>>)(t => t.Country == id)).ToListAsync<Benefit>();
//            List<BenefitModel> benefitModelList = new List<BenefitModel>();
//            benefitModelList = this._mapper.Map<List<BenefitModel>>((object)listAsync);
//            Func<Task<List<BenefitModel>>> func = new Func<Task<List<BenefitModel>>>(this._BenefitService.GetAllBenefits);
//            return JsonSerializer.Serialize<List<Benefit>>(listAsync);
//        }

//        [HttpGet]
//        public async Task<IActionResult> DownloadFile(string filePath)
//        {
//            ReportController reportController = this;
//            string path2 = "BenefitRequestFiles\\";
//            string path = Path.Combine(Path.Combine(reportController._environment.WebRootPath, path2), filePath);
//            MemoryStream memory = new MemoryStream();
//            using (FileStream stream = new FileStream(path, FileMode.Open))
//                await stream.CopyToAsync((Stream)memory);
//            memory.Position = 0L;
//            string contentType = "APPLICATION/octet-stream";
//            string fileName = Path.GetFileName(path);
//            IActionResult actionResult = (IActionResult)reportController.File((Stream)memory, contentType, fileName);
//            path = (string)null;
//            memory = (MemoryStream)null;
//            return actionResult;
//        }
//    }
//}
