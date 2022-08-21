using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class PurchaseReturnSearchModel
    {
        public long? PartyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public IEnumerable<PurchaseReturn> Data { get; set; }
    }
}