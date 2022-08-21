using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using iFinco.UI.Areas.CompanyManager.Models;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class GoDownController : Controller
    {
        iFincoDBEntities context;
        GoDownHandler goDownHandler;

        public GoDownController()
        {
            context = new iFincoDBEntities();
            goDownHandler = new GoDownHandler(context);
        }

        // GET: CompanyManager/GoDown


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GoDown model)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.CreatedBy = User.Identity.GetCustomId();
            model.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.IsActive = true;
            goDownHandler.Add(model);
            return RedirectToAction("Index");
        }

        public ActionResult Index(GoDownSearchModel model, int page =1)
        {
            var list = goDownHandler.List. Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault());
            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Discription))
            {
                list = list.Where(x => x.Description.ToLower().Contains(model.Discription.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Contact))
            {
                list = list.Where(x => x.Contact.ToLower().Contains(model.Contact.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Adress))
            {
                list = list.Where(x => x.Address.ToLower().Contains(model.Adress.ToLower()));
            }
            model.Data = list.OrderByDescending(x=>x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
            
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var Godown = goDownHandler.FindById(id);
            if (Godown.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            return View(Godown);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GoDown model, HttpPostedFileBase logo)
        {
            var godown = goDownHandler.FindById(model.Id);
            if (godown.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            godown.Title = model.Title;
            godown.Description = model.Description;
            godown.Contact = model.Contact;
            godown.Address = model.Address;
            godown.ModifiedOn = DateTime.UtcNow;
            godown.ModifiedBy = User.Identity.GetCustomId();
            goDownHandler.Update(godown);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }
            var godown = goDownHandler.FindById(id.GetValueOrDefault());
            if (godown.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            godown.IsActive = false;
            godown.ModifiedBy = User.Identity.GetCustomId();
            godown.ModifiedOn = DateTime.UtcNow;
            goDownHandler.Update(godown);
            return RedirectToAction("index");
        }

        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {

            }

            var category = goDownHandler.FindById(id.GetValueOrDefault());
            return PartialView("_DetailPartial", category);
        }
    }
}