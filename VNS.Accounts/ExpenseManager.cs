using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;

namespace VNS.Accounts
{
   public class ExpenseManager
    {

        TransactionHandler trnsHandler;
        AccountHandler accountHandler;
        public ExpenseManager(VNSAccountsDBEntities context)
        {
            accountHandler = new AccountHandler(context);
            trnsHandler = new TransactionHandler(context);
        }
        
    }
    }

