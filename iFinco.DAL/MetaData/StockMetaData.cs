using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iFinco.DAL
{
    public class StockMetaData
    {

        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [Display(Name = "Products")]
        public long ProductId { get; set; }
        [Display(Name = "Godown")]
        public Nullable<long> GodownId { get; set; }
       
        [RegularExpression(@"^-?[0-9]\d*(\.\d+)?$", ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "DecimalMessage")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        //[Range(0, 9999999999999999.99)]
        public decimal Quantity { get; set; }
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "DecimalMessage")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [Range(1, 9999999999999999.99)]
        [Display(Name = "Purchasing Price")]
        public decimal PurchasingPrice { get; set; }

        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "DecimalMessage")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [Range(0, 9999999999999999.99)]
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }

        public string Code { get; set; }
        [Display(Name = "Bar-Code")]
        [AreaRemoteAttribute("IsBarCodeAlreadyExits", "Validation", "",ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "BarCodeAlready")]
        public string BarCode { get; set; }
        [Display(Name = "Bar-Code Image")]
        public string BarCodeImgUrl { get; set; }
        [Display(Name = "Expiry Date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "InvalidDate")]
        public Nullable<System.DateTime> ExpiryDate { get; set; }

    }

    [MetadataType(typeof(StockMetaData))]
    public partial class Stock
    {

    }
}
