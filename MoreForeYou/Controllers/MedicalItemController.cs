using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Models.Models;
using MoreForYou.Service.Contracts.Auth;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Contracts;
using Repository.EntityFramework;
using MoreForYou.Services.Models.MasterModels;
using Microsoft.EntityFrameworkCore;
using MoreForYou.Services.Models.API.Medical;
using System.Threading.Tasks;
using System;
using MoreForYou.Services.Models.MedicalRequestViewModels;

namespace MoreForYou.Controllers
{
    public class MedicalItemController : Controller
    {
        private readonly IMedicalRequestService _medicalRequest;
        private readonly IRoleService _roleService;
        private readonly ILogger<MedicalItemController> _logger;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly IRelativeService _relativeService;
        public APPDBContext _context;
        private readonly IRepository<MedicalItem, long> _repository;
        public MedicalItemController(IRoleService roleService,
        ILogger<MedicalItemController> logger,
        UserManager<AspNetUser> userManager,
        IRelativeService rentativeService,
        IEmployeeService employeeService, IMedicalRequestService medicalRequest, APPDBContext context, IRepository<MedicalItem, long> repository)
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
        public IActionResult Index()
        {
            MedicalItemsFilterModel filterModel = new MedicalItemsFilterModel();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            else if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            return View(filterModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MedicalItemsFilterModel filterModel)
        {
            //MedicalItemsFilterModel filterModel = new MedicalItemsFilterModel();
            if (filterModel.type != "0" && filterModel.text != null)
            {
                filterModel.items= _medicalRequest.MedicalItemsSearchPattern(filterModel.type, filterModel.text);
            }
            
            return View(filterModel);
        }

        public async Task<IActionResult> Create()
        {
            MedicalItem relative = new MedicalItem();
            return View(relative);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalItem model)
        {
            model.UpdatedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.IsVisible = true;
            model.IsDelted = false;
            var result3 = _repository.Add(model);
            if (result3 == null)
            {
                TempData["Error"] = "Failed Process,Item Can't be added ";
            }
            else
            {
                TempData["Message"] = "Item has been added Successfuly & Drug Name = " + result3.DrugName;
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(long id)
        {
            //EmployeeRelativesApiModel employeeModel = new EmployeeRelativesApiModel();
           // RelativeFilterModel filterModel = new RelativeFilterModel();
            var result3 = await _repository.Find(i => i.Id == id).FirstOrDefaultAsync();
            return View(result3);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MedicalItem model)
        {
            model.UpdatedDate = DateTime.Now;
            model.IsVisible = true;
            model.IsDelted = false;
            var result3 = _repository.Update(model);
            if (result3 == false)
            {
                TempData["Error"] = "Failed Process,Item Can't be Updated ";
            }
            else
            {
                TempData["Message"] = "Item has been Updated Successfuly & Drug Name = " + model.DrugName;
            }
            return RedirectToAction("Index");

        }
    }
}
