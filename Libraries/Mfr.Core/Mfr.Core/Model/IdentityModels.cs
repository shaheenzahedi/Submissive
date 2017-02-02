using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Mfr.Core.Mapping;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Mfr.Core.Model
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        //applying custom fields to the user table
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarUrl { get; set; }

        public string BirthDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<EMail> EMails { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public virtual async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            if (AvatarUrl!=null)userIdentity.AddClaim(new Claim("AvatarUrl", this.AvatarUrl));
            return userIdentity;
        }

    }

    public class ApplicationUserRole : IdentityUserRole<int>
    {
    }

    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
    }

    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
    }

    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
    {
        public bool BaseCoding { get; set; }

        public bool IsActive { get; set; }

        public bool Show { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<EMail> Email { get; set; }
        public DbSet<PhoneNumber> PhoneNumber { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductPicture> ProductPicture { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        
        public ApplicationDbContext()
            : base("name=CnString1")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new EMailMap());
            modelBuilder.Configurations.Add(new PhoneNumberMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductPictureMap());
            modelBuilder.Configurations.Add(new ProductTypeMap());
            modelBuilder.Configurations.Add(new StateMap());

            base.OnModelCreating(modelBuilder);

            //Rename Identity Tables
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserClaim");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserLogin");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}