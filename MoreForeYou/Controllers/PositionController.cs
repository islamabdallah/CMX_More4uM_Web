using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Implementation;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForYou.Controllers
{
    [Authorize]
    public class PositionController : Controller
    {
        private readonly IPositionService _PositionService;

        public PositionController(IPositionService PositionService)
        {
            _PositionService = PositionService;
        }
        // GET: PositionController
        public ActionResult Index()
        {
            try
            {
                Task<List<PositionModel>> positions = _PositionService.GetAllPositions();
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                else if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }
                return View(positions.Result);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: PositionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PositionController/Create
        public ActionResult Create()
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

        // POST: PositionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PositionModel Model)
        {
            try
            {
                Model.CreatedDate = DateTime.Now;
                Model.UpdatedDate = DateTime.Now;
                Model.IsVisible = true;
                Model.IsDelted = false;
                var response = _PositionService.CreatePosition(Model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createe(string PositionName)
        {
            try
            {
                PositionModel Model = new PositionModel()
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsVisible = true,
                    IsDelted = false,
                    Name = PositionName
                };

                var response = _PositionService.CreatePosition(Model);
                if (response.Result == true)
                {
                    TempData["Message"] = "Positin Created Successfully";
                }
                else
                {
                    TempData["Error"] = "Position can not be created";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: PositionController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                PositionModel positionModel = _PositionService.GetPosition(id);
                return View(positionModel);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }

        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PositionModel positionModel)
        {
            try
            {
                PositionModel DBPositionModel = _PositionService.GetPosition((int)positionModel.Id);
                DBPositionModel.Name = positionModel.Name;
                DBPositionModel.IsVisible = positionModel.IsVisible;
                DBPositionModel.UpdatedDate = DateTime.Today;
                bool result =_PositionService.UpdatePosition(DBPositionModel).Result;
                if(result == true)
                {
                    ViewBag.Message = "Position Updated Successfully";
                }
                else
                {
                    ViewBag.Error = "Failed Process";

                }
            }
            catch
            {
                ViewBag.Error = "Failed Process";
            }
            return View(positionModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editt(string id, string Name, bool IsVisible)
        {
            try
            {
               
                PositionModel DBPositionModel = _PositionService.GetPosition(Convert.ToInt32(id));
                DBPositionModel.Name = Name;
                DBPositionModel.IsVisible = IsVisible;
                DBPositionModel.UpdatedDate = DateTime.Today;
                bool result = _PositionService.UpdatePosition(DBPositionModel).Result;
                if (result == true)
                {
                    TempData["Message"] = "Position Updated Successfully";
                }
                else
                {
                    TempData["Error"] = "Failed Process";

                }
            }
            catch
            {
                ViewBag.Error = "Failed Process";
            }
            return RedirectToAction("Index");
        }

        // GET: PositionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PositionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                PositionModel departmentModel = _PositionService.GetPosition(id);
                departmentModel.UpdatedDate = DateTime.Now;
                departmentModel.IsDelted = true;
                departmentModel.IsVisible = false;
                var result = _PositionService.UpdatePosition(departmentModel);
                if (result != null)
                {
                    ViewBag.Message = "Success Process, position has been deleted";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Failed to delete  position";
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
