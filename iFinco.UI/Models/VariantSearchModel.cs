using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class VariantSearchModel
    {
        public string Title { get; set; }
        public IEnumerable<Variant> Data { get; set; }
    }
}