using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFinco.DAL
{
    public class InvoiceTemplateMetaData
    {
        [Required]
        public int TemplateType { get; set; }
    }
    [MetadataType(typeof(InvoiceTemplateMetaData))]
    public partial class InvoiceTemplate
    {

    }
}
