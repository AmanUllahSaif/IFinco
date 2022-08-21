using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class SaleReturnVM
    {
        public Sale Sale { get; set; }
        public List<SaleDetail> SaleDetail { get; set; }
        public SaleReturn SaleReturn { get; set; }
        public List<SaleReturnDetail> SaleReturnDetail { get; set; }
    }
}