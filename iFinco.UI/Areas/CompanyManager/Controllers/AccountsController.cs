using iFinco.BLL.Handler;
using iFinco.DAL;
using iFinco.UI.Areas.CompanyManager.Models;
using iFinco.UI.Models;
using iFinco.UI.Util;
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
    public class AccountsController : Controller
    {
        iFincoDBEntities context;
        VNSAccountsDBEntities AccountContext;
        AccountHandler accountHandler;
        TransactionManager transactionManager;
        AccountsManager accountManager;
        PartyHandler partyHandler;
        public AccountsController()
        {
            context = new iFincoDBEntities();
            AccountContext = new VNSAccountsDBEntities();
            accountHandler = new AccountHandler(AccountContext);
            transactionManager = new TransactionManager(AccountContext);
            accountManager = new AccountsManager(AccountContext);
            partyHandler = new PartyHandler(context);

        }
        // GET: CompanyManager/Accounts
        public ActionResult Index()
        {
            var accounts = accountManager.GetALLAccounts(User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            return View(accounts);
        }
       
        [HttpPost]
        public ActionResult CreatePartial(VNS.Accounts.DAL.Account model, bool IsCr)
        {
            model.CreatedOn = DateTime.UtcNow;
            accountManager.CreateAccount(model.Name, model.Balance, IsCr,(AccountType)model.Type, User.Identity.GetCompanyId().GetValueOrDefault(), User.Identity.GetBranchId());
            return RedirectToAction("Index");
        }
        public PartialViewResult CreatePartial()
        {
            return PartialView("_CreatePartial");
        }
    }
}