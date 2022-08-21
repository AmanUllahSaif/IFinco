using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFinco.DAL;

namespace iFinco.UI.Models
{
    public class PurchaseVM
    {
        public Purchase Purchase { get; set; }
        public List<PurchaseDetail> PurchaseDetail { get; set; }
        public ShippingDetail ShippingDetail { get; set; }
    }
}