namespace Api.Constants;

/// <summary>
/// Contains all route definitions used in the application.
/// </summary>
public static class Routes
{
    /// <summary>
    /// Base route for all API endpoints.
    /// </summary>
    public static class BaseRoute
    {
        /// <summary>
        /// Base URL format for API endpoints, including versioning.
        /// </summary>
        public const string Name = "api/v{version:apiVersion}/";
    }
    /// <summary>
    /// Routes related to OAuth operations.
    /// </summary>
    public static class OAuth
    {
        /// <summary>
        /// Route for authorizing OAuth requests.
        /// </summary>
        public const string Authorize = "oauth2/authorize";

        /// <summary>
        /// Route for obtaining OAuth tokens.
        /// </summary>
        public const string Token = "oauth2/token";

    }

    /// <summary>
    /// Routes related to user operations.
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// Route for logging out using OAuth.
        /// </summary>
        public const string Logout = "users/logout";

        /// <summary>
        /// Route for logging in using OAuth.
        /// </summary>
        public const string Login = "users/login";
        /// <summary>
        /// Route for reading a user by email, only the currently logged in user.
        /// </summary>
        public const string ReadByEmail = "users";
        /// <summary>
        /// Route for creating a new user.
        /// </summary>
        public const string Create = "users/register";
        /// <summary>
        /// Route for deleting a user by email.
        /// </summary>
        public const string DeleteByEmail = "users/delete";
        /// <summary>
        /// Route for confirming user email.
        /// </summary>
        public const string ConfirmEmail = "users/confirm-email";
        /// <summary>
        /// Route for registering/creating a new user.
        /// </summary>
        public const string Register = "users/register";
        /// <summary>
        /// Route for resetting a user's password.
        /// </summary>
        public const string ResetPassword = "users/reset-password";
        /// <summary>
        /// Route for logging in with two-factor authentication.
        /// </summary>
        public const string TwoFactorLogin = "users/2fa/login";
        /// <summary>
        /// Route for managing two-factor authentication.
        /// </summary>
        public const string TwoFactorManage = "users/2fa/manage";
        /// <summary>
        /// Route for generating two-factor recovery codes.
        /// </summary>
        public const string TwoFactorRecoveryCodes = "users/2fa/recovery/codes";
        /// <summary>
        /// Route for redeeming two-factor recovery codes.
        /// </summary>
        public const string TwoFactorRedeemRecoveryCodes = "users/2fa/recovery";
        /// <summary>
        /// Route for updating a user's email.
        /// </summary>
        public const string UpdateEmail = "users/details/email";
        /// <summary>
        /// Route for updating a user's phone number.
        /// </summary>
        public const string UpdatePhoneNumber = "users/details/number";
        /// <summary>
        /// Route for updating a user's address.
        /// </summary>
        public const string UpdateAddress = "users/details/address";
        /// <summary>
        /// Route for tokens.
        /// </summary>
        public const string Tokens = "users/tokens";
    }
    /// <summary>
    /// Routes related to application operations.
    /// </summary>
    public static class Applications
    {
        /// <summary>
        /// Route for reading an application by name.
        /// </summary>
        public const string ReadByName = "applications";
        /// <summary>
        /// Route for reading all applications.
        /// </summary>
        public const string ReadAll = "applications/all";
        /// <summary>
        /// Route for creating a new application.
        /// </summary>
        public const string Create = "applications";
        /// <summary>
        /// Route for updating an application by name.
        /// </summary>
        public const string UpdateByName = "applications";
        /// <summary>
        /// Route for deleting an application by name.
        /// </summary>
        public const string DeleteByName = "applications";
        /// <summary>
        /// Route for managing application secrets.
        /// </summary>
        public const string ApplicationSecrets = "applications/secrets";
    }
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
        /// Reads all the events in the system.
        /// </summary>
        public const string ReadAllEvents = "admin/events";
        /// <summary>
        /// Reads all the applications.
        /// </summary>
        public const string ReadAllApplications = "admin/applications";
    }
}