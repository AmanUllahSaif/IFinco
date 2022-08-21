using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class PurchaseReturnVM
    {
        public Purchase Purchase { get; set; }
        public List<PurchaseDetail> PurchaseDetail { get; set; }
        public PurchaseReturn PurchaseReturn { get; set; }
        public List<PurchaseReturnDetail> PurchaseReturnDetail { get; set; }
    }
}