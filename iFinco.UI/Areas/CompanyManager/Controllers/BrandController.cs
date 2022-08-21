using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Models;
using iFinco.UI.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        iFincoDBEntities context;
        BrandHandler brandHandler;
        // GET: CompanyManager/Brand

        public BrandController()
        {
            context = new iFincoDBEntities();
            brandHandler = new BrandHandler(context);
        }


        public ActionResult Index(BrandSearchModel model, int page = 1)
        {
            var list = brandHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault());
            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Description))
            {
                list = list.Where(x => x.Description.ToLower().Contains(model.Description.ToLower()));
            }
            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand model, HttpPostedFileBase logo)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.CreatedBy = User.Identity.GetCustomId();
            model.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.BranchId = User.Identity.GetBranchId();
            model.IsActive = true;
            if (logo != null)
            {
                model.ImgUrl = FileManager.SaveImage(logo);
            }
            brandHandler.Add(model);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long id)
        {
            var brand = brandHandler.FindById(id);
            if (brand.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand model, HttpPostedFileBase logo)
        {
            var brand = brandHandler.FindById(model.Id);
            if (brand.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            brand.Title = model.Title;
            brand.Description = model.Description;
            brand.ModifiedOn = DateTime.UtcNow;
            brand.ModifiedBy = User.Identity.GetCustomId();
            if (logo != null)
            {
                brand.ImgUrl = model.ImgUrl = FileManager.SaveImage(logo);
            }
            brandHandler.Update(brand);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }

            var brand = brandHandler.FindById(id.GetValueOrDefault());
            if (brand.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            brand.IsActive = false;
            brand.ModifiedBy = User.Identity.GetCustomId();
            brand.ModifiedOn = DateTime.UtcNow;
            brandHandler.Update(brand);
            return RedirectToAction("index");
        }

        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {

            }
            var brand = brandHandler.FindById(id.GetValueOrDefault());
            return PartialView("_DetailPartial", brand);
        }
    }
}