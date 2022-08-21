using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Models;
using iFinco.UI.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using VNS.Accounts;
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        AccountHandler accountHandler;
        VNSAccountsDBEntities Accountcontext;
        PurchaseHandler purchaseHandler;
        iFincoDBEntities Ifincocontext;
        PartyHandler partyHandler;
        PurchaseManager purchaseManager;
        StockHandler stockHandler;
        public PurchaseController()
        {
            Accountcontext = new VNSAccountsDBEntities();
            accountHandler = new AccountHandler(Accountcontext);
            purchaseManager = new PurchaseManager(Accountcontext);
            Ifincocontext = new iFincoDBEntities();
            purchaseHandler = new PurchaseHandler(Ifincocontext);
            partyHandler = new PartyHandler(Ifincocontext);
            stockHandler = new StockHandler(Ifincocontext);

        }

        // GET: CompanyManager/Purchase
        public ActionResult Index(PurchaseSearchModel model, long? id, int page = 1)
        {
            ViewBag.Id = id;
            var list = purchaseHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault() && x.BranchId == User.Identity.GetBranchId());
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
            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            ViewBag.Party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseVM model)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                decimal totalCost = 0;
                model.Purchase.CreatedOn = DateTime.UtcNow;
                model.Purchase.CreatedBy = User.Identity.GetCustomId();
                model.Purchase.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
                model.Purchase.BranchId = User.Identity.GetBranchId().GetValueOrDefault();
                model.Purchase.InvoiceNo = purchaseHandler.GenerateInvoiceNo(model.Purchase.CompanyId, model.Purchase.BranchId.GetValueOrDefault());
                model.Purchase.IsActive = true;
                foreach (var item in model.PurchaseDetail)
                {
                    item.IsActive = true;
                    item.CreatedOn = DateTime.UtcNow;
                    item.CreatedBy = User.Identity.GetCustomId();
                    model.Purchase.PurchaseDetails.Add(item);
                }

                foreach (var item in model.PurchaseDetail)
                {
                    stockHandler.AddStock(item.ProductId, User.Identity.GetCompanyId().GetValueOrDefault(), item.Quantity, item.Price, item.PACId, User.Identity.GetBranchId());
                }

                if (model.ShippingDetail.Name != null || model.ShippingDetail.Address != null || model.ShippingDetail.ShipmentNo != null)
                {
                    model.ShippingDetail.CreatedBy = User.Identity.GetCustomId();
                    model.ShippingDetail.CreatedOn = DateTime.UtcNow;
                    model.ShippingDetail.IsActive = true;
                    model.Purchase.ShippingDetails.Add(model.ShippingDetail);
                }

                purchaseHandler.Add(model.Purchase);
                purchaseManager.CreatePurchase(model.Purchase.Id, model.Purchase.Total, totalCost, model.Purchase.PaidAmount, model.Purchase.Date, model.Purchase.PartyId.GetValueOrDefault(), User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                scope.Complete();
            }
            return RedirectToAction("Index", new { Id = model.Purchase.Id });

        }

        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }

            var purchase = purchaseHandler.FindById(id.GetValueOrDefault());
            if (purchase.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            purchase.IsActive = false;
            purchase.ModifiedBy = User.Identity.GetCustomId();
            purchase.ModifiedOn = DateTime.UtcNow;

            foreach (var item in purchase.PurchaseDetails)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = User.Identity.GetCustomId();
            }

            purchaseHandler.Update(purchase);
            return RedirectToAction("index");
        }
        public ActionResult Print(long id)
        {

            var purchase = purchaseHandler.FindById(id);
            if (purchase.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            var temp = TemplateResolver.ResolvePurchaseInovice(purchase, User.Identity.GetCompanyName(), User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

    }
}