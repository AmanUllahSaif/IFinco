using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VNS.Accounts.DAL;

namespace iFinco.UI.Models
{
    public class ExpenseVM
    {
        
        
 
        public long AccountId { get; set; }
        public string Discription { get; set; }
        public DateTime date { get; set; }
        public decimal Amount { get; set; }
    }
}