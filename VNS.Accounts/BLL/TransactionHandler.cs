using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.DAL;

namespace VNS.Accounts.BLL
{
    public class TransactionHandler : IGenericRepository<Transaction>
    {
        VNSAccountsDBEntities context;
        public TransactionHandler(VNSAccountsDBEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Transaction> List
        {
            get
            {
                return context.Transactions.Where(x => x.IsActive).OrderByDescending(x => x.Date);
            }
        }

        public void Add(Transaction entity)
        {
            context.Transactions.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Transaction entity)
        {
            entity.IsActive = false;
            Update(entity);
        }

        public Transaction FindById(long Id)
        {
            return context.Transactions.Find(Id);
        }

        public void Update(Transaction entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
    }
}
