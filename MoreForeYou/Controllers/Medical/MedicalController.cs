using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using MoreForYou.Models.Models;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Medical;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.MaterModels;
using MoreForYou.Services.Models.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreForYou.Controllers.Medical
{
    [Authorize]
    public class MedicalController : BaseController
    {
        private readonly IMedicalSubCategoryService _medicalSubCategoryService;
        private readonly IMedicalDetailsService _medicalDetailsService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly IMedicalCategoryService _medicalCategoryService;
        public MedicalController(IMedicalSubCategoryService medicalSubCategoryService,
            IMedicalDetailsService medicalDetailsService,
             UserManager<AspNetUser> userManager,
             IEmployeeService employeeService,
             IMedicalCategoryService medicalCategoryService)
        {
            _medicalSubCategoryService = medicalSubCategoryService;
            _medicalDetailsService = medicalDetailsService;
            _userManager = userManager;
            _employeeService = employeeService;
            _medicalCategoryService = medicalCategoryService;
        }
        // GET: MedicalController

        public async Task<ActionResult> Index()
        {
            try
            {
                MedicalMain medicalMain = new MedicalMain();
                MedicalCategoryModel medicalCategoryModel = new MedicalCategoryModel();
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var Employee = await _employeeService.GetEmployeeByUserId(user.Id);
                    if (Employee != null)
                    {
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
                                            var medicalSubCategories = medicalSubCategoryModels.Where(MS => MS.MedicalCategory.Id == item.Id);
                                            if (medicalSubCategories.Count() == 1)
                                            {
                                                item.hasOneSubCategory = true;
                                                List<MedicalDetailsModel> itemMedicalDetail = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(medicalSubCategories.First().Id, Employee.Country);
                                                if (itemMedicalDetail != null)
                                                {
                                                    if (itemMedicalDetail.Count != 0)
                                                    {
                                                        item.SubCategoriesCount = itemMedicalDetail.Count;
                                                    }
                                                    else
                                                    {
                                                        item.SubCategoriesCount = medicalSubCategories.Count();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                item.hasOneSubCategory = false;
                                                item.SubCategoriesCount = medicalSubCategories.Count();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        medicalMain.MedicalCategoryModels = medicalCategoryModels;
                        return View(medicalMain);
                    }
                }
                return RedirectToAction("ERROR404");
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<ActionResult> LoadSubCategories(int Id)
        {
            try
            {
                MedicalSubCategoryViewModel medicalSubCategoryViewModel = new MedicalSubCategoryViewModel();
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var Employee =  _employeeService.GetEmployeeByUserId2(user.Id);
                    if (Employee != null)
                    {
                        var medicalSubCategories = await _medicalSubCategoryService.GetMedicalSubCategoryModelsByCategoryId(Id);
                        if (medicalSubCategories != null)
                        {
                            foreach (var item in medicalSubCategories)
                            {
                                List<MedicalDetailsModel> itemMedicalDetail = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(item.Id, Employee.Country);
                                if (itemMedicalDetail != null)
                                {
                                    item.MedicalDetailsCount = itemMedicalDetail.Count;
                                }
                            }
                            medicalSubCategoryViewModel.MedicalSubCategoryModels = medicalSubCategories;
                            return View(medicalSubCategoryViewModel);
                        }
                        else
                        {
                            return View(null);
                        }
                    }
                }
                return RedirectToAction("ERROR404");
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }
        public async Task<ActionResult> GetSubCategories(int Id)
        {
            try
            {
                MedicalSubCategoryViewModel medicalSubCategoryViewModel = new MedicalSubCategoryViewModel();
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var Employee = await _employeeService.GetEmployeeByUserId(user.Id);
                    if (Employee != null)
                    {
                        var medicalSubCategories = await _medicalSubCategoryService.GetMedicalSubCategoryModelsByCategoryId(Id);
                        if (medicalSubCategories != null)
                        {
                            if (medicalSubCategories.Count() > 0 && medicalSubCategories.Count() == 1)
                            {
                                var medicalDetails = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(medicalSubCategories.First().Id, Employee.Country);
                                if (medicalDetails != null)
                                {
                                    medicalSubCategories.First().MedicalDetailsModels = medicalDetails;
                                    medicalSubCategories.First().MedicalDetailsCount = medicalDetails.Count;
                                }
                            }
                            if (medicalSubCategories.Count() > 0 && medicalSubCategories.Count() != 1)
                            {
                                List<MedicalDetailsModel> medicalDetailsModels = await _medicalDetailsService.GetMedicalDetailsForCountry(Employee.Country);
                                if (medicalDetailsModels != null)
                                {
                                    foreach (var item in medicalSubCategories)
                                    {
                                        item.MedicalDetailsCount = medicalDetailsModels.Where(MD => MD.MedicalSubCategory.Id == item.Id).Count();
                                    }
                                }
                            }
                            medicalSubCategoryViewModel.MedicalSubCategoryModels = medicalSubCategories;
                            return View(medicalSubCategoryViewModel);
                        }
                        else
                        {
                            return View(new List<MedicalSubCategoryModel>());
                        }
                    }
                    else
                    {
                        return View(new List<MedicalSubCategoryModel>());
                    }
                }
                else
                {
                    return View(new List<MedicalSubCategoryModel>());
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }


        public async Task<ActionResult> GetMedicalDetails(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            EmployeeModel employee = new EmployeeModel();
            MedicalDetailsViewModel medicalDetailsViewModel = new MedicalDetailsViewModel();
            if (user != null)
            {
                employee = await _employeeService.GetEmployeeByUserId(user.Id);
                if (employee == null)
                {
                    return View(null);
                }
                else
                {
                    var medicalDetails = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(Id, employee.Country);
                    if (medicalDetails != null)
                    {
                        medicalDetailsViewModel.MedicalDetailsModels = medicalDetails;
                    }
                    return View(medicalDetailsViewModel);

                }
            }
            return View(null);

        }

        public async Task<string> GetAllMedicalDetailsForSubCategory(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            EmployeeModel employee = new EmployeeModel();
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
                    var medicalDetails = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(Id, employee.Country);
                    if (medicalDetails != null)
                    {
                        medicalDetailsViewModel.MedicalDetailsModels = medicalDetails;
                    }
                    return JsonSerializer.Serialize(medicalDetailsViewModel);

                }
            }
            return null;

        }

        [HttpPost]
        public async Task<string> GetMedicalDetails2(int detailsId)
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = await _employeeService.GetEmployeeByUserId(user.Id);
            if (employee == null)
            {
                return null;
            }
            MedicalDetailsModel medicalDetails = await _medicalDetailsService.GetMedicalDetailsById(detailsId, employee.Country);
            if (medicalDetails != null)
            {
                medicalDetails.Image = CommanData.Url + CommanData.MedicalDetailsFolder + medicalDetails.Image;
                return JsonSerializer.Serialize(medicalDetails);
            }
            else
            {
                return null;
            }
        }


        [HttpPost]
        public async Task<string> GetSubCategories2(int Id)
        {
            try
            {
                MedicalSubCategoryViewModel medicalSubCategoryViewModel = new MedicalSubCategoryViewModel();
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var Employee = await _employeeService.GetEmployeeByUserId(user.Id);
                    if (Employee != null)
                    {
                        var medicalSubCategories = await _medicalSubCategoryService.GetMedicalSubCategoryModelsByCategoryId(Id);
                        if (medicalSubCategories != null)
                        {
                            if (medicalSubCategories.Count() == 1)
                            {
                                var medicalDetails = await _medicalDetailsService.GetMedicalDetailsBySubCategoryId(medicalSubCategories.First().Id, Employee.Country);
                                if (medicalDetails != null)
                                {
                                    medicalSubCategories.First().MedicalDetailsModels = medicalDetails;
                                    medicalSubCategories.First().MedicalDetailsCount = medicalDetails.Count;
                                }
                            }
                            if (medicalSubCategories.Count() > 1)
                            {
                                List<MedicalDetailsModel> medicalDetailsModels = await _medicalDetailsService.GetMedicalDetailsForCountry(Employee.Country);
                                if (medicalDetailsModels != null)
                                {
                                    foreach (var item in medicalSubCategories)
                                    {
                                        item.MedicalDetailsCount = medicalDetailsModels.Where(MD => MD.MedicalSubCategory.Id == item.Id).Count();
                                    }
                                }
                            }
                            medicalSubCategoryViewModel.MedicalSubCategoryModels = medicalSubCategories;
                            var x = JsonSerializer.Serialize(medicalSubCategoryViewModel);
                            return JsonSerializer.Serialize(medicalSubCategoryViewModel);
                        }
                        else
                        {
                            return JsonSerializer.Serialize(new List<MedicalSubCategoryModel>());
                        }
                    }
                    else
                    {
                        return JsonSerializer.Serialize(new List<MedicalSubCategoryModel>());
                    }
                }
                else
                {
                    return JsonSerializer.Serialize(new List<MedicalSubCategoryModel>());
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // GET: MedicalController/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: MedicalController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction();
            }
            catch
            {
                return View();
            }
        }

        // GET: MedicalController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MedicalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction();
            }
            catch
            {
                return View();
            }
        }

        // GET: MedicalController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MedicalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction();
            }
            catch
            {
                return View();
            }
        }
    }
}