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
    
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            this.Brands = new HashSet<Brand>();
            this.Products = new HashSet<Product>();
            this.Parties = new HashSet<Party>();
            this.Branches = new HashSet<Branch>();
            this.CompanyMenusAccesses = new HashSet<CompanyMenusAccess>();
        }
    
        public long Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> Type { get; set; }
        public string NTN_STN { get; set; }
        public Nullable<long> OwnerId { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string LogoUrl { get; set; }
        public System.DateTime CreateOn { get; set; }
        public long CreateBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsContactConfirmed { get; set; }
        public string PhoneCode { get; set; }
        public string SecurityStamp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Brand> Brands { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Party> Parties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Branch> Branches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyMenusAccess> CompanyMenusAccesses { get; set; }
    }
}
