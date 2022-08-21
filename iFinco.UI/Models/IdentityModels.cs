using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iFinco.BLL.Handler;

namespace iFinco.UI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UID { get; set; }
        [StringLength(300)]
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public long? CompanyId { get; set; }
        public long? BranchId { get; set; }
        public bool IsOwner { get; set; }
        public int Status { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            userIdentity.AddClaim(new Claim("UID", this.UID.ToString()));
            userIdentity.AddClaim(new Claim("IsOwner", IsOwner.ToString()));
            if (!string.IsNullOrEmpty(this.ImageUrl))
            {
                userIdentity.AddClaim(new Claim("image", this.ImageUrl));
            }

            userIdentity.AddClaim(new Claim("name", this.Name));
            if (CompanyId != null)
            {
                userIdentity.AddClaim(new Claim("CompanyId", this.CompanyId.GetValueOrDefault().ToString()));
                string cmpnyName = new CompanyHandler(new DAL.iFincoDBEntities()).FindById(CompanyId.GetValueOrDefault()).Title;
                userIdentity.AddClaim(new Claim("CompanyName", cmpnyName));

            }
            if (BranchId != null)
            {
                userIdentity.AddClaim(new Claim("BranchId", this.BranchId.GetValueOrDefault().ToString()));
                string branchName = new BranchHandler(new DAL.iFincoDBEntities()).FindById(BranchId.GetValueOrDefault()).Title;
                userIdentity.AddClaim(new Claim("BranchName", branchName));
            }

            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {

        }

        public ApplicationRole(string name)
            : base(name)
        {
            //this.Description = description;
        }

        public bool IsForAdmin { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
        }
    }
}