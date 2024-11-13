using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Models.Models;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.Medical;
using System.Collections.Generic;
using MoreForYou.Services.Models.MedicalRequestViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using MoreForYou.Services.Models.MaterModels;
using System.Threading.Tasks;
using MoreForYou.Models.Models.MedicalModels;
using System;
using Data.Repository;
using MoreForYou.Models.Models.MasterModels.MedicalModels;
using Microsoft.EntityFrameworkCore;

namespace MoreForYou.Controllers.Medical
{
    public class MedicalNetworkController : Controller
    {
        private readonly IMedicalSubCategoryService _medicalSubCategoryService;
        private readonly IMedicalDetailsService _medicalDetailsService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly IMedicalCategoryService _medicalCategoryService;
        private readonly IRepository<MedicalDetails, long> _repository;
        public MedicalNetworkController(IMedicalSubCategoryService medicalSubCategoryService,
            IMedicalDetailsService medicalDetailsService,
             UserManager<AspNetUser> userManager,
             IEmployeeService employeeService,
             IMedicalCategoryService medicalCategoryService, IRepository<MedicalDetails, long> repository)
        {
            _medicalSubCategoryService = medicalSubCategoryService;
            _medicalDetailsService = medicalDetailsService;
            _userManager = userManager;
            _employeeService = employeeService;
            _medicalCategoryService = medicalCategoryService;
            _repository = repository;
        }
        public IActionResult Index()
        {
            MedicalNetworkVM medicalNetworkVM = new MedicalNetworkVM();
            medicalNetworkVM.medicalCategories = _medicalCategoryService.GetAllMedicalCategories().Result;
            return View(medicalNetworkVM);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(MedicalNetworkVM model)
        {
            //MedicalNetworkVM medicalNetworkVM = new MedicalNetworkVM();
            model.medicalCategories = _medicalCategoryService.GetAllMedicalCategories().Result;
            var user = await _userManager.GetUserAsync(User);            EmployeeModel employee = new EmployeeModel();
            MedicalDetailsViewModel medicalDetailsViewModel = new MedicalDetailsViewModel();
            if (user != null)
            {
                employee = await _employeeService.GetEmployeeByUserId(user.Id);
                if (employee == null)
                {
                    return null;
                }
                else
                {
                    //var medicalDetails = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(model.SubCategoryId, employee.Country);
                    //var medicalDetails = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(model.SubCategoryId, "Assiut");
                    var medicalDetails = await _repository.Find(m => m.MedicalSubCategory.Id == model.SubCategoryId && m.Country == "Assiut", false, m => m.MedicalSubCategory).ToListAsync();

                    if (medicalDetails != null)
                    {
                        model.medicationEntities = medicalDetails;
                    }
                    //return JsonSerializer.Serialize(medicalDetailsViewModel);

                }
            }
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories= _medicalCategoryService.GetAllMedicalCategories().Result;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalDetails model)
        {
            model.UpdatedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.IsVisible = true;
            model.IsDelted = false;
            var result3 = _repository.Add(model);
            if (result3 == null)
            {
                TempData["Error"] = "Failed Process,Entity Can't be added ";
            }
            else
            {
                TempData["Message"] = "Entity has been added Successfuly & Entity Name = " + result3.Name_EN;
            }
            return RedirectToAction("Index");
        }
    }
}
