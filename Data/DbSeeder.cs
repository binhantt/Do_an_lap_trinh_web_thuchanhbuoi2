using Microsoft.AspNetCore.Identity;

namespace baitap.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create admin user
            var adminUser = await userManager.FindByEmailAsync("admin@hutech.edu.vn");
            if (adminUser == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "admin@hutech.edu.vn",
                    Email = "admin@hutech.edu.vn",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}