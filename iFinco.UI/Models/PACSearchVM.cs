using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class PACSearchVM
    {
        public long prodId { get; set; }

        public List<VariantSearch> varints { get; set; }

    }

    public class VariantSearch
    {
        public long varientId { get; set; }
        public long valueId { get; set; }

    }
}