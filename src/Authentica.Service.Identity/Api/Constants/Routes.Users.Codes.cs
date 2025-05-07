namespace Api.Constants;

public static partial class Routes
{
    public static partial class Users
    {
        /// <summary>
        /// Routes for token-related operations.
        /// </summary>
        public static class Codes
        {
            /// <summary>
            /// Route for sending a multi-factor authentication token.
            /// </summary>
            public const string SendMultiFactorToken = "users/codes/mfa";

            /// <summary>
            /// Route for sending a password reset token.
            /// </summary>
            public const string SendPasswordResetToken = "users/codes/reset-password";

            /// <summary>
            /// Route for sending a email confirmation token.
            /// </summary>
            public const string SendEmailConfirmation = "users/codes/confirm-email";

            /// <summary>
            /// Route for sending a email update token.
            /// </summary>
            public const string SendEmailUpdateToken = "users/codes/update-email";

            /// <summary>
            /// Route for sending a phone number update token.
            /// </summary>
            public const string SendPhoneNumberUpdateToken = "users/codes/update-phonenumber";
        }
    }
}