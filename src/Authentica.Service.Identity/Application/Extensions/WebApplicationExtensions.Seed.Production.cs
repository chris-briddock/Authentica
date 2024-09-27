using Persistence.Seed;

namespace Application.Extensions;

/// <summary>
/// Extensions for <see cref="IApplicationBuilder"/>
/// </summary>
public static partial class WebApplicationExtensions
{
    /// <summary>
    /// Seeds data asynchronously.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task UseSeedDataAsync(this WebApplication app)
    {
        await Seed.SeedRolesAsync(app);
        await Seed.SeedAdminUserAsync(app);
        await Seed.SeedClientApplicationAsync(app);
    }
}