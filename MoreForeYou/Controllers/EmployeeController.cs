using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using MoreForYou.Models.Auth;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Email;
using MoreForYou.Services.Implementation;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace MoreForYou.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _EmployeeService;
        private readonly IPositionService _PostionService;
        private readonly IDepartmentService _DepartmentService;
        private readonly INationalityService _NationalityService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IRequestWorkflowService _requestWorkflowService;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IExcelService _excelService;
        private readonly IEmailSender _emailSender;

        List<GenderModel> genderList = new List<GenderModel>()
            {
                new GenderModel { Id=-1, Name="Select Gender"},
                new GenderModel { Id=1, Name="Male"},
                new GenderModel {Id= 2, Name="Famle"}
            };

        public List<Collar> Collars = new List<Collar>()
        {
            new Collar { Id = -1, Name = "Select Collar" },
            new Collar { Id = 1, Name = "White Collar" },
            new Collar { Id = 2, Name = "Blue Collar" }

        };

        public List<Collar> CollarsSearch = new List<Collar>()
        {
            new Collar { Id = 1, Name = "White Collar" },
            new Collar { Id = 1, Name = "Blue Collar" }

        };

        public List<MartialStatusModel> martialStatusModels = new List<MartialStatusModel>()
        {
            new MartialStatusModel {Id =-1 , Name="Any"},
            new MartialStatusModel {Id =1 , Name="Single"},
            new MartialStatusModel {Id=2, Name="Married" },
            new MartialStatusModel {Id=3, Name="Divorced" },
            new MartialStatusModel {Id=4, Name="Widower"}
        };
        public EmployeeController(IEmployeeService EmployeeService,
            IDepartmentService DepartmentService,
            IPositionService PositionService,
            INationalityService NationalityService,
            IUserService userService,
            IRoleService roleService,
            ICompanyService companyService,
            IRequestWorkflowService requestWorkflowService,
            UserManager<AspNetUser> userManager,
            SignInManager<AspNetUser> signInManager,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            IExcelService excelService,
            IEmailSender emailSender)
        {
            _EmployeeService = EmployeeService;
            _DepartmentService = DepartmentService;
            _PostionService = PositionService;
            _NationalityService = NationalityService;
            _userService = userService;
            _roleService = roleService;
            _companyService = companyService;
            _requestWorkflowService = requestWorkflowService;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _environment = environment;
            _excelService = excelService;
            _emailSender = emailSender;
        }

        public JsonResult DepartmentFilter(long id)
        {
            try
            {
                EmployeeModel Supervisor = new EmployeeModel();
                if (id == 0)
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

        public bool CheckUniqueEmail(string email)
        {
            bool result = false;
            try
            {
                if (email != "")
                {

                    UserModel userModel = _userService.SearchEmail(email).Result;
                    if (userModel == null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public async Task<bool> CheckUniqueSapNumber(long SapNumber)
        {
            bool result = false;
            try
            {
                if (SapNumber != 0)
                {
                    EmployeeModel employeeModel = await _EmployeeService.GetEmployeeBySapNumber(SapNumber);


                    if (employeeModel == null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public bool CheckUniqueEmployeeNumber(long EmployeeNumber)
        {
            bool result = false;
            try
            {
                if (EmployeeNumber != 0)
                {
                    EmployeeModel employeeModel = _EmployeeService.GetEmployee(EmployeeNumber).Result;


                    if (employeeModel == null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public bool CheckUniqueId(string Id)
        {
            bool result = false;
            try
            {
                if (Id != "")
                {
                    EmployeeModel employeeModel = _EmployeeService.GetEmployeeById(Id);
                    if (employeeModel == null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public bool CheckUniquePhoneNumber(string PhoneNumber)
        {
            bool result = false;
            try
            {
                if (PhoneNumber != "")
                {
                    EmployeeModel employeeModel = _EmployeeService.GetEmployeeByPhoneNumber(PhoneNumber);


                    if (employeeModel == null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        private FilterModel PrepareFilterModel(FilterModel filterModel)
        {
            filterModel.DepartmentModels = _DepartmentService.GetAllDepartments();
            filterModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
            filterModel.PositionModels = _PostionService.GetAllPositions().Result;
            filterModel.genderModels = genderList;
            filterModel.MartialStatusModels = martialStatusModels;
            filterModel.CompanyModels = _companyService.GetAllCompanies();
            filterModel.collars = CollarsSearch;
            filterModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Department" });
            filterModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Position" });
            filterModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Nationality" });
            //filterModel.MartialStatusModels.Insert(0, new MartialStatusModel { Id = -1, Name = "Martial Status" });
            filterModel.CompanyModels.Insert(0, new CompanyModel { Id = -1, Code = "Company" });
            //filterModel.collars.Insert(0, new Collar { Id = -1, Name = "Collar" });
            //filterModel.genderModels.Insert(0, new GenderModel { Id = -1, Name = "Select Gender" });
            filterModel.JoinDate = DateTime.Today;
            filterModel.BirthDate = DateTime.Today;
            return filterModel;
        }

        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {
            try
            {

                EmployeeModel employeeModel = new EmployeeModel();
                FilterModel filterModel = new FilterModel();
                filterModel = PrepareFilterModel(filterModel);
                //List<EmployeeModel> employees = _EmployeeService.GetAllEmployees().Result.GetRange(0,100);
                //employees.ForEach(e => e.JoiningDate.ToString("yyyy-MM-dd"));

                return View(filterModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(FilterModel filterModel)
        {
            try
            {
                filterModel.EmployeeModels = _EmployeeService.EmployeesSearch(filterModel);
                if (filterModel.EmployeeModels.Count != 0)
                {
                    filterModel.EmployeeModels.ForEach(e => e.GenderString = Enum.GetName(typeof(CommanData.Gender), e.Gender));
                    filterModel.EmployeeModels.ForEach(e => e.MaritalStatusString = Enum.GetName(typeof(CommanData.MaritialStatus), e.MaritalStatus));
                    filterModel.EmployeeModels.ForEach(e => e.CollarString = Enum.GetName(typeof(CommanData.CollarTypes), e.Collar));
                    filterModel.EmployeeModels.ForEach(e => e.BirthDateString = e.BirthDate.ToShortDateString());
                    filterModel.EmployeeModels.ForEach(e => e.JoiningDateString = e.JoiningDate.ToShortDateString());

                }
                EmployeeModel employeeModel = new EmployeeModel();
                filterModel = PrepareFilterModel(filterModel);
                //filterModel.DepartmentModels = _DepartmentService.GetAllDepartments();
                //filterModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
                //filterModel.PositionModels = _PostionService.GetAllPositions().Result;
                //filterModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Departmrnt" });
                //filterModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Position" });

                //filterModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Nationality" });

                //filterModel.BirthDate = DateTime.Today;
                //List<EmployeeModel> employees = _EmployeeService.GetAllEmployees().Result.GetRange(0,100);
                //employees.ForEach(e => e.JoiningDate.ToString("yyyy-MM-dd"));

                return View(filterModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                employeeModel.DepartmentModels = _DepartmentService.GetAllDepartments();
                employeeModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Select departement" });
                employeeModel.DepartmentId = -1;
                employeeModel.PositionModels = _PostionService.GetAllPositions().Result;
                employeeModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Select Position" });
                employeeModel.PositionId = -1;
                employeeModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
                employeeModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Select Nationality" });
                employeeModel.NationalityId = -1;
                employeeModel.genderModels = genderList;
                employeeModel.genderModels.Insert(0, new GenderModel { Id = -1, Name = "Select Gender" });
                employeeModel.Gender = -1;
                employeeModel.RoleModels = _roleService.GetAllRoles().Result;
                employeeModel.BirthDate = DateTime.Today;
                employeeModel.JoiningDate = DateTime.Today;
                employeeModel.Collars = Collars;
                employeeModel.Companies = _companyService.GetAllCompanies();
                employeeModel.CompanyId = -1;
                return View(employeeModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeModel Model)
        {
            try
            {
                bool hasRole = false;
                UserModel user = new UserModel();
                user.Email = Model.Email;
                user.Password = Model.Password;
                if (Model.AsignedRolesNames != null)
                {
                    user.AsignedRolesNames = Model.AsignedRolesNames;
                    hasRole = true;
                }
                UserModel addedUser = _userService.CreateUser(user, hasRole).Result;
                if (addedUser != null)
                {
                    Model.CreatedDate = DateTime.Now;
                    Model.UpdatedDate = DateTime.Now;
                    Model.IsVisible = true;
                    Model.IsDelted = false;
                    Model.ProfilePicture = "userProfile1.png";
                    Model.UserId = addedUser.id;
                    Model.UserToken = "InitialToken";
                    var response = _EmployeeService.CreateEmployee(Model);
                    if (response.Result == true)
                    {
                        ViewBag.Message = "Employee Added Successfully";
                    }
                    else
                    {
                        AspNetUser aspNetUser = await _userManager.FindByIdAsync(addedUser.id);
                        var result = await _userManager.DeleteAsync(aspNetUser);
                        if (result.Succeeded)
                        {
                            ViewBag.Error = "Invalid data, Can not add your Employee";
                        }
                        else
                        {
                            ViewBag.Error = "Problem Occurred while add your employee Data, contact your support";
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Invalid data, Can not add new User";
                }
                Model.DepartmentModels = _DepartmentService.GetAllDepartments();
                Model.PositionModels = _PostionService.GetAllPositions().Result;
                Model.NationalityModels = _NationalityService.GetAllNationalities().Result;
                Model.genderModels = genderList;
                Model.RoleModels = _roleService.GetAllRoles().Result;
                Model.BirthDate = DateTime.Today;
                Model.JoiningDate = DateTime.Today;
                Model.Collars = Collars;
                Model.Companies = _companyService.GetAllCompanies();
                return View(Model);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Invalid data, Can not add your Employee";
                return View(Model);
            }
        }


        public ActionResult Search(FilterModel filterModel)
        {
            try
            {
                if (filterModel.BirthDate.ToShortDateString() == "1/1/0001")
                {
                    filterModel.BirthDate = DateTime.Today;
                }
                if (filterModel.JoinDate.ToShortDateString() == "1/1/0001")
                {
                    filterModel.BirthDate = DateTime.Today;
                }
                filterModel.EmployeeModels = _EmployeeService.EmployeesSearch(filterModel);
                EmployeeModel employeeModel = new EmployeeModel();
                filterModel = PrepareFilterModel(filterModel);
                //filterModel.DepartmentModels = _DepartmentService.GetAllDepartments();
                //filterModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
                //filterModel.PositionModels = _PostionService.GetAllPositions().Result;
                //filterModel.CompanyModels = _companyService.GetAllCompanies();
                //filterModel.collars = CommanData.Collars;
                //filterModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Departmrnt" });
                //filterModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Position" });
                //filterModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Nationality" });
                //filterModel.MartialStatusModels = CommanData.martialStatusModels;
                //filterModel.genderModels = CommanData.genderModels;
                //filterModel.BirthDate = DateTime.Today;
                //List<EmployeeModel> employees = _EmployeeService.GetAllEmployees().Result.GetRange(0,100);
                //employees.ForEach(e => e.JoiningDate.ToString("yyyy-MM-dd"));

                return View("Edit", filterModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit()
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                FilterModel filterModel = new FilterModel();
                filterModel = PrepareFilterModel(filterModel);
                //filterModel.DepartmentModels = _DepartmentService.GetAllDepartments();
                //filterModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
                //filterModel.PositionModels = _PostionService.GetAllPositions().Result;
                //filterModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Departmrnt" });
                //filterModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Position" });
                //filterModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Nationality" });
                //filterModel.BirthDate = DateTime.Today;
                //List<EmployeeModel> employees = _EmployeeService.GetAllEmployees().Result.GetRange(0,100);
                //employees.ForEach(e => e.JoiningDate.ToString("yyyy-MM-dd"));
                //           var routeValue = new RouteValueDictionary
                //(new { action = "View", controller = "Author" });
                //           return RedirectToRoute(routeValue);
                return RedirectToAction("Search", filterModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }


        public ActionResult EditFromIndex(long EmployeeNumber)
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                FilterModel filterModel = new FilterModel();
                filterModel.EmployeeNumber = EmployeeNumber;
                filterModel.BirthDate = DateTime.Today;
                filterModel.JoinDate = DateTime.Today;
                filterModel.SelectedDepartmentId = -1;
                filterModel.SelectedNationalityId = -1;
                filterModel.SelectedPositionId = -1;
                filterModel.SelectedGenderId = -1;

                if (filterModel.BirthDate.ToShortDateString() == "1/1/0001")
                {
                    filterModel.BirthDate = DateTime.Today;
                }
                if (filterModel.JoinDate.ToShortDateString() == "1/1/0001")
                {
                    filterModel.BirthDate = DateTime.Today;
                }
                filterModel.EmployeeModels = _EmployeeService.EmployeesSearch(filterModel);
                filterModel = PrepareFilterModel(filterModel);
                //filterModel.DepartmentModels = _DepartmentService.GetAllDepartments();
                //filterModel.NationalityModels = _NationalityService.GetAllNationalities().Result;
                //filterModel.PositionModels = _PostionService.GetAllPositions().Result;
                //filterModel.CompanyModels = _companyService.GetAllCompanies();
                //filterModel.collars = CommanData.Collars;
                //filterModel.DepartmentModels.Insert(0, new DepartmentModel { Id = -1, Name = "Departmrnt" });
                //filterModel.PositionModels.Insert(0, new PositionModel { Id = -1, Name = "Position" });
                //filterModel.NationalityModels.Insert(0, new NationalityModel { Id = -1, Name = "Nationality" });
                //filterModel.MartialStatusModels = CommanData.martialStatusModels;
                //filterModel.genderModels = CommanData.genderModels;
                //filterModel.BirthDate = DateTime.Today;
                //List<EmployeeModel> employees = _EmployeeService.GetAllEmployees().Result.GetRange(0,100);
                //employees.ForEach(e => e.JoiningDate.ToString("yyyy-MM-dd"));

                return View("Edit", filterModel);
                //return RedirectToAction("Search", filterModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FilterModel filterModel)
        {
            try
            {
                //EmployeeModel DBemployeeModel = _EmployeeService.GetEmployee(employeeModel.EmployeeNumber);
                //DBemployeeModel.FullName = employeeModel.FullName;
                //DBemployeeModel.Id = employeeModel.Id;
                //DBemployeeModel.SapNumber = employeeModel.SapNumber;
                //DBemployeeModel.Address = employeeModel.Address;
                //DBemployeeModel.PhoneNumber = employeeModel.PhoneNumber;
                //DBemployeeModel.DepartmentId = employeeModel.DepartmentId;
                //DBemployeeModel.PositionId = employeeModel.PositionId;
                //DBemployeeModel.JoiningDate = employeeModel.JoiningDate;
                //DBemployeeModel.BirthDate = employeeModel.BirthDate;
                //DBemployeeModel.MaritalStatus = employeeModel.MaritalStatus;
                //DBemployeeModel.NationalityId = employeeModel.NationalityId;
                //DBemployeeModel.SupervisorId = employeeModel.SupervisorId;
                //DBemployeeModel.UpdatedDate = DateTime.Today;
                //DBemployeeModel.IsVisible = employeeModel.IsVisible;
                //_EmployeeService.UpdateEmployee(DBemployeeModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        public Task<bool> EditEmployee(EditEmployee employeeModel)
        {
            try
            {
                EmployeeModel employee = _EmployeeService.GetEmployee(employeeModel.EmployeeNumber).Result;
                employee.FullName = employeeModel.FullName;
                employee.Address = employeeModel.Address;
                employee.SapNumber = employeeModel.SapNumber;
                employee.Id = employeeModel.Id;
                employee.BirthDate = employeeModel.BirthDate;
                employee.JoiningDate = employeeModel.JoinDate;
                employee.Gender = employeeModel.Gender;
                employee.MaritalStatus = employeeModel.MaritalStatus;
                employee.DepartmentId = employeeModel.DepartmentId;
                employee.PositionId = employeeModel.PositionId;
                employee.CompanyId = employeeModel.CompanyId;
                employee.Collar = employeeModel.Collar;
                employee.isDeptManager = employeeModel.IsDeptManager;
                employee.PhoneNumber = employeeModel.PhoneNumber;
                employee.NationalityId = employeeModel.NationalityId;
                employee.IsDirectEmployee = employeeModel.DirectEmployee;
                bool result = _EmployeeService.UpdateEmployee(employee).Result;
                return Task<bool>.FromResult(result);
            }
            catch (Exception e)
            {
                return Task<bool>.FromResult(false);
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        //public async Task<ActionResult> ChangeSetting()
        //{

        //}

        public async Task<ActionResult> UserProfile(string userid)
        {
            try
            {
                //var user = _userManager.FindByIdAsync(Id);
                EmployeeModel employeeModel = _EmployeeService.GetEmployeeByUserId(userid).Result;
                //employeeModel.GenderString = Enum.GetName();
                employeeModel.MaritalStatusString = Enum.GetName(typeof(CommanData.MaritialStatus), employeeModel.MaritalStatus);
                employeeModel.GenderString = Enum.GetName(typeof(CommanData.Gender), employeeModel.Gender);
                employeeModel.CollarString = Enum.GetName(typeof(CommanData.Gender), employeeModel.Collar);
                employeeModel.BirthDateString = employeeModel.BirthDate.ToString("yyyy-MM-dd");
                employeeModel.ProfilePicturePath = null;
                var user = await _userService.GetUser(employeeModel.UserId);
                employeeModel.Email = user.Email;
                var supervisor = _EmployeeService.GetEmployee((long)employeeModel.SupervisorId).Result;
                employeeModel.SupervisorName = supervisor.FullName;
                employeeModel.userSetting = new UserSetting();
                // employeeModel.ProfilePicture = CommanData.UploadMainFolder + CommanData.ProfileFolder + employeeModel.ProfilePicture;
                return View(employeeModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        //public async Task<ActionResult> MySetting()
        //{
        //    AspNetUser CurrentUser = await _userManager.GetUserAsync(User);
        //    EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(CurrentUser.Id);

        //    UserSetting userSetting = new UserSetting();
        //    userSetting.imagePath = employeeModel.ProfilePicture;
        //    userSetting.userId = employeeModel.UserId;
        //    userSetting.employeeName = employeeModel.FullName;
        //    return View(userSetting);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangePasswoard(UserSetting model)
        //{
        //    EmployeeModel employeeModel = await _EmployeeService.GetEmployeeByUserId(model.userId);
        //    return View(employeeModel);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(EmployeeModel employeeModel)
        {
            //EmployeeModel employee = _EmployeeService.GetEmployee(userSetting.employeeNumber);
            //if (employee != null)
            //{
            try
            {
                AspNetUser aspNetUser = await _userManager.GetUserAsync(User);
                if (aspNetUser != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(aspNetUser, employeeModel.oldPassword, true);
                    if (result.Succeeded)
                    {
                        var result2 = await _userManager.ChangePasswordAsync(aspNetUser, employeeModel.oldPassword, employeeModel.newPassword);
                        if (result2.Succeeded)
                        {
                            TempData["Message"] = "sucess process, your Password has been changed";
                        }
                        else
                        {
                            TempData["Error"] = "Failed Process";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Invaild data";
                    }
                }
                else
                {
                    TempData["Error"] = "Invaild data";
                }
                employeeModel = _EmployeeService.GetEmployeeByUserId(aspNetUser.Id).Result;
                return View("UserProfile", employeeModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfilePicture(EmployeeModel employeeModel)
        {
            //EmployeeModel employee = _EmployeeService.GetEmployee(userSetting.employeeNumber);
            //if (employee != null)
            //{
            try
            {
                AspNetUser aspNetUser = await _userManager.GetUserAsync(User);
                EmployeeModel DBEmployeeModel;
                if (aspNetUser != null)
                {
                    DBEmployeeModel = _EmployeeService.GetEmployeeByUserId(aspNetUser.Id).Result;
                    if (employeeModel.ProfilePicturePath != null)
                    {
                        string imageName = _requestWorkflowService.UploadedImageAsync(employeeModel.ProfilePicturePath, CommanData.ProfileFolder).Result;

                        DBEmployeeModel.ProfilePicture = imageName;
                        var result2 = _EmployeeService.UpdateEmployee(DBEmployeeModel);
                        if (result2.Result == true)
                        {
                            TempData["Message"] = "sucess process, your Profile Picture has been changed";
                            return RedirectToAction("UserProfile", new { userid = DBEmployeeModel.UserId });

                        }
                        else
                        {
                            TempData["Error"] = "Failed Process";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "invalid empty file";
                    }
                }
                else
                {
                    TempData["Error"] = "Invaild data";
                }
                return RedirectToAction("UserProfile", new { userid = employeeModel.UserId });
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveProfilePicture(EmployeeModel employeeModel)
        {
            try
            {

                AspNetUser aspNetUser = await _userManager.GetUserAsync(User);
                if (aspNetUser != null)
                {
                    EmployeeModel DBEmployeeModel = _EmployeeService.GetEmployeeByUserId(aspNetUser.Id).Result;
                    DBEmployeeModel.ProfilePicture = "userProfile1.png";
                    var result = _EmployeeService.UpdateEmployee(DBEmployeeModel);
                    if (result.Result == true)
                    {
                        TempData["Message"] = "sucess process, your Profile Picture has been Removed";
                        return RedirectToAction("UserProfile", new { userid = DBEmployeeModel.UserId });
                    }
                    else
                    {
                        TempData["Error"] = "Failed Process";
                    }
                }
                return RedirectToAction("UserProfile", new { userid = employeeModel.UserId });
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");

            }

        }

        public async Task<ActionResult> ShowBirthDayCelebration()
        {
            AspNetUser aspNetUser = await _userManager.GetUserAsync(User);
            EmployeeModel DBEmployeeModel = new EmployeeModel();
            if (aspNetUser != null)
            {
                DBEmployeeModel = _EmployeeService.GetEmployeeByUserId(aspNetUser.Id).Result;
            }
            return View(DBEmployeeModel);
        }


        public async Task<IActionResult> AddEmployee()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<IActionResult> SendUserData()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        private string PrapareMailBody(string Name, string UserNumber, string Password)
        {
            string body = string.Empty;
            //string filePath = "D:/Cemex Project/gitHubProject/Cemex Backulaing/DevArea/Core/CoreServices/MailService/EmailTemplate.html";

            string fileName = "EmailBodySendUserData.html";
            string path = Path.Combine(@"D:\Work\MoreForYou\More4uDesign\MoreForYou.Services\MailTemplate\", fileName);

            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{employeeNumber}", UserNumber);
            body = body.Replace("{password}", Password);
            body = body.Replace("{employeeName}", Name);
            body = body.Replace("{Title}", "More4U Invitation");

            return body;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendUserData(IFormFile postedFile)
        {
            try
            {
                bool fixResult = false;
                string ExcelConnectionString = this._configuration.GetConnectionString("ExcelCon");
                DataTable dt = null;
                string Email = "";
                string password = "";
                string mailBody = "";
                List<string> to = new List<string>();
                if (postedFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine(this._environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    dt = _excelService.ReadExcelData(filePath, ExcelConnectionString);
                    List<EmployeeUserData> EmployeesData = new List<EmployeeUserData>();
                    if (dt != null)
                    {
                        for (int index = 0; index < dt.Columns.Count; index++)
                        {
                            dt.Columns[index].ColumnName = dt.Columns[index].ColumnName.Trim();
                        }
                        foreach (DataRow row in dt.Rows)
                        {
                            to.Clear();
                            Email = row["Mail"].ToString();
                            to.Add(Email);
                            password = row["Password"].ToString();
                            mailBody = PrapareMailBody(row["FullName"].ToString(), row["EmployeeNumber"].ToString(), password);
                            await _emailSender.SendEmailAsync(mailBody, to, "More4U Invitation", 0, null, null);
                        }
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(IFormFile postedFile)
        {
            List<EmployeeUserData> EmployeesData = new List<EmployeeUserData>();
            try
            {

                bool fixResult = false;
                string ExcelConnectionString = this._configuration.GetConnectionString("ExcelCon");
                DataTable dt = null;
                string Email = "";
                string password = "";
                bool isSupervisorExist;
                bool userCreated = false;
                AspNetUser aspNetUser = null;
                double x = 0;
                int phonelen = 0;
                string phone = "";
                if (postedFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine(this._environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    dt = _excelService.ReadExcelData(filePath, ExcelConnectionString);

                    if (dt != null)
                    {
                        for (int index = 0; index < dt.Columns.Count; index++)
                        {
                            dt.Columns[index].ColumnName = dt.Columns[index].ColumnName.Trim();
                        }
                        x = 72222222222222;
                        // dt.PrimaryKey = new DataColumn[] {dt.Columns["EmployeeNumber"] };
                        foreach (DataRow row2 in dt.Rows)
                        {
                            DataRow row = row2;
                            EmployeeModel employeeModel1 = _EmployeeService.GetEmployee(Convert.ToInt32(row["EmployeeNumber"])).Result;
                            //Email = "NotProvided" + row["EmployeeNumber"] + "@NotProvided.Cemex.com";
                            if (employeeModel1 == null)
                            {
                                x = x + 1;
                                isSupervisorExist = CheckSupervisor(Convert.ToInt64(row["SupervisorId"]));
                                if (isSupervisorExist == true)
                                {
                                    Email = row["Email"].ToString().Trim();
                                    if (Email.Contains("Not Provided"))
                                    {
                                        Email = "NotProvided" + row["EmployeeNumber"] + "@NotProvided.Cemex.com";
                                    }
                                    password = _EmployeeService.CreateRandomPassword(7);
                                    var user = _userManager.FindByEmailAsync(Email).Result;
                                    if (user == null)
                                    {
                                        aspNetUser = new AspNetUser { Email = Email, UserName = Email };
                                        var result = await _userManager.CreateAsync(aspNetUser, password);
                                        if (result.Succeeded)
                                            userCreated = true;
                                    }
                                    else
                                    {
                                        //aspNetUser = _userManager.FindByEmailAsync(Email).Result;
                                        userCreated = false;
                                    }
                                    if (userCreated == true)
                                    {
                                        phonelen = row["PhoneNumber"].ToString().Length;
                                        if (phonelen < 11)
                                        {
                                            phone = "0" + row["PhoneNumber"].ToString();
                                        }
                                        else
                                        {
                                            phone = row["PhoneNumber"].ToString();
                                        }
                                        //row["PhoneNumber"] = "0" + row["PhoneNumber"].ToString();
                                        //await _userManager.AddToRoleAsync(aspNetUser, "Driver");
                                        EmployeeModel employeeModel = new EmployeeModel();
                                        employeeModel.EmployeeNumber = Convert.ToInt64(row["EmployeeNumber"]);
                                        employeeModel.FullName = row["FullName"].ToString();
                                        employeeModel.SapNumber = Convert.ToInt64(row["SapNumber"]);
                                        employeeModel.DepartmentId = Convert.ToInt64(row["DepartmentId"]);
                                        employeeModel.PositionId = Convert.ToInt64(row["PositionId"]);
                                        employeeModel.PhoneNumber = phone;
                                        employeeModel.Address = row["Address"].ToString();
                                        employeeModel.SupervisorId = Convert.ToInt32(row["SupervisorId"]);
                                        employeeModel.Id = x.ToString();
                                        employeeModel.Gender = Convert.ToInt32(row["Gender"]);
                                        employeeModel.JoiningDate = Convert.ToDateTime(row["JoiningDate"]);
                                        employeeModel.BirthDate = Convert.ToDateTime(row["BirthDate"]);
                                        employeeModel.MaritalStatus = Convert.ToInt32(row["MaritalStatus"]);
                                        employeeModel.isDeptManager = Convert.ToBoolean(row["isDeptManager"]);
                                        employeeModel.NationalityId = Convert.ToInt32(row["NationalityId"]);
                                        employeeModel.HRId = Convert.ToInt32(row["HRId"]);
                                        employeeModel.ProfilePicture = row["ProfilePicture"].ToString();
                                        employeeModel.Collar = Convert.ToInt32(row["Collar"]);
                                        employeeModel.CompanyId = Convert.ToInt32(row["CompanyId"]);
                                        employeeModel.UserToken = row["UserToken"].ToString();
                                        employeeModel.Country = row["Country"].ToString();
                                        employeeModel.HasChilderen = Convert.ToBoolean(row["HasChilderen"]);
                                        employeeModel.IsDirectEmployee = Convert.ToBoolean(row["IsDirectEmployee"]);
                                        employeeModel.IsDelted = false;
                                        employeeModel.IsVisible = true;
                                        employeeModel.UpdatedDate = DateTime.Now;
                                        employeeModel.CreatedDate = DateTime.Now;
                                        employeeModel.UserId = aspNetUser.Id;
                                        var createResult = await _EmployeeService.CreateEmployee(employeeModel);
                                        if (createResult == true)
                                        {
                                            EmployeeUserData employeeData = new EmployeeUserData()
                                            {
                                                Number = employeeModel.EmployeeNumber,
                                                FullName = employeeModel.FullName,
                                                Password = password,
                                                Email = Email
                                            };
                                            EmployeesData.Add(employeeData);
                                        }
                                        else
                                        {
                                            if (EmployeesData.Count > 0)
                                            {
                                                var memoryStream = _excelService.ExportDriversDataToExcel(EmployeesData);
                                                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportDriverData.xlsx");
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (EmployeesData.Count > 0)
                    {
                        var memoryStream = _excelService.ExportDriversDataToExcel(EmployeesData);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportDriverData.xlsx");
                    }
                }
                else
                {
                    ViewBag.Error = "File Not Uploaded, Please Select Valid File";
                    if (EmployeesData.Count > 0)
                    {
                        var memoryStream = _excelService.ExportDriversDataToExcel(EmployeesData);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportDriverData.xlsx");
                    }
                    // return View();
                }
                return View();
            }
            catch (Exception e)
            {
                if (EmployeesData.Count > 0)
                {
                    var memoryStream = _excelService.ExportDriversDataToExcel(EmployeesData);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportDriverData.xlsx");
                }
                return RedirectToAction("ERROR404");
            }
        }

        public bool CheckSupervisor(long supervisorId)
        {
            try
            {
                EmployeeModel employeeModel = _EmployeeService.GetEmployee(supervisorId).Result;
                if (employeeModel != null)
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


    }
}
