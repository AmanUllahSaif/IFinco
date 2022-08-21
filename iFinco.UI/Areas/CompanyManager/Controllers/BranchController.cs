using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Models;
using iFinco.UI.Util;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class BranchController : Controller
    {
        iFincoDBEntities context;
        BranchHandler branchHandler;
        BranchAccessHandler branchAccesssHandler;
        private ApplicationUserManager _userManager;
        public BranchController()
        {
            context = new iFincoDBEntities();
            branchHandler = new BranchHandler(context);
            branchAccesssHandler = new BranchAccessHandler(context);
        }

        public BranchController(ApplicationUserManager userManager)
        {
            context = new iFincoDBEntities();
            branchHandler = new BranchHandler(context);
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: CompanyManager/Branch
        public ActionResult Index(BranchSearchModel model, int page = 1)
        {

            var list = branchHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault());

            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Contact))
            {
                list = list.Where(x => x.Contact.ToLower().Contains(model.Contact.ToLower()));
            }
            else if (model.Status != null)
            {
                list = list.Where(x => x.Status == model.Status);
            }
            else if (!string.IsNullOrEmpty(model.Address))
            {
                list = list.Where(x => x.Adress.ToLower().Contains(model.Address.ToLower()));
            }

            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var branch = branchHandler.FindById(id);
            return View(branch);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Branch model)
        {
           
            var branch = branchHandler.FindById(model.Id);
            if (model.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            branch.Title = model.Title;
            branch.Adress = model.Adress;
            branch.Status = model.Status;
            branch.ModifiedBy = User.Identity.GetCustomId();
            branch.ModifiedOn = DateTime.UtcNow;
            branchHandler.Update(branch);
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Branch model)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.CreatedBy = User.Identity.GetCustomId();
            model.IsActive = true;
            model.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            branchHandler.Add(model);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long? id)
        {
            var branch = branchHandler.FindById(id.GetValueOrDefault());
            if (id == null || id < 1 || branch.CompanyId != User.Identity.GetCompanyId())
            {
                return HttpNotFound();
            }

            if (!branch.IsHeadOffice)
            {
                branch.IsActive = false;
                branch.ModifiedBy = User.Identity.GetCustomId();
                branch.ModifiedOn = DateTime.UtcNow;
                branchHandler.Update(branch);
            }
            return RedirectToAction("index");
        }
        [HttpGet]
        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {

            }

            var branch = branchHandler.FindById(id.GetValueOrDefault());
            return PartialView("_DetailPartial", branch);
        }

        [HttpGet]
        public ActionResult Access()
        {
            ViewBag.Branchs = branchHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId());
            var companyId = User.Identity.GetCompanyId().GetValueOrDefault();
            ViewBag.Users = UserManager.Users.Where(x => x.CompanyId == companyId).ToList();
            return View();
        }


        [HttpGet]
        public JsonResult GetAccessBranchs(long id)
        {
            var access = branchAccesssHandler.List.Where(x => x.UId == id && x.IsActive).Select(x => x.BranchId).ToList();
            if (access != null && access.Count > 0)
                return Json(access, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SaveAccess(long id, long branchId)
        {
            var acs = branchAccesssHandler.List.FirstOrDefault(x => x.UId == id && x.BranchId == branchId);
            if (acs != null && acs.Id > 0)
            {
                acs.IsActive = true;
                acs.ModifiedBy = User.Identity.GetCustomId();
                acs.ModifiedOn = DateTime.UtcNow;
                branchAccesssHandler.Update(acs);
            }
            else
            {
                acs = new BranchAccess();
                acs.BranchId = branchId;
                acs.UId = id;
                acs.CreatedBy = User.Identity.GetCustomId();
                acs.CreatedOn = DateTime.UtcNow;
                acs.IsActive = true;
                branchAccesssHandler.Add(acs);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}