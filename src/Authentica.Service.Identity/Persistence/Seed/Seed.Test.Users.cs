using Api.Constants;

namespace Persistence.Seed;

public static partial class Seed
{
    public static partial class Test
    {

        /// <summary>
        /// Seeds the test admin user.
        /// </summary>
        public static async Task SeedTestAdminUserAsync(WebApplication app) =>
            await SeedUserAsync(app, AdminEmail, "fR<pGWqvn4Mu,6w[Z8axP;b5=", false, false, roles: [RoleDefaults.Admin, RoleDefaults.User]);

        /// <summary>
        /// Seeds a deleted user.
        /// </summary>
        public static async Task SeedDeletedUser(WebApplication app) =>
            await SeedUserAsync(app, DeleteUserEmail, "fR<pGW'qvn4Mu,6w[Z8axP;b5=", false, true, DateTime.UtcNow, RoleDefaults.User);

        /// <summary>
        /// Seeds a user for testing authorization.
        /// </summary>
        public static async Task SeedAuthorizeUser(WebApplication app) =>
            await SeedUserAsync(app, AuthorizeUserEmail, "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`", false, false, roles: RoleDefaults.User);

        /// <summary>
        /// Seeds a user with multi-factor authentication enabled.
        /// </summary>
        public static async Task SeedMultiFactorUser(WebApplication app) =>
            await SeedUserAsync(app, MultiFactorUserEmail, "Ar*P`w8R.WyXb7'UKxh;!-", true, false, roles: [RoleDefaults.User]);

        /// <summary>
        /// Seeds background service users.
        /// </summary>
        public static async Task SeedBackgroundServiceUsers(WebApplication app)
        {
            await SeedUserAsync(app, OldDeletedUserEmail, "dnjdnjdnwjdnwqjdnqwj", true, true, DateTime.UtcNow.AddYears(-8), RoleDefaults.User);
            await SeedUserAsync(app, RecentlyDeletedUserEmail, "dnjdnjdnwjdnwqjdnqwj", true, true, DateTime.UtcNow, RoleDefaults.User);
        }
    }
}