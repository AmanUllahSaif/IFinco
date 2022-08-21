using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;


namespace VNS.Accounts
{
    public class SalesManager
    {
        AccountHandler accountHandler;
        TransactionHandler transactionHandler;
        public SalesManager(VNSAccountsDBEntities context)
        {
            accountHandler = new AccountHandler(context);
            transactionHandler = new TransactionHandler(context);
        }

        public void CreateSale(long invoiceId, decimal amount, decimal cost, decimal paid, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            if (amount == paid)
            {
                CashSale(invoiceId, amount, transcationDate, companyId, branchId);
            }
            if (paid == 0)
            {
                CrSales(invoiceId, amount, transcationDate, PartyId, companyId, branchId);
            }
            if (paid != 0 && amount != paid)
            {
                PartialSales(invoiceId, amount, cost, paid, transcationDate, PartyId, companyId, branchId);
            }
        }
        /// <summary>
        /// Sales on csah
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="amount"></param>
        /// <param name="transcationDate"></param>
        /// <param name="companyId"></param>
        /// <param name="branchId"></param>
        /// <param name="PartyId"></param>
        public void CashSale(long invoiceId, decimal amount, DateTime transcationDate, long companyId, long? branchId = null, long? PartyId = null)
        {
            Account cash = accountHandler.FindAccount("cash", AccountType.Assets, companyId, branchId);
            Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account Sales = accountHandler.FindAccount("Sales", AccountType.Revenue, companyId, branchId);

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


            AccountDetail salesDetail = new AccountDetail();
            salesDetail.Cr = amount;
            salesDetail.BranchId = branchId;
            salesDetail.CompanyId = companyId;
            salesDetail.CreatedOn = DateTime.UtcNow;
            salesDetail.AccountId = Sales.Id;
            salesDetail.IsActive = true;

            Transaction transaction = new Transaction();
            transaction.AccountDetails.Add(cashDetail);
            transaction.AccountDetails.Add(stockDetail);
            transaction.AccountDetails.Add(salesDetail);

            transaction.ReceiptId = invoiceId;
            transaction.Narration = "Items Cash Sales";
            transaction.CompanyId = companyId;
            transaction.BranchId = branchId;
            transaction.CreatedOn = DateTime.UtcNow;
            transaction.Date = transcationDate;
            transaction.IsActive = true;
            transaction.Type = (int)TransactionType.Sales;
            transaction.IsActive = true;

            transactionHandler.Add(transaction);


        }
        /// <summary>
        /// Sales on Credit
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="amount"></param>
        /// <param name="transcationDate"></param>
        /// <param name="PartyId"></param>
        /// <param name="companyId"></param>
        /// <param name="branchId"></param>
        public void CrSales(long invoiceId, decimal amount, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account sales = accountHandler.FindAccount("Sales", AccountType.Revenue, companyId, branchId);
            Account party = accountHandler.FindAccount(companyId, PartyId, branchId);

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

            AccountDetail salesDetail = new AccountDetail();
            salesDetail.Cr = amount;
            salesDetail.BranchId = branchId;
            salesDetail.CompanyId = companyId;
            salesDetail.CreatedOn = DateTime.UtcNow;
            salesDetail.AccountId = sales.Id;
            salesDetail.IsActive = true;

            Transaction transaction = new Transaction();
            transaction.Narration = "Credit Sales";
            transaction.CreatedOn = DateTime.UtcNow;
            transaction.BranchId = branchId;
            transaction.Date = transcationDate;

            transaction.CompanyId = companyId;
            transaction.IsActive = true;
            transaction.ReceiptId = invoiceId;
            transaction.Type = (int)TransactionType.Sales;

            transaction.AccountDetails.Add(stockDetail);
            transaction.AccountDetails.Add(partyDetail);
            transaction.AccountDetails.Add(salesDetail);

            transactionHandler.Add(transaction);
        }
        /// <summary>
        ///  When Party paid partaily use this function.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="amount"></param>
        /// <param name="cost"></param>
        /// <param name="paid"></param>
        /// <param name="transcationDate"></param>
        /// <param name="PartyId"></param>
        /// <param name="companyId"></param>
        /// <param name="branchId"></param>
        public void PartialSales(long invoiceId, decimal amount, decimal cost, decimal paid, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            if (PartyId != 0)
            {
                Account Cash = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
                Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
                Account sales = accountHandler.FindAccount("Sales", AccountType.Revenue, companyId, branchId);
                Account party = accountHandler.FindAccount(companyId, PartyId, branchId);

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
                stockDetail.AccountId = stock.Id;
                stockDetail.IsActive = true;

                AccountDetail salesDetail = new AccountDetail();
                salesDetail.Cr = amount;
                salesDetail.BranchId = branchId;
                salesDetail.CompanyId = companyId;
                salesDetail.CreatedOn = DateTime.UtcNow;
                salesDetail.AccountId = sales.Id;
                salesDetail.IsActive = true;

                Transaction transaction = new Transaction();
                transaction.Narration = "Credit Sales";
                transaction.CreatedOn = DateTime.UtcNow;
                transaction.BranchId = branchId;
                transaction.CompanyId = companyId;
                transaction.IsActive = true;
                transaction.ReceiptId = invoiceId;
                transaction.Type = (int)TransactionType.Sales;
                transaction.Date = transcationDate;
                AccountDetail partyDetail = new AccountDetail();
                //partyDetail.Cr = amount - paid;
                partyDetail.CreatedOn = DateTime.UtcNow;
                partyDetail.AccountId = party.Id;
                partyDetail.IsActive = true;

                if (amount > paid)
                {
                    cashDetail.Dr = paid;
                    partyDetail.Dr = amount - paid;
                }
                else
                {
                    cashDetail.Dr = paid;
                    partyDetail.Cr = paid - amount;
                }
                transaction.AccountDetails.Add(stockDetail);
                transaction.AccountDetails.Add(partyDetail);
                transaction.AccountDetails.Add(salesDetail);
                transaction.AccountDetails.Add(cashDetail);

                transactionHandler.Add(transaction);
            }
            else
            {
                throw new Exception("No Party Exist");
            }
        }
        public void CreateSaleReturn(long invoiceId, decimal amount, decimal cost, decimal paid, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            if (amount == paid)
            {
                ReturnSalesCash(invoiceId, amount, transcationDate, companyId, branchId, PartyId);
            }
            if (paid == 0)
            {
                CrSalesReturn(invoiceId, amount, transcationDate, PartyId, companyId, branchId);
            }
            if (paid != 0 && amount != paid)
            {
                PartialSalesReturn(invoiceId, amount, cost, paid, transcationDate, PartyId, companyId, branchId);
            }
        }
        public void ReturnSalesCash(long invoiceId, decimal amount, DateTime transcationDate, long companyId, long? branchId = null, long? PartyId = null)
        {
            Account cash = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
            Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account sales = accountHandler.FindAccount("Sales", AccountType.Revenue, companyId, branchId);
            //Account client = accountHandler.FindAccount(clientId);

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

            AccountDetail salesDetail = new AccountDetail();
            salesDetail.Dr = amount;
            salesDetail.CompanyId = companyId;
            salesDetail.BranchId = branchId;
            salesDetail.CreatedOn = DateTime.UtcNow;
            salesDetail.AccountId = sales.Id;
            salesDetail.IsActive = true;

            //AccountDetail clientDetail = new AccountDetail();
            //clientDetail.Cr = amount;
            //clientDetail.CreatedOn = DateTime.UtcNow;
            //clientDetail.AccountId = client.Id;
            //clientDetail.IsActive = true;

            Transaction trnscation = new Transaction();
            trnscation.AccountDetails.Add(cashDetail);
            //trnscation.AccountDetails.Add(stockDetail);
            trnscation.AccountDetails.Add(salesDetail);
            trnscation.BranchId = branchId;
            trnscation.CompanyId = companyId;
            trnscation.ReceiptId = invoiceId;
            trnscation.Narration = "Sales returned";
            trnscation.CreatedOn = DateTime.UtcNow;
            trnscation.Date = transcationDate;
            trnscation.IsActive = true;
            trnscation.Type = (int)TransactionType.SalesReturn;
            trnscation.IsActive = true;

            transactionHandler.Add(trnscation);
        }
        public void CrSalesReturn(long invoiceId, decimal amount, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
            Account sales = accountHandler.FindAccount("Sales", AccountType.Revenue, companyId, branchId);
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

            AccountDetail salesDetail = new AccountDetail();
            salesDetail.Dr = amount;
            salesDetail.BranchId = branchId;
            salesDetail.CompanyId = companyId;
            salesDetail.CreatedOn = DateTime.UtcNow;
            salesDetail.AccountId = sales.Id;
            salesDetail.IsActive = true;

            Transaction transaction = new Transaction();
            transaction.Narration = "Credit Sales Return";
            transaction.CreatedOn = DateTime.UtcNow;
            transaction.BranchId = branchId;
            transaction.Date = transcationDate;

            transaction.CompanyId = companyId;
            transaction.IsActive = true;
            transaction.ReceiptId = invoiceId;
            transaction.Type = (int)TransactionType.SalesReturn;

            transaction.AccountDetails.Add(stockDetail);
            transaction.AccountDetails.Add(partyDetail);
            transaction.AccountDetails.Add(salesDetail);

            transactionHandler.Add(transaction);
        }
        public void PartialSalesReturn(long invoiceId, decimal amount, decimal cost, decimal paid, DateTime transcationDate, long PartyId, long companyId, long? branchId = null)
        {
            if (PartyId != 0)
            {
                Account Cash = accountHandler.FindAccount("Cash", AccountType.Assets, companyId, branchId);
                Account stock = accountHandler.FindAccount("Stock", AccountType.Assets, companyId, branchId);
                Account sales = accountHandler.FindAccount("Sales", AccountType.Revenue, companyId, branchId);
                Account party = accountHandler.FindAccount(companyId, PartyId, branchId);

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
                stockDetail.AccountId = stock.Id;
                stockDetail.IsActive = true;

                AccountDetail salesDetail = new AccountDetail();
                salesDetail.Dr = amount;
                salesDetail.BranchId = branchId;
                salesDetail.CompanyId = companyId;
                salesDetail.CreatedOn = DateTime.UtcNow;
                salesDetail.AccountId = sales.Id;
                salesDetail.IsActive = true;

                Transaction transaction = new Transaction();
                transaction.Narration = "partial sale return";
                transaction.CreatedOn = DateTime.UtcNow;
                transaction.BranchId = branchId;
                transaction.CompanyId = companyId;
                transaction.IsActive = true;
                transaction.ReceiptId = invoiceId;
                transaction.Type = (int)TransactionType.SalesReturn;
                transaction.Date = transcationDate;
                AccountDetail partyDetail = new AccountDetail();
                //partyDetail.Cr = amount - paid;
                partyDetail.CreatedOn = DateTime.UtcNow;
                partyDetail.AccountId = party.Id;
                partyDetail.IsActive = true;

                if (amount > paid)
                {
                    cashDetail.Cr = paid;
                    partyDetail.Cr = amount - paid;
                }
                else
                {
                    cashDetail.Cr = paid;
                    partyDetail.Dr = paid - amount;
                }
                transaction.AccountDetails.Add(stockDetail);
                transaction.AccountDetails.Add(partyDetail);
                transaction.AccountDetails.Add(salesDetail);
                transaction.AccountDetails.Add(cashDetail);

                transactionHandler.Add(transaction);
            }
            else
            {
                throw new Exception("No Party Exist");
            }
        }
    }
}
