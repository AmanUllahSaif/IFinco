using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;

namespace VNS.Accounts
{
    public static class AccountsStartUpManager
    {
        public static void CreateSetupAccounts(long companyId, long? branchId = null)
        {
            AccountHandler handler = new AccountHandler(new VNSAccountsDBEntities());

            Account cash = new Account
            {
                Name = "Cash",
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                CompanyId = companyId,
                BranchId = branchId,
                IsDefualt = true,
                ShowInTransaction = true,
                Type = (int)AccountType.Assets
            };
            if (!handler.IsAccountExits(cash))
            {
                handler.Add(cash);
            }

            Account stock = new Account
            {
                Name = "Stock",
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                IsDefualt = true,
                CompanyId = companyId,
                BranchId = branchId,
                Type = (int)AccountType.Assets,
                ShowInTransaction = false
            };
            if (!handler.IsAccountExits(stock))
            {
                handler.Add(stock);
            }

            Account Sales = new Account
            {
                Name = "Sales",
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                IsDefualt = true,
                CompanyId = companyId,
                BranchId = branchId,
                Type = (int)AccountType.Revenue,
                ShowInTransaction = true
            };
            if (!handler.IsAccountExits(Sales))
            {
                handler.Add(Sales);
            }

            Account purchase = new Account
            {
                Name = "Purchases",
                CreatedOn = DateTime.UtcNow,
                CompanyId = companyId,
                BranchId = branchId,
                IsActive = true,
                IsDefualt = true,
                Type = (int)AccountType.Expense,
                ShowInTransaction = true

            };
            if (!handler.IsAccountExits(purchase))
            {
                handler.Add(purchase);
            }

            Account Capital = new Account
            {
                Name = "Capital",
                CreatedOn = DateTime.UtcNow,
                CompanyId = companyId,
                BranchId = branchId,
                IsActive = true,
                IsDefualt = true,
                Type = (int)AccountType.Capital,
                ShowInTransaction = true
            };
            if (!handler.IsAccountExits(Capital))
            {
                handler.Add(Capital);
            }



        }
    }
}
