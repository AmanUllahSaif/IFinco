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
    public class PurchaseReturnController : Controller
    {
        // GET: CompanyManager/PurchaseReturn
        AccountHandler accountHandler;
        VNSAccountsDBEntities AccountDBcontext;
        PurchaseHandler purchaseHandler;
        iFincoDBEntities IfincoDBcontext;
        PartyHandler partyHandler;
        StockHandler stockHandler;
        PurchaseManager purchaseManager;
        PurchaseReturnHandler purchaseReturnHandler;

        public PurchaseReturnController()
        {

            AccountDBcontext = new VNSAccountsDBEntities();
            IfincoDBcontext = new iFincoDBEntities();
            accountHandler = new AccountHandler(AccountDBcontext);
            purchaseManager = new PurchaseManager(AccountDBcontext);

            purchaseHandler = new PurchaseHandler(IfincoDBcontext);
            purchaseReturnHandler = new PurchaseReturnHandler(IfincoDBcontext);
            partyHandler = new PartyHandler(IfincoDBcontext);
            stockHandler = new StockHandler(IfincoDBcontext);
        }

        // GET: CompanyManager/PurchaseReturn
        public ActionResult Index(PurchaseReturnSearchModel model, long? id, int page = 1)
        {
            ViewBag.Id = id;
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
            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            ViewBag.Party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View(model);
        }

        public ActionResult Create(long? id, string InvoiceNo)
        {
            PurchaseReturnVM purchaseReturnVM = new PurchaseReturnVM();
            if (id != null)
            {
                var purchase = purchaseHandler.FindById(id.GetValueOrDefault());
                purchaseReturnVM.Purchase = purchase;
                purchaseReturnVM.PurchaseDetail = purchase.PurchaseDetails.Where(x => x.IsActive).ToList();
                return View(purchaseReturnVM);
            }

            if (!string.IsNullOrEmpty(InvoiceNo))
            {
                var purchase = purchaseHandler.FindByInvoiceNum(InvoiceNo, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                if (purchase != null)
                {
                    purchaseReturnVM.Purchase = purchase;
                    purchaseReturnVM.PurchaseDetail = purchase.PurchaseDetails.Where(x => x.IsActive).ToList();
                }
                else
                {
                    ViewBag.Msg = "No Such invoice no exits.";
                }

                return View(purchaseReturnVM);
            }
            return View(purchaseReturnVM);
        }
        [HttpPost]
        public ActionResult Create(PurchaseReturnVM model)
        {
            decimal totalCost = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                model.PurchaseReturn.CreatedOn = DateTime.UtcNow;
                model.PurchaseReturn.CreatedBy = User.Identity.GetCustomId();
                model.PurchaseReturn.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
                model.PurchaseReturn.BranchId = User.Identity.GetBranchId();
                model.PurchaseReturn.InvoiceNo = purchaseReturnHandler.GenerateInvoiceNo(model.PurchaseReturn.CompanyId, model.PurchaseReturn.BranchId);
                model.PurchaseReturn.IsActive = true;

                foreach (var item in model.PurchaseReturnDetail)
                {
                    item.IsActive = true;
                    item.CreatedOn = DateTime.UtcNow;
                    item.CreatedBy = User.Identity.GetCustomId();
                    model.PurchaseReturn.PurchaseReturnDetails.Add(item);
                }

                foreach (var item in model.PurchaseReturnDetail)
                {
                    stockHandler.DeductStock(item.ProductId, User.Identity.GetCompanyId().GetValueOrDefault(), item.Quantity, item.PACId, User.Identity.GetBranchId());
                }
                purchaseReturnHandler.Add(model.PurchaseReturn);
                purchaseManager.CreatePurchaseReturn(model.PurchaseReturn.Id, model.PurchaseReturn.Total, totalCost, model.PurchaseReturn.PaidAmount, model.PurchaseReturn.Date, model.PurchaseReturn.PartyId.GetValueOrDefault(), User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                scope.Complete();
            }
            return RedirectToAction("Index", new { Id = model.PurchaseReturn.Id });
        }


        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }

            var purchaseReturn = purchaseReturnHandler.FindById(id.GetValueOrDefault());
            if (purchaseReturn.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            purchaseReturn.IsActive = false;
            purchaseReturn.ModifiedBy = User.Identity.GetCustomId();
            purchaseReturn.ModifiedOn = DateTime.UtcNow;

            foreach (var item in purchaseReturn.PurchaseReturnDetails)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = User.Identity.GetCustomId();
            }

            purchaseReturnHandler.Update(purchaseReturn);
            return RedirectToAction("index");
        }
        public ActionResult Print(long id)
        {

            var purchaseReturn = purchaseReturnHandler.FindById(id);
            if (purchaseReturn.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            var temp = TemplateResolver.ResolvePurchaseReturnInovice(purchaseReturn, User.Identity.GetCompanyName(), User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
    }
}