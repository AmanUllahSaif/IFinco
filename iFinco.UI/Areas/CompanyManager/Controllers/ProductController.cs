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

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        iFincoDBEntities context;
        ProductHandler productHandler;
        CategoryHandler categoryHandler;
        VariantHandler variantHandler;
        BrandHandler brandHandler;
        PACHandler pacHandler;
        ProductVariantHandler productVariantHandler;
        StockHandler stockHandler;
        public ProductController()
        {
            context = new iFincoDBEntities();
            productHandler = new ProductHandler(context);
            categoryHandler = new CategoryHandler(context);
            variantHandler = new VariantHandler(context);
            productVariantHandler = new ProductVariantHandler(context);
            brandHandler = new BrandHandler(context);
            pacHandler = new PACHandler(context);
            stockHandler = new StockHandler(context);
        }
        // GET: CompanyManager/Product
        public ActionResult Index(ProductSearchModel model, int page = 1)
        {
            var list = productHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            else
            if (!string.IsNullOrEmpty(model.Tags))
            {
                list = list.Where(x => x.Tags.ToLower().Contains(model.Tags.ToLower()));
            }
            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);
        }
        public ActionResult Create()
        {
            ViewBag.Categories = categoryHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            ViewBag.Variants = variantHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId() && x.IsActive).ToList();
            ViewBag.Brands = brandHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId() && x.IsActive).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductVM model, HttpPostedFileBase logo)
        {
            model.Product.CreatedOn = DateTime.UtcNow;
            model.Product.CreatedBy = User.Identity.GetCustomId();
            model.Product.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.Product.BranchId = User.Identity.GetBranchId();
            model.Product.IsActive = true;
            if (logo != null)
            {
                model.Product.ImageUrl = FileManager.SaveImage(logo);
            }
            if (model.Product.HaveVarients && model.ProductVariant != null)
            {
                foreach (var item in model.ProductVariant)
                {
                    item.IsActive = true;
                    item.CreatedOn = DateTime.UtcNow;
                    item.CreatedBy = User.Identity.GetCustomId();
                    model.Product.ProductVariants.Add(item);
                }
            }


            productHandler.Add(model.Product);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }

            var product = productHandler.FindById(id.GetValueOrDefault());
            product.ModifiedOn = DateTime.UtcNow;
            product.ModifiedBy = User.Identity.GetCustomId();
            product.IsActive = false;
            foreach (var item in product.Stocks)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = product.ModifiedBy;
            }
            productHandler.Update(product);
            return RedirectToAction("index");
        }
        [HttpGet]
        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {

            }

            ViewBag.Categories = categoryHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            var model = productHandler.FindById(id.GetValueOrDefault());
            return PartialView("_DetailPartial", model);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            ViewBag.Categories = categoryHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            ViewBag.Variants = variantHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId() && x.IsActive).ToList();
            ViewBag.Brands = brandHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId() && x.IsActive).ToList();
            ProductVM productVM = new ProductVM();
            productVM.Product = productHandler.FindById(id);
            productVM.ProductVariant = productVM.Product.ProductVariants.Where(x => x.IsActive).ToList();
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductVM model, HttpPostedFileBase logo)
        {
            var product = productHandler.FindById(model.Product.Id);

            product.ModifiedOn = DateTime.UtcNow;
            product.ModifiedBy = User.Identity.GetCustomId();
            product.Title = model.Product.Title;
            product.Tags = model.Product.Tags;
            product.Description = model.Product.Description;
            product.HasExpiry = model.Product.HasExpiry;
            product.HaveVarients = model.Product.HaveVarients;
            model.Product.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.Product.BranchId = User.Identity.GetBranchId();

            if (logo != null)
            {
                product.ImageUrl = model.Product.ImageUrl = FileManager.SaveImage(logo);
            }

            foreach (var item in product.ProductVariants)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = User.Identity.GetCustomId();
            }

            if (model.Product.HaveVarients && model.ProductVariant != null)
            {
                foreach (var item in model.ProductVariant)
                {
                    if (item.Id > 0)
                    {
                        var oldVal = product.ProductVariants.SingleOrDefault(x => x.Id == item.Id);
                        oldVal.IsActive = true;
                    }
                    else
                    {
                        item.IsActive = true;
                        item.CreatedOn = DateTime.UtcNow;
                        item.CreatedBy = User.Identity.GetCustomId();
                        product.ProductVariants.Add(item);
                    }
                }
            }

            productHandler.Update(product);
            return RedirectToAction("Index");
        }


        public JsonResult GetVariants(int id)
        {
            var product = productHandler.FindById(id);
            object data = new object();
            if (product.HaveVarients)
            {
                var prodVarints = productVariantHandler.List.Where(x => x.IsActive && x.ProductId == id);
                data = from pv in prodVarints select new { Id = pv.VariantId, Title = pv.Variant.Title, Values = pv.Variant.VariantValues.Where(x => x.IsActive).Select(x => new { Id = x.Id, Title = x.Title }) };
            }
            var prodData = new { HasExpri = product.HasExpiry, varints = data };
            return Json(prodData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProducts(string prefix)
        {
            var products = productHandler.List.Where(x => x.IsActive && x.Title.ToLower().Contains(prefix.ToLower()) && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            var data = (from pd in products select new ProductSearchResultVM { Title = pd.Title, Id = pd.Id, HaveVarrient = pd.HaveVarients, SellingPrice = 0, PurchasingPrice = 0 }).ToList();
            foreach (var item in data)
            {
                var stk = stockHandler.List.Where(x => x.ProductId == item.Id && x.IsActive && (x.SellingPrice > 0 || x.PurchasingPrice > 0)).OrderByDescending(x => x.Id).FirstOrDefault();
                if (stk != null && stk.Id > 0)
                {
                    item.SellingPrice = stk.SellingPrice;
                    item.PurchasingPrice = stk.PurchasingPrice;
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PACSearch(PACSearchVM model)
        {
            var pacs = pacHandler.GetByProdId(model.prodId);
            ProductAttributeCombination pc = null;

            foreach (var item in pacs)
            {
                bool isThis = true;
                if (item.PACDetails.Where(x => x.IsActive).Count() == model.varints.Count)
                {
                    foreach (var virnts in model.varints)
                    {
                        var vf = item.PACDetails.Where(x => x.IsActive && x.VariantId == virnts.varientId && x.VariantValueId == virnts.valueId).FirstOrDefault();
                        if (vf == null || vf.Id < 1)
                        {
                            isThis = false;
                        }
                    }
                    if (isThis)
                    {
                        pc = item;
                        break;
                    }
                }
            }

            if (pc != null && pc.Id > 0)
            {
                var stock = stockHandler.List.Where(x => x.IsActive && x.PACId == pc.Id && x.ProductId == model.prodId && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId() && (x.SellingPrice > 0 || x.PurchasingPrice > 0)).OrderByDescending(x => x.Id).FirstOrDefault();
                var price = new PACSearchResultVM { PACId = pc.Id, SellingPrice = 0, PurchasingPrice = 0 };
                if (stock != null && stock.Id > 0)
                {
                    price.SellingPrice = stock.SellingPrice;
                    price.PurchasingPrice = stock.PurchasingPrice;
                }
                return Json(price, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}