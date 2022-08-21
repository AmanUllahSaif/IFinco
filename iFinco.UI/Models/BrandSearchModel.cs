using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class BrandSearchModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Brand> Data { get; set; }
    }
}