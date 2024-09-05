using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForYou.Controllers
{
    public class PrivilegeController : Controller
    {
        private readonly IPrivilegeService _privilegeService;
        private readonly IRequestWorkflowService _requestWorkflowService;


        public PrivilegeController(IPrivilegeService privilegeService,
            IRequestWorkflowService requestWorkflowService)
        {
            _privilegeService = privilegeService;
            _requestWorkflowService = requestWorkflowService;
        }
        // GET: PrivilegeController
        public ActionResult Index()
        {
           List<PrivilegeModel> privilges= _privilegeService.GetAllPrivileges().Result;
            return View(privilges);
        }

        // GET: PrivilegeController/Details/5
        public ActionResult Details(int id)
        {
           PrivilegeModel privilegeModel = _privilegeService.GetPrivilege(id);
            return View(privilegeModel);
        }

        // GET: PrivilegeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrivilegeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PrivilegeModel model)
        {
            try
            {
                model.IsDelted = false;
                model.IsVisible = true;
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                string imageName = _requestWorkflowService.UploadedImageAsync(model.ImagePath, "images/PriviligeImages").Result;
                model.Image = imageName;
                var newPrivilege= _privilegeService.CreatePrivilege(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: PrivilegeController/Edit/5
        public ActionResult Edit(int id)
        {
            PrivilegeModel privilegeModel = _privilegeService.GetPrivilege(id);
            return View(privilegeModel);
        }

        // POST: PrivilegeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PrivilegeModel privilegeModel)
        {
            try
            {
                PrivilegeModel DBPrivilegeModel = _privilegeService.GetPrivilege(privilegeModel.Id);
                DBPrivilegeModel.Name = privilegeModel.Name;
                DBPrivilegeModel.Description = privilegeModel.Description;
                DBPrivilegeModel.IsVisible = privilegeModel.IsVisible;
                DBPrivilegeModel.UpdatedDate = privilegeModel.UpdatedDate;
                _privilegeService.UpdatePrivilege(DBPrivilegeModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: PrivilegeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PrivilegeController/Delete/5
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
