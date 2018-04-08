using Lmyc.Models;
using LmycWeb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Data
{
    public class DBInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
                                      UserManager<ApplicationUser> userManager,
                                      RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            String adminId1 = "";
            String adminId2 = "";

            string role1 = "Admin";
            string role2 = "Member";

            string password = "P@$$w0rd";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role1));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role2));
            }

            if (await userManager.FindByNameAsync("a") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "a",
                    Email = "a@a.a",
                    FirstName = "Allen",
                    LastName = "Aldridge",
                    Street = "Fake St",
                    City = "Vancouver",
                    Province = "BC",
                    PostalCode = "V5U K8I",
                    Country = "Canada",
                    PhoneNumber = "6902341234"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
                adminId1 = user.Id;
            }

            if (await userManager.FindByNameAsync("b") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "b",
                    Email = "b@b.b",
                    FirstName = "Bob",
                    LastName = "Barker",
                    Street = "Vermont St",
                    City = "Surrey",
                    Province = "BC",
                    PostalCode = "V1P I5T",
                    Country = "Canada",
                    PhoneNumber = "7788951456"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
                adminId2 = user.Id;
            }

            if (await userManager.FindByNameAsync("m") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "m",
                    Email = "m@m.m",
                    FirstName = "Mike",
                    LastName = "Myers",
                    Street = "Yew St",
                    City = "Vancouver",
                    Province = "BC",
                    PostalCode = "V3U E2Y",
                    Country = "Canada",
                    PhoneNumber = "6572136821"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            if (await userManager.FindByNameAsync("d") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "d",
                    Email = "d@d.d",
                    FirstName = "Donald",
                    LastName = "Duck",
                    Street = "Well St",
                    City = "Vancouver",
                    Province = "BC",
                    PostalCode = "V8U R9Y",
                    Country = "Canada",
                    PhoneNumber = "6041234567"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            context.Boats.AddRange(DummyData.GetBoats(adminId1, adminId2));
            context.SaveChanges();

            context.Bookings.AddRange(DummyData.GetBookings(context));
            context.SaveChanges();
        }
    }
}
