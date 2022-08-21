using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Areas.CompanyManager.Models;
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
    public class VariantController : Controller
    {
        iFincoDBEntities context;
        VariantHandler variantHandler;
        public VariantController()
        {
            context = new iFincoDBEntities();
            variantHandler = new VariantHandler(context);
        }

        // GET: CompanyManager/Variant
        public ActionResult Index(VariantSearchModel model, int page = 1)
        {
            var list = variantHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());

            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            //else if (!string.IsNullOrEmpty(model.))
            //{
            //    list = list.Where(x => x.Contact.ToLower().Contains(model.Contact.ToLower()));
            //}
            //else if (model.Status != null)
            //{
            //    list = list.Where(x => x.Status == model.Status);
            //}
            //else if (!string.IsNullOrEmpty(model.Address))
            //{
            //    list = list.Where(x => x.Adress.ToLower().Contains(model.Address.ToLower()));
            //}

            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(VariantVM model)
        {
            model.Variant.CreatedOn = DateTime.UtcNow;
            model.Variant.CreatedBy = User.Identity.GetCustomId();
            model.Variant.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.Variant.BranchId = User.Identity.GetBranchId();
            model.Variant.IsActive = true;
            foreach (var item in model.Values)
            {
                item.IsActive = true;
                item.CreatedOn = DateTime.UtcNow;
                item.CreatedBy = User.Identity.GetCustomId();
                item.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
                item.BranchId = User.Identity.GetBranchId();
                model.Variant.VariantValues.Add(item);
            }

            variantHandler.Add(model.Variant);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            VariantVM model = new VariantVM();
            model.Variant = variantHandler.FindById(id);
            model.Values = model.Variant.VariantValues.Where(x => x.IsActive == true).ToList();

            if (model.Variant.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VariantVM model)
        {
            var variant = variantHandler.FindById(model.Variant.Id);

            if (variant.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            variant.Title = model.Variant.Title;
            variant.ModifiedOn = DateTime.UtcNow;
            variant.ModifiedBy = User.Identity.GetCustomId();
            foreach (var item in variant.VariantValues)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = User.Identity.GetCustomId();
            }

            foreach (var item in model.Values)
            {
                if (item.Id > 0)
                {
                    var oldVal = variant.VariantValues.SingleOrDefault(x => x.Id == item.Id);
                    oldVal.IsActive = true;
                    oldVal.Title = item.Title;
                }
                else
                {
                    item.IsActive = true;
                    item.CreatedOn = DateTime.UtcNow;
                    item.CreatedBy = User.Identity.GetCustomId();
                    variant.VariantValues.Add(item);
                }

            }
            variantHandler.Update(variant);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }

            var variant = variantHandler.FindById(id.GetValueOrDefault());
            if (variant.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            variant.IsActive = false;
            variant.ModifiedBy = User.Identity.GetCustomId();
            variant.ModifiedOn = DateTime.UtcNow;

            foreach (var item in variant.VariantValues)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = User.Identity.GetCustomId();
                item.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            }

            variantHandler.Update(variant);
            return RedirectToAction("index");
        }

        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {

            }
            var variant = variantHandler.FindById(id.GetValueOrDefault());
            variant.VariantValues = variant.VariantValues.Where(x => x.IsActive).ToList();
            return PartialView("_DetailPartial", variant);
        }
    }
}