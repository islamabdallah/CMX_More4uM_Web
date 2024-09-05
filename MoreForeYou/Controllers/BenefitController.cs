using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

using static MoreForYou.Services.Models.CommanData;
using MoreForYou.Controllers.hub;
using Microsoft.AspNetCore.SignalR;
using MoreForYou.Services.Models.API;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MoreForYou.Service.Implementation.Email;
using System.Data;
using Castle.Core.Configuration;
using MoreForYou.Services.Contracts.Email;
using MoreForYou.Services.Implementation.Email;
using System.Text.Json;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models.Medical;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Services.Models.Message;

namespace MoreForYou.Controllers
{
    [Authorize]
    public class BenefitController : BaseController
    {
        private readonly IBenefitService _BenefitService;
        private readonly IBenefitWorkflowService _benefitWorkflowService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogger<BenefitController> _logger;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _EmployeeService;
        private readonly IBenefitRequestService _benefitRequestService;
        private readonly IRequestWorkflowService _requestWorkflowService;
        private readonly IDepartmentService _departmentService;
        private readonly IBenefitTypeService _benefitTypeService;
        private readonly IRequestStatusService _requestStatusService;
        //private readonly IEmployeeRequestService _employeeRequestService;
        private readonly IGroupService _groupService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IGroupEmployeeService _groupEmployeeService;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IRequestDocumentService _requestDocumentService;
        private readonly IFirebaseNotificationService _firebaseNotificationService;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IBenefitMailService _benefitMailService;
        private readonly IMGraphMailService _mGraphMailService;
        private readonly IMedicalCategoryService _medicalCategoryService;
        private readonly IMedicalSubCategoryService _medicalSubCategoryService;
        private readonly IMedicalDetailsService _medicalDetailsService;
        //private readonly IWebHostEnvironment _environment;
        //private readonly IConfiguration _configuration;
        //private readonly IExcelService _excelService;

        public BenefitController(IBenefitService BenefitService,
            IBenefitWorkflowService BenefitWorkflowService,
            IUserService userService,
            IRoleService roleService,
            ILogger<BenefitController> logger,
            UserManager<AspNetUser> userManager,
            IEmployeeService EmployeeService,
            IBenefitRequestService benefitRequestService,
            IRequestWorkflowService requestWorkflowService,
            IDepartmentService departmentService,
            IBenefitTypeService benefitTypeService,
            IRequestStatusService requestStatusService,
            IGroupService groupService,
            IWebHostEnvironment hostEnvironment,
            IGroupEmployeeService groupEmployeeService,
            IHubContext<NotificationHub> hub,
            INotificationService notificationService,
            IUserNotificationService userNotificationService,
            IUserConnectionManager userConnectionManager,
            IRequestDocumentService requestDocumentService,
            IFirebaseNotificationService firebaseNotificationService,
            IBenefitMailService benefitMailService,
            SignInManager<AspNetUser> signInManager,
            IMGraphMailService mGraphMailService,
            IMedicalCategoryService medicalCategoryService,
            IMedicalSubCategoryService medicalSubCategoryService,
            IMedicalDetailsService medicalDetailsService)
        //IWebHostEnvironment environment,
        //IConfiguration configuration,
        //IExcelService excelService)
        {
            _BenefitService = BenefitService;
            _benefitWorkflowService = BenefitWorkflowService;
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
            _userManager = userManager;
            _EmployeeService = EmployeeService;
            _benefitRequestService = benefitRequestService;
            _requestWorkflowService = requestWorkflowService;
            _departmentService = departmentService;
            _benefitTypeService = benefitTypeService;
            _requestStatusService = requestStatusService;
            _groupService = groupService;
            _hostEnvironment = hostEnvironment;
            _groupEmployeeService = groupEmployeeService;
            _hub = hub;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _userConnectionManager = userConnectionManager;
            _requestDocumentService = requestDocumentService;
            _firebaseNotificationService = firebaseNotificationService;
            _signInManager = signInManager;
            _benefitMailService = benefitMailService;
            _mGraphMailService = mGraphMailService;
            _medicalCategoryService = medicalCategoryService;
            _medicalSubCategoryService = medicalSubCategoryService;
            _medicalDetailsService = medicalDetailsService;
        }

        List<GenderModel> genderList = new List<GenderModel>()
            {
                new GenderModel { Id=-1, Name="Any"},
                new GenderModel { Id=1, Name="Male"},
                new GenderModel { Id=2, Name="Famle"}
            };

        List<ResonseStatus> resonseStatuses = new List<ResonseStatus>()
        {
            new ResonseStatus {Id =-1, Name ="None"},
            new ResonseStatus {Id =1, Name ="Approve"},
            new ResonseStatus {Id =1, Name ="Disapprove"},
        };
        List<RequestStatusModel> whoIsConcernRequestStatusModels = new List<RequestStatusModel>()
        {
            new RequestStatusModel {Id =-1, Name ="Status"},
            new RequestStatusModel {Id =1, Name ="Pending"},
            new RequestStatusModel {Id =3, Name ="Approved"},
            new RequestStatusModel {Id =4, Name ="Rejected"},

        };

        List<TimingModel> timingModels = new List<TimingModel>()
        {
             new TimingModel{Id=-1, Name="Date"},
             new TimingModel{Id=1, Name="Today"},
            new TimingModel{Id=2, Name="Last Day"},
            new TimingModel{Id=3, Name="Current Week"},
            new TimingModel{Id=4, Name="Current Month"},
        };

        public List<Collar> Collars = new List<Collar>()
        {
               new Collar { Id = -1, Name = "Any" },
            new Collar { Id = 1, Name = "White Collar" },
            new Collar { Id = 2, Name = "Blue Collar" }

        };

        public List<string> AgeSigns = new List<string>()
        {
           ">",
           "<",
           "="
        };

        public List<string> DatesToMatch = new List<string>()
        {
            "Any",
           "Birth Date",
           "Join Date",
           "certain Date"
        };

        public async Task<JsonResult> supervisorFilter(long id)
        {
            try
            {
                AspNetUser applicationUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(applicationUser.Id).Result;
                EmployeeModel Supervisor = new EmployeeModel();
                if (id == 0 || id == employeeModel.EmployeeNumber)
                {
                    Supervisor = null;
                }
                else
                {
                    Supervisor = _EmployeeService.GetEmployee(id).Result;
                }
                if (Supervisor != null)
                {
                    return Json(Supervisor.FullName);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                //return (JsonResult)ERROR404();
            }
            return null;
        }

        // GET: BenefitController
        public ActionResult Index()
        {
            try
            {
                _mGraphMailService.SendAsync("Test");
                List<BenefitModel> beniefitModels = _BenefitService.GetAllBenefits().Result;
                foreach (BenefitModel benefitModel in beniefitModels)
                {
                    if (benefitModel.gender == -1)
                    {
                        benefitModel.GenderText = "Any";
                    }
                    else if (benefitModel.gender == 1)
                    {
                        benefitModel.GenderText = "Male";
                    }
                    else if (benefitModel.gender == 2)
                    {
                        benefitModel.GenderText = "Female";
                    }


                    if (benefitModel.MaritalStatus == -1)
                    {
                        benefitModel.MartialStatusText = "Any";
                    }
                    else if (benefitModel.MaritalStatus == 1)
                    {
                        benefitModel.MartialStatusText = "Single";
                    }
                    else if (benefitModel.MaritalStatus == 2)
                    {
                        benefitModel.MartialStatusText = "Married";
                    }
                    else if (benefitModel.MaritalStatus == 3)
                    {
                        benefitModel.MartialStatusText = "Divorced";
                    }


                    benefitModel.CollarText = Enum.GetName(typeof(CommanData.CollarTypes), benefitModel.Collar);

                    var workflows = _benefitWorkflowService.GetBenefitWorkflowS(benefitModel.Id);
                    if (workflows != null)
                    {
                        workflows.ForEach(w => w.RoleName = _roleService.GetRole(w.RoleId).Result.Name);
                        benefitModel.BenefitWorkflowModels = workflows;
                        benefitModel.BenefitWorkflowModels = benefitModel.BenefitWorkflowModels.Where(w => w.Order != 0).OrderBy(w => w.Order).ToList();
                    }
                }

                return View(beniefitModels);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        // GET: BenefitController/Details/5
        //public ActionResult BenefitDetails(long id)
        //{
        //    BenefitModel benefitModel = _BenefitService.GetBenefit(id);
        //    benefitModel.BenefitConditions = CreateBenefitConditions(benefitModel);
        //    return View(benefitModel);
        //}
        public ActionResult loginn()
        {
            return View();
        }

        public ActionResult test()
        {
            return View();
        }
        public async Task<ActionResult> RequestDetails(long id)
        {
            try
            {
                AspNetUser applicationUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(applicationUser.Id).Result;
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
                var requestWokflowsModels = await _requestWorkflowService.GetRequestWorkflow(id);
                List<RequestWokflowModel> requestWokflowModels = requestWokflowsModels.Where(rw => rw.EmployeeId == employeeModel.EmployeeNumber).ToList();
                return View(benefitRequestModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        //used
        [HttpPost]
        public async Task<ActionResult> RequestDetails2(long id)
        {
            try
            {
                AspNetUser applicationUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(applicationUser.Id);
                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
                var requestWokflowsModels = await _requestWorkflowService.GetRequestWorkflow(id);
                List<RequestWokflowModel> requestWokflowModels = requestWokflowsModels.Where(rw => rw.EmployeeId == employeeModel.EmployeeNumber).ToList();
                return View(benefitRequestModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RequestCancel(long id, long benefitId)
        {
            try
            {
                string message = await _requestWorkflowService.CancelMyRequest(id);
                if (message == "Success Process")
                {
                    TempData["Message"] = "Sucess process, your request with number " + id + " has been cancelled";
                    BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
                    RequestWokflowModel requestWokflowModel = _requestWorkflowService.GetRequestWorkflow(id).Result.FirstOrDefault();
                    bool result = _requestWorkflowService.SendNotification(benefitRequestModel, requestWokflowModel, "RequestCancel", 0).Result;

                }
                else
                {
                    TempData["Error"] = message;
                }
                return RedirectToAction("ShowMyBenefitRequests", new { BenefitId = benefitId, requestNumber = -1 });
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        //used
        [HttpPost]
        public async Task<bool> RequestCancel2(long id, long benefitId)
        {
            try
            {
                string message = await _requestWorkflowService.CancelMyRequest(id);
                if (message == "Success Process")
                {
                  //  TempData["Message"] = "Sucess process, your request with number " + id + " has been cancelled";
                    BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
                    RequestWokflowModel requestWokflowModel = _requestWorkflowService.GetRequestWorkflow(id).Result.FirstOrDefault();
                    bool result = _requestWorkflowService.SendNotification(benefitRequestModel, requestWokflowModel, "RequestCancel", 0).Result;

                }
                else
                {
                    TempData["Error"] = message;
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }


        // GET: BenefitController/Create
        public ActionResult Create()
        {
            try
            {
                BenefitModel benefitModel = new BenefitModel();
                List<RoleModel> RoleModels = _roleService.GetAllRoles().Result;
                benefitModel.Collars = Collars;
                benefitModel.DatesToMatch = DatesToMatch;
                benefitModel.RolesOrder = new List<RoleOrder>();
                benefitModel.genderModels = genderList;
                benefitModel.AgeSigns = AgeSigns;
                benefitModel.BenefitTypeId = (int)CommanData.BenefitTypes.Individual;
                benefitModel.BenefitTypeModels = _benefitTypeService.GetAllBenefitTypes();
                benefitModel.MartialStatusModels = martialStatusModels;
                benefitModel.MinParticipant = 1;
                benefitModel.MaxParticipant = 1;
                benefitModel.numberOfDays = 1;
                benefitModel.Times = 1;
                benefitModel.Year = DateTime.Now.Year;
                for (int index = 0; index < RoleModels.Count; index++)
                {
                    RoleOrder roleOrder = new RoleOrder()
                    { order = 0, RoleId = RoleModels[index].Id, RoleName = RoleModels[index].Name };
                    benefitModel.RolesOrder.Insert(index, roleOrder);
                }
                benefitModel.RolesOrder = benefitModel.RolesOrder.Where(r => r.RoleName != "Admin").ToList();
                return View(benefitModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        // POST: BenefitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BenefitModel Model)
        {
            try
            {
                bool addMailBenefitResult = false;
                Model.CreatedDate = DateTime.Now;
                Model.UpdatedDate = DateTime.Now;
                Model.IsVisible = true;
                Model.IsDelted = false;
                string imageName = await _requestWorkflowService.UploadedImageAsync(Model.ImageName, @"images\BenefitCards");
                Model.BenefitCard = imageName;
                Model.CertainDate = Model.CertainDate;
                if ((Model.HasMails == true && Model.BenefitMails != null) || (Model.HasMails == false && Model.BenefitMails == null))
                {
                    if ((Model.HasWorkflow == true && Model.RolesOrder.Count > 0) || (Model.HasWorkflow == false))
                    {
                        var addedBenefitModel = _BenefitService.CreateBenefit(Model);
                        if (addedBenefitModel != null)
                        {
                            if (addedBenefitModel.HasMails == true)
                            {
                                //string[] mails = Model.BenefitMails.Split(";");
                                //for (int mailIndex=0; mailIndex < mails.Length; mailIndex++)
                                //{
                                //    BenefitMailModel benefitMailModel = new BenefitMailModel
                                //    {
                                //        SendTo = mails[mailIndex],
                                //        CreatedDate = DateTime.Now,
                                //        UpdatedDate = DateTime.Now,
                                //        IsDelted = false,
                                //        IsVisible = true,
                                //        BenefitId = addedBenefitModel.Id
                                //    };
                                //    addMailBenefitResult = _benefitMailService.CreateBenefitMail(benefitMailModel);
                                //    if(!addMailBenefitResult)
                                //    {
                                //        break;
                                //    }
                                //}
                                BenefitMailModel benefitMailModel = new BenefitMailModel();
                                if (Model.CarbonCopies == null)
                                {

                                    benefitMailModel.SendTo = Model.BenefitMails;
                                    benefitMailModel.CreatedDate = DateTime.Now;
                                    benefitMailModel.UpdatedDate = DateTime.Now;
                                    benefitMailModel.IsDelted = false;
                                    benefitMailModel.IsVisible = true;
                                    benefitMailModel.BenefitId = addedBenefitModel.Id;

                                }
                                else
                                {
                                    benefitMailModel.SendTo = Model.BenefitMails;
                                    benefitMailModel.CarbonCopies = Model.BenefitMails;
                                    benefitMailModel.CreatedDate = DateTime.Now;
                                    benefitMailModel.UpdatedDate = DateTime.Now;
                                    benefitMailModel.IsDelted = false;
                                    benefitMailModel.IsVisible = true;
                                    benefitMailModel.BenefitId = addedBenefitModel.Id;
                                }
                                addMailBenefitResult = _benefitMailService.CreateBenefitMail(benefitMailModel);
                            }
                            if (addedBenefitModel.HasWorkflow == true)
                            {
                                bool response = false;
                                foreach (var workflow in Model.RolesOrder.Where(R => R.order != 0))
                                {
                                    BenefitWorkflowModel benefitWorkflowModel = new BenefitWorkflowModel();
                                    benefitWorkflowModel.RoleId = workflow.RoleId;
                                    benefitWorkflowModel.BenefitId = addedBenefitModel.Id;
                                    benefitWorkflowModel.Order = workflow.order;
                                    benefitWorkflowModel.IsDelted = false;
                                    benefitWorkflowModel.IsVisible = true;
                                    benefitWorkflowModel.CreatedDate = DateTime.Today;
                                    benefitWorkflowModel.UpdatedDate = DateTime.Today;
                                    response = _benefitWorkflowService.CreateBenefitWorkflow(benefitWorkflowModel).Result;
                                    if (response != true)
                                    {
                                        response = _benefitWorkflowService.DeleteBenefitWorkflow(addedBenefitModel.Id);
                                        ViewBag.Error = "Failed process, Fail to add Benefit workflow";
                                        break;
                                    }
                                }
                                if (response == true)
                                {
                                    ViewBag.Message = "Success Process, Benefit has been added";

                                }
                                else
                                {
                                    ViewBag.Error = "Failed to add new Benefit Workflow";
                                    _BenefitService.DeleteBenefit(addedBenefitModel.Id);
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Success Process, Benefit has been added";
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Failed to add new Benefit";
                        }
                        List<RoleModel> RoleModels = _roleService.GetAllRoles().Result;
                        Model.Collars = Collars;
                        Model.DatesToMatch = DatesToMatch;
                        Model.RolesOrder = new List<RoleOrder>();
                        Model.genderModels = CommanData.genderModels;
                        Model.AgeSigns = AgeSigns;
                        for (int index = 0; index < RoleModels.Count; index++)
                        {
                            RoleOrder roleOrder = new RoleOrder()
                            { order = 0, RoleId = RoleModels[index].Id, RoleName = RoleModels[index].Name };
                            Model.RolesOrder.Insert(index, roleOrder);
                        }
                        Model.RolesOrder = Model.RolesOrder.Where(r => r.RoleName != "Admin").ToList();
                        return View(Model);
                    }
                    else
                    {
                        ViewBag.Error = "Invalid Workflow Data";
                        return View(Model);
                    }
                }
                else
                {
                    ViewBag.Error = "Invalid Mails Data";
                    return View(Model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                ViewBag.Error = "Error, Failed to add new Benefit";
                return RedirectToAction("ERROR404");
            }
        }

        //private  List<RequestDocumentModel> UploadedFileAsync(IFormFile[] files, string path)
        //{
        //    string uniqueFileName = null;
        //    string filePath = null;
        //    List<RequestDocumentModel> RequestDocumentModels = new List<RequestDocumentModel>();
        //    for(int x =0; x<files.Length; x++)
        //    {
        //        RequestDocumentModel requestDocumentModel = new RequestDocumentModel();
        //        if (files[x] != null)
        //        {
        //            if (files[x].Length > 0)
        //            {
        //                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, path);
        //                uniqueFileName = Guid.NewGuid().ToString() + "_" + files[x].FileName;
        //                requestDocumentModel.filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //                using (var fileStream = new FileStream(requestDocumentModel.filePath, FileMode.Create))
        //                {
        //                    files[x].CopyTo(fileStream);

        //                }
        //                using (var memoryStrem = new MemoryStream())
        //                {
        //                    files[x].CopyTo(memoryStrem);
        //                    requestDocumentModel.DataFiles = memoryStrem.ToArray();

        //                }
        //                requestDocumentModel.FileType = files[x].ContentType;
        //                requestDocumentModel.Name = files[x].Name;
        //                RequestDocumentModels.Add(requestDocumentModel);
        //            }
        //        }
        //    }

        //    return RequestDocumentModels;
        //}


        // GET: BenefitController/Edit/5
        public async Task<ActionResult> Edit(long id)
        {
            try
            {
                BenefitModel benefitModel = new BenefitModel();
                benefitModel = await _BenefitService.GetBenefit(id);
                List<RoleModel> RoleModels = _roleService.GetAllRoles().Result;
                benefitModel.Collars = Collars;
                benefitModel.DatesToMatch = DatesToMatch;
                benefitModel.RolesOrder = new List<RoleOrder>();
                benefitModel.genderModels = genderList;
                benefitModel.AgeSigns = AgeSigns;
                benefitModel.ImageUrl = benefitModel.BenefitCard;
                benefitModel.BenefitTypeModels = _benefitTypeService.GetAllBenefitTypes();
                benefitModel.MartialStatusModels = martialStatusModels;
                benefitModel.BenefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(id);

                for (int index = 0; index < RoleModels.Count; index++)
                {
                    var ExistWorkflow = benefitModel.BenefitWorkflowModels.Where(BW => BW.RoleId == RoleModels[index].Id);
                    if (ExistWorkflow.Any() == true)
                    {
                        RoleOrder roleOrder = new RoleOrder()
                        { order = benefitModel.BenefitWorkflowModels.Where(r => r.RoleId == RoleModels[index].Id).First().Order, RoleId = RoleModels[index].Id, RoleName = RoleModels[index].Name };
                        benefitModel.RolesOrder.Insert(index, roleOrder);

                    }
                    else
                    {
                        RoleOrder roleOrder = new RoleOrder()
                        { order = 0, RoleId = RoleModels[index].Id, RoleName = RoleModels[index].Name };
                        benefitModel.RolesOrder.Insert(index, roleOrder);

                    }
                }
                benefitModel.RolesOrder = benefitModel.RolesOrder.Where(r => r.RoleName != "Admin").ToList();
                //foreach (var workflow in benefitModel.BenefitWorkflowModels)
                //{
                //    workflow.RoleName = RoleModels.Where(r => r.Id == workflow.RoleId).First().Name;
                //}
                return View(benefitModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BenefitModel Model)
        {
            try
            {
                BenefitModel DBBenefitModel = await _BenefitService.GetBenefit(Model.Id);
                Model.CreatedDate = DateTime.Now;
                Model.UpdatedDate = DateTime.Now;
                Model.IsVisible = true;
                Model.IsDelted = false;
                //Model.HasWorkflow = true;
                //string imageName = _requestWorkflowService.UploadedImageAsync(Model.ImageName, "Cards").Result;
                Model.BenefitCard = Model.ImageUrl;
                var result = _BenefitService.UpdateBenefit(Model);
                if (result.Result == true)
                {
                    if (Model.HasWorkflow == true)
                    {
                        var DBBenefitWorkflow = _benefitWorkflowService.GetBenefitWorkflowS(Model.Id);
                        bool response = false;
                        var flow = Model.RolesOrder;//.Where(r => r.order != 0);
                        foreach (var workflow in flow)
                        {
                            //var matchedWorkflow= DBBenefitWorkflow.Where(dbW => dbW.RoleId == workflow.RoleId);
                            // if (matchedWorkflow.Count() >0)
                            // {
                            //     if(matchedWorkflow.First().Order != workflow.order )
                            //     {

                            BenefitWorkflowModel benefitWorkflowModel = _benefitWorkflowService.GetBenefitWorkflow(Model.Id, workflow.RoleId);

                            if (benefitWorkflowModel != null && workflow.order != 0)
                            {

                                benefitWorkflowModel.Order = workflow.order;
                                benefitWorkflowModel.UpdatedDate = DateTime.Today;
                                response = _benefitWorkflowService.UpdateBenefitWorkflow(benefitWorkflowModel).Result;
                            }
                            else if (benefitWorkflowModel != null && workflow.order == 0)
                            {

                                benefitWorkflowModel.IsDelted = true;
                                benefitWorkflowModel.IsVisible = false;
                                benefitWorkflowModel.Order = workflow.order;
                                benefitWorkflowModel.UpdatedDate = DateTime.Today;
                                response = _benefitWorkflowService.UpdateBenefitWorkflow(benefitWorkflowModel).Result;
                            }
                            else if (benefitWorkflowModel == null && workflow.order != 0)
                            {
                                benefitWorkflowModel = new BenefitWorkflowModel();
                                benefitWorkflowModel.RoleId = workflow.RoleId;
                                benefitWorkflowModel.BenefitId = DBBenefitModel.Id;
                                benefitWorkflowModel.Order = workflow.order;
                                benefitWorkflowModel.UpdatedDate = DateTime.Today;
                                benefitWorkflowModel.CreatedDate = DateTime.Today;
                                benefitWorkflowModel.IsDelted = false;
                                benefitWorkflowModel.IsVisible = true;
                                response = _benefitWorkflowService.CreateBenefitWorkflow(benefitWorkflowModel).Result;
                            }
                            else
                            {
                                response = true;
                            }

                            if (response != true)
                            {
                                //response = _benefitWorkflowService.DeleteBenefitWorkflow(DBBenefitModel.Id);
                                ViewBag.Error = "Failed process, Fail to update Benefit workflow";
                                break;
                            }
                        }
                        if (response == true)
                        {

                            ViewBag.Message = "Success Process, Benefit has been updated";

                        }
                    }
                    else
                    {
                        ViewBag.Message = "Success Process, Benefit has been updated";
                    }
                }
                else
                {
                    ViewBag.Error = "Failed to update the Benefit";
                }
                List<RoleModel> RoleModels = _roleService.GetAllRoles().Result;
                Model.Collars = Collars;
                Model.DatesToMatch = DatesToMatch;
                Model.RolesOrder = new List<RoleOrder>();
                Model.genderModels = CommanData.genderModels;
                Model.AgeSigns = AgeSigns;
                Model.BenefitTypeModels = _benefitTypeService.GetAllBenefitTypes();
                Model.MartialStatusModels = martialStatusModels;
                for (int index = 0; index < RoleModels.Count; index++)
                {
                    RoleOrder roleOrder = new RoleOrder()
                    { order = 0, RoleId = RoleModels[index].Id, RoleName = RoleModels[index].Name };
                    Model.RolesOrder.Insert(index, roleOrder);
                }
                Model.RolesOrder = Model.RolesOrder.Where(r => r.RoleName != "Admin").ToList();
                return View(Model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                ViewBag.Error = "Failed to update the Benefit";
                return View(Model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                BenefitModel DBBenefitModel = await _BenefitService.GetBenefit(id);
                DBBenefitModel.UpdatedDate = DateTime.Now;
                DBBenefitModel.IsDelted = true;
                DBBenefitModel.IsVisible = false;
                //string imageName = _requestWorkflowService.UploadedImageAsync(Model.ImageName, "Cards").Result;
                var result = _BenefitService.UpdateBenefit(DBBenefitModel);
                if (result != null)
                {
                    if (DBBenefitModel.HasWorkflow == true)
                    {
                        var DBBenefitWorkflow = _benefitWorkflowService.GetBenefitWorkflowS(DBBenefitModel.Id);
                        bool response = false;
                        foreach (var workflow in DBBenefitWorkflow)
                        {
                            {
                                workflow.IsVisible = false;
                                workflow.IsDelted = true;
                                workflow.UpdatedDate = DateTime.Now;
                                response = _benefitWorkflowService.UpdateBenefitWorkflow(workflow).Result;
                            }

                            if (response != true)
                            {
                                //response = _benefitWorkflowService.DeleteBenefitWorkflow(DBBenefitModel.Id);
                                ViewBag.Error = "Failed process, Fail to delete Benefit workflow";
                                break;
                            }
                        }
                        if (response == true)
                        {
                            ViewBag.Message = "Success Process, Benefit has been deleted";

                        }
                    }
                    else
                    {
                        ViewBag.Message = "Success Process, Benefit has been deleted";
                    }
                }
                else
                {
                    ViewBag.Error = "Failed to delete Benefit";
                }

                return RedirectToAction("index");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                ViewBag.Error = "Failed to delete Benefit";
                return RedirectToAction("index");
            }
        }
        public ActionResult RequestEdit(int id)
        {
            BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(id);
            return View(benefitRequestModel);
        }

        // POST: BenefitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestEdit(int id, BenefitRequestModel benefitRequestModel)
        {
            try
            {
                BenefitRequestModel DBbenefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                if (DBbenefitRequestModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending)

                {
                    DBbenefitRequestModel.UpdatedDate = DateTime.Today;
                    DBbenefitRequestModel.ExpectedDateFrom = benefitRequestModel.ExpectedDateFrom;
                    DBbenefitRequestModel.ExpectedDateTo = benefitRequestModel.ExpectedDateTo;
                    DBbenefitRequestModel.Message = benefitRequestModel.Message;
                    bool updateResult = _benefitRequestService.UpdateBenefitRequest(DBbenefitRequestModel).Result;
                    if (updateResult == true)
                    {
                        ViewBag.Message = "Sucessful process, you have updated resquest with number " + DBbenefitRequestModel.Id;
                        return View(DBbenefitRequestModel);

                    }
                    else
                    {
                        ViewBag.Error("Failed process");
                        BenefitRequestModel benefitRequestModel1 = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
                        return View(benefitRequestModel1);
                    }
                }
                else
                {
                    ViewBag.Error("You can not update this request, As it's status is " + (CommanData.BenefitStatus)DBbenefitRequestModel.RequestStatusId);
                    return View(benefitRequestModel);

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return View(benefitRequestModel);
            }
        }

        // POST: BenefitController/Delete/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AddResponseAsync(RequestFilterModel RequestFilterModel)
        //{
        //    try
        //    {
        //        AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
        //        EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
        //        List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
        //        foreach (RequestWokflowModel requestWokflowModel in RequestFilterModel.RequestWokflowModels)
        //        {
        //            if (requestWokflowModel.canResponse && requestWokflowModel.RequestStatusSelectedId != -1)
        //            {
        //                RequestWokflowModel DBRequestWorkflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(requestWokflowModel.EmployeeId, requestWokflowModel.BenefitRequestId);
        //                requestWokflowModel.ReplayDate = DateTime.Now;
        //                requestWokflowModel.RoleId = DBRequestWorkflowModel.RoleId;
        //                requestWokflowModel.IsVisible = true;
        //                requestWokflowModel.IsDelted = false;
        //                requestWokflowModel.UpdatedDate = DateTime.Now;
        //                bool updateResult = false;
        //                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(requestWokflowModel.BenefitRequestId);
        //                benefitRequestModel.ConfirmedDateFrom = benefitRequestModel.ExpectedDateFrom;
        //                benefitRequestModel.ConfirmedDateTo = benefitRequestModel.ExpectedDateTo;
        //                if (requestWokflowModel.RequestStatusSelectedId == 2)
        //                {
        //                    requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
        //                    benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
        //                    updateResult = _requestWorkflowService.UpdateRequestWorkflow(requestWokflowModel).Result;
        //                    if (updateResult == true)
        //                    {
        //                        updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
        //                        if (updateResult == true)
        //                        {
        //                            ViewBag.Message = "Thank you for kind response";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ViewBag.Error = "Failed process";
        //                    }
        //                }
        //                else if (requestWokflowModel.RequestStatusSelectedId == 1)
        //                {
        //                    requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
        //                    updateResult = _requestWorkflowService.UpdateRequestWorkflow(requestWokflowModel).Result;
        //                    if (updateResult == true)
        //                    {
        //                        List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
        //                        int benefitWorflowsCount = benefitWorkflowModels.Count;
        //                        int order = benefitWorkflowModels.Where(bw => bw.RoleId == DBRequestWorkflowModel.RoleId).Select(bw => bw.Order).First();
        //                        if (order == benefitWorflowsCount)
        //                        {
        //                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
        //                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
        //                            if (updateResult == true)
        //                            {
        //                                ViewBag.Message = "Thank you for kind response";
        //                            }

        //                        }
        //                        else if (order <= benefitWorflowsCount)
        //                        {
        //                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.InProgress;
        //                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
        //                            if (updateResult == true)
        //                            {
        //                                int nextOrder = order + 1;
        //                                string message = SendReuestToWhoIsConcern(benefitRequestModel.Id, nextOrder).Result;
        //                                ViewBag.Message = "Thank you for kind response";
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.ToString());
        //    }
        //    return null;
        //}

        public async Task<ActionResult> ShowBenefits()
        {
            try
            {
                // await _firebaseNotificationService.SendNotification("dQjttMxNSOWPea11KPL6uF:APA91bH2bntvIZiAGkDBeO3OQK3_Il1r2BR1yhJ0EIaI6vqfC-aNos9R6kJCD6DW5utBV43Bm0Rwt5-oOyoT5GAYHFQdeuS2SFx4YG7L2OOCB5X-kJ70G4paodVfWQB-w4EiLjMlGduJ", "Request", "tesst");
                //List<string> to = new List<string>()
                //{ "islammohamed.abdallah@cemex.com"};
                //var result = _mGraphMailService.SendAsync("tesssst", to, "more4uApplication", 100, "BreakTime", null);
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                HomeModel homeModel = await _BenefitService.ShowAllBenefits(employeeModel, (int)CommanData.Languages.English);
                homeModel.user.Email = CurrentUser.Email;
                int subCateoryCount = 0;
                List<MedicalCategoryModel> medicalCategoryModels = _medicalCategoryService.GetAllMedicalCategories().Result;
                if (medicalCategoryModels != null)
                {
                    if (medicalCategoryModels.Count > 0)
                    {
                        List<MedicalSubCategoryModel> medicalSubCategoryModels = await _medicalSubCategoryService.GetAllMedicalSubCategories();
                        if (medicalSubCategoryModels != null)
                        {
                            if (medicalCategoryModels.Count > 0)
                            {
                                foreach (var item in medicalCategoryModels)
                                {
                                    subCateoryCount = medicalSubCategoryModels.Where(MS => MS.MedicalCategory.Id == item.Id).Count();
                                    if (subCateoryCount == 1)
                                    {
                                        var itemMedicalDetail = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(item.Id, employeeModel.Country);
                                        if (itemMedicalDetail != null)
                                        {
                                            item.SubCategoriesCount = itemMedicalDetail.Count;
                                        }
                                    }
                                    else
                                    {
                                        item.SubCategoriesCount = subCateoryCount;
                                    }
                                }
                            }
                        }
                        homeModel.MedicalCategoryModels = medicalCategoryModels;
                    }
                    else
                    {
                        homeModel.MedicalCategoryModels = new List<MedicalCategoryModel>();
                    }
                }
                else
                {
                    homeModel.MedicalCategoryModels = new List<MedicalCategoryModel>();
                }
                return View(homeModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<ActionResult> BenefitDetails(long id)
        {
            try
            {
                AspNetUser aspNetUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(aspNetUser.Id).Result;
                BenefitAPIModel benefitAPIModel = await _BenefitService.GetBenefitDetails(id, employeeModel, (int)CommanData.Languages.English);
                return View("BenefitDetails", benefitAPIModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<string> getBenefitDetails(string id)
        {
            try
            {
                AspNetUser aspNetUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(aspNetUser.Id).Result;
                BenefitAPIModel benefitAPIModel = await _BenefitService.GetBenefitDetails(Convert.ToInt32(id), employeeModel, (int)CommanData.Languages.English);
                // return View("BenefitDetails", benefitAPIModel);
                //TempData["detail"] = benefitAPIModel;
                return JsonSerializer.Serialize(benefitAPIModel);
            }
            catch (Exception e)
            {
                return null;
                // _logger.LogError(e.ToString());
                //return RedirectToAction("ERROR404");
            }
        }

        // POST: BenefitController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ShowBenefits(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public async Task<ActionResult> Redeem(int id)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                WebRequest request = await _BenefitService.BenefitRedeem(id, CurrentUser.Id);
                ViewBag.BenefitId = id;
                //if (TempData["Message"] != null)
                //{
                //    ViewBag.Message = TempData["Message"];
                //}
                //else if(TempData["Error"] != null)
                //{
                //    ViewBag.Error = TempData["Error"];
                //}
                if (request.BenefitType == Enum.GetName(typeof(CommanData.BenefitTypes), CommanData.Individual))
                {
                    return View("BenefitRequest", request);
                }
                else
                {
                    return View("GroupRequest", request);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<string> RedeemmAsync(string id)
        {

            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                WebRequest request = await _BenefitService.BenefitRedeem(Convert.ToInt32(id), CurrentUser.Id);
                ViewBag.BenefitId = id;
                //if (TempData["Message"] != null)
                //{
                //    ViewBag.Message = TempData["Message"];
                //}
                //else if(TempData["Error"] != null)
                //{
                //    ViewBag.Error = TempData["Error"];
                //}
                if (request.BenefitType == Enum.GetName(typeof(CommanData.BenefitTypes), CommanData.Individual))
                {
                    return JsonSerializer.Serialize(request); /*View("BenefitRequest", request);*/
                }
                else
                {
                    return JsonSerializer.Serialize(request);/* View("GroupRequest", request);*/
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmRequest(WebRequest request)
        {
            try
            {
                string result = "";
                List<string> docs = new List<string>();
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                BenefitModel benefitModel = await _BenefitService.GetBenefit(request.benefitId);
                bool flag = false;
                EmployeeModel employeeModel =  _EmployeeService.GetEmployeeByUserId2(CurrentUser.Id);
                bool canRedeem = _benefitRequestService.EmployeeCanRedeemthisbenefit(benefitModel, employeeModel);
                DateTime requiredDate = Convert.ToDateTime(request.From);
                if (requiredDate.Date > DateTime.Today.Date)
                {
                    if (benefitModel.numberOfDays == 1)
                    {
                        request.To = request.From;
                    }
                    else
                    {
                        request.To = Convert.ToDateTime(request.From).AddDays(benefitModel.numberOfDays).ToString();
                    }
                    if (canRedeem == true)
                    {
                        if (benefitModel.IsAgift == true)
                        {
                            var sendToPerson = _EmployeeService.GetEmployee(request.SendToId);
                            if (sendToPerson == null)
                            {
                                TempData["Error"] = "Invalid data";
                                return RedirectToAction("Redeem", new { id = benefitModel.Id });
                            }
                        }

                        if (benefitModel.RequiredDocuments != null)
                        {
                            //if (request.Documents.Count() > 0)
                            //{
                            int length = benefitModel.RequiredDocuments.Split(";").Length;
                            for (int index = 0; index < request.DocumentFiles.Count; index++)
                            {
                                if (request.DocumentFiles[index].Length > 0)
                                {
                                    //using (var ms = new MemoryStream())
                                    //{
                                    //    request.DocumentFiles[index].CopyTo(ms);
                                    //    var fileBytes = ms.ToArray();
                                    //    string s = Convert.ToBase64String(fileBytes);
                                    //    docs.Add(s);
                                    //}
                                    if (_requestWorkflowService.UploadedImageAsync(request.DocumentFiles[index], CommanData.DocumentsFolder).Result != null)
                                    {
                                        docs.Add(_requestWorkflowService.UploadedImageAsync(request.DocumentFiles[index], CommanData.DocumentsFolder).Result);
                                    }
                                }
                            }
                        }
                        request.Documents = docs;
                        Request request1 = _benefitRequestService.ConvertFromWebRequestToRequestModel(request);

                        if (benefitModel.BenefitTypeId == (int)CommanData.BenefitTypes.Individual)
                        {

                            result = await _requestWorkflowService.AddIndividualRequest(request1, CurrentUser.Id, benefitModel);

                        }
                        else
                        {
                            result = await _requestWorkflowService.ConfirmGroupRequest(request1, CurrentUser.Id, benefitModel);
                        }
                        if (result.Contains("Success Process"))
                        {
                            TempData["Message"] = "Your Request Added Successfully";
                            flag = true;
                            // return RedirectToAction("ShowMyBenefitRequests", new { BenefitId = request.benefitId });
                        }
                        else
                        {
                            TempData["Error"] = result;
                            flag = false;
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Invalid data";
                        flag = false;
                    }
                    if (flag == true)
                    {
                        TempData["BenefitId"] = benefitModel.Id;
                        return RedirectToAction("ShowMyBenefits");
                    }
                    if (benefitModel.BenefitType.Name == Enum.GetName(typeof(CommanData.BenefitTypes), (int)CommanData.BenefitTypes.Individual))
                    {
                        return RedirectToAction("Redeem", new { id = benefitModel.Id });
                    }
                    else
                    {
                        return RedirectToAction("Redeem", new { id = benefitModel.Id });
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid Required Data";
                    return RedirectToAction("Redeem", new { id = benefitModel.Id });
                }

            }
            catch (Exception e)
            {
                //TempData["Message"] = e.ToString();
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ConfirmGroupRequest(GroupModel groupModel)
        //{
        //    AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
        //    EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
        //    string[] insertedEmployeeNumbersString = groupModel.BenefitRequestModel.SelectedEmployeeNumbers.Split(";");
        //    insertedEmployeeNumbersString[insertedEmployeeNumbersString.Length - 1] = CurrentEmployee.EmployeeNumber.ToString();
        //    BenefitModel benefitModel = _BenefitService.GetBenefit(groupModel.BenefitRequestModel.Benefit.Id);
        //    int GroupMembersCount = insertedEmployeeNumbersString.Length;
        //    bool result = false;
        //    string Message = "";
        //    if (GroupMembersCount <= benefitModel.MaxParticipant && GroupMembersCount >= benefitModel.MinParticipant)
        //    {
        //        groupModel.BenefitId = groupModel.BenefitRequestModel.Benefit.Id;
        //        groupModel.CreatedDate = DateTime.Now;
        //        groupModel.UpdatedDate = DateTime.Now;
        //        groupModel.IsDelted = false;
        //        groupModel.IsVisible = true;
        //        groupModel.CreatedBy = CurrentUser.Id;
        //        if (groupModel.ExpectedDateTo == null)
        //        {
        //            groupModel.ExpectedDateTo = groupModel.ExpectedDateFrom;
        //        }
        //        if (GroupMembersCount == benefitModel.MaxParticipant)
        //        {
        //            groupModel.GroupStatus = "Closed";
        //            groupModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //        }
        //        else if (GroupMembersCount >= benefitModel.MinParticipant)
        //        {
        //            groupModel.GroupStatus = "Open";
        //            groupModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //        }
        //        groupModel.Benefit = null;
        //        groupModel.BenefitRequestModel = null;
        //        GroupModel newGroupModel = _groupService.CreateGroup(groupModel);
        //        if (newGroupModel != null)
        //        {
        //            newGroupModel.Code = "G-" + DateTime.Today.ToString("yyyyMMdd") + newGroupModel.Id;
        //            result = _groupService.UpdateGroup(newGroupModel);
        //            GroupEmployeeModel groupEmployeeModel = new GroupEmployeeModel();
        //            GroupEmployeeModel newGroupEmployeeModel = new GroupEmployeeModel();
        //            if (result == true)
        //            {
        //                for (int index = 0; index < insertedEmployeeNumbersString.Length; index++)
        //                {
        //                    long employeeNumber = long.Parse(insertedEmployeeNumbersString[index]);
        //                    EmployeeModel employeeMember = _EmployeeService.GetEmployee(employeeNumber);
        //                    groupEmployeeModel.EmployeeId = employeeMember.EmployeeNumber;
        //                    groupEmployeeModel.GroupId = newGroupModel.Id;
        //                    groupEmployeeModel.JoinDate = DateTime.Now;
        //                    newGroupEmployeeModel = _groupEmployeeService.CreateGroupEmployee(groupEmployeeModel);
        //                }
        //            }
        //            BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
        //            benefitRequestModel.RequestDate = DateTime.Now;
        //            benefitRequestModel.CreatedDate = DateTime.Now;
        //            benefitRequestModel.UpdatedDate = DateTime.Now;
        //            benefitRequestModel.IsDelted = false;
        //            benefitRequestModel.IsVisible = true;
        //            benefitRequestModel.Message = groupModel.Message;
        //            benefitRequestModel.ExpectedDateFrom = groupModel.ExpectedDateFrom;
        //            benefitRequestModel.ExpectedDateTo = groupModel.ExpectedDateTo;
        //            benefitRequestModel.BenefitId = benefitModel.Id;
        //            benefitRequestModel.GroupId = newGroupModel.Id;
        //            benefitRequestModel.EmployeeId = CurrentEmployee.EmployeeNumber;
        //            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //            BenefitRequestModel newBenefitRequestModel = _benefitRequestService.CreateBenefitRequest(benefitRequestModel);
        //            if (newBenefitRequestModel != null)
        //            {   
        //                result = SendGroupRequestToHR(newBenefitRequestModel);
        //                ViewBag.Message = "Success Process, your request has been sent";
        //                return RedirectToAction("ShowMyBenefits");

        //            }
        //            else
        //            {
        //                ViewBag.Error = "Can not send Request";
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Error = "Can not create group";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Failed Process, Group Members does not match";
        //    }

        //    return View(groupModel);
        //}

        //public bool SendGroupRequestToHR(BenefitRequestModel benefitRequestModel)
        //{
        //    try
        //    {
        //        List<AspNetUser> HRUsers = _userManager.GetUsersInRoleAsync("HR").Result.ToList();
        //        RoleModel roleModel = _roleService.GetRoleByName("HR").Result;
        //        RequestWokflowModel newRequestWokflowModel = new RequestWokflowModel();
        //        bool result = false;
        //        foreach (AspNetUser user in HRUsers)
        //        {
        //            EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(user.Id).Result;
        //            RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
        //            requestWokflowModel.EmployeeId = employeeModel.EmployeeNumber;
        //            requestWokflowModel.BenefitRequestId = benefitRequestModel.Id;
        //            requestWokflowModel.RoleId = roleModel.Id;
        //            requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //            requestWokflowModel.CreatedDate = DateTime.Today;
        //            requestWokflowModel.UpdatedDate = DateTime.Today;
        //            requestWokflowModel.IsDelted = false;
        //            requestWokflowModel.IsVisible = true;
        //            newRequestWokflowModel = _requestWorkflowService.CreateRequestWorkflow(requestWokflowModel).Result;
        //            benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequestModel.Id);
        //            if(newRequestWokflowModel != null)
        //            {
        //                newRequestWokflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(newRequestWokflowModel.EmployeeId, newRequestWokflowModel.BenefitRequestId);
        //                result = SendNotification(benefitRequestModel, newRequestWokflowModel, "Request").Result;

        //            }
        //        }
        //        result = SendNotification(benefitRequestModel, newRequestWokflowModel, "CreateGroup").Result;

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.ToString());
        //        return false;
        //    }
        //}
        //public int CreateGroupMembersRequests(string[] insertedEmployeeNumbersString, GroupModel newGroupModel)
        //{
        //    try
        //    {
        //        BenefitRequestModel benefitRequestModel = new BenefitRequestModel();
        //        benefitRequestModel.RequestDate = DateTime.Now;

        //        benefitRequestModel.Message = newGroupModel.Message;
        //        benefitRequestModel.ExpectedDateFrom = newGroupModel.ExpectedDateFrom;
        //        benefitRequestModel.ExpectedDateTo = newGroupModel.ExpectedDateTo;
        //        benefitRequestModel.BenefitId = newGroupModel.BenefitId;
        //        benefitRequestModel.GroupId = newGroupModel.Id;
        //        benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //        BenefitRequestModel newBenefitRequestModel = _benefitRequestService.CreateBenefitRequest(benefitRequestModel);
        //        if (newBenefitRequestModel != null)
        //        {
        //            RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
        //        }
        //        if (newGroupModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending)
        //        {
        //            for (int index = 0; index < insertedEmployeeNumbersString.Length; index++)
        //            {
        //                long employeeNumber = long.Parse(insertedEmployeeNumbersString[index]);
        //                EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber);
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.ToString());
        //    }
        //    return 0;
        //}
        //public async Task<string> SendReuestToWhoIsConcern(long benefitRequetId, int orderNumber)
        //{
        //    try
        //    {
        //        string message = "";
        //        BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(benefitRequetId);
        //        benefitRequestModel.Benefit.BenefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.Benefit.Id);
        //        if (benefitRequestModel.Benefit.BenefitWorkflowModels != null)
        //        {
        //            BenefitWorkflowModel benefitWorkflowModel = benefitRequestModel.Benefit.BenefitWorkflowModels.Where(w => w.Order == orderNumber).First();
        //            string roleName = _roleService.GetRole(benefitWorkflowModel.RoleId).Result.Name;
        //            //EmployeeModel employeeWhoRequest = _EmployeeService.GetEmployee(benefitRequestModel.Employee.EmployeeNumber);
        //            EmployeeModel whoIsConcern = new EmployeeModel();
        //            if (roleName != null)
        //            {
        //                if (roleName == "Supervisor")
        //                {
        //                    whoIsConcern = _EmployeeService.GetEmployee((long)benefitRequestModel.Employee.SupervisorId);
        //                }
        //                else if (roleName == "HR")
        //                {
        //                    //EmployeeModel HR = _EmployeeService.GetEmployee((long)benefitRequestModel.Employee.HRId);
        //                    DepartmentModel departmentModel = _departmentService.GetDepartmentByName("HR");
        //                    whoIsConcern = _EmployeeService.GetDepartmentManager(departmentModel.Id);
        //                }
        //                else if (roleName == "Department Manager")
        //                {
        //                    DepartmentModel departmentModel = _departmentService.GetDepartment(benefitRequestModel.Employee.DepartmentId);
        //                    whoIsConcern = _EmployeeService.GetDepartmentManager(departmentModel.Id);
        //                }

        //                if (whoIsConcern != null)
        //                {

        //                    RequestWokflowModel requestWokflowModel = new RequestWokflowModel();
        //                    requestWokflowModel.EmployeeId = whoIsConcern.EmployeeNumber;
        //                    requestWokflowModel.BenefitRequestId = benefitRequestModel.Id;
        //                    requestWokflowModel.RoleId = benefitWorkflowModel.RoleId;
        //                    requestWokflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Pending;
        //                    requestWokflowModel.CreatedDate = DateTime.Now;
        //                    requestWokflowModel.UpdatedDate = DateTime.Now;
        //                    requestWokflowModel.IsDelted = false;
        //                    requestWokflowModel.IsVisible = true;
        //                    var requestWorkflow = _requestWorkflowService.CreateRequestWorkflow(requestWokflowModel);
        //                    if (requestWorkflow != null)
        //                    {
        //                        RequestWokflowModel requestWokflowModel1 = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(requestWorkflow.Result.EmployeeId, requestWorkflow.Result.BenefitRequestId);
        //                        message = "successful Process, your request will be proceed";
        //                        bool result = SendNotification(benefitRequestModel, requestWokflowModel1, "Request").Result;


        //                        //NotificationModel notificationModel = CreateNotification("Request", requestWokflowModel1);
        //                        //await SendToSpecificUser(requestWokflowModel1, "Request");
        //                    }
        //                    else
        //                    {
        //                        message = "Failed Process, failed to send it";
        //                    }
        //                }
        //                else
        //                {
        //                    message = "Failed Process, There is a problem in this benefit";
        //                }
        //            }
        //            else
        //            {
        //                message = "Failed Process, There is a problem in this benefit";
        //            }
        //        }
        //        else
        //        {
        //            message = "Failed Process, There is a problem in this benefit";
        //        }
        //        return message;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.ToString());
        //        return "There is a problem, please contact with your Technical support";
        //    }
        //}

        public async Task<ActionResult> ShowMyBenefits2()
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<BenefitAPIModel> benefitModels = await _BenefitService.GetMyBenefits(CurrentEmployee.EmployeeNumber, (int)CommanData.Languages.English);

                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                if (benefitModels != null)
                {
                    if (benefitModels.Count > 0)
                    {
                        List<BenefitAPIModel> pendingBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.Pending)).ToList();
                        List<BenefitAPIModel> approvedBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.Approved)).ToList();
                        List<BenefitAPIModel> rejectedBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.Rejected)).ToList();
                        List<BenefitAPIModel> inprogressBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.InProgress)).ToList();
                        List<BenefitAPIModel> cancelledBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.Cancelled)).ToList();
                        MyBenefitsModel myBenefitsModel = new MyBenefitsModel
                        {
                            PendingBenefits = pendingBenefits,
                            ApprovedBenefits = approvedBenefits,
                            RejectedBenefits = rejectedBenefits,
                            InprogressBenefits = inprogressBenefits,
                            CancelledBenefits = cancelledBenefits
                        };
                        return View(myBenefitsModel);
                    }
                    else
                    {
                        ViewBag.Error = "you do not have any benefits";
                        MyBenefitsModel myBenefitsModel = null;
                        return View(myBenefitsModel);
                    }
                }
                else
                {
                    ViewBag.Error = "Failed Process";
                    return RedirectToAction("ERROR404");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<ActionResult> ShowMyBenefits()
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<BenefitAPIModel> benefitModels = await _BenefitService.GetMyBenefits(CurrentEmployee.EmployeeNumber, (int)CommanData.Languages.English);

                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                if (benefitModels != null)
                {
                    if (benefitModels.Count > 0)
                    {
                        //List<BenefitAPIModel> pendingBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.Pending)).ToList();
                        //List<BenefitAPIModel> approvedBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.Approved)).ToList();
                        //List<BenefitAPIModel> rejectedBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.Rejected)).ToList();
                        //List<BenefitAPIModel> inprogressBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.InProgress)).ToList();
                        //List<BenefitAPIModel> cancelledBenefits = benefitModels.Where(b => b.LastStatus == Enum.GetName(typeof(CommanData.BenefitStatus), (int)CommanData.BenefitStatus.Cancelled)).ToList();
                        MyBenefitsViewModel myBenefitsModel = new MyBenefitsViewModel
                        {
                            myBenefits = benefitModels
                        };
                        var giftt = benefitModels
                           .Select(s => new
                           {
                               UserName = s.Name
                           }).Distinct()
                       .ToList();

                        // ViewBag.Vessel = vessels;
                        ViewBag.giftt = giftt;
                        return View(myBenefitsModel);
                    }
                    else
                    {
                        ViewBag.Error = "you do not have any benefits";
                        return View(null);
                    }
                }
                else
                {
                    ViewBag.Error = "Failed Process";
                    return RedirectToAction("ERROR404");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }


        public async Task<ActionResult> ShowMyBenefitRequests(long BenefitId, long requestNumber)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);

                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                var benefitType = await _BenefitService.GetBenefit(BenefitId);
                long benefitTypeId = benefitType.BenefitTypeId;
                List<Request> requests = await _requestWorkflowService.GetMyBenefitRequests(CurrentEmployee.EmployeeNumber, BenefitId, benefitTypeId, (int)CommanData.Languages.English);
                MyRequests myRequests = new MyRequests();
                if (requests != null)
                {
                    requests = requests.OrderByDescending(r => r.Requestedat).ToList();
                    //requests.ForEach(r => r.CreatedBy.ProfilePicture = _EmployeeService.GetEmployee(r.CreatedBy.EmployeeNumber).ProfilePicture);
                    //requests.Where(r => r.benefitId == (long)CommanData.BenefitTypes.Group).ToList().ForEach(r => r.FullParticipantsData.ForEach(f => f.ProfilePicture = _EmployeeService.GetEmployee(f.EmployeeNumber).ProfilePicture));
                    if (requestNumber != -1)
                    {
                        List<Request> arrangedRequests = new List<Request>();
                        var myrequest = requests.Where(r => r.RequestNumber == requestNumber).First();
                        requests.Remove(myrequest);
                        arrangedRequests.AddRange(requests);
                        arrangedRequests.Insert(0, myrequest);
                        myRequests.Requests = arrangedRequests.OrderByDescending(r => r.Requestedat).ToList();
                    }
                    else
                    {
                        myRequests.Requests = requests.OrderByDescending(r => r.Requestedat).ToList();
                    }
                }

                if (requests == null)
                {
                    ViewBag.Error = "Error in the system";
                }

                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                else if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }
                return View(myRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }

        }

        //used

        [HttpPost]
        public async Task<string> GetMyBenefitRequests2(long BenefitId, long requestNumber)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);

                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                var benefitType = await _BenefitService.GetBenefit(BenefitId);
                long benefitTypeId = benefitType.BenefitTypeId;
                List<Request> requests = await _requestWorkflowService.GetMyBenefitRequests(CurrentEmployee.EmployeeNumber, BenefitId, benefitTypeId, (int)CommanData.Languages.English);
                MyRequests myRequests = new MyRequests();
                if (requests != null)
                {
                    requests = requests.OrderByDescending(r => r.Requestedat).ToList();
                    //requests.ForEach(r => r.CreatedBy.ProfilePicture = _EmployeeService.GetEmployee(r.CreatedBy.EmployeeNumber).ProfilePicture);
                    //requests.Where(r => r.benefitId == (long)CommanData.BenefitTypes.Group).ToList().ForEach(r => r.FullParticipantsData.ForEach(f => f.ProfilePicture = _EmployeeService.GetEmployee(f.EmployeeNumber).ProfilePicture));
                    if (requestNumber != -1)
                    {
                        List<Request> arrangedRequests = new List<Request>();
                        var myrequest = requests.Where(r => r.RequestNumber == requestNumber).First();
                        requests.Remove(myrequest);
                        arrangedRequests.AddRange(requests);
                        arrangedRequests.Insert(0, myrequest);
                        myRequests.Requests = arrangedRequests.OrderByDescending(r => r.Requestedat).ToList();
                    }
                    else
                    {
                        myRequests.Requests = requests.OrderByDescending(r => r.Requestedat).ToList();
                    }
                }

                if (requests == null)
                {
                    ViewBag.Error = "Error in the system";
                    return null;
                }

                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                else if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }
                return JsonSerializer.Serialize(myRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }

        }


        public async Task<ActionResult> ShowMyGroups(long BenefitId)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel CurrentEmployee = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetGroupsByEmployeeId(CurrentEmployee.EmployeeNumber).Result;
                List<GroupModel> groupModels = groupEmployeeModels.Where(g => g.Group.BenefitId == BenefitId).Select(g => g.Group).ToList();
                foreach (GroupModel group in groupModels)
                {
                    group.BenefitRequestModel = _benefitRequestService.GetBenefitRequestByGroupId(group.Id);
                    group.BenefitRequestModel.requestWokflowModel = _requestWorkflowService.GetRequestWorkflow(group.BenefitRequestModel.Id).Result.FirstOrDefault();

                }
                return View(groupModels);

            }
            catch (Exception e)
            {
                return null;
            }
        }
        [Authorize(Roles = "HR,Admin,Supervisor,Department Manager,Payroll")]
        public async Task<ActionResult> ShowRequests(long requestNumber = -1)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                ManageRequest manageRequest = new ManageRequest();
                if (userRoles != null)
                {
                    if (userRoles.Contains("Admin"))
                    {
                        //requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows().Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                        //List<DepartmentModel> departmentModels = _departmentService.GetAllDepartments().ToList();
                        //manageRequest.DepartmentModels = new List<DepartmentAPI>();
                        //foreach (var dept in departmentModels)
                        //{
                        //    DepartmentAPI departmentAPI = new DepartmentAPI();
                        //    departmentAPI.Id = dept.Id;
                        //    departmentAPI.Name = dept.Name;
                        //    manageRequest.DepartmentModels.Add(departmentAPI);
                        //}
                        //manageRequest.IsAdmin = true;
                    }
                    if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR") || userRoles.Contains("Payroll"))
                    {
                        requestWokflowModels = await _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber);
                        requestWokflowModels = requestWokflowModels.Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();

                    }
                    manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id, manageRequest).Result;
                    //manageRequest.SelectedDepartmentId = -1;
                    //manageRequest.SelectedRequestStatus = -1;
                    //manageRequest.SelectedTimingId = -1;
                    //manageRequest.SearchDateFrom = DateTime.Today;
                    //manageRequest.SearchDateTo = DateTime.Today;
                    //manageRequest.SelectedBenefitType = -1;
                    //manageRequest.BenefitTypeModels.Insert(0, new BenefitTypeModel { Id = -2, Name="Select Type" }); 
                    if (requestWokflowModels.Count != 0)
                    {
                        if (requestNumber != -1)
                        {
                            var requiredRequest = requestWokflowModels.Where(w => w.BenefitRequestId == requestNumber);
                            if (requiredRequest.Count() == 0)
                            {

                                RequestWokflowModel requiredRequestWokflowModel = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber, requestNumber);
                                requestWokflowModels.Add(requiredRequestWokflowModel);
                                //NotificationModel notificationModel = _notificationService.GetNotificationByRequestWorkflowId(requiredRequestWokflowModel.Id);
                                //if(notificationModel != null)
                                //{
                                //    UserNotificationModel userNotificationModel = _userNotificationService.GetUserNotificationByUserIdAndNotificationId(CurrentUser.Id, notificationModel.Id);
                                //    userNotificationModel.Seen = true;
                                //    _userNotificationService.UpdateUserNotification(userNotificationModel);

                                //}
                            }
                        }
                        requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                        requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                        manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels, (int)CommanData.Languages.English);
                        if (manageRequest.Requests != null)
                        {
                            manageRequest.Requests = manageRequest.Requests.OrderByDescending(r => r.Requestedat).ToList();
                            if (requestNumber != -1)
                            {
                                var myRequest = manageRequest.Requests.Where(r => r.RequestNumber == requestNumber).First();
                                List<Request> requests = new List<Request>();
                                manageRequest.Requests.Remove(myRequest);
                                requests.AddRange(manageRequest.Requests);
                                requests.Insert(0, myRequest);
                                manageRequest.Requests = requests;
                            }
                        }
                    }
                }
                return View(manageRequest);

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }

        }

        [Authorize(Roles = "HR,Admin,Supervisor,Department Manager,Payroll")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowRequests(ManageRequest manageRequest)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(CurrentUser.Id);
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                if (userRoles != null)
                {
                    //if (userRoles.Contains("Admin"))
                    //{
                    //    if (manageRequest.SelectedDepartmentId != -1)
                    //    {
                    //        requestWokflowModels = await _requestWorkflowService.GetAllRequestWorkflows();
                    //        requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.Employee.DepartmentId == manageRequest.SelectedDepartmentId).ToList();
                    //    }
                    //    else
                    //    {
                    //        requestWokflowModels = await _requestWorkflowService.GetAllRequestWorkflows();
                    //    }
                    //    requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
                    //    //List<DepartmentModel> departmentModels = _departmentService.GetAllDepartments().ToList();
                    //    //foreach (var dept in departmentModels)
                    //    //{
                    //    //    DepartmentAPI departmentAPI = new DepartmentAPI();
                    //    //    departmentAPI.Id = dept.Id;
                    //    //    departmentAPI.Name = dept.Name;
                    //    //    manageRequest.DepartmentModels.Add(departmentAPI);
                    //    //}
                    //}
                    if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR") || userRoles.Contains("Admin"))
                    {

                        requestWokflowModels = await _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber);
                        requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
                    }
                    if (requestWokflowModels.Count != 0)
                    {

                        if (manageRequest.employeeNumberSearch != 0)
                        {
                            requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.EmployeeId == manageRequest.employeeNumberSearch).ToList();

                        }

                        if (manageRequest.SelectedRequestStatus != -1)
                        {
                            requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId == manageRequest.SelectedRequestStatus).ToList();
                        }
                        if (manageRequest.SearchDateFrom.ToString("yyyy-MM-dd") != "0001-01-01" && manageRequest.SearchDateTo.ToString("yyyy-MM-dd") != "0001-01-01")
                        {
                            var x = manageRequest.SearchDateFrom.Date;
                            requestWokflowModels = requestWokflowModels.Where(rw => rw.CreatedDate.Date >= manageRequest.SearchDateFrom.Date && rw.CreatedDate.Date <= manageRequest.SearchDateTo.Date).ToList();
                        }
                        if (manageRequest.SelectedBenefitType != -1 && manageRequest.SelectedBenefitType != -2)
                        {
                            requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.Benefit.BenefitTypeId == manageRequest.SelectedBenefitType).ToList();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "You do not have any requests";
                    }
                }
                else
                {
                    ViewBag.Error = "You do not have any requests";
                }
                        manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id, manageRequest).Result;
                        if (requestWokflowModels.Count > 0)
                        {
                            requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                            requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                            manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels, (int)CommanData.Languages.English);
                        }                 
                return View(manageRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }
        public async Task<ActionResult> RequestResponse(long RequestId)
        {
            try
            {
                bool canResponse = false;
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(CurrentUser.Id);
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();

                BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(RequestId);
                //RequestWokflowModel requestWokflowModel =  _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(RequestId,employeeModel.EmployeeNumber);

                List<RequestWokflowModel> requestWokflowModels = await _requestWorkflowService.GetRequestWorkflow(RequestId);
                RequestWokflowModel employeeRequestWokflowModel = requestWokflowModels.Where(rw => rw.EmployeeId == employeeModel.EmployeeNumber).First();
                if (employeeRequestWokflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                {
                    canResponse = true;
                }
                else if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Approved || employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Rejected)
                {
                    List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
                    List<int> workflowOrders = benefitWorkflowModels.Select(bw => bw.Order).ToList();
                    int workflowLevelsCount = benefitWorkflowModels.Count;
                    if (workflowLevelsCount > 1)
                    {
                        int order = benefitWorkflowModels.Where(bw => bw.RoleId == employeeRequestWokflowModel.RoleId).Select(bw => bw.Order).First();
                        int nextOrder = order + 1;
                        string nextRole = benefitWorkflowModels.Where(bw => bw.Order == nextOrder).Select(bw => bw.RoleId).First();
                        if (nextRole != null)
                        {
                            RequestWokflowModel nextEmployeeWorkflow = requestWokflowModels.Where(rw => rw.RoleId == nextRole).First();
                            if (nextEmployeeWorkflow.Status == (int)CommanData.BenefitStatus.Pending && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                            {
                                canResponse = true;
                            }
                            else
                            {
                                canResponse = false;
                            }
                        }
                        else
                        {
                            if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Rejected && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                            {
                                canResponse = true;
                            }
                            else if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Approved)
                            {

                                canResponse = false;
                            }
                        }
                    }
                    else
                    {
                        if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Rejected && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                        {
                            canResponse = true;
                        }
                        else if (employeeRequestWokflowModel.Status == (int)CommanData.BenefitStatus.Approved && employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom >= DateTime.Now)
                        {
                            canResponse = true;

                        }
                        else
                        {
                            canResponse = false;
                        }
                    }
                }
                employeeRequestWokflowModel.ResonseStatuses = resonseStatuses;
                employeeRequestWokflowModel.canResponse = canResponse;
                employeeRequestWokflowModel.BenefitRequest.RequestDateString = employeeRequestWokflowModel.BenefitRequest.RequestDate.ToString("dd-MM-yyyy");
                employeeRequestWokflowModel.BenefitRequest.ExpectedDateFromString = employeeRequestWokflowModel.BenefitRequest.ExpectedDateFrom.ToString("dd-MM-yyyy");
                employeeRequestWokflowModel.BenefitRequest.ExpectedDateToString = employeeRequestWokflowModel.BenefitRequest.ExpectedDateTo.ToString("dd-MM-yyyy");
                return View(employeeRequestWokflowModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public async Task<bool> AddResponse(long requestWorkflowId, int status, string message)
        {
            try
            {
                bool result = false;
                string type = "Response";
                string subject = "";
                string mailMessage = "";
                int order2 = 0;
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(CurrentUser.Id);
                List<string> userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                RequestWokflowModel DBRequestWorkflowModel = _requestWorkflowService.GetRequestWorkflowById(requestWorkflowId);
                var request = _benefitRequestService.GetBenefitRequest(DBRequestWorkflowModel.BenefitRequestId);
                if (request.GroupId == null)
                {
                    if (DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.Pending ||
                   DBRequestWorkflowModel.RequestStatusId == (int)CommanData.BenefitStatus.InProgress)
                    {
                        //DBRequestWorkflowModel.IsVisible = true;
                        //DBRequestWorkflowModel.IsDelted = false;
                        //DBRequestWorkflowModel.UpdatedDate = DateTime.Now;
                        DBRequestWorkflowModel.ReplayDate = DateTime.Now;
                        DBRequestWorkflowModel.Notes = message;
                        bool updateResult = false;
                        BenefitRequestModel benefitRequestModel = _benefitRequestService.GetBenefitRequest(DBRequestWorkflowModel.BenefitRequestId);
                        if (status == 2)
                        {
                            DBRequestWorkflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Rejected;
                            updateResult = await _requestWorkflowService.UpdateRequestWorkflow(DBRequestWorkflowModel);
                            if (updateResult == true)
                            {
                                updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                                if (updateResult == true)
                                {
                                    result = true;
                                    result = await _requestWorkflowService.SendNotification(benefitRequestModel, DBRequestWorkflowModel, type, 0);
                                    if (userRoles.Contains("HR"))
                                    {
                                        result = await _requestWorkflowService.AddSameResponseForAllHRRole(benefitRequestModel.Id, status, message, DBRequestWorkflowModel.EmployeeId);
                                        if (result == true)
                                        {
                                            //replay = "Thank you for kind response";
                                        }
                                        else
                                        {
                                            //replay = "request has been rejected successfully , but there is Problem in in HR workflow";
                                            result = false;
                                        }
                                    }
                                }
                                else
                                {
                                    //replay = "Failed to update Request status, please contact your technical support";
                                    result = false;
                                }
                            }
                            else
                            {
                                //replay = "Failed process";
                                result = false;
                            }
                        }
                        else if (status == 1)
                        {
                            DBRequestWorkflowModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                            updateResult = await _requestWorkflowService.UpdateRequestWorkflow(DBRequestWorkflowModel);

                            if (updateResult == true)
                            {
                                if (userRoles.Contains("HR"))
                                {
                                    result = await _requestWorkflowService.AddSameResponseForAllHRRole(benefitRequestModel.Id, status, message, DBRequestWorkflowModel.EmployeeId);
                                }
                                else
                                {
                                    result = true;
                                }
                                if (result == true)
                                {
                                    List<BenefitWorkflowModel> benefitWorkflowModels = _benefitWorkflowService.GetBenefitWorkflowS(benefitRequestModel.BenefitId);
                                    int benefitWorflowsCount = benefitWorkflowModels.Count;
                                    int Currentorder = benefitWorkflowModels.Where(bw => bw.RoleId == DBRequestWorkflowModel.RoleId).Select(bw => bw.Order).First();
                                    if (Currentorder < benefitWorflowsCount)
                                    {
                                        order2 = _requestWorkflowService.CheckSameWorkflow(Currentorder, benefitRequestModel.BenefitId, DBRequestWorkflowModel, benefitRequestModel.EmployeeId);
                                    }
                                    else
                                    {
                                        order2 = Currentorder;
                                    }
                                    if (order2 != 0)
                                    {
                                        int order = order2 + 1;

                                        if (order > benefitWorflowsCount)
                                        {
                                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.Approved;
                                            updateResult = _benefitRequestService.UpdateBenefitRequest(benefitRequestModel).Result;
                                            if (updateResult == true)
                                            {
                                                //replay = "Thank you for kind response";
                                                result = true;
                                                result = await _requestWorkflowService.SendNotification(benefitRequestModel, DBRequestWorkflowModel, type, 0);
                                                //// Send Mail to Mail List/////////
                                                if (benefitRequestModel.Benefit.HasMails == true)
                                                {
                                                    // _benefitMailService.SendToMailList(benefitRequestModel);
                                                }
                                            }
                                        }
                                        else if (order <= benefitWorflowsCount)
                                        {
                                            benefitRequestModel.RequestStatusId = (int)CommanData.BenefitStatus.InProgress;
                                            updateResult = await _benefitRequestService.UpdateBenefitRequest(benefitRequestModel);
                                            if (updateResult == true)
                                            {
                                                updateResult = await _requestWorkflowService.SendNotification(benefitRequestModel, DBRequestWorkflowModel, type, 0);
                                                //int nextOrder = order + 1;
                                                string messageResult = await _requestWorkflowService.SendReuestToWhoIsConcern(benefitRequestModel.Id, order);
                                                //replay = "Thank you for kind response";
                                                result = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //replay = "Thank you for your response, there is problem in workflow";

                                        //return BadRequest(new { Message = replay, Data = false });
                                        result = false;

                                    }
                                }
                            }
                            else
                            {
                                //replay = "Failed to update Request status, please contact your technical support";
                                //return BadRequest(new { Message = replay, Data = false });
                                result = false;

                            }
                        }
                        else
                        {
                            //return BadRequest(new { Message = "Wrong Response", Data = false });
                            result = false;
                        }
                        if (result == true)
                        {
                            //return Ok(new { Message = "Thanks for your response", Data = true });
                            result = true;


                        }
                        else
                        {
                            // return BadRequest(new { Message = replay, Data = false });
                            result = false;

                        }
                    }
                    else
                    {
                        //return BadRequest(new { Message = "Can not add Response, request status is" + DBRequestWorkflowModel.RequestStatus.Name, Data = false });
                        result = false;

                    }
                }
                else
                {
                    var groupResult = await _requestWorkflowService.GroupRequestResponse((long)request.GroupId, status, message, employeeModel.EmployeeNumber);
                    if (groupResult == true)
                    {
                        //return Ok(new { Message = "Success Process" + DBRequestWorkflowModel.RequestStatus.Name, Data = true });
                        result = true;

                    }
                    else
                    {
                        //return BadRequest(new { Message = "Failed Process", Data = false });
                        result = false;

                    }
                }
                return result;
            }
            catch (Exception e)
            {
                //return BadRequest(new { Message = "Failed Process", Data = false });
                return false;
            }
        }

        //public JsonResult GetEmployeesCanRedeemThisGroupBenefit(string text, long benefitId)
        //{
        //    //EmployeeModel employeeModel = _EmployeeService.GetEmployee(employeeNumber);
        //    List<Participant> participants = new List<Participant>();

        //    //if (employeeModel != null)
        //    //{
        //    BenefitModel benefitModel = _BenefitService.GetBenefit(benefitId);
        //    int EmployeeGroupsCount = 0;
        //    List<EmployeeModel> employeeModels = _EmployeeService.GetAllDirectEmployees().Result.ToList();
        //    List<GroupEmployeeModel> groupEmployeeModels = _groupEmployeeService.GetAllGroupEmployees().Result.ToList();
        //    groupEmployeeModels = _groupEmployeeService.GetAllGroupEmployees().Result.ToList().Where(ge => ge.Employee.FullName.Contains(text) == true).ToList();

        //    foreach (EmployeeModel employee in employeeModels)
        //    {
        //        EmployeeGroupsCount = groupEmployeeModels.Where(ge => ge.EmployeeId == employee.EmployeeNumber &&
        //        ge.Group.BenefitId == benefitId &&
        //       (ge.Group.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled ||
        //        ge.Group.RequestStatusId != (int)CommanData.BenefitStatus.Rejected)).ToList().Count;
        //        if (EmployeeGroupsCount < benefitModel.Times)
        //        {
        //            Participant participant = new Participant();
        //            participant.EmployeeNumber = employee.EmployeeNumber;
        //            participant.FullName = employee.FullName;
        //            participant.ProfilePicture = employee.ProfilePicture;
        //            participants.Add(participant);
        //        }
        //    }
        //    //}
        //    return Json(new { items = participants });

        //    //return Json(new SelectList(participants, "EmployeeNumber", "FullName"));

        //}
        public async Task<ActionResult> ShowAdminRequests()
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(CurrentUser.Id);
               var userRoles = await _userManager.GetRolesAsync(CurrentUser);
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                ManageRequest manageRequest = new ManageRequest();
                //if (userRoles != null)
                //{
                //    if (userRoles.Contains("Admin"))
                //    {
                //        requestWokflowModels = _requestWorkflowService.GetAllRequestWorkflows().Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                //    }
                //    else if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR"))
                //    {
                //        requestWokflowModels = _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber).Where(rw => rw.CreatedDate.Year == DateTime.Now.Year && rw.RequestStatusId == (int)CommanData.BenefitStatus.Pending).ToList();
                //    }
                //if (requestWokflowModels.Count != 0)
                //{
                //    foreach (var request in requestWokflowModels)
                //    {
                //        if (request.BenefitRequest.Benefit.RequiredDocuments != null)
                //        {
                //            List<RequestDocumentModel> requestDocumentModels = _requestDocumentService.GetRequestDocuments(request.BenefitRequestId);
                //            if (requestDocumentModels != null)
                //            {
                //                //string[] documents = new string[requestDocumentModels.Count];
                //                //for (int index = 0; index < requestDocumentModels.Count; index++)
                //                //{
                //                //    documents[index] = string.Concat("/BenefitRequestFiles/", requestDocumentModels[index].fileName);
                //                //    request.Documents = documents;
                //                //}
                //                request.Documents = requestDocumentModels.Select(d => d.fileName).ToArray();
                //            }
                //        }
                //    }
                //requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                //requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                //manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels);
                //}
                //}
                manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id, manageRequest).Result;
                //manageRequest.SelectedDepartmentId = -1;
                //manageRequest.SelectedRequestStatus = -1;
                ////manageRequest.SelectedTimingId = -1;
                //manageRequest.SearchDateFrom = DateTime.Today;
                //manageRequest.SearchDateTo = DateTime.Today;
                //manageRequest.SelectedBenefitType = -1;
                //manageRequest.BenefitTypeModels.Insert(0, new BenefitTypeModel { Id = -2, Name = "Select Type" });

                return View(manageRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowAdminRequests(ManageRequest manageRequest)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(CurrentUser.Id);
                var userRoles = _userManager.GetRolesAsync(CurrentUser).Result.ToList();
                
                List<RequestWokflowModel> requestWokflowModels = new List<RequestWokflowModel>();
                if (userRoles != null)
                {
                    if (userRoles.Contains("Admin"))
                    {
                        if (manageRequest.SelectedDepartmentId != -1)
                        {
                            requestWokflowModels = await _requestWorkflowService.GetAllRequestWorkflows();
                            requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.Employee.DepartmentId == manageRequest.SelectedDepartmentId).ToList();
                        }
                        else
                        {
                            requestWokflowModels = await _requestWorkflowService.GetAllRequestWorkflows();
                        }
                        requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
                        //List<DepartmentModel> departmentModels = _departmentService.GetAllDepartments().ToList();
                        //foreach (var dept in departmentModels)
                        //{
                        //    DepartmentAPI departmentAPI = new DepartmentAPI();
                        //    departmentAPI.Id = dept.Id;
                        //    departmentAPI.Name = dept.Name;
                        //    manageRequest.DepartmentModels.Add(departmentAPI);
                        //}
                    }
                    else if (userRoles.Contains("Supervisor") || userRoles.Contains("Department Manager") || userRoles.Contains("HR"))
                    {

                        requestWokflowModels = await _requestWorkflowService.GetRequestWorkflowByEmployeeNumber(employeeModel.EmployeeNumber);
                        requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId != (int)CommanData.BenefitStatus.Cancelled).ToList();
                    }
                    if (requestWokflowModels.Count != 0)
                    {
                        if (manageRequest.SelectedAll == false)
                        {


                            if (manageRequest.employeeNumberSearch != 0)
                            {
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.EmployeeId == manageRequest.employeeNumberSearch).ToList();

                            }

                            if (manageRequest.SelectedRequestStatus != -1)
                            {
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.RequestStatusId == manageRequest.SelectedRequestStatus).ToList();
                            }
                            if (manageRequest.SelectedTimingId != -1)
                            {
                                if (manageRequest.SelectedTimingId == 1)
                                {
                                    requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate == DateTime.Today).ToList();
                                }
                                else if (manageRequest.SelectedTimingId == 2)
                                {
                                    requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate == DateTime.Today.AddDays(-1)).ToList();
                                }
                                else if (manageRequest.SelectedTimingId == 3)
                                {
                                    int days = DateTime.Now.DayOfWeek - DayOfWeek.Sunday;
                                    DateTime pastDate = DateTime.Now.AddDays(-(days + 7));
                                    requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate >= pastDate && rw.BenefitRequest.RequestDate <= pastDate.AddDays(7)).ToList();
                                }
                                else if (manageRequest.SelectedTimingId == 4)
                                {

                                    requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.RequestDate.Month == DateTime.Today.Month - 1).ToList();
                                }
                            }
                            if (manageRequest.SelectedBenefitType != -1 && manageRequest.SelectedBenefitType != -2)
                            {
                                requestWokflowModels = requestWokflowModels.Where(rw => rw.BenefitRequest.Benefit.BenefitTypeId == manageRequest.SelectedBenefitType).ToList();
                            }
                        }
                        //requestWokflowModels = _requestWorkflowService.EmployeeCanResponse(requestWokflowModels);
                        //requestWokflowModels = _requestWorkflowService.CreateWarningMessage(requestWokflowModels);
                        manageRequest.Requests = _requestWorkflowService.CreateRequestToApprove(requestWokflowModels, (int)CommanData.Languages.English);
                    }

                }
                else
                {
                    ViewBag.Error = "You do not have any requests";
                }
                manageRequest = _requestWorkflowService.CreateManageRequestFilter(CurrentUser.Id, manageRequest).Result;

                return View(manageRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<ActionResult> ShowOneHundredNotifications()
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<NotificationAPIModel> notificationAPIModels = new List<NotificationAPIModel>();
                EmployeeModel employee = _EmployeeService.GetEmployee(employeeModel.EmployeeNumber).Result;
                List<UserNotificationModel> userNotificationModels = _userNotificationService.GetFiftyUserNotification(employee.UserId);
                if (userNotificationModels != null)
                {
                    foreach (var notification in userNotificationModels)
                    {
                        notification.Seen = true;
                        _userNotificationService.UpdateUserNotification(notification);
                    }
                    List<NotificationAPIModel> NotificationAPIModels = new List<NotificationAPIModel>();
                    userNotificationModels = userNotificationModels.OrderByDescending(un => un.CreatedDate).ToList();
                    notificationAPIModels = _userNotificationService.CreateNotificationAPIModel(userNotificationModels, (int)CommanData.Languages.English);
                    notificationAPIModels = notificationAPIModels.ToList();
                }
                else
                {
                    notificationAPIModels = new List<NotificationAPIModel>();

                }

                return View("ShowNotifications", notificationAPIModels);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");

            }



        }


        public async Task<IActionResult> ShowMyGifts(long requestNumber)
        {
            try
            {
                List<Gift> myGifts = new List<Gift>();
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                List<Gift> gifts = await _requestWorkflowService.GetMyGifts(employeeModel.EmployeeNumber, (int)CommanData.Languages.English);
                if (gifts.Count > 0)
                {
                    if (requestNumber != -1)
                    {
                        var requiredGift = gifts.Where(g => g.RequestNumber == requestNumber).First();
                        gifts.Remove(requiredGift);
                        myGifts.AddRange(gifts);
                        myGifts.Insert(0, requiredGift);
                        return View(myGifts);
                    }
                    else
                    {
                        var giftt = gifts
                            .Select(s => new
                            {
                                UserName = s.UserName
                            }).Distinct()
                        .ToList();
                        
                        // ViewBag.Vessel = vessels;
                        ViewBag.giftt = giftt;
                        return View(gifts);
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public bool updateSeenNotification()
        {
            var CurrentUser = _userManager.GetUserAsync(User).Result;
            if (CurrentUser != null)
            {
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeById(CurrentUser.Id);
                bool result = _userNotificationService.UpdateUserUnseenNotification(employeeModel.EmployeeNumber);
                return result;
            }
            else
            {
                return false;
            }
        }
    }

}
