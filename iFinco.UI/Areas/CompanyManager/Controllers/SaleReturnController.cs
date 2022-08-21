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
    public class SaleReturnController : Controller
    {
        AccountHandler accountHandler;
        VNSAccountsDBEntities AccountDBcontext;
        SaleHandler saleHandler;
        iFincoDBEntities IfincoDBcontext;
        PartyHandler partyHandler;
        StockHandler stockHandler;
        SalesManager saleManager;
        SaleReturnHandler saleReturnHandler;

        public SaleReturnController()
        {

            AccountDBcontext = new VNSAccountsDBEntities();
            IfincoDBcontext = new iFincoDBEntities();
            accountHandler = new AccountHandler(AccountDBcontext);
            saleManager = new SalesManager(AccountDBcontext);

            saleHandler = new SaleHandler(IfincoDBcontext);
            saleReturnHandler = new SaleReturnHandler(IfincoDBcontext);
            partyHandler = new PartyHandler(IfincoDBcontext);
            stockHandler = new StockHandler(IfincoDBcontext);
        }

        // GET: CompanyManager/SaleReturn
        public ActionResult Index(SaleReturnSearchModel model, long? id, int page = 1)
        {
            ViewBag.Id = id;
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
            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            ViewBag.Party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View(model);
        }

        public ActionResult Create(long? id, string InvoiceNo)
        {
            SaleReturnVM saleReturnVM = new SaleReturnVM();
            if (id != null)
            {
                var sale = saleHandler.FindById(id.GetValueOrDefault());
                saleReturnVM.Sale = sale;
                saleReturnVM.SaleDetail = sale.SaleDetails.Where(x => x.IsActive).ToList();
                return View(saleReturnVM);
            }

            if (!string.IsNullOrEmpty(InvoiceNo))
            {
                var sale = saleHandler.FindByInvoiceNum(InvoiceNo, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                if (sale != null)
                {
                    saleReturnVM.Sale = sale;
                    saleReturnVM.SaleDetail = sale.SaleDetails.Where(x => x.IsActive).ToList();
                }
                else
                {
                    ViewBag.Msg = "No Such invoice no exits.";
                }

                return View(saleReturnVM);
            }
            return View(saleReturnVM);
        }
        [HttpPost]
        public ActionResult Create(SaleReturnVM model)
        {
            decimal totalCost = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                model.SaleReturn.CreatedOn = DateTime.UtcNow;
                model.SaleReturn.CreatedBy = User.Identity.GetCustomId();
                model.SaleReturn.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
                model.SaleReturn.BranchId = User.Identity.GetBranchId();
                model.SaleReturn.InvoiceNo = saleReturnHandler.GenerateInvoiceNo(model.SaleReturn.CompanyId, model.SaleReturn.BranchId);
                model.SaleReturn.IsActive = true;

                foreach (var item in model.SaleReturnDetail)
                {
                    item.IsActive = true;
                    item.CreatedOn = DateTime.UtcNow;
                    item.CreatedBy = User.Identity.GetCustomId();
                    model.SaleReturn.SaleReturnDetails.Add(item);
                }

                foreach (var item in model.SaleReturnDetail)
                {
                    stockHandler.AddStock(item.ProductId, User.Identity.GetCompanyId().GetValueOrDefault(), item.Quantity, item.Price, item.PACId, User.Identity.GetBranchId());
                }
                saleReturnHandler.Add(model.SaleReturn);
               saleManager.CreateSaleReturn(model.SaleReturn.Id, model.SaleReturn.Total, totalCost, model.SaleReturn.PaidAmount, model.SaleReturn.Date, model.SaleReturn.PartyId.GetValueOrDefault(), User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                scope.Complete();
            }
            return RedirectToAction("Index", new { Id =  model.SaleReturn.Id});
        }


        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }

            var saleReturn = saleReturnHandler.FindById(id.GetValueOrDefault());
            if (saleReturn.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            saleReturn.IsActive = false;
            saleReturn.ModifiedBy = User.Identity.GetCustomId();
            saleReturn.ModifiedOn = DateTime.UtcNow;

            foreach (var item in saleReturn.SaleReturnDetails)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = User.Identity.GetCustomId();
            }

            saleReturnHandler.Update(saleReturn);
            return RedirectToAction("index");
        }
        public ActionResult Print(long id)
        {

            var saleReturn = saleReturnHandler.FindById(id);
            if (saleReturn.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            var temp = TemplateResolver.ResolveSaleReturnInovice(saleReturn, User.Identity.GetCompanyName(), User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
    }
}