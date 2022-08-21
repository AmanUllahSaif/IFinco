using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class BranchSearchModel
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public string Contact { get; set; }
        public IEnumerable<Branch> Data { get; set; }
    }
}