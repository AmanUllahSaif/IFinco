using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class StockSearchModel
    {
        public string Title { get; set; }
        public long? GodownId { get; set; }
        public IEnumerable<Stock> Data { get; set; }
    }
}