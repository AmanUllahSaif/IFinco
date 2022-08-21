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

namespace iFinco.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MenuController : Controller
    {
        iFincoDBEntities context;
        MenuHandler menuHandler;
        public MenuController()
        {
            context = new iFincoDBEntities();
            menuHandler = new MenuHandler(context);
        }
        // GET: Admin/Menu
        public ActionResult Index(MenuSearchModel model, int page = 1)
        {
            var list = menuHandler.List.Where(x => x.IsActive);
            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.AreaName))
            {
                list = list.Where(x => x.Area.ToLower().Contains(model.AreaName.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.ControllerName))
            {
                list = list.Where(x => x.Controller.ToLower().Contains(model.ControllerName.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.ActionName))
            {
                list = list.Where(x => x.Action.ToLower().Contains(model.ActionName.ToLower()));
            }

            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
        }
        public ActionResult Create()
        {
            ViewBag.ParentType = menuHandler.List.Where(x => x.IsActive).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Menu model)
        {
            model.CreatedBy = User.Identity.GetCustomId();
            model.CreatedOn = DateTime.UtcNow;
            model.IsActive = true;
            menuHandler.Add(model);
            return RedirectToAction("Create");
        }

        public ActionResult Edit(long id)
        {
            ViewBag.ParentType = menuHandler.List.Where(x => x.IsActive).ToList();
            var menu = menuHandler.FindById(id);
            return View(menu);
        }
        [HttpPost]
        public ActionResult Edit(Menu model)
        {
            var menu = menuHandler.FindById(model.Id);
            menu.Title = model.Title;
            menu.ClassName = model.ClassName;
            menu.Description = model.Description;
            menu.Area = model.Area;
            menu.Action = model.Action;
            menu.Controller = model.Controller;
            menu.ParentId = model.ParentId;
            menu.Type = model.Type;
            menu.ModifiedBy = User.Identity.GetCustomId();
            menu.ModifiedOn = DateTime.UtcNow;
            menu.InNav = model.InNav;
            menuHandler.Update(menu);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }
            var menu = menuHandler.FindById(id.GetValueOrDefault());
            menu.IsActive = false;
            menu.ModifiedBy = User.Identity.GetCustomId();
            menu.ModifiedOn = DateTime.UtcNow;
            menuHandler.Update(menu);
            return RedirectToAction("Index");
        }

        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {

            }
            var menu = menuHandler.FindById(id.GetValueOrDefault());
            ViewBag.ParentType = menuHandler.List.Where(x => x.IsActive).ToList();
            return PartialView("_DetailPartial", menu);
        }
    }
}