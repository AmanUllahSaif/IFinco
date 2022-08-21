using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class BarCodeController : Controller
    {
        iFincoDBEntities context;
        StockHandler stockHandler;

        public BarCodeController()
        {
            context = new iFincoDBEntities();
            stockHandler = new StockHandler(context);
        }

        // GET: CompanyManager/BarCode
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GenerateBarCode(string id)
        {
            bool IsExist = stockHandler.BarCodeExits(id, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            if (!IsExist)
            {
                var data = BarCodeGenerator.GetBarCode(id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}