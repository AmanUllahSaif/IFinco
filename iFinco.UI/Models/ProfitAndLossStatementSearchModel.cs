using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class ProfitAndLossStatementSearchModel
    {
        public DateTime? To { get; set; }
        public DateTime? From { get; set; }
        public IEnumerable<VNS.Accounts.DAL.Account> expenseAccount { get; set; }
        public IEnumerable<VNS.Accounts.DAL.Account> revenueAccount { get; set; }
    }
}