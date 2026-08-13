using Microsoft.AspNetCore.Identity;
using TourGuide.Domain.Entities;

namespace TourGuide.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAndAdminAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        // Seed Roles
        string[] roles = ["Tourist", "Guide", "Admin"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Seed Admin User
        const string adminEmail = "admin@tourguide.com";
        const string adminPassword = "Admin@123456";

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                FullName = "System Admin",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}