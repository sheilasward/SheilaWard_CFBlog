namespace SheilaWard_CFBlog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SheilaWard_CFBlog.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SheilaWard_CFBlog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SheilaWard_CFBlog.Models.ApplicationDbContext";
        }

        protected override void Seed(SheilaWard_CFBlog.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));  // Instantiates a RoleManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            #region Roles
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }
            #endregion

            #region Users
            if (!context.Users.Any(r => r.UserName == "admin@myblog.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "admin@myblog.com",
                    Email = "admin@myblog.com",
                    FirstName = "Sheila",
                    LastName = "Ward",
                    DisplayName = "Sheila Ward"
                }, "P@ssw0rd");
            }

            if (!context.Users.Any(r => r.UserName == "moderator@myblog.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "moderator@myblog.com",
                    Email = "moderator@myblog.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "Twich"
                }, "P@ssw0rd");
            }
            #endregion

            #region AssignToRoles
            var adminId = userManager.FindByEmail("admin@myblog.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var modId = userManager.FindByEmail("moderator@myblog.com").Id;
            userManager.AddToRole(modId, "Moderator");
            #endregion
        }
    }
}
