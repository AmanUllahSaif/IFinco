using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNS.Accounts.DAL;

namespace iFinco.UI.Models
{
    public class TransactionSearchModel
    {
        public DateTime? To { get; set; }
        public DateTime? From { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }

    }
}