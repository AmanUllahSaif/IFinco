using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class PACSearchResultVM
    {
        public long PACId { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasingPrice { get; set; }
    }
}