using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class SaleVM
    {
        public Sale Sale { get; set; }
        public List<SaleDetail> SaleDetails { get; set; }
        public ShippingDetail ShippingDetail { get; set; }
    }
}