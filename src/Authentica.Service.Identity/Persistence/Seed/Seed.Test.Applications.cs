namespace Persistence.Seed;

public static partial class Seed
{
    public static partial class Test
    {
        /// <summary>
        /// Represents the client id for testing.
        /// </summary>
        public const string TestClientId = "2e5cf15b-bf5b-4d80-aa01-2a596403530d";
        /// <summary>
        /// Seeds a test client application into the database.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task SeedTestClientApplicationAsync(WebApplication app) =>
            SeedClientApplicationAsync(app, "Default Test Application", false, Secret, TestClientId, null!);
        /// <summary>
        /// Seeds a test client application into the database.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task SeedOldDeletedClientApplicationAsync(WebApplication app) =>
            SeedClientApplicationAsync(app, "Default Old Deleted Application", true, Secret, Guid.NewGuid().ToString(),DateTime.UtcNow.AddYears(-8));
        /// <summary>
        /// Seeds a test client application into the database.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task SeedRecentDeletedClientApplicationAsync(WebApplication app) =>
            SeedClientApplicationAsync(app, "Default Recent Deleted Application", true, Secret, Guid.NewGuid().ToString(), DateTime.UtcNow);
    }
}
