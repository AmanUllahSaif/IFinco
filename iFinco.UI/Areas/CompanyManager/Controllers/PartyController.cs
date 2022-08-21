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
    [Authorize]
    public class PartyController : Controller
    {
        iFincoDBEntities context;
        VNSAccountsDBEntities accountContext;
        PartyHandler partyHandler;
        AccountsManager accountManager;
        SaleHandler saleHandler;
        SaleReturnHandler saleReturnHandler;
        PurchaseHandler purchaseHandler;
        PurchaseReturnHandler purchaseReturnHandler;
        int pageSize;
        public PartyController()
        {
            context = new iFincoDBEntities();
            accountContext = new VNSAccountsDBEntities();
            partyHandler = new PartyHandler(context);
            accountManager = new AccountsManager(accountContext);
            pageSize = PagingManager.GetPageSize();
        }

        // GET: CompanyManager/Party
        public ActionResult Index(PartySearchModel model, int page = 1)
        {
            var list = partyHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            if (!string.IsNullOrEmpty(model.Title))
            {
                list = list.Where(x => x.Title.ToLower().Contains(model.Title.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                list = list.Where(x => x.Email.ToLower().Contains(model.Email.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Contact))
            {
                list = list.Where(x => x.Contact.ToLower().Contains(model.Contact.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.Adress))
            {
                list = list.Where(x => x.Adress.ToLower().Contains(model.Adress.ToLower()));
            }
            else if (!string.IsNullOrEmpty(model.CompanyTitle))
            {
                list = list.Where(x => x.CompanyTitle.ToLower().Contains(model.CompanyTitle.ToLower()));
            }

            model.Data = list.OrderByDescending(x => x.Id).ToPagedList(page, PagingManager.GetPageSize());
            return View(model);


        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Party model)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.CreatedBy = User.Identity.GetCustomId();
            model.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.BranchId = User.Identity.GetBranchId();
            model.IsActive = true;
            partyHandler.Add(model);
            AccountType type = AccountType.Assets;
            if (model.IsCredit)
            {
                type = AccountType.Libilites;
            }
            accountManager.CreateAccount(model.Title + " Account", model.Balance, model.IsCredit, type, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId(), model.Id);

            return RedirectToAction("Index");
        }
        public ActionResult Delete(long? id)
        {
            if (id == null || id < 1)
            {
                return HttpNotFound();
            }
            var party = partyHandler.FindById(id.GetValueOrDefault());
            party.IsActive = false;
            party.ModifiedOn = DateTime.UtcNow;
            party.ModifiedBy = User.Identity.GetCustomId();
            partyHandler.Update(party);
            return RedirectToAction("index");
        }
        [HttpGet]
        public PartialViewResult Detail(long? id)
        {
            if (id == null || id < 1)
            {

            }
            ViewBag.Categories = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            var model = partyHandler.FindById(id.GetValueOrDefault());
            return PartialView("_DetailPartial", model);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var party = partyHandler.FindById(id);
            if (party != null && party.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            return View(party);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Party model)
        {
            var party = partyHandler.FindById(model.Id);
            if (party != null && party.CompanyId != User.Identity.GetCompanyId().GetValueOrDefault())
            {
                return View("Error");
            }
            party.Title = model.Title;
            party.Email = model.Email;
            party.Contact = model.Contact;
            party.Adress = model.Adress;
            party.CompanyTitle = model.CompanyTitle;
            party.CompanyContact = model.CompanyContact;
            party.CompanyEmail = model.CompanyEmail;
            party.ModifiedOn = DateTime.UtcNow;
            party.ModifiedBy = User.Identity.GetCustomId();
            partyHandler.Update(party);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetParties(string prefix)
        {
            var parties = partyHandler.List.Where(x => x.IsActive && x.Title.ToLower().Contains(prefix.ToLower()) && x.CompanyId == User.Identity.GetCompanyId() && x.BranchId == User.Identity.GetBranchId());
            var data = (from pd in parties select new { Title = pd.Title, Id = pd.Id }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartyHistory(long? Id)
        {
            if (Id == null || Id < 0)
            {
                return View("error");
            }
            PartyHistoryVM partyHistoryVM = new PartyHistoryVM();
            var party = partyHandler.FindById(Id.GetValueOrDefault());
            if (party.CompanyId == User.Identity.GetCompanyId() && party.BranchId == User.Identity.GetBranchId())
            {
                partyHistoryVM.Party = party;
                partyHistoryVM.Sale = party.Sales.Where(x => x.IsActive).ToList();
                partyHistoryVM.SaleReturn = party.SaleReturns.Where(x => x.IsActive).ToList();
                partyHistoryVM.Purchase = party.Purchases.Where(x => x.IsActive).ToList();
                partyHistoryVM.PurchaseReturn = party.PurchaseReturns.Where(x => x.IsActive).ToList();
            }
            else
            {
                return HttpNotFound();
            }
            return View(partyHistoryVM);
        }
        public ActionResult PartySaleHistory(long? Id,int page=1)
        {
            if (Id == null || Id < 0)
            {
                return View("error");
            }
            PartyHistoryVM partyHistoryVM = new PartyHistoryVM();
            var party = partyHandler.FindById(Id.GetValueOrDefault());
            if (party.CompanyId == User.Identity.GetCompanyId() && party.BranchId == User.Identity.GetBranchId())
            {
                partyHistoryVM.Party = party;
                partyHistoryVM.Sale = party.Sales.Where(x => x.IsActive).ToList();
            }
            else
            {
                return HttpNotFound();
            }
            var sale = partyHistoryVM.Sale.OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            return PartialView("_PartySaleHistory", sale);
        }
        public ActionResult PartySaleReturnHistory(long? Id, int page = 1)
        {
            if (Id == null || Id < 0)
            {
                return View("error");
            }
            PartyHistoryVM partyHistoryVM = new PartyHistoryVM();
            var party = partyHandler.FindById(Id.GetValueOrDefault());
            if (party.CompanyId == User.Identity.GetCompanyId() && party.BranchId == User.Identity.GetBranchId())
            {
                partyHistoryVM.Party = party;
                partyHistoryVM.SaleReturn = party.SaleReturns.Where(x => x.IsActive).ToList();
            }
            else
            {
                return HttpNotFound();
            }
            var saleReturn = partyHistoryVM.SaleReturn.OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            return PartialView("_PartySaleReturnHistory", saleReturn);
        }
        public ActionResult PartyPurchaseHistory(long? Id, int page = 1)
        {
            if (Id == null || Id < 0)
            {
                return View("error");
            }
            PartyHistoryVM partyHistoryVM = new PartyHistoryVM();
            var party = partyHandler.FindById(Id.GetValueOrDefault());
            if (party.CompanyId == User.Identity.GetCompanyId() && party.BranchId == User.Identity.GetBranchId())
            {
                partyHistoryVM.Party = party;
                partyHistoryVM.Purchase = party.Purchases.Where(x => x.IsActive).ToList();
            }
            else
            {
                return HttpNotFound();
            }
            var purchase = partyHistoryVM.Purchase.OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            return PartialView("_PartyPurchaseHistory", purchase);
        }
        public ActionResult PartyPurchaseReturnHistory(long? Id, int page = 1)
        {
            if (Id == null || Id < 0)
            {
                return View("error");
            }
            PartyHistoryVM partyHistoryVM = new PartyHistoryVM();
            var party = partyHandler.FindById(Id.GetValueOrDefault());
            if (party.CompanyId == User.Identity.GetCompanyId() && party.BranchId == User.Identity.GetBranchId())
            {
                partyHistoryVM.Party = party;
                partyHistoryVM.PurchaseReturn = party.PurchaseReturns.Where(x => x.IsActive).ToList();
            }
            else
            {
                return HttpNotFound();
            }
            var purchaseReturn = partyHistoryVM.PurchaseReturn.OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            return PartialView("_PartyPurchaseReturnHistory", purchaseReturn);
        }
    }
}