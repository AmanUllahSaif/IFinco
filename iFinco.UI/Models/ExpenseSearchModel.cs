using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNS.Accounts.DAL;

namespace iFinco.UI.Models
{
    public class ExpenseSearchModel
    {
        public DateTime? To { get; set; }
        public DateTime? From { get; set; }
        public decimal Amount { get; set; }
        
        public IEnumerable<Transaction> transaction { get; set; }
    }
}