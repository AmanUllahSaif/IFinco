using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.DAL
{
    public class CategoryMetadata
    {
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Title { get; set; }
        [StringLength(300, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Description { get; set; }
    }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {

    }
}
