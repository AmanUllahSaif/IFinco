using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class PartyPaymentVM
    {
        public Decimal amount { get; set; }
        public DateTime date { get; set; }
        public long partyId { get; set; }

    }
}