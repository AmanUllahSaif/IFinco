using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNS.Accounts.DAL;

namespace iFinco.UI.Areas.CompanyManager.Models
{
    public class TransactionVM
    {
        public Transaction transaction { get; set; }
        public List<AccountDetail> Details { get; set; }
    }
}