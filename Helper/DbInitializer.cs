using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TECH_Academy_of_Programming.Constants;
using TECH_Academy_of_Programming.Models;

namespace TECH_Academy_of_Programming.Helper
{
    public class DbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Roles
            if (!roleManager.Roles.Any())
            {
                foreach (var roleName in UserRoles.All)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }

            //Users with Roles
            if (!userManager.Users.Any())
            {
                var admin = new User()
                {
                    UserName = "App_Admin",
                    Email = "App_Admin@gmail.com",
                    firstName = "App",
                    lastName = "Admin",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var result = await userManager.CreateAsync(admin, "admin123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, UserRoles.Admin);


                var user1 = new User()
                {
                    UserName = "John_Doe",
                    Email = "John_Doe@gmail.com",
                    firstName = "John",
                    lastName = "Doe",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user1Result = await userManager.CreateAsync(user1, "john123");
                if (user1Result.Succeeded)
                    await userManager.AddToRoleAsync(user1, UserRoles.User);

                var user2 = new User()
                {
                    UserName = "Marie_Clarke",
                    Email = "Marie_Clarke@gmail.com",
                    firstName = "Marie",
                    lastName = "Clarke",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user2Result = await userManager.CreateAsync(user2, "marie123");
                if (user2Result.Succeeded)
                    await userManager.AddToRoleAsync(user2, UserRoles.User);

                var user3 = new User()
                {
                    UserName = "Robert_Ken",
                    Email = "Robert_Ken@gmail.com",
                    firstName = "Robert",
                    lastName = "Ken",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user3Result = await userManager.CreateAsync(user3, "robert123");
                if (user3Result.Succeeded)
                    await userManager.AddToRoleAsync(user3, UserRoles.User);

                var user4 = new User()
                {
                    UserName = "Adam_Blake",
                    Email = "Adam_Blake@gmail.com",
                    firstName = "Adam",
                    lastName = "Blake",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user4Result = await userManager.CreateAsync(user4, "adam123");
                if (user4Result.Succeeded)
                    await userManager.AddToRoleAsync(user4, UserRoles.User);

                var user5 = new User()
                {
                    UserName = "Monica_Geller",
                    Email = "Monica_Geller@gmail.com",
                    firstName = "Monica",
                    lastName = "Geller",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user5Result = await userManager.CreateAsync(user5, "monica123");
                if (user5Result.Succeeded)
                    await userManager.AddToRoleAsync(user5, UserRoles.User);

                var user6 = new User()
                {
                    UserName = "Tom_Holland",
                    Email = "Tom_Holland@gmail.com",
                    firstName = "Tom",
                    lastName = "Holland",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user6Result = await userManager.CreateAsync(user6, "tom123");
                if (user6Result.Succeeded)
                    await userManager.AddToRoleAsync(user6, UserRoles.User);

                var user7 = new User()
                {
                    UserName = "Jason_Bard",
                    Email = "Jason_Bard@gmail.com",
                    firstName = "Jason",
                    lastName = "Bard",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user7Result = await userManager.CreateAsync(user7, "jason123");
                if (user7Result.Succeeded)
                    await userManager.AddToRoleAsync(user7, UserRoles.User);

                var user8 = new User()
                {
                    UserName = "Kate_Fox",
                    Email = "Kate_Fox@gmail.com",
                    firstName = "Kate",
                    lastName = "Fox",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user8Result = await userManager.CreateAsync(user8, "kate123");
                if (user8Result.Succeeded)
                    await userManager.AddToRoleAsync(user8, UserRoles.User);

                var user9 = new User()
                {
                    UserName = "Chris_Stark",
                    Email = "Chris_Stark@gmail.com",
                    firstName = "chris",
                    lastName = "stark",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user9Result = await userManager.CreateAsync(user9, "chris123");
                if (user9Result.Succeeded)
                    await userManager.AddToRoleAsync(user9, UserRoles.User);

                var user10 = new User()
                {
                    UserName = "Raven_Fallon",
                    Email = "Raven_Fallon@gmail.com",
                    firstName = "raven",
                    lastName = "fallon",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                };

                var user10Result = await userManager.CreateAsync(user10, "raven123");
                if (user10Result.Succeeded)
                    await userManager.AddToRoleAsync(user10, UserRoles.User);
            }
        }
    }
}