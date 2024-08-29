using Api.Constants;
using Domain.Aggregates.Identity;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seed;

public static partial class Seed
{
    public static partial class Test
    {
        /// <summary>
        /// Seeds an admin user into the database if it doesn't already exist.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedTestAdminUserAsync(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var adminEmail = AdminEmail;

            User adminUser = new()
            {
                UserName = adminEmail,
                Email = adminEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                CreatedBy = CreatedBy,
                Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue)
            };

            // Hash the password for security.
            adminUser.PasswordHash = userManager.PasswordHasher.HashPassword(adminUser, "fR<pGWqvn4Mu,6w[Z8axP;b5=");

            var existingUser = await userManager.FindByEmailAsync(adminUser.Email);

            if (existingUser is null)
            {
                await userManager.CreateAsync(adminUser);
                // Add roles to the admin user.
                await userManager.AddToRoleAsync(adminUser, RoleDefaults.Admin);
                await userManager.AddToRoleAsync(adminUser, RoleDefaults.User);
            }

        }
    
        /// <summary>
        /// Seeds an deleted user into the database if it doesn't already exist.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedDeletedUser(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var userEmail = "deletedUser@default.com";

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = true,
                DeletedOnUtc = DateTime.UtcNow,
                EmailConfirmed = true,
                CreatedBy = CreatedBy,
                Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue)
            };

            // Hash the password for security.
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "fR<pGW'qvn4Mu,6w[Z8axP;b5=");

            var existingUser = await userManager.FindByEmailAsync(user.Email);

            if (existingUser is null)
            {
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, RoleDefaults.User);
            }



        }
        /// <summary>
        /// Seeds a user into the database if it doesn't already exist for testing 
        /// the authorize endpoint.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAuthorizeUser(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var userEmail = "authorizeTest@default.com";

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = false,
                DeletedOnUtc = default!,
                EmailConfirmed = true,
                Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue, AddressValue, AddressValue),
                CreatedBy = CreatedBy
            };

            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`");

            var existingUser = await userManager.FindByEmailAsync(user.Email);
            if (existingUser is null)
            {
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, RoleDefaults.User);
            }
        }

        /// <summary>
        /// Seeds a user into the database if it doesn't already exist for testing 
        /// the authorize endpoint.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedTwoFactorUser(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var userEmail = "twoFactorTest@default.com";

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = false,
                DeletedOnUtc = default!,
                EmailConfirmed = true,
                CreatedBy = CreatedBy,
                Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue)
            };

            // Hash the password for security.
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "Ar*P`w8R.WyXb7'UKxh;!-");

            var existingUser = await userManager.FindByEmailAsync(user.Email);
            if (existingUser is null)
            {
                await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, RoleDefaults.User);
            }
        }
        /// <summary>
        /// Seeds two test users into the database if it doesn't already exist for testing
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedBackgroundServiceUsers(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var oldDeletedUserEmail = "olddeleted@default.com";
            var recentDeletedUserEmail = "recentlydeleted@default.com";

            var oldDeletedUser = new User()
            {
                UserName = oldDeletedUserEmail,
                Email = oldDeletedUserEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = true,
                DeletedOnUtc = DateTime.UtcNow.AddYears(-8),
                EmailConfirmed = true,
                CreatedBy = CreatedBy,
                Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue)

            };

            oldDeletedUser.PasswordHash = userManager.PasswordHasher.HashPassword(oldDeletedUser, "dnjdnjdnwjdnwqjdnqwj");

            var recentDeletedUser = new User
            {
                UserName = recentDeletedUserEmail,
                Email = recentDeletedUserEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsDeleted = true,
                EmailConfirmed = true,
                CreatedBy = CreatedBy,
                Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue)

            };
            recentDeletedUser.PasswordHash = userManager.PasswordHasher.HashPassword(recentDeletedUser, "dnjdnjdnwjdnwqjdnqwj");

            var oldDeletedUserExists = await userManager.FindByEmailAsync(oldDeletedUserEmail);
            var recentDeletedUserExists = await userManager.FindByEmailAsync(recentDeletedUserEmail);

            if (oldDeletedUserExists is null &&
                recentDeletedUserExists is null)
            {
                await userManager.CreateAsync(oldDeletedUser);
                await userManager.CreateAsync(recentDeletedUser);

                await userManager.AddToRoleAsync(oldDeletedUser, RoleDefaults.User);
                await userManager.AddToRoleAsync(recentDeletedUser, RoleDefaults.User);
            }
        }
    }
}
