using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Mfr.Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Mfr.Core.Model;
    internal sealed class Configuration : DbMigrationsConfiguration<Mfr.Core.Model.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Mfr.Core.Model.ApplicationDbContext context)
        {

            //Adding superAdmin Role To Role Table, If it is not Exist
            if (!context.Roles.Any(r => r.Name == "SuperAdmin"))
            {
                var store = new RoleStore<ApplicationRole,int,ApplicationUserRole>(context);
                var manager = new RoleManager<ApplicationRole,int>(store);
                
                var role = new ApplicationRole { Name = "SuperAdmin" ,BaseCoding=true,IsActive=true,Show=true};
                
                manager.Create(role);
            }

            //Adding Member Role To Role Table, If it is not Exist
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                var store = new RoleStore<ApplicationRole, int, ApplicationUserRole>(context);
                var manager = new RoleManager<ApplicationRole, int>(store);

                var role = new ApplicationRole { Name = "Member", BaseCoding = false, IsActive = false, Show = true };
                manager.Create(role);
            }
            //Adding Main User, And Assign SuperAdmin Role To him ;-/
            if (!context.Users.Any(u => u.UserName == "MrAdmin"))
            {
                var store = new UserStore<ApplicationUser,ApplicationRole,int,ApplicationUserLogin,ApplicationUserRole,ApplicationUserClaim>(context);
                var manager = new UserManager<ApplicationUser,int>(store);


                var user = new ApplicationUser
                {
                    UserName = "MrAdmin",
                    Email = "shaheen.d2@gmail.com",
                    FirstName = "Admin",
                    LastName = "Admininstrator",
                    BirthDate = "30-05-1995",
                    PhoneNumber = "9111593572"
                };
                manager.Create(user, "!23Qwe");
                manager.AddToRole(user.Id, "SuperAdmin");
            }
            //Adding Default Comments, Beacause We Doesn't have Comment Input At the Moment
            /*if (!context.Comment.Any())
            {
                var com = new Comment
                {
                    UserApplicationId = "",
                    Text = "",
                    ProductId = "",
                    Like = "",
                    DisLike = "",
                    AdminConfirm = false,


                };*/
            }
    }
      

}