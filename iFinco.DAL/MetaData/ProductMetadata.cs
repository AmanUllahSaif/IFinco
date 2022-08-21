using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.DAL
{
    public class ProductMetadata
    {
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Title { get; set; }
        [StringLength(300, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Description { get; set; }
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Tags { get; set; }
        [Display(Name ="Brand")]
        public Nullable<long> BrandId { get; set; }
        [Display(Name = "Branch")]
        public Nullable<long> BranchId { get; set; }
        [Display(Name = "Category")]
        public long CategoryId { get; set; }
        [Display(Name = "Have Varients")]
        public bool HaveVarients { get; set; }
        [Display(Name = "Has Expiry")]
        public bool HasExpiry { get; set; }

       
    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {

    }
}
