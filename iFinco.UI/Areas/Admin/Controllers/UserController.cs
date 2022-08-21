using iFinco.UI.Util;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IdentityModel.Services;
using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using PagedList.Mvc;
using iFinco.DAL.Enum;

namespace iFinco.UI.Areas.Admin.Controllers
{
    //[ClaimsPrincipalPermission(Action = System.Security.Permissions.SecurityAction.)]
   // [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private ApplicationUserManager _userManager;
        iFincoDBEntities context;
        BranchHandler branchHandler;
        BranchAccessHandler branchAccessHandler;
        int pageSize;

        public UserController()
        {
            context = new iFincoDBEntities();
            branchHandler = new BranchHandler(context);
            branchAccessHandler = new BranchAccessHandler(context);
            pageSize = PagingManager.GetPageSize();
        }

        RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
        public UserController(ApplicationUserManager userManager)
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
        // GET: CompanyManager/User
        public ActionResult Index(int page = 1)
        {
            var users = UserManager.Users.Where(x => x.CompanyId == null);
            ViewBag.Roles = roleManager.Roles.Where(x => x.IsForAdmin);
           
            return View(users.OrderByDescending(x => x.UID).ToPagedList(page, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Roles = roleManager.Roles.Where(x => x.IsForAdmin);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel model, HttpPostedFileBase logo)
        {
            ModelState.Remove("BranchId");
            if (ModelState.IsValid)
            {
                string imgUrl = null;
                if (logo != null)
                {
                    imgUrl = FileManager.SaveImage(logo);
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name, IsOwner = false, Status = (int)Status.Approve, ImageUrl = imgUrl };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, model.Role);
                    return RedirectToAction("index");
                }
                AddErrors(result);
            }
            ViewBag.Roles = roleManager.Roles.Where(x => x.IsForAdmin);
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return View("error");
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return View("error");
            }
            RegisterViewModel model = new RegisterViewModel();
            model.Name = user.Name;
            model.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.BranchId = user.BranchId.GetValueOrDefault();
            model.Email = user.Email;
            model.Role = User.Identity.GetUserRole(id);
            model.Password = "$$$$$$$$$";
            model.ConfirmPassword = "$$$$$$$$$";
            ViewBag.ImgUrl = user.ImageUrl;
            ViewBag.Roles = roleManager.Roles.Where(x => x.IsForAdmin);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegisterViewModel model, HttpPostedFileBase logo)
        {
            ModelState.Remove("BranchId");
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByEmail(model.Email);
                if (user == null)
                {
                    return View("error");
                }
                user.Name = model.Name;
                user.CompanyId = User.Identity.GetCompanyId();
                user.BranchId = model.BranchId;
                if (logo != null)
                {
                    user.ImageUrl = FileManager.SaveImage(logo);
                }
                var result = UserManager.Update(user);

                if (result.Succeeded)
                {
                    if (!model.Password.Equals("$$$$$$$$$$"))
                    {
                        UserManager.RemovePassword(user.Id);
                        UserManager.AddPassword(user.Id, model.Password);
                    }
                    UserManager.RemoveFromRole(user.Id, User.Identity.GetUserRole(user.Id));
                    UserManager.AddToRole(user.Id, model.Role);
                    var brnchAccess = new BranchAccess { UId = user.UID, BranchId = model.BranchId, CreatedOn = DateTime.UtcNow, CreatedBy = User.Identity.GetCustomId() };
                    branchAccessHandler.RemoveAllAccess(user.UID);
                    branchAccessHandler.Add(brnchAccess);
                    return RedirectToAction("index");
                }
                AddErrors(result);
            }
            ViewBag.Roles = roleManager.Roles.Where(x => !x.IsForAdmin);
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        public PartialViewResult Detail(string id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return PartialView("error");
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return PartialView("error");
            }
            RegisterViewModel model = new RegisterViewModel();
            model.Name = user.Name;
            model.Email = user.Email;
            model.Role = User.Identity.GetUserRole(id);
            model.Password = "$$$$$$$$$";
            model.ConfirmPassword = "$$$$$$$$$";
            ViewBag.ImgUrl = user.ImageUrl;
            ViewBag.Roles = roleManager.Roles.Where(x => x.IsForAdmin);
            return PartialView("_DetailPartial", model);
        }

        public ActionResult StatusChange(string uid, int statusid, int page = 1)
        {
            var user = UserManager.FindById(uid);
            user.Status = statusid;
            UserManager.Update(user);
            return RedirectToAction("Index", new { page = page });
        }

        public ActionResult RoleChange(string uid, string role, int page = 1)
        {
            var user = UserManager.FindById(uid);
            var rle = User.Identity.GetUserRole(uid);
            UserManager.RemoveFromRole(uid, rle);
            UserManager.AddToRole(uid, role);
            UserManager.Update(user);
            return RedirectToAction("Index", new { page = page });
        }
    }
}