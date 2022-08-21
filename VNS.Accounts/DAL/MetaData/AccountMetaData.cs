using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNS.Accounts.DAL
{
    public class AccountMetaData
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 9999999999999999.99)]
        public decimal Balance { get; set; }
    }
    [MetadataType(typeof(AccountMetaData))]
    public partial class Account
    {
        public decimal Balance { get; set; }
    }
}
