using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ServicesLib.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ServicesLib.Domain.Utilities
{
    public static class SeedAdminUser
    {
        public static async Task InitializeAdminUser(IServiceProvider serviceProvider, string adminPw, string adminEmail)
        {
            await CreateAdminUserAndRole(serviceProvider, adminPw, adminEmail);
        }

        private static async Task<string> CreateAdminUserAndRole(IServiceProvider serviceProvider,
                                            string adminPw, string adminEmail)
        {
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();

            if (userManager == null)
            {
                string err = "SeedData, Error: Could not get UserManager service reference";
                // Log.Error(err);
                throw new Exception(err);
            }
            var user = await userManager.FindByNameAsync("Administrator");
            if (user == null)
            {
                // Log.Information("SeedData - Administrator account does not exist. Create now.");

                // Administrator does not exist
                user = new AppUser
                {
                    UserName = "Administrator",
                    FullName = "Administrator",
                    Email = adminEmail,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPw);
                if (result.Succeeded)
                {
                    // Add new account to the Administrator role
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    string err = $"Failed to create the \"Administrator\" account " +
                                          $"{result.ToString()}";
                    // Log.Error(err);
                    throw new Exception(err);
                }
                return user.Id;
            }
            else
            {
                // Log.Information("SeedData, Administrator account already exists");
                return "0";
            }
        }
    }
}