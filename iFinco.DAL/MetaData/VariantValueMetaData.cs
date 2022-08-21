using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.DAL
{
    public class VariantValueMetaData
    {
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Title { get; set; }
    }

    [MetadataType(typeof(VariantValueMetaData))]
    public partial class VariantValue
    {

    }
}
