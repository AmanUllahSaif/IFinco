using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFinco.UI.Models
{
    public class ContactVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Message { get; set; }

    }
}