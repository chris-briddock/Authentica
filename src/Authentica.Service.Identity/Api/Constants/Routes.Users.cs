namespace Api.Constants;

public static partial class Routes
{
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
        /// Route for logging in with mfa authentication.
        /// </summary>
        public const string MultiFactorLogin = "users/mfa/login";
        /// <summary>
        /// Route for managing mfa authentication.
        /// </summary>
        public const string MultiFactorManage = "users/mfa/manage";
        /// <summary>
        /// Route for managing mfa codes using an application like Google or Microsoft Authenticator. 
        /// </summary>
        public const string MultiFactorManageAuthenticator = "users/mfa/manage/authenticator";
        /// <summary>
        /// Route for generating mfa recovery codes.
        /// </summary>
        public const string MultiFactorRecoveryCodes = "users/mfa/recovery/codes";
        /// <summary>
        /// Route for redeeming mfa recovery codes.
        /// </summary>
        public const string MultiFactorRedeemRecoveryCodes = "users/mfa/recovery";
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
}