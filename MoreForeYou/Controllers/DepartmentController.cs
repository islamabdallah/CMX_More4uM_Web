using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForYou.Controllers
{
    [Authorize]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _DepartmentService;

        public DepartmentController(IDepartmentService DepartmentService)
        {
            _DepartmentService = DepartmentService;
        }

        public bool CheckUniqueDepartmentName(string Name)
        {
            bool result = false;
            try
            {
                    var Department = _DepartmentService.GetDepartmentByName(Name);
                    if (Department == null)
                    {
                        result = true;
                    }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }
        // GET: DepartmentController
        public ActionResult Index()
        {
            try
            {
                List<DepartmentModel> departments = _DepartmentService.GetAllDepartments().ToList();
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                else if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }
                return View(departments);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: DepartmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartmentController/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentModel Model)
        {
            try
            {
                Model.CreatedDate = DateTime.Now;
                Model.UpdatedDate = DateTime.Now;
                Model.IsVisible = true;
                Model.IsDelted = false;
                //long latestId = _DepartmentService.GetLatestId();
                //if (latestId > 0)
                //{
                //    Model.Id = latestId + 1;
                //}
                //else
                //{
                //    Model.Id = 1;
                //}
              var response = _DepartmentService.CreateDepartment(Model);
                    if (response.Result == true)
                    {
                        ViewBag.Message = "Success Process, Department has been added";
                    }
                    else
                    {
                        ViewBag.Error = "Failed process, Fail to add department";
                    }
                
               // else
                //{
                //    ViewBag.Error = "Failed process, Fail to add department";
                //}

                return View(Model);
            }
            catch
            {
                ViewBag.Error = "Failed process, Fail to add department";
                return View(Model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createe(string DepName)
        {
            try
            {
                DepartmentModel Model = new DepartmentModel()
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsVisible = true,
                    IsDelted = false,
                    Name = DepName
                };
                //long latestId = _DepartmentService.GetLatestId();
                //if (latestId > 0)
                //{
                //    Model.Id = latestId + 1;
                    var response = _DepartmentService.CreateDepartment(Model);
                    if (response.Result == true)
                    {
                        TempData["Message"] = "Success Process, Department has been added";
                    }
                    else
                    {
                        TempData["Error"] = "Failed process, Fail to add department";
                    }
                //}
                //else
                //{
                //    TempData["Error"] = "Failed process, Fail to add department";
                //}

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Failed process, Fail to add department";
                return RedirectToAction("Index");
            }
        }

        // GET: DepartmentController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                DepartmentModel departmentModel = _DepartmentService.GetDepartment(id);
                return View(departmentModel);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentModel departmentModel)
        {
            try
            {
               DepartmentModel DBdepartmentModel = _DepartmentService.GetDepartment(departmentModel.Id);
                DBdepartmentModel.Name = departmentModel.Name;
                DBdepartmentModel.IsVisible = departmentModel.IsVisible;
                DBdepartmentModel.UpdatedDate = DateTime.Today;
               var result = _DepartmentService.UpdateDepartment(DBdepartmentModel);
                if (result.Result == true)
                {
                    ViewBag.Message = "Success Process, Department has been updated";
                }
                else
                {
                    ViewBag.Error = "Failed process, Fail to update department";
                }
                return View(departmentModel);
            }
            catch
            {
                ViewBag.Error = "Failed process, Fail to update department";
                return View(departmentModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editt(string id, string Name, bool IsVisible)
        {
            try
            {
                DepartmentModel DBdepartmentModel = _DepartmentService.GetDepartment(Convert.ToInt32(id));
                DBdepartmentModel.Name = Name;
                DBdepartmentModel.IsVisible = IsVisible;
                DBdepartmentModel.UpdatedDate = DateTime.Today;
                var result = _DepartmentService.UpdateDepartment(DBdepartmentModel);
                if (result.Result == true)
                {
                    TempData["Message"] = "Success Process, Department has been updated";
                }
                else
                {
                    TempData["Error"] = "Failed process, Fail to update department";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Failed process, Fail to update department";
                return RedirectToAction("Index");
            }
        }


        // GET: DepartmentController/Delete/5


        // POST: DepartmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                DepartmentModel departmentModel = _DepartmentService.GetDepartment(id);
                departmentModel.UpdatedDate = DateTime.Now;
                departmentModel.IsDelted = true;
                departmentModel.IsVisible = false;
                var result = _DepartmentService.UpdateDepartment(departmentModel);
                if (result != null)
                {
                    ViewBag.Message = "Success Process, department has been deleted";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Failed to delete  department";
                    return RedirectToAction(nameof(Index));

                }
            }
            catch
            {
                ViewBag.Error = "Failed to delete  department";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
