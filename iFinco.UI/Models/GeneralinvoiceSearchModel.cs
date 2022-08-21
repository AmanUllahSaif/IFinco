using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class GeneralinvoiceSearchModel
    {
        public long invoiceNum { get; set; }
        public long Type { get; set; }
        public long partyId { get; set; }
        public decimal amount { get; set; }
        public DateTime? To { get; set; }
        public DateTime? From { get; set; }
        public List<Generalinvoice> invoice { get; set; }
    }
}