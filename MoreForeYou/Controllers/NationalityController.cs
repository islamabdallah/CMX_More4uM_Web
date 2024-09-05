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
    public class NationalityController : Controller
    {
        private readonly INationalityService _NationalityService;

        public NationalityController(INationalityService NationalityService)
        {
            _NationalityService = NationalityService;
        }
        // GET: NationalityController
        public ActionResult Index()
        {
            try
            {
                Task<List<NationalityModel>> nationalities = _NationalityService.GetAllNationalities();
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                else if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }
                return View(nationalities.Result);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: NationalityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NationalityController/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception e) { return RedirectToAction("ERROR404"); }
        }

        // POST: NationalityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NationalityModel Model)
        {
            try
            {
                Model.CreatedDate = DateTime.Now;
                Model.UpdatedDate = DateTime.Now;
                Model.IsVisible = true;
                Model.IsDelted = false;
                var response = _NationalityService.CreateNationality(Model);
                if(response.Result == true)
                {
                    ViewBag.Message = "Nationality Created Successfully";
                }
                else
                {
                    ViewBag.Error = "Nationality can not be created";
                }
            }
            catch(Exception e)
            {
                ViewBag.Error = "Nationality can not be Updated";
            }
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createe(string NationalityName)
        {
            try
            {
                NationalityModel Model = new NationalityModel()
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsVisible = true,
                    IsDelted = false,
                    Name = NationalityName
                };

                var response = _NationalityService.CreateNationality(Model);
                if (response.Result == true)
                {
                    TempData["Message"] = "Nationality Created Successfully";
                }
                else
                {
                    TempData["Error"] = "Nationality can not be created";
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Nationality can not be Updated";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editt(string id,string Name,bool IsVisible)
        {
            try
            {
                NationalityModel DBPositionModel = _NationalityService.GetNationality(Convert.ToInt32(id));
                DBPositionModel.Name = Name;
                DBPositionModel.IsVisible = IsVisible;
                DBPositionModel.UpdatedDate = DateTime.Today;
                var response = _NationalityService.UpdateNationality(DBPositionModel);
                if (response.Result == true)
                {
                    TempData["Message"] = "Nationality Updated Successfully";
                }
                else
                {
                    TempData["Error"] = "Nationality can not be Updated";
                }
            }
            catch
            {
                ViewBag.Error = "Nationality can not be Updated";

            }
            return RedirectToAction("Index");
        }


        // GET: NationalityController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                NationalityModel nationalityModel = _NationalityService.GetNationality(id);
                return View(nationalityModel);
            }
            catch (Exception e) 
            { 
                return RedirectToAction("ERROR404"); 
            } 
        }

        // POST: NationalityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NationalityModel positionModel)
        {
            try
            {
                NationalityModel DBPositionModel = _NationalityService.GetNationality((int)positionModel.Id);
                DBPositionModel.Name = positionModel.Name;
                DBPositionModel.IsVisible = positionModel.IsVisible;
                DBPositionModel.UpdatedDate = DateTime.Today;
               var response = _NationalityService.UpdateNationality(DBPositionModel);
                if (response.Result == true)
                {
                    ViewBag.Message = "Nationality Updated Successfully";
                }
                else
                {
                    ViewBag.Error = "Nationality can not be Updated";
                }
            }
            catch
            {
                ViewBag.Error = "Nationality can not be Updated";

            }
            return View(positionModel);
        }

        // GET: NationalityController/Delete/5
        public ActionResult Delete(int id)
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

        // POST: NationalityController/Delete/5
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
                return RedirectToAction("ERROR404");
            }
        }
    }
}
