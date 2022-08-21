using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public List<ProductVariant> ProductVariant { get; set; }
    }
}
