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
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class StockController : Controller
    {

        VNSAccountsDBEntities accountsContext;
        AccountsManager accountManager;

        iFincoDBEntities context;
        StockHandler stockHandler;
        ProductHandler productHandler;
        GoDownHandler godownHandler;
        public StockController()
        {
            context = new iFincoDBEntities();
            accountsContext = new VNSAccountsDBEntities();
            stockHandler = new StockHandler(context);
            productHandler = new ProductHandler(context);
            godownHandler = new GoDownHandler(context);
            accountManager = new AccountsManager(accountsContext);
        }
        // GET: CompanyManager/Stock
        public ActionResult Index(StockSearchModel model, int page = 1)
        {
            var list = stockHandler.GetStock(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault());
            //var list = stockHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault() && x.BranchId == User.Identity.GetBranchId());
            ViewBag.Godown = godownHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Product.Title.ToLower().Contains(model.Title.ToLower())).ToList();
            }
            else if (model.GodownId > 0)
            {
                list = list.Where(x => x.GodownId == model.GodownId).ToList();
            }
            model.Data = list.OrderByDescending(x => x.ProductId).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.Product = productHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            ViewBag.Godown = godownHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockVM model)
        {
            model.Stock.CreatedOn = DateTime.UtcNow;
            model.Stock.CreatedBy = User.Identity.GetCustomId();
            model.Stock.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.Stock.BranchId = User.Identity.GetBranchId();
            model.Stock.IsActive = true;
            var pac = new ProductAttributeCombination();
            pac.IsActive = true;
            pac.CreatedOn = DateTime.UtcNow;
            pac.CreatedBy = User.Identity.GetCustomId();
            pac.ProductId = model.Stock.ProductId;
            if (model.PACDetail != null)
            {
                foreach (var item in model.PACDetail)
                {
                    item.IsActive = true;
                    item.CreatedBy = pac.CreatedBy;
                    item.CreatedOn = pac.CreatedOn;
                    pac.PACDetails.Add(item);
                }
                model.Stock.ProductAttributeCombination = pac;
            }

            stockHandler.Add(model.Stock);
            accountManager.SaveStock(User.Identity.GetCompanyId().GetValueOrDefault(), (model.Stock.PurchasingPrice * model.Stock.Quantity), User.Identity.GetBranchId());
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long? id, long? pacId)
        {
            if (id == null)
            {
                return View("error");
            }

            ViewBag.Product = productHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId() && x.IsActive).ToList();
            ViewBag.Godown = godownHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();

            var stocks = stockHandler.GetDetail(id.GetValueOrDefault(), pacId, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault());
            var stckGrp = stocks.GroupBy(x => new { x.ProductId, x.PACId });
            var model = new Stock();
            model.ProductId = id.GetValueOrDefault();
            model.PACId = pacId;
            model.Product = stckGrp.FirstOrDefault().FirstOrDefault().Product;
            model.Quantity = stckGrp.FirstOrDefault().Sum(x => x.Quantity);
            model.SellingPrice = stckGrp.FirstOrDefault().FirstOrDefault().SellingPrice;
            model.PurchasingPrice = stckGrp.FirstOrDefault().FirstOrDefault().PurchasingPrice;
            model.Code = stckGrp.FirstOrDefault().FirstOrDefault().Code;
            model.BarCode = stckGrp.FirstOrDefault().FirstOrDefault().BarCode;
            model.BarCodeImgUrl = stckGrp.FirstOrDefault().FirstOrDefault().BarCodeImgUrl;
            model.ExpiryDate = stckGrp.FirstOrDefault().FirstOrDefault().ExpiryDate;
            model.ProductId = stckGrp.FirstOrDefault().FirstOrDefault().ProductId;
            model.GodownId = stckGrp.FirstOrDefault().FirstOrDefault().GodownId;

            StockVM stockVM = new StockVM();
            stockVM.Stock = model;
            if (stckGrp.FirstOrDefault().FirstOrDefault().ProductAttributeCombination != null)
            {
                stockVM.PACDetail = stckGrp.FirstOrDefault().FirstOrDefault().ProductAttributeCombination.PACDetails.ToList();
            }
            return View(stockVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeductStock(StockVM model)
        {
            stockHandler.DeductStock(model.Stock.ProductId, User.Identity.GetCompanyId().GetValueOrDefault(), model.Stock.Quantity, model.Stock.PACId, User.Identity.GetBranchId());
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(StockVM model, HttpPostedFileBase logo)
        {
            var stock = stockHandler.FindById(model.Stock.Id);
            if (stock.CompanyId != User.Identity.GetCompanyId())
            {
                return View("error");
            }
            stock.ModifiedOn = DateTime.UtcNow;
            stock.ModifiedBy = User.Identity.GetCustomId();
            stock.ProductId = model.Stock.ProductId;
            stock.PurchasingPrice = model.Stock.PurchasingPrice;
            stock.Quantity = model.Stock.Quantity;
            //stock.CompanyId = model.Stock.CompanyId;
            //stock.BranchId = model.Stock.BranchId;
            stock.GodownId = model.Stock.GodownId;
            //stock.PACId = model.Stock.PACId;
            stock.SellingPrice = model.Stock.SellingPrice;
            stock.Code = model.Stock.Code;
            stock.BarCode = model.Stock.BarCode;
            stock.BarCodeImgUrl = model.Stock.BarCodeImgUrl;
            stock.ExpiryDate = model.Stock.ExpiryDate;
            if (logo != null)
            {
                model.Stock.BarCodeImgUrl = FileManager.SaveImage(logo);
            }

            if (stock.ProductAttributeCombination != null)
            {
                foreach (var item in stock.ProductAttributeCombination.PACDetails)
                {
                    item.IsActive = false;
                    item.ModifiedBy = User.Identity.GetCustomId();
                    item.ModifiedOn = DateTime.UtcNow;
                }
                stock.ProductAttributeCombination.IsActive = false;
                stock.ProductAttributeCombination.ModifiedBy = User.Identity.GetCustomId();
                stock.ProductAttributeCombination.ModifiedOn = DateTime.UtcNow;
            }

            if (model.PACDetail != null)
            {
                if (stock.ProductAttributeCombination == null)
                {
                    stock.ProductAttributeCombination = new ProductAttributeCombination();
                    stock.ProductAttributeCombination.IsActive = true;
                    stock.ProductAttributeCombination.CreatedBy = User.Identity.GetCustomId();
                    stock.ProductAttributeCombination.CreatedOn = DateTime.UtcNow;
                    stock.ProductAttributeCombination.ProductId = stock.ProductId;
                }
                else
                {
                    stock.ProductAttributeCombination.IsActive = true;
                }
                foreach (var item in model.PACDetail)
                {
                    if (item.Id > 0)
                    {
                        var oldVal = stock.ProductAttributeCombination.PACDetails.SingleOrDefault(x => x.Id == item.Id);
                        oldVal.VariantId = item.VariantId;
                        oldVal.VariantValueId = item.VariantValueId;
                        oldVal.IsActive = true;
                    }
                    else
                    {
                        item.IsActive = true;
                        item.CreatedOn = DateTime.UtcNow;
                        item.CreatedBy = User.Identity.GetCustomId();
                        stock.ProductAttributeCombination.PACDetails.Add(item);
                    }
                }
            }
            stockHandler.Update(stock);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long? id, long? pacId)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }
            var stock = stockHandler.FindById(id.GetValueOrDefault());
            if (stock.CompanyId != User.Identity.GetCompanyId())
            {
                return View("error");
            }
            stock.IsActive = false;
            stockHandler.Update(stock);
            return RedirectToAction("Index");
        }

        public PartialViewResult Detail(long? id, long? pacId)
        {
            if (id == null || id < 1)
            {
                return PartialView("error");
            }
            var stocks = stockHandler.GetDetail(id.GetValueOrDefault(), pacId, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault());
            return PartialView("_DetailPartial", stocks);
        }
    }
}