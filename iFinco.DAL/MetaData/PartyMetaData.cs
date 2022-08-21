using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.DAL
{
    public class PartyMetaData
    {
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Title { get; set; }
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(20, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Contact { get; set; }
        [Display(Name = "Company Title")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string CompanyTitle { get; set; }
        [Display(Name = "Company Email")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string CompanyEmail { get; set; }
        [Display(Name = "Company Contact")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string CompanyContact { get; set; }
        [Display(Name = "Address")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(300, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Adress { get; set; }

    }
    [MetadataType(typeof(PartyMetaData))]
    public partial class Party
    {
        private iFincoDBEntities context;

        public Party(iFincoDBEntities context)
        {
            this.context = context;
        }

        [Display(Name = "Balance")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
       
        public Decimal  Balance  { get; set; }
        [Display(Name = "Is Credit")]
        [Required(ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "RequiredMessege")]
        public bool IsCredit { get; set; }
    }

}
