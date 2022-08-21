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
    public class ExpenseController : Controller
    {
        VNSAccountsDBEntities Context;
        AccountHandler accountHandler;
        TransactionManager transactionManager;
        AccountsManager accountManager;
        public ExpenseController()
        {
            Context = new VNSAccountsDBEntities();
            accountHandler = new AccountHandler(Context);
            transactionManager = new TransactionManager(Context);
            accountManager = new AccountsManager(Context);

        }
        public ActionResult Index(ExpenseSearchModel model)
        {
            if (model.From != null || model.To != null)
            {
                var list = accountManager.GetExpenseTranscations(model.From, model.To, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                model.transaction = list;
            }
            else
            {
                var list = accountManager.GetExpenseTranscations(DateTime.UtcNow.Date, DateTime.UtcNow.Date, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
                model.transaction = list;

            }
            return View(model);
                }


        



        [HttpPost]
        public ActionResult CreatePartial(ExpenseVM model)
        {


            accountManager.CreateExpense(model.Amount, model.AccountId, model.date, User.Identity.GetCompanyId().GetValueOrDefault(), model.Discription, User.Identity.GetBranchId());

            return RedirectToAction("Index");
        }
        [HttpGet]
        public PartialViewResult CreatePartial()
        {
            ViewBag.Exp = accountManager.GetExpenseAccounts(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId()).ToList();
            return PartialView( "_CreatePartial");
        }
    }
}