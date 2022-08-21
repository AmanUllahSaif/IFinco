using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;

namespace VNS.Accounts
{
  public  class TransactionManager
        
    {
        TransactionHandler transactionHandler;
        public TransactionManager(VNSAccountsDBEntities context)
        {
            transactionHandler = new TransactionHandler(context);
        }
        public void Add(Transaction trans)
        {
            transactionHandler.Add(trans);
        }

        public void Add(Transaction entity, List<AccountDetail> details)
        {
            entity.AccountDetails = details;
            transactionHandler.Add(entity);
        }
    }
}
