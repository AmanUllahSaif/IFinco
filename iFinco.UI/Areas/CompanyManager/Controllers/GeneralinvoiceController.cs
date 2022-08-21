using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.DAL.Enum;
using iFinco.UI.Models;
using iFinco.UI.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNS.Accounts;
using VNS.Accounts.DAL;

namespace iFinco.UI.Areas.CompanyManager.Controllers
{
    [Authorize]
    public class GeneralinvoiceController : Controller
    {
        VNSAccountsDBEntities AccountDBcontext;
        iFincoDBEntities context;
        PartyHandler partyHandler;
        AccountsManager accountManager;
        GeneralinvoiceHandler generalinvoiceHandler;
        public GeneralinvoiceController()
        {
            context = new iFincoDBEntities();
            AccountDBcontext = new VNSAccountsDBEntities();
            partyHandler = new PartyHandler(context);
            generalinvoiceHandler = new GeneralinvoiceHandler(context);
            accountManager = new AccountsManager(AccountDBcontext);
        }
        // GET: CompanyManager/Generalinvoice
        public ActionResult Index(GeneralinvoiceSearchModel model, long? id)
        {
            ViewBag.Id = id;
            var list = generalinvoiceHandler.List.Where(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault() && x.BranchId == User.Identity.GetBranchId());

            if (model.From != null)
            {
                list = list.Where(x => x.Date >= model.From);
            }
            if (model.To != null)
            {
                list = list.Where(x => x.Date <= model.To);
            }
            model.invoice = list.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult RecivedFromParty()
        {
            ViewBag.party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault() && x.BranchId == User.Identity.GetBranchId() && x.IsActive);

            return View();
        }
        [HttpPost]
        public ActionResult RecivedFromParty(GeneralinvoiceVM model)
        {
            Generalinvoice generalinvoice = new Generalinvoice();
            ViewBag.party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault() && x.BranchId == User.Identity.GetBranchId() && x.IsActive);
            generalinvoice.PartyId = model.PartyId;
            generalinvoice.Amount = model.Amount;
            generalinvoice.Date = model.Date;
            generalinvoice.CreatedOn = DateTime.UtcNow;
            generalinvoice.CreatedBy = User.Identity.GetCustomId();
            generalinvoice.IsActive = true;
            generalinvoice.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            generalinvoice.BranchId = User.Identity.GetBranchId().GetValueOrDefault();
            generalinvoice.Type = (int)InvoiceType.Recived;
            generalinvoice.InvoiceNum = generalinvoiceHandler.GenerateInvoiceNo(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            generalinvoiceHandler.Add(generalinvoice);
            accountManager.RecivedFromParty(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault(), generalinvoice.Id, generalinvoice.PartyId.GetValueOrDefault(), generalinvoice.Amount, DateTime.UtcNow);
            return RedirectToAction("index", new { id = generalinvoice.Id });
        }
        [HttpGet]
        public ActionResult PaidToParty()
        {
            ViewBag.party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault() && x.BranchId == User.Identity.GetBranchId().GetValueOrDefault() && x.IsActive);
            return View();
        }
        [HttpPost]
        public ActionResult PaidToParty(GeneralinvoiceVM model)
        {
            Generalinvoice generalinvoice = new Generalinvoice();
            ViewBag.party = partyHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault() && x.BranchId == User.Identity.GetBranchId() && x.IsActive);
            generalinvoice.PartyId = model.PartyId;
            generalinvoice.Amount = model.Amount;
            generalinvoice.Date = model.Date;
            generalinvoice.CreatedOn = DateTime.UtcNow;
            generalinvoice.CreatedBy = User.Identity.GetCustomId();
            generalinvoice.IsActive = true;
            generalinvoice.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            generalinvoice.BranchId = User.Identity.GetBranchId().GetValueOrDefault();
            generalinvoice.Type = (int)InvoiceType.Paid;
            generalinvoice.InvoiceNum = generalinvoiceHandler.GenerateInvoiceNo(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());

            generalinvoiceHandler.Add(generalinvoice);
            accountManager.PaidToParty(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId().GetValueOrDefault(), generalinvoice.Id, generalinvoice.PartyId.GetValueOrDefault(), generalinvoice.Amount, DateTime.UtcNow);
            return RedirectToAction("index", new { Id = generalinvoice.Id });
        }
        public ActionResult Print(long Id)
        {
            var invoice = generalinvoiceHandler.List.FirstOrDefault(x => x.IsActive && x.CompanyId == User.Identity.GetCompanyId().GetValueOrDefault() && x.BranchId == User.Identity.GetBranchId() && x.Id == Id);
            var temp = TemplateResolver.ResolveGeneralInvoice(invoice, User.Identity.GetCompanyName(), DateTime.UtcNow, DateTime.UtcNow, User.Identity.GetBranchName(), User.Identity.GetName().ToString());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProfitAndLossStatement(ProfitAndLossStatementSearchModel model)
        {
            model.expenseAccount = accountManager.GetExpenseAccounts(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            model.revenueAccount = accountManager.GetRevenueAccounts(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            //if (model.From != null)
            //{
            //    model.expenseAccount = model.expenseAccount.Where(x => x. >= model.From);
            //    model.revenueAccount = model.revenueAccount.Where(x => l x. >= model.From);
            //}
            //if (model.To != null)
            //{
            //    model.expenseAccount = model.expenseAccount.Where(x => x.CreatedOn <= model.To);
            //    model.revenueAccount = model.revenueAccount.Where(x => x.CreatedOn <= model.To);
            //}
            return View(model);
        }
        public ActionResult PrintProfitAndLossStatement()
        {
            ProfitAndLossStatementSearchModel model = new ProfitAndLossStatementSearchModel();
            model.expenseAccount = accountManager.GetExpenseAccounts(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            model.revenueAccount = accountManager.GetRevenueAccounts(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            var temp = TemplateResolver.ResolveProfitAndLostStatementReportTemplate(model, User.Identity.GetCompanyName(), DateTime.UtcNow, DateTime.UtcNow, "Profit and Loss Statement", User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
    }
}