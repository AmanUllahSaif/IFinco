using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class PartySearchModel
    {
        public string Title { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Adress { get; set; }

        public string CompanyTitle { get; set; }
        public IEnumerable<Party> Data { get; set; }
    }
}