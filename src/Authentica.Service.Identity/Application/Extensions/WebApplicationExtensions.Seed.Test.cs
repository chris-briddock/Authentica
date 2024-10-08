using Persistence.Seed;

namespace Application.Extensions;

/// <summary>
/// Extensions for <see cref="IApplicationBuilder"/>
/// </summary>
public static partial class WebApplicationExtensions
{
    /// <summary>
    /// Seeds all test data asynchronously
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task UseSeedTestDataAsync(this WebApplication app)
    {
        await Seed.Test.SeedTestAdminUserAsync(app);
        await Seed.Test.SeedTestRolesAsync(app);
        await Seed.Test.SeedTestClientApplicationAsync(app);
        await Seed.Test.SeedAuthorizeUser(app);
        await Seed.Test.SeedDeletedUser(app);
        await Seed.Test.SeedMultiFactorUser(app);
        await Seed.Test.SeedBackgroundServiceUsers(app);
        await Seed.Test.SeedOldDeletedClientApplicationAsync(app);
        await Seed.Test.SeedRecentDeletedClientApplicationAsync(app);
    }
}