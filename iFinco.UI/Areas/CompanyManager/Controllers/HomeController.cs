using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNS.Accounts;
using VNS.Accounts.DAL;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        iFincoDBEntities context;
        StockHandler stockHandler;
        SaleHandler saleHandler;
        VNSAccountsDBEntities Accountcontext;
        AccountsManager accountManager;

        PurchaseHandler purchaseHandler;
        public HomeController()
        {
            Accountcontext = new VNSAccountsDBEntities();
            context = new iFincoDBEntities();
            stockHandler = new StockHandler(context);
            saleHandler = new SaleHandler(context);
            accountManager = new AccountsManager(Accountcontext);
            purchaseHandler = new PurchaseHandler(context);
        }
        // GET: CompanyManager/Home
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult StockStatus()
        {
            var stocks = stockHandler.GetStock(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault());
            stocks = stocks.Where(x => x.Quantity < 1).ToList();
            return PartialView("_StockStatusPartialView", stocks);
        }

        public PartialViewResult ExpriedStock()
        {
            var stocks = stockHandler.GetExpiredStock(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault());
            return PartialView("_StockStatusPartialView", stocks);
        }

        public PartialViewResult NearToExpireStock()
        {
            var stocks = stockHandler.GetNearExpireStock(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault());
            return PartialView("_StockStatusPartialView", stocks);
        }
        public PartialViewResult TotalSales()
        {
            Sale sale = new Sale();
            sale.Total = saleHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId()).Sum(x => x.Total);
            return PartialView("_TotalSalePatialView", sale);
        }
        public PartialViewResult TotalRevinue()
        {

            decimal sum = accountManager.GetRevenue(DateTime.UtcNow, DateTime.UtcNow, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault()).Sum(x => x.Balance);
            if (sum < 0)
            {
                sum *= -1;
                ViewBag.sum = "(" + sum + ")";
            }
            else
            {
                ViewBag.sum = sum;
            }
            return PartialView("_TotalRevinuePartial");
        }
        public PartialViewResult TotalDr()
        {
            ViewBag.Dr = accountManager.GetAllDr(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault()).Sum(x => x.Balance);
            return PartialView("_TotalDrPartial");
        }
        public PartialViewResult TotalCr()
        {
            ViewBag.Cr = accountManager.GetAllCr(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault()).Sum(x => x.Balance * -1);

            return PartialView("_TotalCrPartial");
        }

        public JsonResult WeeklyTotalSale()
        {
            int diff = (7 + (DateTime.UtcNow.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime dt = DateTime.UtcNow.AddDays(-1 * diff).Date;
            List<decimal> sales = new List<decimal>();
            //decimal total = 0 ;
            for (int i = 0; i < 7; i++)
            {
                var total = saleHandler.List.Where(x => x.Date == dt.AddDays(i).Date && x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId()).Sum(x => x.Total - x.Discount);
                sales.Add(Convert.ToDecimal(total));
            }
            return Json(sales, JsonRequestBehavior.AllowGet);
        }

        public JsonResult WeeklyTotalPurchase()
        {
            int diff = (7 + (DateTime.UtcNow.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime dt = DateTime.UtcNow.AddDays(-1 * diff).Date;
            List<decimal> purchases = new List<decimal>();
            //decimal total = 0 ;
            for (int i = 0; i < 7; i++)
            {
                var total = purchaseHandler.List.Where(x => x.Date == dt.AddDays(i).Date && x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId()).Sum(x => x.Total - x.Discount);
                purchases.Add(Convert.ToDecimal(total));
            }
            return Json(purchases, JsonRequestBehavior.AllowGet);
        }
    }
}