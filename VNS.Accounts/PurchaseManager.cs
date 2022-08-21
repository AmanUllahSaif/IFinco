using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.DAL;
using VNS.Accounts.BLL;

namespace VNS.Accounts
{
    public class PurchaseManager
    {
        TransactionHandler trnsHandler;
        AccountHandler accountHandler;
        public PurchaseManager(VNSAccountsDBEntities context)
        {
            trnsHandler = new TransactionHandler(context);
            accountHandler = new AccountHandler(context);
        }

        public void CreatePurchase(long invoiceId, decimal amount, decimal cost, decimal paid, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            if (amount == paid)
            {
                CashPurchase(invoiceId, amount, transcationDate, companyId, branchId, PartyId);
            }
            if (paid == 0)
            {
                CrPurchase(invoiceId, amount, transcationDate, PartyId, companyId, branchId);
            }
            if (paid != 0 && amount != paid)
            {
                {
                    PartialPurchase(invoiceId, amount, cost, paid, transcationDate, PartyId, companyId, branchId);
                }
            }
        }
        public void CashPurchase(long invoiceId, decimal amount, DateTime transcationDate, long companyId, long? branchId = null, long? PartyId = null)
        {
            Account cash = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
            Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account purchase = accountHandler.FindAccount("Purchases", AccountType.Expense, companyId, branchId);

            AccountDetail cashDetail = new AccountDetail();
            cashDetail.Cr = amount;
            cashDetail.CreatedOn = DateTime.UtcNow;
            cashDetail.CompanyId = companyId;
            cashDetail.BranchId = branchId;
            cashDetail.AccountId = cash.Id;
            cashDetail.IsActive = true;

            AccountDetail stockDetail = new AccountDetail();
            stockDetail.Dr = amount;
            stockDetail.CreatedOn = DateTime.UtcNow;
            stockDetail.AccountId = stock.Id;
            stockDetail.IsActive = true;


            AccountDetail purchaseDetail = new AccountDetail();
            purchaseDetail.Dr = amount;
            purchaseDetail.BranchId = branchId;
            purchaseDetail.CompanyId = companyId;
            purchaseDetail.CreatedOn = DateTime.UtcNow;
            purchaseDetail.AccountId = purchase.Id;
            purchaseDetail.IsActive = true;

            Transaction transaction = new Transaction();
            transaction.AccountDetails.Add(cashDetail);
            transaction.AccountDetails.Add(stockDetail);
            transaction.AccountDetails.Add(purchaseDetail);

            transaction.ReceiptId = invoiceId;
            transaction.Narration = "Items Cash Purchase";
            transaction.CompanyId = companyId;
            transaction.BranchId = branchId;
            transaction.CreatedOn = DateTime.UtcNow;
            transaction.Date = transcationDate;
            transaction.IsActive = true;
            transaction.Type = (int)TransactionType.Purchase;
            transaction.IsActive = true;

            trnsHandler.Add(transaction);


        }
        /// <summary>
        /// Purchase on Credit
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="amount"></param>
        /// <param name="transcationDate"></param>
        /// <param name="PartyId"></param>
        /// <param name="companyId"></param>
        /// <param name="branchId"></param>
        public void CrPurchase(long invoiceId, decimal amount, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account purchase = accountHandler.FindAccount("Purchases", AccountType.Expense, companyId, branchId);
            Account party = accountHandler.FindAccount(companyId, PartyId, branchId);

            AccountDetail stockDetail = new AccountDetail();
            stockDetail.Dr = amount;
            stockDetail.CreatedOn = DateTime.UtcNow;
            stockDetail.AccountId = stock.Id;
            stockDetail.IsActive = true;

            AccountDetail partyDetail = new AccountDetail();
            partyDetail.Cr = amount;
            partyDetail.CreatedOn = DateTime.UtcNow;
            partyDetail.AccountId = party.Id;
            partyDetail.IsActive = true;

            AccountDetail purchaseDetail = new AccountDetail();
            purchaseDetail.Dr = amount;
            purchaseDetail.BranchId = branchId;
            purchaseDetail.CompanyId = companyId;
            purchaseDetail.CreatedOn = DateTime.UtcNow;
            purchaseDetail.AccountId = purchase.Id;
            purchaseDetail.IsActive = true;

            Transaction transaction = new Transaction();
            transaction.Narration = "Credit Purchase";
            transaction.CreatedOn = DateTime.UtcNow;
            transaction.BranchId = branchId;
            transaction.CompanyId = companyId;
            transaction.IsActive = true;
            transaction.ReceiptId = invoiceId;
            transaction.Date = transcationDate;
            transaction.Type = (int)TransactionType.Purchase;

            transaction.AccountDetails.Add(stockDetail);
            transaction.AccountDetails.Add(partyDetail);
            transaction.AccountDetails.Add(purchaseDetail);

            trnsHandler.Add(transaction);
        }
        /// <summary>
        ///  When Party Recived partaily use this function.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="amount"></param>
        /// <param name="cost"></param>
        /// <param name="paid"></param>
        /// <param name="transcationDate"></param>
        /// <param name="PartyId"></param>
        /// <param name="companyId"></param>
        /// <param name="branchId"></param>
        public void PartialPurchase(long invoiceId, decimal amount, decimal cost, decimal paid, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            Account Cash = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
            Account Stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account Purchase = accountHandler.FindAccount("Purchases", AccountType.Expense, companyId, branchId);
            Account Party = accountHandler.FindAccount(companyId, PartyId, branchId);

            AccountDetail cashDetail = new AccountDetail();
            //cashDetail.Dr = amount;
            cashDetail.CreatedOn = DateTime.UtcNow;
            cashDetail.CompanyId = companyId;
            cashDetail.BranchId = branchId;
            cashDetail.AccountId = Cash.Id;
            cashDetail.IsActive = true;

            AccountDetail stockDetail = new AccountDetail();
            stockDetail.Dr = amount;
            stockDetail.CreatedOn = DateTime.UtcNow;
            stockDetail.AccountId = Stock.Id;
            stockDetail.IsActive = true;

            AccountDetail purchaseDetail = new AccountDetail();
            purchaseDetail.Dr = amount;
            purchaseDetail.BranchId = branchId;
            purchaseDetail.CompanyId = companyId;
            purchaseDetail.CreatedOn = DateTime.UtcNow;
            purchaseDetail.AccountId = Purchase.Id;
            purchaseDetail.IsActive = true;

            Transaction transaction = new Transaction();
           
            transaction.CreatedOn = DateTime.UtcNow;
            transaction.BranchId = branchId;
            transaction.CompanyId = companyId;
            transaction.IsActive = true;
            transaction.ReceiptId = invoiceId;
            transaction.Date = transcationDate;
            transaction.Type = (int)TransactionType.Purchase;

            AccountDetail partyDetail = new AccountDetail();
            //partyDetail.Cr = amount - paid;
            partyDetail.CreatedOn = DateTime.UtcNow;
            partyDetail.AccountId = Party.Id;
            partyDetail.IsActive = true;

            if (paid > amount)
            {
                cashDetail.Cr = paid;
                partyDetail.Dr = paid - amount;
                transaction.Narration = "Credit purchase";
            }
            else
            {
                cashDetail.Cr = paid;
                partyDetail.Cr = amount - paid;
                transaction.Narration = "Debit purchase";
            }
            transaction.AccountDetails.Add(stockDetail);
            transaction.AccountDetails.Add(partyDetail);
            transaction.AccountDetails.Add(purchaseDetail);
            transaction.AccountDetails.Add(cashDetail);

            trnsHandler.Add(transaction);
        }
      
        public void CreatePurchaseReturn(long invoiceId, decimal amount, decimal cost, decimal paid, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            if (amount == paid)
            {
                ReturnPurchaseCash(invoiceId, amount, transcationDate, companyId, branchId, PartyId);
            }
            if (paid == 0)
            {
                CrPurchaseReturn(invoiceId, amount, transcationDate, PartyId, companyId, branchId);
            }
            if (paid != 0 && amount != paid)
            {
                {
                    PartialPurchaseReturn(invoiceId, amount, cost, paid, transcationDate, PartyId, companyId, branchId);
                }
            }
        }
        public void ReturnPurchaseCash(long invoiceId, decimal amount, DateTime transcationDate, long companyId, long? branchId = null, long? PartyId = null)
        {
            Account cash = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
            Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account purchase = accountHandler.FindAccount("Purchases", AccountType.Expense, companyId, branchId);
            //Account client = accountHandler.FindAccount(clientId);

            AccountDetail cashDetail = new AccountDetail();
            cashDetail.Dr = amount;
            cashDetail.CreatedOn = DateTime.UtcNow;
            cashDetail.CompanyId = companyId;
            cashDetail.BranchId = branchId;
            cashDetail.AccountId = cash.Id;
            cashDetail.IsActive = true;

            AccountDetail stockDetail = new AccountDetail();
            stockDetail.Cr = amount;
            stockDetail.CreatedOn = DateTime.UtcNow;
            stockDetail.AccountId = stock.Id;
            stockDetail.IsActive = true;

            AccountDetail purchaseDetail = new AccountDetail();
            purchaseDetail.Cr = amount;
            purchaseDetail.CompanyId = companyId;
            purchaseDetail.BranchId = branchId;
            purchaseDetail.CreatedOn = DateTime.UtcNow;
            purchaseDetail.AccountId = purchase.Id;
            purchaseDetail.IsActive = true;

            //AccountDetail clientDetail = new AccountDetail();
            //clientDetail.Cr = amount;
            //clientDetail.CreatedOn = DateTime.UtcNow;
            //clientDetail.AccountId = client.Id;
            //clientDetail.IsActive = true;

            Transaction trnscation = new Transaction();
            trnscation.AccountDetails.Add(cashDetail);
            trnscation.AccountDetails.Add(stockDetail);
            trnscation.AccountDetails.Add(purchaseDetail);
            trnscation.BranchId = branchId;
            trnscation.CompanyId = companyId;
            trnscation.ReceiptId = invoiceId;
            trnscation.Narration = "Purchase returned.";
            trnscation.CreatedOn = DateTime.UtcNow;
            trnscation.Date = transcationDate;
            trnscation.IsActive = true;
            trnscation.Type = (int)TransactionType.PurchaseReturn;


            trnsHandler.Add(trnscation);
        }
        public void CrPurchaseReturn(long invoiceId, decimal amount, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account purchase = accountHandler.FindAccount("Purchases", AccountType.Expense, companyId, branchId);
            Account party = accountHandler.FindAccount(companyId, PartyId, branchId);
            Account cash = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);

            AccountDetail stockDetail = new AccountDetail();
            stockDetail.Cr = amount;
            stockDetail.CreatedOn = DateTime.UtcNow;
            stockDetail.AccountId = stock.Id;
            stockDetail.IsActive = true;


            AccountDetail partyDetail = new AccountDetail();
            partyDetail.Dr = amount;
            partyDetail.CreatedOn = DateTime.UtcNow;
            partyDetail.AccountId = party.Id;
            partyDetail.IsActive = true;

            AccountDetail purchaseDetail = new AccountDetail();
            purchaseDetail.Cr = amount;
            purchaseDetail.BranchId = branchId;
            purchaseDetail.CompanyId = companyId;
            purchaseDetail.CreatedOn = DateTime.UtcNow;
            purchaseDetail.AccountId = purchase.Id;
            purchaseDetail.IsActive = true;

            Transaction transaction = new Transaction();
            transaction.Narration = "Credit Purchase Return";
            transaction.CreatedOn = DateTime.UtcNow;
            transaction.BranchId = branchId;
            transaction.CompanyId = companyId;
            transaction.IsActive = true;
            transaction.ReceiptId = invoiceId;
            transaction.Date = transcationDate;
            transaction.Type = (int)TransactionType.PurchaseReturn;

            transaction.AccountDetails.Add(stockDetail);
            transaction.AccountDetails.Add(partyDetail);
            transaction.AccountDetails.Add(purchaseDetail);

            trnsHandler.Add(transaction);
        }
        public void PartialPurchaseReturn(long invoiceId, decimal amount, decimal cost, decimal paid, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            Account Cash = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
            Account Stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account Purchase = accountHandler.FindAccount("Purchases", AccountType.Expense, companyId, branchId);
            Account Party = accountHandler.FindAccount(companyId, PartyId, branchId);

            AccountDetail cashDetail = new AccountDetail();
            //cashDetail.Dr = amount;
            cashDetail.CreatedOn = DateTime.UtcNow;
            cashDetail.CompanyId = companyId;
            cashDetail.BranchId = branchId;
            cashDetail.AccountId = Cash.Id;
            cashDetail.IsActive = true;

            AccountDetail stockDetail = new AccountDetail();
            stockDetail.Cr = amount;
            stockDetail.CreatedOn = DateTime.UtcNow;
            stockDetail.AccountId = Stock.Id;
            stockDetail.IsActive = true;

            AccountDetail purchaseDetail = new AccountDetail();
            purchaseDetail.Cr = amount;
            purchaseDetail.BranchId = branchId;
            purchaseDetail.CompanyId = companyId;
            purchaseDetail.CreatedOn = DateTime.UtcNow;
            purchaseDetail.AccountId = Purchase.Id;
            purchaseDetail.IsActive = true;

            Transaction transaction = new Transaction();

            transaction.CreatedOn = DateTime.UtcNow;
            transaction.BranchId = branchId;
            transaction.CompanyId = companyId;
            transaction.IsActive = true;
            transaction.ReceiptId = invoiceId;
            transaction.Date = transcationDate;
            transaction.Type = (int)TransactionType.PurchaseReturn;

            AccountDetail partyDetail = new AccountDetail();
            //partyDetail.Cr = amount - paid;
            partyDetail.CreatedOn = DateTime.UtcNow;
            partyDetail.AccountId = Party.Id;
            partyDetail.IsActive = true;

            if (paid > amount)
            {
                cashDetail.Dr = paid;
                partyDetail.Cr = paid - amount;
                transaction.Narration = "partial Credit purchase Return";
            }
            else
            {
                cashDetail.Dr = paid;
                partyDetail.Dr = amount - paid;
                transaction.Narration = "partial Debit purchase Return";
            }
            transaction.AccountDetails.Add(stockDetail);
            transaction.AccountDetails.Add(partyDetail);
            transaction.AccountDetails.Add(purchaseDetail);
            transaction.AccountDetails.Add(cashDetail);

            trnsHandler.Add(transaction);
        }

    }
}
