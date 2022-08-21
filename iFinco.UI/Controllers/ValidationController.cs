using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFinco.UI.Controllers
{
    public class ValidationController : Controller
    {
        CompanyHandler companyHandler;
        iFincoDBEntities context;
        StockHandler stockHandler;
        public ValidationController()
        {
            context = new iFincoDBEntities();
            companyHandler = new CompanyHandler(context);
            stockHandler = new StockHandler(context);
        }
        // GET: Validation
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult IsComapnyEmailExits(string email)
        {
            bool isExit = false;
            var cmpny = companyHandler.List.Where(x => x.Email.ToLower().Equals(email.ToLower()) && x.IsActive).FirstOrDefault();
            if (cmpny != null)
            {
                isExit = true;
            }
            return Json(!isExit, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsBarCodeAlreadyExits(Stock stock)
        {
            bool isExit = false;
            isExit = stockHandler.BarCodeExits(stock.BarCode, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            return Json(!isExit, JsonRequestBehavior.AllowGet);
        }
    }
}