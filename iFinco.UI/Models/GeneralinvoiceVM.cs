using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class GeneralinvoiceVM
    {
        [Required]
        public long PartyId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }

    }
}