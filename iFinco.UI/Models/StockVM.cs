using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class StockVM
    {
        public Stock Stock { get; set; }
        public List<PACDetail> PACDetail { get; set; }
    }
}