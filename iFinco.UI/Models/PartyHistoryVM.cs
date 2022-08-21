using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class PartyHistoryVM
    {
        public Party Party { get; set; }
        public List<Sale> Sale { get; set; }
        public List<SaleReturn> SaleReturn { get; set; }
        public List<Purchase> Purchase { get; set; }
        public List<PurchaseReturn> PurchaseReturn { get; set; }
    }
}