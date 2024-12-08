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

            User adminUser = new()
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityDeletionStatus = new(false, DateTime.UtcNow, CreatedBy),
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

            var userEmail = DeleteUserEmail;

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                EmailConfirmed = true,
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityDeletionStatus = new(true, DateTime.UtcNow, CreatedBy),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
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

            var userEmail = AuthorizeUserEmail;

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                EntityDeletionStatus = new(false, null, null),
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                EmailConfirmed = true,
                Address = new Address(AddressValue, AddressValue, AddressValue, AddressValue, AddressValue, AddressValue, AddressValue)
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
        public static async Task SeedMultiFactorUser(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var userEmail = MultiFactorUserEmail;

            User user = new()
            {
                UserName = userEmail,
                Email = userEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                EntityDeletionStatus = new(false, null, null),
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                EmailConfirmed = true,
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

            var oldDeletedUserEmail = OldDeletedUserEmail;
            var recentDeletedUserEmail = RecentlyDeletedUserEmail;

            var oldDeletedUser = new User()
            {
                UserName = oldDeletedUserEmail,
                Email = oldDeletedUserEmail,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                EntityDeletionStatus = new(true, DateTime.UtcNow.AddYears(-8), CreatedBy),
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                EmailConfirmed = true,
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
                EntityDeletionStatus = new(true, DateTime.UtcNow, CreatedBy),
                EntityCreationStatus = new(DateTime.UtcNow, CreatedBy),
                EntityModificationStatus = new(DateTime.UtcNow, CreatedBy),
                EmailConfirmed = true,
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
