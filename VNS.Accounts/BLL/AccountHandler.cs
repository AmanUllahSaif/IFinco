using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.DAL;

namespace VNS.Accounts.BLL
{
    public class AccountHandler : IGenericRepository<Account>
    {
        VNSAccountsDBEntities context;
        public AccountHandler(VNSAccountsDBEntities context)
        {
            this.context = context;
        }
        public IEnumerable<Account> List
        {
            get
            {
                return context.Accounts.Where(x => x.IsActive);
            }
        }

        public void Add(Account entity)
        {
            context.Accounts.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Account entity)
        {
            entity.IsActive = false;
            Update(entity);
        }

        public Account FindById(long Id)
        {
            return context.Accounts.Find(Id);
        }

        public void Update(Account entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public bool IsAccountExits(Account entity)
        {
            var account = context.Accounts.SingleOrDefault(x => x.Name.ToLower().Equals(entity.Name.ToLower()) && x.Type == entity.Type && x.CompanyId == entity.CompanyId && x.BranchId == entity.BranchId);
            return account != null;
        }
        public IEnumerable<Account> GetAllAccounts(long companyId, long? branchId = null)
        {
            return List.Where(x => x.CompanyId == companyId && x.BranchId == branchId);
        }

        public Account FindAccount(string name, AccountType type, long companyId, long? branchId = null)
        {
            return context.Accounts.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()) && x.Type == (int)type && x.IsActive && x.CompanyId == companyId && x.BranchId == branchId);
        }
        public Account FindAccount(long companyId, long PartyId, long? branchId = null)
        {
            return context.Accounts.SingleOrDefault(x => x.PartyId == PartyId && x.IsActive && x.CompanyId == companyId && x.BranchId == branchId);
        }
    }
}
