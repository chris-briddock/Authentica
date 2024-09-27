namespace Api.Constants;

public static partial class Routes
{
    /// <summary>
    /// Routes related to administrative operations.
    /// </summary>
    public static class Admin
    {
        /// <summary>
        /// Allows the admin to reset a password of any user.
        /// </summary>
        public const string ResetPassword = "admin/reset-password";
        /// <summary>
        /// Allows the admin to reset a password of any user.
        /// </summary>
        public const string DisableTwoFactor = "admin/2fa/disable";
        /// <summary>
        /// Allows the admin to create a new admin user.
        /// </summary>
        public const string Create = "admin/register";
        /// <summary>
        /// Reads all the users in the database.
        /// </summary>
        public const string ReadAllUsers = "admin/users";
        /// <summary>
        /// Reads all the activities in the system.
        /// </summary>
        public const string ReadAllActivities = "admin/activities";
        /// <summary>
        /// Reads all the applications.
        /// </summary>
        public const string ReadAllApplications = "admin/applications";
    }
}