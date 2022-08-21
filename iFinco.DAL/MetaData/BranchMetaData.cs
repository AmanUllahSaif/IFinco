using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.DAL
{
    public class BranchMetaData
    {
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Title { get; set; }
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(300, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Adress { get; set; }
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        public int Status { get; set; }
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Contact { get; set; }


    }

    [MetadataType(typeof(BranchMetaData))]
    public partial class Branch
    {

    }
}
