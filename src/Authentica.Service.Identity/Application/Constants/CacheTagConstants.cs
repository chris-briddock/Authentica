namespace Application.Constants;

/// <summary>
/// A static class that holds cache tag constants for tag-based cache eviction.
/// </summary>
public static class CacheTagConstants
{
    /// <summary>
    /// The cache tag for user-related cache entries.
    /// </summary>
    public const string Users = "users";

    /// <summary>
    /// The cache tag for user roles.
    /// </summary>
    public const string UserRoles = "user_roles";

    /// <summary>
    /// The cache tag for multi-factor authentication settings.
    /// </summary>
    public const string MultiFactorSettings = "multi_factor_settings";

    /// <summary>
    /// The cache tag for sessions.
    /// </summary>
    public const string Sessions = "sessions";

    /// <summary>
    /// The cache tag for passkey credentials.
    /// </summary>
    public const string PasskeyCredentials = "passkey_credentials";

    /// <summary>
    /// The cache tag for activities.
    /// </summary>
    public const string Activities = "activities";

    /// <summary>
    /// The cache tag for individual user data, used for caching user-specific operations like GetUserById or GetUserByEmail.
    /// </summary>
    public const string IndividualUser = "individual_user";

    /// <summary>
    /// The cache tag for all users.
    /// </summary>
    public const string AllUsers = "all_users";

    /// <summary>
    /// The cache tag for specific user roles.
    /// </summary>
    public const string SpecificUserRoles = "specific_user_roles";

    /// <summary>
    /// The cache tag for applications.
    /// </summary>
    public const string Applications = "applications";
}
