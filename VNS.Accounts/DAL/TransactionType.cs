using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNS.Accounts.DAL
{
    public enum TransactionType
    {
        Sales = 1,
        Purchase = 2,
        SalesReturn = 3,
        PurchaseReturn = 4,
        PaidToParty = 5,
        RecievedByParty = 6
    }
}
