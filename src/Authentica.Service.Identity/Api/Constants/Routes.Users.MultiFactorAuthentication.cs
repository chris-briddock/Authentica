namespace Api.Constants;

public static partial class Routes
{
    public static partial class Users 
    {
        /// <summary>
        /// Routes for multi-factor authentication.
        /// </summary>
        public static class MultiFactorAuthentication
        {
            /// <summary>
            /// Route for logging in with mfa authentication.
            /// </summary>
            public const string LoginEmail = "users/mfa/login/email";
            /// <summary>
            /// Route for logging in with mfa authentication.
            /// </summary>
            public const string LoginAuthenticator = "users/mfa/login/authenticator";
            /// <summary>
            /// Route for managing mfa authentication.
            /// </summary>
            public const string Manage = "users/mfa/manage";
            /// <summary>
            /// Route for managing mfa codes using an application like Google or Microsoft Authenticator. 
            /// </summary>
            public const string ManageAuthenticator = "users/mfa/manage/authenticator";
            /// <summary>
            /// Route for generating mfa recovery codes.
            /// </summary>
            public const string RecoveryCodes = "users/mfa/recovery/codes";
            /// <summary>
            /// Route for redeeming mfa recovery codes.
            /// </summary>
            public const string RedeemRecoveryCodes = "users/mfa/recovery";
        }
    }
}