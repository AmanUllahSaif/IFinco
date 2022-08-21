using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class CompanySearchModel
    {
        public string Title { get; set; }
        public string Email { get; set; }
        public IEnumerable<Company> Data { get; set; }
    }
}