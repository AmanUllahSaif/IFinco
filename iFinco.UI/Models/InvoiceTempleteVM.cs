using iFinco.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class InvoiceTempleteVM
    {
        [Required]
        public string content { get; set; }
        public InvoiceTemplate InvoiceTemplate { get; set; }
    }
}