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
    /// Default admin email value for test data.
    /// </summary>
    public const string AdminEmail = "admin@default.com";


    /// <summary>
    /// Seeds all test user data.
    /// </summary>
    public static partial class Test
    {
    }
}