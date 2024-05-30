using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Clean.Infra.Data.Seed
{
    public class UserSeedingService
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = serviceProvider.GetRequiredService<ILogger<UserSeedingService>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        logger.LogError($"Error creating role {roleName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
            }

            var adminUser = new IdentityUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com"
            };

            string userPassword = Environment.GetEnvironmentVariable("ADMIN_USER_PASSWORD") ?? "P@ssword123";
            var user = await userManager.FindByEmailAsync("admin@admin.com");

            if (user == null)
            {
                var createUser = await userManager.CreateAsync(adminUser, userPassword);
                if (createUser.Succeeded)
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!addToRoleResult.Succeeded)
                    {
                        logger.LogError($"Error adding user to Admin role: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    logger.LogError($"Error creating admin user: {string.Join(", ", createUser.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                logger.LogInformation("Admin user already exists.");
            }
        }
    }
}
