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
        public static async Task SeedTestClientApplicationAsync(WebApplication app) 
        {
            string TestClientId = Guid.Parse("2e5cf15b-bf5b-4d80-aa01-2a596403530d").ToString();
            await SeedClientApplicationAsync(app, "Default Test Application", false, Secret, TestClientId, null!);
        }
            
        /// <summary>
        /// Seeds a test client application into the database.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task SeedOldDeletedClientApplicationAsync(WebApplication app) =>
            await SeedClientApplicationAsync(app, "Default Old Deleted Application", true, Secret, Ulid.NewUlid().ToString(),DateTime.UtcNow.AddYears(-8));
        /// <summary>
        /// Seeds a test client application into the database.
        /// </summary>
        /// <param name="app">The web application instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task SeedRecentDeletedClientApplicationAsync(WebApplication app) =>
            await SeedClientApplicationAsync(app, "Default Recent Deleted Application", true, Secret, Ulid.NewUlid().ToString(), DateTime.UtcNow);
    }
}
