using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Areas.CompanyManager.Models
{
    public class GoDownSearchModel
    {
        public string Title { get; set; }
        public string Discription { get; set; }
        public string Contact { get; set; }
        public string Adress { get; set; }
        public IEnumerable<GoDown> Data { get; set; }
    }
}