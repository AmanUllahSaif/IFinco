using iFinco.UI.Areas.CompanyManager.Models;
using iFinco.UI.Models;
using iFinco.UI.Util;
using System;
using System.Collections;
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
    public class TransactionController : Controller
    {
        VNSAccountsDBEntities Context;
        AccountHandler accountHandler;
        TransactionManager transactionManager;
        AccountsManager accountManager;
        public TransactionController()
        {
            Context = new VNSAccountsDBEntities();
            accountHandler = new AccountHandler(Context);
            transactionManager = new TransactionManager(Context);
            accountManager = new AccountsManager(Context);
        }
        // GET: CompanyManager/Transaction
        public ActionResult Index(TransactionSearchModel model)
        {
            if (model.From != null || model.To != null)
            {
                var list = accountManager.GetTransaction(model.From, model.To, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                model.Transactions = list;
                return View(model);
            }
            else
            {
                var list = accountManager.GetTodayTransactions(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId()).ToList();
                model.Transactions = list;
                return View(model);
            }
        }
        public ActionResult Create()
        {
            ViewBag.Name = accountHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(TransactionVM model)
        {
            ViewBag.Name = accountHandler.List.Where(x => x.CompanyId == User.Identity.GetCompanyId() && x.IsActive).ToList();
            model.transaction.CreatedOn = DateTime.UtcNow;
            model.transaction.IsActive = true;
            model.transaction.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
            model.transaction.BranchId = User.Identity.GetBranchId();

            foreach (var item in model.Details)
            {
                item.IsActive = true;
                item.CreatedOn = DateTime.UtcNow;
                item.BranchId = User.Identity.GetBranchId();
                item.CompanyId = User.Identity.GetCompanyId().GetValueOrDefault();
                model.transaction.AccountDetails.Add(item);
            }
            transactionManager.Add(model.transaction);
            return RedirectToAction("Index");
        }
        public ActionResult Print()
        {
            var transaction = accountManager.GetTodayTransactions(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            string reportType = "General Ledgre";
            var temp = TemplateResolver.ResolveTransactionReportTemplate(transaction.ToList(), User.Identity.GetCompanyName(), DateTime.UtcNow, DateTime.UtcNow, reportType, User.Identity.GetBranchName());
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

    }
}