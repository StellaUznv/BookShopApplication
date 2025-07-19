using BookShopApplication.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Common;

namespace BookShopApplication.Data.Seeding
{
    public static class Seeding
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Define roles
            string[] roles = { "Admin", "Manager" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }

            // Seed Admin User (optional, but useful for testing)
            var adminEmail = "admin@bookshop.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            //// Seed Admin User (optional, but useful for testing)
            //var managerEmail = "manager@bookshop.com";
            //var managerUser = await userManager.FindByEmailAsync(managerEmail);

            //if (managerUser == null)
            //{
            //    managerUser = new ApplicationUser
            //    {
            //        Id = SeedGuids.Manager,
            //        UserName = managerEmail,
            //        Email = managerEmail,
            //        EmailConfirmed = true
            //    };

            //    var result = await userManager.CreateAsync(managerUser, "Manager@123");

            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(managerUser, "Manager");
            //    }
            //}
        }

    }
}
