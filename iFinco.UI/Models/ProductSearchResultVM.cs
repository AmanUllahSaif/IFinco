using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class ProductSearchResultVM
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public bool HaveVarrient { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasingPrice { get; set; }
    }
}