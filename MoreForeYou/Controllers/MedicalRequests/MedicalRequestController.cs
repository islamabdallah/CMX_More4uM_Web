using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Implementation;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        public MedicalRequestController(IRoleService roleService,
          ILogger<MedicalRequestController> logger,
          UserManager<AspNetUser> userManager,
          IEmployeeService employeeService, IMedicalRequestService medicalRequest)
        {
            _roleService = roleService;
            _logger = logger;
            _userManager = userManager;
            _employeeService = employeeService;
            _medicalRequest = medicalRequest;
        }
        public async Task<IActionResult> IndexAsync()
        {
            AspNetUser applicationUser = await _userManager.GetUserAsync(User);
            // var user = await userManager.GetUserAsync(User);
            // var displayName = user.UserName;
            var roles = await _userManager.GetRolesAsync(applicationUser);
            EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;

            if (roles.Contains("Doctor"))
            {
                return RedirectToAction("Doctor");
            }
            else if (roles.Contains("MedicalAdmin"))
            {
                return RedirectToAction("MedicalAdmin");
            }
            else
            {
               
                PendingRequestVModel model = new PendingRequestVModel();
                model.pendingRequest = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
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
                //else
                //{
                    PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                    model.pendingRequest.requestsCount = medicalRequestsBy.requestsCount;
               // }
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
                model.pendingRequest.requestsCount = medicalRequestsBy.requestsCount;
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
            AspNetUser applicationUser = await _userManager.GetUserAsync(User);
            
            EmployeeModel employeeModel = _employeeService.GetEmployeeByUserId(applicationUser.Id).Result;
           // PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
            PendingRequestVModel model = new PendingRequestVModel();
            model.pendingRequest = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
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
                PendingRequestApiModel medicalRequestsBy = _medicalRequest.GetEmployeeMedicalRequestsBy(employeeModel.EmployeeNumber, 1);
                model.pendingRequest.requestsCount = medicalRequestsBy.requestsCount;
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
    }
}
