using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class CategorySearchModel
    {
        public string Title { get; set; }
        public string Discription { get; set; }
        public int Type { get; set; }
        public IEnumerable<Category> Data { get; set; }
    }
}