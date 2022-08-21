//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iFinco.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SaleReturn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaleReturn()
        {
            this.SaleReturnDetails = new HashSet<SaleReturnDetail>();
        }
    
        public long Id { get; set; }
        public long SaleId { get; set; }
        public long CompanyId { get; set; }
        public Nullable<long> PartyId { get; set; }
        public Nullable<long> BranchId { get; set; }
        public decimal Total { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string Notes { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public int PaymentType { get; set; }
        public decimal PaidAmount { get; set; }
        public string InvoiceNo { get; set; }
    
        public virtual Party Party { get; set; }
        public virtual Sale Sale { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleReturnDetail> SaleReturnDetails { get; set; }
    }
}
