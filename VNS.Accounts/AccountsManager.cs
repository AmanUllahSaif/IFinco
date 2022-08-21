using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;

namespace VNS.Accounts
{
    public class AccountsManager
    {
        VNSAccountsDBEntities context;
        AccountHandler accountHandler;
 
        public AccountsManager(VNSAccountsDBEntities context)
        {
            this.context = context;
            accountHandler = new AccountHandler(context);
        }
        /// <summary>
        /// This Methode Create a new account.
        /// </summary>
        /// <param name="AccountName">Account Name like Mr.ABC Account.</param>
        /// <param name="openingBalance">Opening Balance of account</param>
        /// <param name="isCr">Is Account Balance in Credit</param>
        /// <param name="type">Nature of account ie. Assets, Libilties</param>
        /// <param name="clientId">If this account belongs to some client</param>
        /// <param name="vndorId">If this account belongs to some Vendor</param>
        public void CreateAccount(string AccountName, decimal openingBalance, bool isCr, AccountType type,long companyId, long? branchId, long? partyId = null)
        {
            Account acnt = new Account
            {
                Name = AccountName,
                CreatedOn = DateTime.UtcNow,
                Type = (int)type,
                CompanyId = companyId,
                BranchId = branchId,
                IsActive = true,
                ShowInTransaction = true,
               PartyId = partyId
            };
            if (openingBalance > 0)
            {
                AccountDetail detail = new AccountDetail();
                detail.IsActive = true;
                if (isCr)
                {
                    detail.Cr = openingBalance;
                }
                else
                {
                    detail.Dr = openingBalance;
                }
                detail.CreatedOn = DateTime.UtcNow;
                acnt.AccountDetails.Add(detail);
            }
            context.Accounts.Add(acnt);
            context.SaveChanges();
        }

        /// <summary>
        /// Get All Accounts of any company and branch  
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>

        public void CreateExpense(decimal amount, long expnseId, DateTime date, long companyId, string discrption, long? branchId=null)
        {
            Transaction expense = new Transaction
            {
                CompanyId = companyId,
                BranchId = branchId,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                Date = date,
                Narration = discrption
            };
           var cashAccount = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
            var expAccount = context.Accounts.SingleOrDefault(x => x.Id == expnseId && x.IsActive && x.Type == (int)AccountType.Expense);
            if (cashAccount != null && expAccount != null)
            {
                AccountDetail cashDetail = new AccountDetail
                {
                    Cr = amount,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    AccountId = cashAccount.Id
                };
                AccountDetail expDetail = new AccountDetail
                {
                    Dr = amount,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    AccountId = expAccount.Id
                };
                expense.AccountDetails.Add(cashDetail);
                expense.AccountDetails.Add(expDetail);
                context.Transactions.Add(expense);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Could not found cash account or exp account please check spell");
            }
        }
        public IEnumerable<Account> GetALLAccounts(long companyId, long? branchId= null)
        {
            var Accounts = context.Accounts.Where(x => x.IsActive && x.CompanyId == companyId && x.BranchId == branchId);
            foreach (var item in Accounts)
            {
                item.Balance = item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Dr.GetValueOrDefault()) - item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Cr.GetValueOrDefault());
            }
            return Accounts;
        }
        public IEnumerable<Account> GetALLPartiesAccounts(long companyId, long? branchId = null )
        {
            var Accounts = context.Accounts.Where(x => x.IsActive && x.CompanyId == companyId && x.BranchId == branchId && x.PartyId != null);
            foreach (var item in Accounts)
            {
                item.Balance = item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Dr.GetValueOrDefault()) - item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Cr.GetValueOrDefault());
            }
            return Accounts;
        }
        public Account GetPartyAccount(long companyId,long partyId, long? branchId = null)
        {
            return GetALLPartiesAccounts(companyId, branchId).FirstOrDefault(x => x.PartyId == partyId);
        }

        public IEnumerable<Account> GetAllDr(long companyId, long? branchId = null)
        {
           return GetALLPartiesAccounts(companyId, branchId).Where(x => x.Balance > 0);
        }
        public IEnumerable<Account> GetAllCr(long companyId, long? branchId = null)
        {
            return GetALLPartiesAccounts(companyId, branchId).Where(x => x.Balance < 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public IEnumerable<Account> GetExpenseAccounts(long companyId, long? branchId = null)
        {
            var expAccounts = this.GetALLAccounts(companyId, branchId).Where(x => x.Type == (int)AccountType.Expense);
            return expAccounts;
        }
        public IEnumerable<Account> GetRevenueAccounts(long companyId, long? branchId = null)
        {
            var expAccounts = this.GetALLAccounts(companyId, branchId).Where(x => x.Type == (int)AccountType.Revenue);
            return expAccounts;
        }

        public void  SaveStock(long companyId, decimal price,long? branchId = null)
        {
             var stockAccount = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            if (stockAccount != null)
            {
                AccountDetail stockDetail = new AccountDetail
                {
                    CompanyId = companyId,
                    Dr = price,
                    BranchId = branchId,
                    IsActive = true,
                    AccountId = stockAccount.Id,
                    CreatedOn = DateTime.UtcNow
                    
                };
               context.AccountDetails.Add(stockDetail);
                 
                context.SaveChanges();

            }
        }


        public IEnumerable<Transaction> GetExpenseTranscations(DateTime? from, DateTime? to, long companyId, long? branchId=null)
        {

            var trns = context.Transactions.Where(x => x.IsActive && x.CompanyId == companyId && x.BranchId == branchId);
            List<Transaction> expTrans = new List<Transaction>();
            if (from != null)
            {
                var date = from.GetValueOrDefault().Date;
                trns = trns.Where(x => DbFunctions.TruncateTime(x.Date) >= date);
            }
            if (to != null)
            {
                var date = to.GetValueOrDefault().Date;
                trns = trns.Where(x => DbFunctions.TruncateTime(x.Date) <= date);
            }

            foreach (var item in trns)
            {
                if (item.AccountDetails.Where(x=>x.Account.Type == (int)AccountType.Expense).Count() > 0)
                {
                    expTrans.Add(item);
                }
            }
            return expTrans;
        }

        /// <summary>
        /// Get All Transcaiton of any company and branch for today
        /// </summary>
        /// <param name="companyId">Unique Company Id</param>
        /// <param name="branchId">Unique branch Id</param>
        /// <returns>IEnumerable<Transaction> All Transactions for today</returns>
        public IEnumerable<Transaction> GetTodayTransactions(long companyId, long? branchId)
        {
            return GetTransaction(DateTime.UtcNow, DateTime.UtcNow, companyId, branchId);
        }
        /// <summary>
        /// Get Transaction between period 
        /// </summary>
        /// <param name="from">Starting date</param>
        /// <param name="to">Ending date</param>
        /// <param name="CompanyId">Unique Company Id</param>
        /// <param name="BranchId">Unique Branch Id</param>
        /// <returns> IEnumerable<Transaction> All Transactions accordingly to perameters</returns>
        public IEnumerable<Transaction> GetTransaction(DateTime? from, DateTime? to, long companyId, long? branchId)
        {
            var trns = context.Transactions.Where(x => x.IsActive && x.CompanyId == companyId && x.BranchId == branchId);
            if (from != null)
            {
                var date = from.GetValueOrDefault().Date;
                trns = trns.Where(x => DbFunctions.TruncateTime(x.Date) >= date);
            }
            if (to != null)
            {
                var date = to.GetValueOrDefault().Date;
                trns = trns.Where(x => DbFunctions.TruncateTime(x.Date) <= date);
            }
            return trns;
        }
         
        public IEnumerable<Account> GetExpense(DateTime? from, DateTime? to, long companyId, long branchId)
        {
            var exp = context.Accounts.Where(x => x.IsActive && x.CompanyId == companyId && x.BranchId == branchId && x.Type == (int)AccountType.Expense);
            
            foreach (var item in exp)
            {
                if (from != null)
                {
                    var date = from.GetValueOrDefault();
                    item.AccountDetails = item.AccountDetails.Where(x => x.Transaction.Date.Day >= date.Day && x.Transaction.Date.Month >= date.Month && x.Transaction.Date.Year >= date.Year).ToList();
                }

                if (to != null)
                {
                    var date = to.GetValueOrDefault();
                    item.AccountDetails = item.AccountDetails.Where(x => x.Transaction.Date.Day <= date.Day && x.Transaction.Date.Month <= date.Month && x.Transaction.Date.Year <= date.Year).ToList();
                }

                item.Balance = item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Dr.GetValueOrDefault()) - item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Cr.GetValueOrDefault());
            }
            return exp;
        }

        public IEnumerable<Account> GetRevenue(DateTime? from, DateTime? to, long companyId ,long branchId)
        {
            var reve = context.Accounts.Where(x => x.IsActive && x.CompanyId == companyId && x.BranchId == branchId && x.Type == (int)AccountType.Revenue);

            foreach (var item in reve)
            {
                if (from != null)
                {
                    var date = from.GetValueOrDefault();
                    item.AccountDetails = item.AccountDetails.Where(x => x.Transaction.Date.Day >= date.Day && x.Transaction.Date.Month >= date.Month && x.Transaction.Date.Year >= date.Year).ToList();
                }

                if (to != null)
                {
                    var date = to.GetValueOrDefault();
                    item.AccountDetails = item.AccountDetails.Where(x => x.Transaction.Date.Day <= date.Day && x.Transaction.Date.Month <= date.Month && x.Transaction.Date.Year <= date.Year).ToList();
                }
                
                item.Balance = item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Dr.GetValueOrDefault()) - item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Cr.GetValueOrDefault());
               
            }
            return reve;
        }
        public void RecivedFromParty(long companyId, long branchId,long Id, long partyId, decimal amount,DateTime date)
        {
            Transaction expense = new Transaction
            {
                CompanyId = companyId,
                BranchId = branchId,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                Date = date,
                Type = (int)TransactionType.RecievedByParty,
                ReceiptId = Id

            };
            var cashAccount = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
            var partyaccount = GetPartyAccount(companyId, partyId, branchId);
            if (cashAccount!= null && partyaccount != null)
            {
                AccountDetail cashDetail = new AccountDetail
                {
                    Dr = amount,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    AccountId = cashAccount.Id
                };
                AccountDetail partyDetail = new AccountDetail
                {
                    Cr = amount,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    AccountId = partyaccount.Id
                };
                expense.AccountDetails.Add(cashDetail);
                expense.AccountDetails.Add(partyDetail);
                context.Transactions.Add(expense);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Could not recived from party");
            }
        }
        public void PaidToParty(long companyId, long branchId, long Id, long partyId, decimal amount, DateTime date)
        {
            Transaction expense = new Transaction
            {
                CompanyId = companyId,
                BranchId = branchId,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                Date = date,
                Type = (int)TransactionType.PaidToParty,
                ReceiptId = Id
            };

            var cashAccount = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
            var partyaccount = GetPartyAccount(companyId, partyId, branchId);
            if (cashAccount != null && partyaccount != null)
            {
                AccountDetail cashDetail = new AccountDetail
                {
                    Cr = amount,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    AccountId = cashAccount.Id
                };
                AccountDetail partyDetail = new AccountDetail
                {
                    Dr = amount,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    AccountId = partyaccount.Id
                };
                expense.AccountDetails.Add(cashDetail);
                expense.AccountDetails.Add(partyDetail);
                context.Transactions.Add(expense);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Could not paid to party");
            }
        }
    }
}
