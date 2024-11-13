using Data.Repository;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using MimeKit;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Implementation;
using MoreForYou.Services.Implementation.MedicalServices;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using MoreForYou.Services.Models.Medical;
using MoreForYou.Services.Models.MedicalRequestViewModels;
using Repository.EntityFramework;
using Repository.EntityFramework.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreForYou.Controllers.MedicalRequests
{
    public class MedicalRequestController : Controller
    {
        private readonly IMedicalRequestService _medicalRequest;
        private readonly IRoleService _roleService;
        private readonly ILogger<MedicalRequestController> _logger;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly IRelativeService _rentativeService;
        private readonly IMedicalCategoryService _medicalCategoryService;
        public APPDBContext _context;
        private readonly IRepository<MedicalItem, long> _itemsRepository;
        private readonly IMedicalResponseService _medicalResponse;
       // private readonly IRelativeService _relativeService;
        public MedicalRequestController(IRoleService roleService,
          ILogger<MedicalRequestController> logger,
          UserManager<AspNetUser> userManager,
          IRelativeService rentativeService, IMedicalCategoryService categoryService,
          IEmployeeService employeeService, IMedicalRequestService medicalRequest, APPDBContext context, 
          IRepository<MedicalItem, long> itemsRepository, IMedicalResponseService medicalResponse)
        {
            _roleService = roleService;
            _logger = logger;
            _userManager = userManager;
            _employeeService = employeeService;
            _medicalRequest = medicalRequest;
            _rentativeService = rentativeService;
            _medicalCategoryService = categoryService;
            _context = context;
            _itemsRepository = itemsRepository;
            _medicalResponse = medicalResponse;
        }
        public async Task<IActionResult> IndexAsync()
        {
            AspNetUser applicationUser = await _userManager.GetUserAsync(User);           
            var roles = await _userManager.GetRolesAsync(applicationUser);
            EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;
            //MedicalRequestDetailsModel result = _medicalRequest.GetMedicalRequestsDetailsAsync(225, 14869, 1).Result;
            if (roles.Contains("Doctor"))
            {
                return RedirectToAction("Doctor");
            }
            else if (roles.Contains("MedicalAdmin"))
            {
                return RedirectToAction("Doctor");
               // return RedirectToAction("MedicalAdmin");
            }
            else
            {
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }

                else if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }
                PendingRequestVModel model = new PendingRequestVModel();
                model.pendingRequest = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                if (model.pendingRequest == null)
                {
                    PendingRequestApiModel pendingRequestApiModel2 = new PendingRequestApiModel();
                    PendingRequestCount pendingRequestCount = new PendingRequestCount();
                    pendingRequestCount.totalRequest = "0";
                    pendingRequestCount.sickleave = "0";
                    pendingRequestCount.checkups = "0";
                    pendingRequestCount.medications = "0";
                    List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
                    pendingRequestApiModel2.requestsCount = pendingRequestCount;
                    pendingRequestApiModel2.requests = requestSummeyModelList;
                    model.pendingRequest = pendingRequestApiModel2;
                }
                return View(model);
            }
            //  return View();
        }
        bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if(pi.GetValue(myObject)!=null)/* (pi.PropertyType == typeof(string))*/
                {
                    //string value = (string)pi.GetValue(myObject);
                    //if (!string.IsNullOrEmpty(value))
                    //{
                        return true;
                   // }
                }
            }
            return false;
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(MedicalRequestSearchModel searchModel)
        {
           
            var result=IsAnyNullOrEmpty(searchModel);
            if (result)
            {
                AspNetUser applicationUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;
                MedicalRequestSearch medicalRequestSearch = new MedicalRequestSearch();
                if (searchModel.selectedRequestType == null)
                {
                    medicalRequestSearch.selectedRequestType = "";
                }
                else
                {
                    medicalRequestSearch.selectedRequestType = searchModel.selectedRequestType;
                }

                if (searchModel.selectedRequestStatus == null)
                {
                    medicalRequestSearch.selectedRequestStatus = "";
                }
                else
                {
                    medicalRequestSearch.selectedRequestStatus = searchModel.selectedRequestStatus;
                }
                if (searchModel.requestId == null)
                {
                    medicalRequestSearch.requestId = "";
                }
                else
                {
                    medicalRequestSearch.requestId = searchModel.requestId.ToString();
                }

                if (searchModel.searchDateFrom == null)
                {
                    medicalRequestSearch.searchDateFrom = "";
                }
                else
                {
                    medicalRequestSearch.searchDateFrom = searchModel.searchDateFrom.ToString();
                }

                if (searchModel.searchDateTo == null)/*.ToString("yyyy-MM-dd") == "0001-01-01")*/
                {
                    medicalRequestSearch.searchDateTo = "";
                }
                else
                {
                    medicalRequestSearch.searchDateTo = searchModel.searchDateTo.ToString();
                }
                medicalRequestSearch.userNumberSearch = employeeModel.EmployeeNumber.ToString();
                medicalRequestSearch.userNumber = employeeModel.EmployeeNumber.ToString();
                medicalRequestSearch.languageId = "1";
                medicalRequestSearch.relativeId = "";

                // PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                PendingRequestVModel model = new PendingRequestVModel();
                model.pendingRequest = _medicalRequest.GetAllEmployeeMedicalRequests(medicalRequestSearch);
                if (model.pendingRequest == null)
                {
                    PendingRequestApiModel pendingRequestApiModel2 = new PendingRequestApiModel();
                    PendingRequestCount pendingRequestCount = new PendingRequestCount();
                    pendingRequestCount.totalRequest = "0";
                    pendingRequestCount.sickleave = "0";
                    pendingRequestCount.checkups = "0";
                    pendingRequestCount.medications = "0";
                    List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
                    pendingRequestApiModel2.requestsCount = pendingRequestCount;
                    pendingRequestApiModel2.requests = requestSummeyModelList;
                    model.pendingRequest = pendingRequestApiModel2;
                }
                else
                {
                    PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                    model.pendingRequest.requestsCount = medicalRequestsBy.requestsCount;
                }
                model.searchModel = searchModel;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
            //  return View();
        }

        public async Task<IActionResult> MedicalAdminAsync()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            else if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            AspNetUser applicationUser = await _userManager.GetUserAsync(User);
           
            EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;
           
           // PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
            PendingRequestVModel model = new PendingRequestVModel();
            model.participants = new List<EmployeeApiModel>();
            foreach (EmployeeModel allDirectEmployee in await _employeeService.GetAllDirectEmployees())
                model.participants.Add(new EmployeeApiModel()
                {
                    name = allDirectEmployee.FullName,
                    employeeNumber = allDirectEmployee.EmployeeNumber.ToString()
                });
            model.pendingRequest = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
            if (model.pendingRequest == null)
            {
                PendingRequestApiModel pendingRequestApiModel2 = new PendingRequestApiModel();
                PendingRequestCount pendingRequestCount = new PendingRequestCount();
                pendingRequestCount.totalRequest = "0";
                pendingRequestCount.sickleave = "0";
                pendingRequestCount.checkups = "0";
                pendingRequestCount.medications = "0";
                List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
                pendingRequestApiModel2.requestsCount = pendingRequestCount;
                pendingRequestApiModel2.requests = requestSummeyModelList;
                model.pendingRequest = pendingRequestApiModel2;
            }
            return View("MedicalAdmin",model);
        }

        [HttpPost]
        public async Task<IActionResult> MedicalAdminAsync(MedicalRequestSearchModel searchModel)
        {

            var result = IsAnyNullOrEmpty(searchModel);
            if (result)
            {
                AspNetUser applicationUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;
                MedicalRequestSearch medicalRequestSearch = new MedicalRequestSearch();
                if (searchModel.selectedRequestType == null)
                {
                    medicalRequestSearch.selectedRequestType = "";
                }
                else
                {
                    medicalRequestSearch.selectedRequestType = searchModel.selectedRequestType;
                }

                if (searchModel.selectedRequestStatus == null)
                {
                    medicalRequestSearch.selectedRequestStatus = "";
                }
                else
                {
                    medicalRequestSearch.selectedRequestStatus = searchModel.selectedRequestStatus;
                }
                if (searchModel.requestId == null)
                {
                    medicalRequestSearch.requestId = "";
                }
                else
                {
                    medicalRequestSearch.requestId = searchModel.requestId.ToString();
                }

                if (searchModel.searchDateFrom == null)
                {
                    medicalRequestSearch.searchDateFrom = "";
                }
                else
                {
                    medicalRequestSearch.searchDateFrom = searchModel.searchDateFrom.ToString();
                }

                if (searchModel.searchDateTo == null)/*.ToString("yyyy-MM-dd") == "0001-01-01")*/
                {
                    medicalRequestSearch.searchDateTo = "";
                }
                else
                {
                    medicalRequestSearch.searchDateTo = searchModel.searchDateTo.ToString();
                }
                if (searchModel.userNumberSearch == null)/*.ToString("yyyy-MM-dd") == "0001-01-01")*/
                {
                    medicalRequestSearch.userNumberSearch = "";
                }
                else
                {
                    medicalRequestSearch.userNumberSearch = searchModel.userNumberSearch.ToString();
                }
                //medicalRequestSearch.userNumberSearch = employeeModel.EmployeeNumber.ToString();
                medicalRequestSearch.userNumber = employeeModel.EmployeeNumber.ToString();
                medicalRequestSearch.languageId = "1";
                medicalRequestSearch.relativeId = "";

                // PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                PendingRequestVModel model = new PendingRequestVModel();
                model.pendingRequest = _medicalRequest.GetAllEmployeeMedicalRequests(medicalRequestSearch);
                if (model.pendingRequest == null)
                {
                    PendingRequestApiModel pendingRequestApiModel2 = new PendingRequestApiModel();
                    PendingRequestCount pendingRequestCount = new PendingRequestCount();
                    pendingRequestCount.totalRequest = "0";
                    pendingRequestCount.sickleave = "0";
                    pendingRequestCount.checkups = "0";
                    pendingRequestCount.medications = "0";
                    List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
                    pendingRequestApiModel2.requestsCount = pendingRequestCount;
                    pendingRequestApiModel2.requests = requestSummeyModelList;
                    model.pendingRequest = pendingRequestApiModel2;
                }
                //else
                //{
                PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                if(medicalRequestsBy == null)
                {
                    model.pendingRequest.requestsCount = new PendingRequestCount();
                    model.pendingRequest.requestsCount.checkups = "0";
                    model.pendingRequest.requestsCount.totalRequest = "0";
                    model.pendingRequest.requestsCount.medications = "0";
                    model.pendingRequest.requestsCount.sickleave="0";
                }
                else
                {
                    model.pendingRequest.requestsCount = medicalRequestsBy.requestsCount;
                }
                
                // }
                model.searchModel = searchModel;
                return View("MedicalAdmin",model);
            }
            else
            {
                return RedirectToAction("Index");
            }
            //  return View();
        }
        public async Task<IActionResult> DoctorAsync()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            AspNetUser applicationUser = await _userManager.GetUserAsync(User);
            
            EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;
           // PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
            PendingRequestVModel model = new PendingRequestVModel();
            model.pendingRequest = _medicalRequest.GetAllMedicalRequestsByType(4, 1);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DoctorAsync(MedicalRequestSearchModel searchModel)
        {

            var result = IsAnyNullOrEmpty(searchModel);
            if (result)
            {
                AspNetUser applicationUser = await _userManager.GetUserAsync(User);
                EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;
                MedicalRequestSearch medicalRequestSearch = new MedicalRequestSearch();
                if (searchModel.selectedRequestType == null)
                {
                    medicalRequestSearch.selectedRequestType = "";
                }
                else
                {
                    medicalRequestSearch.selectedRequestType = searchModel.selectedRequestType;
                }

                if (searchModel.selectedRequestStatus == null)
                {
                    medicalRequestSearch.selectedRequestStatus = "";
                }
                else
                {
                    medicalRequestSearch.selectedRequestStatus = searchModel.selectedRequestStatus;
                }
                if (searchModel.requestId == null)
                {
                    medicalRequestSearch.requestId = "";
                }
                else
                {
                    medicalRequestSearch.requestId = searchModel.requestId.ToString();
                }

                if (searchModel.searchDateFrom == null)
                {
                    medicalRequestSearch.searchDateFrom = "";
                }
                else
                {
                    medicalRequestSearch.searchDateFrom = searchModel.searchDateFrom.ToString();
                }

                if (searchModel.searchDateTo == null)/*.ToString("yyyy-MM-dd") == "0001-01-01")*/
                {
                    medicalRequestSearch.searchDateTo = "";
                }
                else
                {
                    medicalRequestSearch.searchDateTo = searchModel.searchDateTo.ToString();
                }
                if (searchModel.userNumberSearch == null)/*.ToString("yyyy-MM-dd") == "0001-01-01")*/
                {
                    medicalRequestSearch.userNumberSearch = "";
                }
                else
                {
                    medicalRequestSearch.userNumberSearch = searchModel.userNumberSearch.ToString();
                }
                //medicalRequestSearch.userNumberSearch = employeeModel.EmployeeNumber.ToString();
                medicalRequestSearch.userNumber = employeeModel.EmployeeNumber.ToString();
                medicalRequestSearch.languageId = "1";
                medicalRequestSearch.relativeId = "";

                // PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                PendingRequestVModel model = new PendingRequestVModel();
                model.pendingRequest = _medicalRequest.GetAllEmployeeMedicalRequests(medicalRequestSearch);
                if (model.pendingRequest == null)
                {
                    PendingRequestApiModel pendingRequestApiModel2 = new PendingRequestApiModel();
                    PendingRequestCount pendingRequestCount = new PendingRequestCount();
                    pendingRequestCount.totalRequest = "0";
                    pendingRequestCount.sickleave = "0";
                    pendingRequestCount.checkups = "0";
                    pendingRequestCount.medications = "0";
                    List<PendingRequestSummeyModel> requestSummeyModelList = new List<PendingRequestSummeyModel>();
                    pendingRequestApiModel2.requestsCount = pendingRequestCount;
                    pendingRequestApiModel2.requests = requestSummeyModelList;
                    model.pendingRequest = pendingRequestApiModel2;
                }
                //else
                //{
                PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetAllMedicalRequestsByType(4, 1);// _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                                                                                                             // model.pendingRequest.requestsCount = medicalRequestsBy.requestsCount;
                if (medicalRequestsBy == null)
                {
                    model.pendingRequest.requestsCount = new PendingRequestCount();
                    model.pendingRequest.requestsCount.checkups = "0";
                    model.pendingRequest.requestsCount.totalRequest = "0";
                    model.pendingRequest.requestsCount.medications = "0";
                    model.pendingRequest.requestsCount.sickleave = "0";
                }
                else
                {
                    model.pendingRequest.requestsCount = medicalRequestsBy.requestsCount;
                }

                // }
                model.searchModel = searchModel;
                return View("Doctor", model);
            }
            else
            {
                return RedirectToAction("Index");
            }
            //  return View();
        }

        public async Task<ActionResult> addRequest()
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(CurrentUser);
                EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                MedicalRequestVM model= new MedicalRequestVM();
                EmployeeRelativesApi result1 = _rentativeService.GetEmployeeRelativesApi(employeeModel.EmployeeNumber, 1, 1, employeeModel.Country).Result;
                EmployeeRelativesApi result2 = _rentativeService.GetEmployeeRelativesApi(employeeModel.EmployeeNumber, 1, 2, employeeModel.Country).Result;
                List<MedicalCategoryModel> medicalCategoryModels = _medicalCategoryService.GetAllMedicalCategories().Result;
                model.RelativesApiModel = result1.RelativesApiModel;
                model.medicationEntities = result1.medicalDetailsAPIModels;
                model.CheckupCategoryAPIModels = medicalCategoryModels.Where(t => t.Id != 4).ToList();
                model.CheckupSubCategoryAPIModels = result2.medicalSubCategoryAPIModels;
                model.CheckupDetailsAPIModels = result2.medicalDetailsAPIModels;
                if (roles.Contains("MedicalAdmin") || roles.Contains("Doctor"))
                {
                    return View("AdminMedicalRequest", model);
                }
                else
                {
                    return View("MedicalRequest", model);
                }



            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        public async Task<ActionResult> addRequest(MedicalRequestApiModel mRequest)
        {
            try
            {
                AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(CurrentUser);
                EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(CurrentUser.Id).Result;
                mRequest.LanguageId = "1";
                mRequest.requestDate = DateTime.Now;
                var result = _medicalRequest.CreateMedicalRequestModelAsync(mRequest).Result;
                if (result == null)
                    return RedirectToAction("ERROR404");
                if (mRequest.attachment == null)
                    mRequest.attachment = new List<IFormFile>();
                var medicalRequest = _medicalRequest.addMedicalRequest(result, mRequest.attachment);
                if (medicalRequest == null)
                {
                    TempData["Error"] = "Failed Process, Vessel has been added, but related qty can not be added ";
                }
                else
                {
                    TempData["Message"] = "Request has been added & Request ID = "+medicalRequest.Id;
                }
                if (roles.Contains("MedicalAdmin"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }                  
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<ActionResult> Details(long id)
        {
            try
            {
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                AspNetUser applicationUser = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(applicationUser);
                EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;

                MedicalRequestDetailsModel result = _medicalRequest.GetMedicalRequestsDetailsAsync(id, employeeModel.EmployeeNumber, 1).Result;
                
               
               
                if (result == null)
                {
                    return RedirectToAction("Index");
                }
                EmployeeModel result1 = _employeeService.GetEmployee(Convert.ToInt64(result.MedicalRequest.requestedByNumber)).Result;
                ViewBag.Picture = "https://more4u.cemex.com.eg/more4u/images/userProfile/"+ result1.ProfilePicture;

                if (roles.Contains("Doctor"))
                {
                    if(result.MedicalResponse.medicalItems==null)
                    {
                        result.MedicalResponse.medicalItems = new List<MedicalItemsAPIModel>();
                    }
                    else if(result.MedicalResponse.medicalItems.Count>0 && result.RequestStatus=="Pending")
                    {
                        result.MedicalResponse.medicalItems = new List<MedicalItemsAPIModel>();
                        string category = "";
                        if (result.MedicalRequest.requestType == 1)
                        {
                            category = "Medication";
                        }
                        else
                        {
                            category = "CheckUp";
                        }
                        var itemList = _itemsRepository.Find(t => t.IsVisible == true && t.Category == category).ToList();

                        if (itemList.Count > 0)
                        {
                            foreach (var item in itemList)
                            {
                                MedicalItemsAPIModel medical = new MedicalItemsAPIModel()
                                {
                                    itemId = item.Id.ToString(),
                                    itemName = item.DrugName + "," + item.Dose,
                                    itemType = item.Type,
                                    itemQuantity = "0", //item.Quantity,
                                    itemDose = item.Dose
                                };
                                result.MedicalResponse.medicalItems.Add(medical);
                            }
                        }
                    }
                  
                   
                    return View("DetailsForDoctor",result);
                }
                return View("RequestDetails",result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        public async Task<string> GetItems(string Id)
        {
            try
            {
                string[] insertedEmployeeNumbersString = new string[] { };
                if (Id !=null && Id !="")
                {
                    string[] insertedEmployeeNumbersString2 = Id.Split(";");
                    insertedEmployeeNumbersString= insertedEmployeeNumbersString2.ToArray();
                }
                List<MedicalItem>medicalItems = new List<MedicalItem>();
                if (insertedEmployeeNumbersString !=null)
                {
                    if (insertedEmployeeNumbersString.Length > 0)
                    {
                        foreach(var item  in insertedEmployeeNumbersString)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var medItem = _context.MedicalItems.Where(t => t.Id == Convert.ToInt64(item)).FirstOrDefault();
                                medicalItems.Add(medItem);
                            }
                           
                        }
                    }

                }
                    return JsonSerializer.Serialize(medicalItems);
                
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<string> AddResponse(MedicalResponseApiModel responseModel,List<MedicalItem>?items)
        {
            try
            {
                if (items != null)
                {
                    if(items.Count > 0)
                    {
                        responseModel.medicalItems=new List<MedicalItemsAPIModel>();
                        foreach (var item in items)
                        {
                            MedicalItemsAPIModel medical=new MedicalItemsAPIModel();
                            medical.itemId = item.Id.ToString();
                            medical.itemName = item.DrugName;
                            medical.itemQuantity = item.Quantity;
                            medical.itemDose = item.Dose;
                            responseModel.medicalItems.Add(medical);
                        }
                    }
                }
                AspNetUser applicationUser = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(applicationUser);
                EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;
                if(employeeModel != null)
                {
                    responseModel.createdBy=employeeModel.EmployeeNumber.ToString();
                }
                responseModel.LanguageId = "1";
                responseModel.responseDate=DateTime.Now.ToString();
                if(roles.Contains("Doctor"))
                {
                    var result = _medicalResponse.addMedicalResponseAsync(responseModel);
                    if(result != null)
                    {
                        if (responseModel.status == "4")
                        {
                            TempData["Message"] = "Request has been Rejected & Request ID = " + responseModel.requestId;
                        }
                        else
                        {
                            TempData["Message"] = "Request has been Approved & Request ID = " + responseModel.requestId;
                        }
                        return JsonSerializer.Serialize(responseModel.requestId);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<string> GetEmpRelatives(string Id)
        {
            try
            {
                EmployeeModel result1 = _employeeService.GetEmployee(Convert.ToInt64(Id)).Result;
                if(result1 == null) 
                {
                    return null;
                }
                else if (!result1.IsDirectEmployee)
                {
                    return null;
                }
                else if (!(bool)result1.HasMedicalService)
                {
                    return null;
                }
                else
                {
                    EmployeeRelativesApi result2 = _rentativeService.GetEmployeeRelativesApi(Convert.ToInt64(Id), 1, 1, result1.Country).Result;
                    if (result2 == null)
                    {
                        return null;
                    }
                    else
                    {
                        result2.RelativesApiModel.ProfilePicture = CommanData.Url + CommanData.ProfileFolder + result1.ProfilePicture;
                        result2.RelativesApiModel.EmployeeDepartment = result1.Department.Name;
                        return JsonSerializer.Serialize(result2.RelativesApiModel);
                    }

                }
               
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
