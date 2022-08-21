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
using iFinco.UI.Models;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        iFincoDBEntities context;
        CategoryHandler categoryHandler;
        public CategoryController()
        {
            context = new iFincoDBEntities();
            categoryHandler = new CategoryHandler(context);
        }


        public ActionResult Index(CategorySearchModel model, int page = 1)
        {
            var list = categoryHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault());
            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Discription))
            {
                list = list.Where(x => x.Description.ToLower().Contains(model.Discription.ToLower()));
            }
            else if (model.Type != 0)
            {
                list = list.Where(x => x.Type == model.Type);
            }

            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
        }

        // GET: CompanyManager/Category
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category model, HttpPostedFileBase logo)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.CreatedBy = User.Identity.GetCustomId();
            model.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.IsActive = true;
            if (logo != null)
            {
                model.ImgUrl = FileManager.SaveImage(logo);
            }

            categoryHandler.Add(model);
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult Edit(long id)
        {
            var category = categoryHandler.FindById(id);
            if (category.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model, HttpPostedFileBase logo)
        {
            var category = categoryHandler.FindById(model.Id);

            if (category.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            category.Title = model.Title;
            category.Description = model.Description;
            category.Type = model.Type;
            category.ModifiedOn = DateTime.UtcNow;
            category.ModifiedBy = User.Identity.GetCustomId();
            if (logo != null)
            {
                category.ImgUrl = FileManager.SaveImage(logo);
            }
            categoryHandler.Update(category);
            return RedirectToAction("Index");
        }

        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {
                return PartialView("error");
            }
            var category = categoryHandler.FindById(id.GetValueOrDefault());
            return PartialView("_DetailPartial", category);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }

            var category = categoryHandler.FindById(id.GetValueOrDefault());
            if (category.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            category.IsActive = false;
            category.ModifiedBy = User.Identity.GetCustomId();
            category.ModifiedOn = DateTime.UtcNow;
            categoryHandler.Update(category);
            return RedirectToAction("index");
        }

    }
}