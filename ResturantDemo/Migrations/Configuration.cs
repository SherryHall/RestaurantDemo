namespace ResturantDemo.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using ResturantDemo.Models;
	using System.Collections.Generic;
	using Microsoft.AspNet.Identity.EntityFramework;
	using Microsoft.AspNet.Identity;

	internal sealed class Configuration : DbMigrationsConfiguration<ResturantDemo.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ResturantDemo.Models.ApplicationDbContext context)
        {
			// ** Add User Roles **
			// Add admin role
			var adminRole = "admin";
			var store = new RoleStore<IdentityRole>(context);
			var roleManager = new RoleManager<IdentityRole>(store);
			if (!context.Roles.Any(a => a.Name == adminRole))
			{
				var newRole = new IdentityRole { Name = adminRole };
				roleManager.Create(newRole);
			}
			// Add customer role
			var customerRole = "customer";
			if (!context.Roles.Any(a => a.Name == customerRole))
			{
				var newRole = new IdentityRole { Name = customerRole };
				roleManager.Create(newRole);
			}
			// Add User Id
			var adminEmail = "info@mydiner.com";
			var defaultPassword = "myPassword";
			if (!context.Users.Any(a => a.UserName == adminEmail))
			{
				var userStore = new UserStore<ApplicationUser>(context);
				var userManager = new UserManager<ApplicationUser>(userStore);
				var user = new ApplicationUser { UserName = adminEmail };

				userManager.Create(user, defaultPassword);
				userManager.AddToRole(user.Id, adminRole);
			}


            var winList = new Category { Name = "Wines" };
            //  This method will be called after migrating to the latest version.
            context.Categories.AddOrUpdate(cat => cat.Name,
               winList,
                new Models.Category { Name = "Desserts" },
                new Models.Category { Name = "Entrees" },
                new Models.Category { Name = "Appetizers" }
                );

            context.SaveChanges();

            var wine1 = new MenuItem { Name = "Riseling", Price = 2.45, CategoryId = winList.Id };
            var wine2 = new MenuItem { Name = "Chardenauy", Price = 10.12, CategoryId = winList.Id };
            context.MenuItems.AddOrUpdate(mi => mi.Name,
                wine1, 
                wine2);
        }
    }
}
