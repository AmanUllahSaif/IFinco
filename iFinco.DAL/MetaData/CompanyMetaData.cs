using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace iFinco.DAL 
{
  public  class CompanyMetaData
    {

        [Required (ErrorMessageResourceType =typeof(MetaDataResource),ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Title { get; set; }
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string NTN_STN { get; set; }
        [Required (ErrorMessageResourceType =typeof(MetaDataResource),ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(20, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        public string Contact { get; set; }
        [Required (ErrorMessageResourceType =typeof(MetaDataResource),ErrorMessageResourceName = "RequiredMessege")]
        [StringLength(50, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
        [EmailAddress]
        [Remote("IsComapnyEmailExits", "Validation",ErrorMessageResourceType =typeof(MetaDataResource), ErrorMessageResourceName = "EmailAlready")]
        public string Email { get; set; }
        [Required (ErrorMessageResourceType =typeof(MetaDataResource),ErrorMessageResourceName = "InvalidEmail")]
        [StringLength(300, ErrorMessageResourceType = typeof(MetaDataResource), ErrorMessageResourceName = "StringLength")]
         
        public string Adress { get; set; }
        [Display(Name ="Approved")]
        public bool IsApproved { get; set; }
        [Display(Name ="Email Confirmed")]
        public bool IsEmailConfirmed { get; set; }
        [Display(Name ="Phone Confrimed")]
        public bool IsContactConfirmed { get; set; }

    }
    [MetadataType(typeof(CompanyMetaData))]
    public partial class Company
    {

    }
}
