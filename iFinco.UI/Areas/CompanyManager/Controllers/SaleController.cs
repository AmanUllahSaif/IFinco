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
    public class SaleController : Controller
    {
        AccountHandler accountHandler;
        VNSAccountsDBEntities Accountcontext;
        SaleHandler saleHandler;
        iFincoDBEntities Ifincocontext;
        PartyHandler partyHandler;
        StockHandler stockHandler;
        SalesManager saleManager;

        public SaleController()
        {
            Accountcontext = new VNSAccountsDBEntities();
            Ifincocontext = new iFincoDBEntities();
            accountHandler = new AccountHandler(Accountcontext);
            saleHandler = new SaleHandler(Ifincocontext);
            partyHandler = new PartyHandler(Ifincocontext);
            stockHandler = new StockHandler(Ifincocontext);
            saleManager = new SalesManager(Accountcontext);
        }
        // GET: CompanyManager/Sale
        public ActionResult Index(SaleSearchModel model, long? id, int page = 1)
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
        public ActionResult Create(SaleVM model)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                decimal totalCost = 0;
                model.Sale.CreatedOn = DateTime.UtcNow;
                model.Sale.CreatedBy = User.Identity.GetCustomId();

                model.Sale.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
                model.Sale.BranchId = User.Identity.GetBranchId();
                model.Sale.InvoiceNo = saleHandler.GenerateInvoiceNo(model.Sale.CompanyId, model.Sale.BranchId);
                model.Sale.IsActive = true;
                foreach (var item in model.SaleDetails)
                {
                    item.IsActive = true;
                    item.CreatedOn = DateTime.UtcNow;
                    item.CreatedBy = User.Identity.GetCustomId();
                    model.Sale.SaleDetails.Add(item);
                }

                foreach (var item in model.SaleDetails)
                {
                    totalCost += stockHandler.DeductStock(item.ProductId, User.Identity.GetCompanyId().GetValueOrDefault(), item.Quantity, item.PACId, User.Identity.GetBranchId());
                }

                if (model.ShippingDetail.Name != null || model.ShippingDetail.Address != null || model.ShippingDetail.ShipmentNo != null)
                {
                    model.ShippingDetail.CreatedBy = User.Identity.GetCustomId();
                    model.ShippingDetail.CreatedOn = DateTime.UtcNow;
                    model.ShippingDetail.IsActive = true;
                    model.Sale.ShippingDetails.Add(model.ShippingDetail);
                }

                saleHandler.Add(model.Sale);
                saleManager.CreateSale(model.Sale.Id, model.Sale.Total, totalCost, model.Sale.PaidAmount, model.Sale.Date, model.Sale.PartyId.GetValueOrDefault(), User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                scope.Complete();
            }
            return RedirectToAction("Index", new { Id = model.Sale.Id });
        }

        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }

            var sale = saleHandler.FindById(id.GetValueOrDefault());
            if (sale.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }

            sale.IsActive = false;
            sale.ModifiedBy = User.Identity.GetCustomId();
            sale.ModifiedOn = DateTime.UtcNow;

            foreach (var item in sale.SaleDetails)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = User.Identity.GetCustomId();
            }

            saleHandler.Update(sale);
            return RedirectToAction("index");
        }
        public ActionResult Print(long id)
        {
            var sale = saleHandler.FindById(id);
            if (sale.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            var temp = TemplateResolver.ResolveSaleInovice(sale, User.Identity.GetCompanyName(), User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
    }
}