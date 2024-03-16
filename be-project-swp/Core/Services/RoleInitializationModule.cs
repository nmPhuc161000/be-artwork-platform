using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Entities;
using be_project_swp.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace be_project_swp.Core.Services
{
    public class RoleInitializationModule : IHostInitialization
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationDbContext _context;

        public RoleInitializationModule(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            // Lấy RoleManager từ ServiceProvider
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Kiểm tra xem các vai trò đã tồn tại chưa
            bool isAdminRoleExists = await roleManager.RoleExistsAsync(StaticUserRole.ADMIN);
            bool isCreatorRoleExists = await roleManager.RoleExistsAsync(StaticUserRole.CREATOR);

            // Nếu các vai trò đã tồn tại, không cần làm gì cả
            if (isAdminRoleExists && isCreatorRoleExists)
                return;

            // Nếu không, tạo các vai trò
            await roleManager.CreateAsync(new IdentityRole(StaticUserRole.ADMIN));
            await roleManager.CreateAsync(new IdentityRole(StaticUserRole.CREATOR));

            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Tạo vai trò "Admin" nếu chưa tồn tại
            if (await roleManager.FindByNameAsync("ADMIN") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
            }

            // Tạo người dùng "Admin" nếu chưa tồn tại
            string adminEmail = "admin@admin.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin",
                    Address = "FPT Univarsity",
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    IsActive = true
                };

                await userManager.CreateAsync(adminUser, "Admin@123456789");
                await userManager.AddToRoleAsync(adminUser, "ADMIN");
            }
        }
    }
}
