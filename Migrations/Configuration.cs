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

            #region AddBlogPosts
            DateTime T1Date = new DateTime(2019, 06, 03, 14, 04, 43, 05);
            DateTime T2Date = new DateTime(2019, 06, 05, 18, 33, 21, 43);
            DateTime T3Date = new DateTime(2019, 06, 07, 20, 09, 13, 17);
            context.Posts.AddOrUpdate(
                p => p.Title,
                    new BlogPost
                    {
                        AuthorId = adminId,
                        Created = new DateTimeOffset(T1Date.Year, T1Date.Month, T1Date.Day, T1Date.Hour, T1Date.Minute, T1Date.Second, T1Date.Millisecond, new TimeSpan(2, 0, 0)),
                        Title = "HELLO WORLD IN C#",
                        Abstract = "THE ONE THAT STARTED IT ALL",
                        Body = "<p>I guess just about every computer language that I have learned has started out with the famous <em>Hello World</em> application!  I can't remember if we did this in class, or if I looked up how to do it, but I do remember doing this small application early on in my education at Coder Foundry.</p>",
                        Published = true
                    },
                    new BlogPost
                    {
                        AuthorId = adminId,
                        Created = new DateTimeOffset(T2Date.Year, T2Date.Month, T2Date.Day, T2Date.Hour, T2Date.Minute, T2Date.Second, T2Date.Millisecond, new TimeSpan(2, 0, 0)),
                        Title = "CODER FOUNDRY FIRST CLASS",
                        Abstract = "OVERVIEW OF WHAT WE'LL LEARN",
                        Body = "<p>The first class of the first Coder Foundry Part-Time Bootcamp began on June 3, 2019.  Our instructor's name is Jason Twichell.  The tools we are going to need are:</p>" +
                        "<ol><li> Visual Studio 2019 (our main IDE)</li> <li> SSMS - Sequel Server Management Studio</li> <li> Azure Account - can get 1 year free but have to register a credit card</li> <li> Github Account</li> <li> Slack - messaging app (our own channel)</li> <li> Notepad++ for viewing HTML files</li> <li> Agent Ransack - Jason's favorite network search tool</li></ol> " +
                        "<p>Jason warned us about 'Impostor Syndrome' - where a person feels as if everyone knows what is going on except himself/herself.  That is how I felt <em>a lot</em> during the UNC-CH Bootcamp!</p>" +
                        "<p>He told us about Blazor, which is client-side C#.  However we will be using Razor, which is a server-side markup language in our Views.  For instance, if you want to have an unordered list of items, you can use a loop written in Razor, and put the 'li' tags inside of the loop!</p>" +
                        "He warned us not to publish large files on Azure (> 200K or 72dpi).  Videos should be served from YouTube or Vimeo.  Azure should be free for the first year, but after that, putting up large media files is more costly.<p>" +
                        "He also told us about the Bootstrap Grid, and that Visual Studio has Bootstrap 3 that comes with it.  He told us about our first project - a portfolio which we will need to search for a 'bootstrap template' to use.</p>",
                        Published = true
                    },
                    new BlogPost
                    {
                        AuthorId = adminId,
                        Created = new DateTimeOffset(T3Date.Year, T3Date.Month, T3Date.Day, T3Date.Hour, T3Date.Minute, T3Date.Second, T3Date.Millisecond, new TimeSpan(2, 0, 0)),
                        Title = "CODER FOUNDRY CLASS #2",
                        Abstract = "ON THE GRID...",
                        Body = "<p>In the 2nd class, we started learning the Bootstrap 3 grid, and how to use it in HTML.  Basically, at the top tier, you have a 'class=row'.  What that does is to divide the container " +
                        "(or if there is no container, the whole width of the screen) into a sum of 12 columns (such as col-md-4, col-md-4, col-md-4, or col-md-2, col-md-4, col-md-6).  Then you can make your columns accordingly.  " +
                        "Then, within each column, you can have another row with a sum of 12 columns.</p>",
                        Published = true
                    }
            );
            #endregion
        }
    }
}
