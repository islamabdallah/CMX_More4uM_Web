using Microsoft.AspNetCore.Mvc;
using MoreForYou.Services.Models.API.Medical;
using MoreForYou.Services.Models.Message;
using MoreForYou.Services.Models;
using System;
using MoreForYou.Services.Models.MaterModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MoreForYou.Controllers.MedicalRequests;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Contracts;
using Data.Repository;
using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Services.Implementation.MedicalServices;
using Repository.EntityFramework;
using System.Threading.Tasks;
using MoreForYou.Services.Models.MasterModels;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MoreForYou.Controllers
{
    public class RelativeController : Controller
    {
        private readonly IMedicalRequestService _medicalRequest;
        private readonly IRoleService _roleService;
        private readonly ILogger<RelativeController> _logger;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly IRelativeService _relativeService;
        public APPDBContext _context;
        private readonly IRepository<Relative, long> _repository;
        public RelativeController(IRoleService roleService,
        ILogger<RelativeController> logger,
        UserManager<AspNetUser> userManager,
        IRelativeService rentativeService, 
        IEmployeeService employeeService, IMedicalRequestService medicalRequest, APPDBContext context, IRepository<Relative, long> repository)
        {
            _roleService = roleService;
            _logger = logger;
            _userManager = userManager;
            _employeeService = employeeService;
            _medicalRequest = medicalRequest;
            _relativeService = rentativeService;
            _context = context;
            _repository = repository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            EmployeeRelativesApiModel employeeModel = new EmployeeRelativesApiModel();
            RelativeFilterModel filterModel = new RelativeFilterModel();
            return View(filterModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexAsync(RelativeFilterModel filterModel)
        {
            if (filterModel.EmployeeNumber !=0)
            {
                EmployeeModel result1 = _employeeService.GetEmployee(filterModel.EmployeeNumber).Result;
                if(result1 != null)
                {
                    if(result1.HasMedicalService==true)
                    {
                        EmployeeRelativesApi result2 = _relativeService.GetEmployeeRelativesApi(result1.EmployeeNumber, 1, 1, result1.Country).Result;
                       // var result3 = _relativeService.GetEmployeeRelatives(result1.EmployeeNumber).Result;
                        var result3 = await this._repository.Find((Expression<Func<Relative, bool>>)(i => i.IsVisible == true  && i.EmployeeNumber == result1.EmployeeNumber)).ToListAsync();

                        if (result2 != null)
                        {
                            result2.RelativesApiModel.ProfilePicture = CommanData.Url + CommanData.ProfileFolder + result1.ProfilePicture;
                            result2.RelativesApiModel.EmployeeDepartment = result1.Department.Name;
                            filterModel.EmployeeRelativeModel = result2.RelativesApiModel;
                        }
                        if(result3 != null)
                        {
                            filterModel.relatives = result3;
                            filterModel.employeeModel = result1;
                            filterModel.employeeModel.ProfilePicture= CommanData.Url + CommanData.ProfileFolder + result1.ProfilePicture;
                        }
                    }
                }
               
            }
            return View(filterModel);
        }

        public async Task<IActionResult> PendingRelative()
        {
            EmployeeRelativesApiModel employeeModel = new EmployeeRelativesApiModel();
            RelativeFilterModel filterModel = new RelativeFilterModel();
            var result3 = _relativeService.GetPendingRelative().Result;
            List<Relative>test= new List<Relative>();
            return View(result3);
        }


        public async Task<IActionResult> Create()
        {
            //EmployeeRelativesApiModel employeeModel = new EmployeeRelativesApiModel();
            //RelativeFilterModel filterModel = new RelativeFilterModel();
            //var result3 = await _repository.Find(i => i.Id == id).FirstOrDefaultAsync();
            Relative relative = new Relative();
            return View(relative);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Relative model)
        {
            model.UpdatedDate = DateTime.Now;
            model.IsActive = true;
            model.IsVisible = true;
            model.IsDelted = false;
            var result3 = _repository.Update(model);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(long id)
        {
            EmployeeRelativesApiModel employeeModel = new EmployeeRelativesApiModel();
            RelativeFilterModel filterModel = new RelativeFilterModel();
            var result3 = await _repository.Find(i => i.Id ==id).FirstOrDefaultAsync();
            return View(result3);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Relative model)
        {
            model.UpdatedDate = DateTime.Now;
            model.IsActive = true;
            model.IsVisible = true;
            model.IsDelted = false;
            var result3 = _repository.Update(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Approve(long id)
        {
            var result3 = await _repository.Find(i => i.Id == id).FirstOrDefaultAsync();
            result3.IsVisible = true;
            result3.IsActive = true;
            result3.IsDelted = false;
            result3.UpdatedDate = DateTime.Now;
            var result = _repository.Update(result3);
            return RedirectToAction("Index");
        }

    }
}
