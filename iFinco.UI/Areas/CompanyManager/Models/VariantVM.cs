using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Areas.CompanyManager.Models
{
    public class VariantVM
    {
        public Variant Variant { get; set; }
        public List<VariantValue> Values { get; set; }
    }
}