using iFinco.DAL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class EmployeeHistoryVM
    {
        public ApplicationUser employee { get; set; }
        public IEnumerable<Sale> Sale { get; set; }
        public IEnumerable<SaleReturn> SaleReturn { get; set; }
        public IEnumerable<Purchase> Purchase { get; set; }
        public IEnumerable<PurchaseReturn> PurchaseReturn { get; set; }
    }
}