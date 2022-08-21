using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFinco.UI.Util;

namespace iFinco.UI.Areas.Admin.Controllers
{
    [Authorize]
    public class MenuAccessController : Controller
    {
        iFincoDBEntities context;
        CompanyHandler companyHandler;
        MenuHandler menuHandler;
        CompanyMenuAccessHandler companyAccessHandler;

        public MenuAccessController()
        {
            context = new iFincoDBEntities();
            companyHandler = new CompanyHandler(context);
            menuHandler = new MenuHandler(context);
            companyAccessHandler = new CompanyMenuAccessHandler(context);

        }
        // GET: Admin/MenuAccess
        public ActionResult Index()
        {
            ViewBag.Companies = companyHandler.List.Where(x => x.IsActive);
            //ViewBag.Menus = menuHandler.List.Where(x => x.IsActive && x.Type == (int)MenuType.Module);
            var menu = menuHandler.List.Where(x => x.IsActive && x.Type == (int)MenuType.Module);
            return View(menu);
        }

        public JsonResult Access(long id)
        {
            var menuIds = companyAccessHandler.GetAccessByCompany(id).OrderBy(x=>x.MenuId).Select(x => x.MenuId);
            return Json(menuIds, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveAccess(long id, long companyId)
        {
            var menuAccess = companyAccessHandler.List.FirstOrDefault(x => x.MenuId == id && x.CompanyId == companyId);
            if (menuAccess != null)
            {
                menuAccess.IsActive = true;
                menuAccess.ModifiedOn = DateTime.UtcNow;
                menuAccess.ModifiedBy = User.Identity.GetCustomId();
                companyAccessHandler.Update(menuAccess);
            }
            else
            {
                CompanyMenusAccess nMenu = new CompanyMenusAccess();
                nMenu.IsActive = true;
                nMenu.CreatedOn = DateTime.UtcNow;
                nMenu.CreatedBy = User.Identity.GetCustomId();
                nMenu.MenuId = id;
                nMenu.CompanyId = companyId;
                companyAccessHandler.Add(nMenu);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveAccess(long id, long companyId)
        {
            var menuAccess = companyAccessHandler.List.FirstOrDefault(x => x.MenuId == id && x.CompanyId == companyId);
            if (menuAccess != null)
            {
                menuAccess.IsActive = false;
                menuAccess.ModifiedOn = DateTime.UtcNow;
                menuAccess.ModifiedBy = User.Identity.GetCustomId();
                companyAccessHandler.Update(menuAccess);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}