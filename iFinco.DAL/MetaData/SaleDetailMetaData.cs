using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.DAL
{
    public class SaleDetailMetaData
    {
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public long ProductId { get; set; }

    }

    [MetadataType(typeof(SaleDetailMetaData))]
    public partial class SaleDetail
    {

    }
}
