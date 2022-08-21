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
using VNS.Accounts;
using VNS.Accounts.DAL;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{


    public class ReportsController : Controller
    {
        iFincoDBEntities context;
        VNSAccountsDBEntities acountContext;
        Party parties;
        PartyHandler partyHandler;
        AccountsManager accountManager;
        SaleHandler saleHandler;
        PurchaseHandler purchaseHandler;
        SaleReturnHandler saleReturnHandler;
        PurchaseReturnHandler purchaseReturnHandler;
        public ReportsController()
        {
            acountContext = new VNSAccountsDBEntities();
            context = new iFincoDBEntities();
            parties = new Party(context);
            partyHandler = new PartyHandler(context);
            saleHandler = new SaleHandler(context);
            accountManager = new AccountsManager(acountContext);
            purchaseHandler = new PurchaseHandler(context);
            saleReturnHandler = new SaleReturnHandler(context);
            purchaseReturnHandler = new PurchaseReturnHandler(context);
        }
        public ActionResult PartyLedgre()
        {
            var parties = partyHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            foreach (var item in parties)
            {
                item.Balance = accountManager.GetPartyAccount(User.Identity.GetCompanyId().GetValueOrDefault(), item.Id, User.Identity.GetBranchId().GetValueOrDefault()).Balance;
            }
            return View(parties);
        }
        public ActionResult PartyPrint(Party model)
        {
            var parties = partyHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            foreach (var item in parties)
            {
                item.Balance = accountManager.GetPartyAccount(User.Identity.GetCompanyId().GetValueOrDefault(), item.Id, User.Identity.GetBranchId().GetValueOrDefault()).Balance;
            }
            string reportType = "General Ledgre";
            var temp = TemplateResolver.ResolvePartyReport(parties.ToList(), User.Identity.GetCompanyName(), DateTime.UtcNow, DateTime.UtcNow, reportType, User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sale(SaleSearchModel model, long? id)
        {
            ViewBag.Id = id;
            var list = saleHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault());
            if (model.PartyId > 0)
            {
                list = list.Where(x => x.PartyId == model.PartyId);
            }
            if (model.DateFrom != null)
            {
                list = list.Where(x => x.Date >= model.DateFrom.GetValueOrDefault().Date);
            }
            if (model.DateTo != null)
            {
                list = list.Where(x => x.Date <= model.DateTo.GetValueOrDefault().Date);
            }
            model.Data = list.OrderByDescending(x => x.Id);
            ViewBag.Party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View(model);
        }

        public ActionResult SalePrint(long? PartyId, DateTime? DateFrom, DateTime? DateTo)
        {
            var sales = saleHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            if (PartyId != null)
            {
                sales = sales.Where(x => x.PartyId == PartyId);
            }
            if (DateFrom != null)
            {
                var dateFrom = DateFrom.GetValueOrDefault();
                sales = sales.Where(x => x.Date >= dateFrom.Date);
            }
            else
            {
                DateFrom = DateTime.UtcNow;
            }
            if (DateTo != null)
            {
                var dateTo = DateTo.GetValueOrDefault();
                sales = sales.Where(x => x.Date <= dateTo.Date);
            }
            else
            {
                DateTo = DateTime.UtcNow;
            }
            string reportType = "Sales Report";
            var temp = TemplateResolver.ResolveSaleReport(sales.ToList(), User.Identity.GetCompanyName(), DateFrom.GetValueOrDefault(), DateTo.GetValueOrDefault(), reportType, User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaleReturn(SaleReturnSearchModel model, long? id)
        {
            var list = saleReturnHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault());
            if (model.PartyId > 0)
            {
                list = list.Where(x => x.PartyId == model.PartyId);
            }
            if (model.DateFrom != null)
            {
                list = list.Where(x => x.Date >= model.DateFrom.GetValueOrDefault().Date);
            }
            if (model.DateTo != null)
            {
                list = list.Where(x => x.Date <= model.DateTo.GetValueOrDefault().Date);
            }
            model.Data = list.OrderByDescending(x => x.Id);
            ViewBag.Party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View(model);
        }

        public ActionResult SaleReturnPrint(long? PartyId, DateTime? DateFrom, DateTime? DateTo)
        {
            var salesreturn = saleReturnHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            if (PartyId != null)
            {
                salesreturn = salesreturn.Where(x => x.PartyId == PartyId);
            }
            if (DateFrom != null)
            {
                var dateFrom = DateFrom.GetValueOrDefault();
                salesreturn = salesreturn.Where(x => x.Date >= dateFrom.Date);
            }
            else
            {
                DateFrom = DateTime.UtcNow;
            }
            if (DateTo != null)
            {
                var dateTo = DateTo.GetValueOrDefault();
                salesreturn = salesreturn.Where(x => x.Date <= dateTo.Date);
            }
            else
            {
                DateTo = DateTime.UtcNow;
            }
            string reportType = "Sales Return Report";
            var temp = TemplateResolver.ResolveSaleReturnReport(salesreturn.ToList(), User.Identity.GetCompanyName(), DateFrom.GetValueOrDefault(), DateTo.GetValueOrDefault(), reportType, User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Purchase(PurchaseSearchModel model, long? id)
        {
           
            var list = purchaseHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault());
            if (model.PartyId > 0)
            {
                list = list.Where(x => x.PartyId == model.PartyId);
            }
            if (model.DateFrom != null)
            {
                list = list.Where(x => x.Date >= model.DateFrom.GetValueOrDefault().Date);
            }
            if (model.DateTo != null)
            {
                list = list.Where(x => x.Date <= model.DateTo.GetValueOrDefault().Date);
            }
            model.Data = list.OrderByDescending(x => x.Id);
            ViewBag.Party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View(model);
        }

        public ActionResult PurchasePrint(long? PartyId, DateTime? DateFrom, DateTime? DateTo)
        {
            var purchases = purchaseHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            if (PartyId != null)
            {
                purchases = purchases.Where(x => x.PartyId == PartyId);
            }
            if (DateFrom != null)
            {
                var dateFrom = DateFrom.GetValueOrDefault();
                purchases = purchases.Where(x => x.Date >= dateFrom.Date);
            }
            else
            {
                DateFrom = DateTime.UtcNow;
            }
            if (DateTo != null)
            {
                var dateTo = DateTo.GetValueOrDefault();
                purchases = purchases.Where(x => x.Date <= dateTo.Date);
            }
            else
            {
                DateTo = DateTime.UtcNow;
            }
            string reportType = "Purchase Report";
            var temp = TemplateResolver.ResolvePurchaseReport(purchases.ToList(), User.Identity.GetCompanyName(), DateFrom.GetValueOrDefault(), DateTo.GetValueOrDefault(), reportType, User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PurchaseReturn(PurchaseReturnSearchModel model, long? id)
        {

            var list = purchaseReturnHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault());
            if (model.PartyId > 0)
            {
                list = list.Where(x => x.PartyId == model.PartyId);
            }
            if (model.DateFrom != null)
            {
                list = list.Where(x => x.Date >= model.DateFrom.GetValueOrDefault().Date);
            }
            if (model.DateTo != null)
            {
                list = list.Where(x => x.Date <= model.DateTo.GetValueOrDefault().Date);
            }
            model.Data = list.OrderByDescending(x => x.Id);
            ViewBag.Party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View(model);
        }

        public ActionResult PurchaseReturnPrint(long? PartyId, DateTime? DateFrom, DateTime? DateTo)
        {
            var purchasesreturn = purchaseReturnHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            if (PartyId != null)
            {
                purchasesreturn = purchasesreturn.Where(x => x.PartyId == PartyId);
            }
            if (DateFrom != null)
            {
                var dateFrom = DateFrom.GetValueOrDefault();
                purchasesreturn = purchasesreturn.Where(x => x.Date >= dateFrom.Date);
            }
            else
            {
                DateFrom = DateTime.UtcNow;
            }
            if (DateTo != null)
            {
                var dateTo = DateTo.GetValueOrDefault();
                purchasesreturn = purchasesreturn.Where(x => x.Date <= dateTo.Date);
            }
            else
            {
                DateTo = DateTime.UtcNow;
            }
            string reportType = "Purchase Return Report";
            var temp = TemplateResolver.ResolvePurchaseReturnReport(purchasesreturn.ToList(), User.Identity.GetCompanyName(), DateFrom.GetValueOrDefault(), DateTo.GetValueOrDefault(), reportType, User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

    }
}