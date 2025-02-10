namespace Persistence.Seed;

public static partial class Seed
{
    public static partial class Test
    {
        /// <summary>
        /// Seeds a test client application into the database.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task SeedTestClientApplicationAsync(WebApplication app) =>
            SeedClientApplicationAsync(app, "Default Test Application", false);
        /// <summary>
        /// Seeds a test client application into the database.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task SeedOldDeletedClientApplicationAsync(WebApplication app) =>
            SeedClientApplicationAsync(app, "Default Old Deleted Application", true, DateTime.UtcNow.AddYears(-8));
        /// <summary>
        /// Seeds a test client application into the database.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task SeedRecentDeletedClientApplicationAsync(WebApplication app) =>
            SeedClientApplicationAsync(app, "Default Recent Deleted Application", true, DateTime.UtcNow);
    }
}
