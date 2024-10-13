namespace Persistence.Seed;

/// <summary>
/// Provides methods for seeding initial data into the application database.
/// </summary>
public static partial class Seed
{
    /// <summary>
    /// Default value for created by.
    /// </summary>
    public const string CreatedBy = "SYSTEM";
    /// <summary>
    /// Default value for an address.
    /// </summary>
    public const string AddressValue = "DEFAULT";


    /// <summary>
    /// Seeds all test user data.
    /// </summary>
    public static partial class Test
    {
        /// <summary>
        /// Default admin email value for test data.
        /// </summary>
        public const string AdminEmail = "admin@default.com";
        /// <summary>
        /// Deleted user for test data.
        /// </summary>
        public const string DeleteUserEmail = "deletedUser@default.com";
        /// <summary>
        /// User for test data.
        /// </summary>
        public const string AuthorizeUserEmail = "authorizeTest@default.com";
        /// <summary>
        /// User for mfa test data.
        /// </summary>
        public const string MultiFactorUserEmail = "multiFactorTest@default.com";
        /// <summary>
        /// User for account purge test data.
        /// </summary>
        public const string RecentlyDeletedUserEmail = "recentlydeleted@default.com";
        /// <summary>
        /// User for account purge test data.
        /// </summary>
        public const string OldDeletedUserEmail = "olddeleted@default.com";
    }
}