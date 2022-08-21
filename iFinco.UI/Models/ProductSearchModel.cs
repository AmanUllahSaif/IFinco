using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class ProductSearchModel
    {
        public string Title { get; set; }
        public string Tags { get; set; }
        public IEnumerable<Product> Data { get; set; }

    }
}