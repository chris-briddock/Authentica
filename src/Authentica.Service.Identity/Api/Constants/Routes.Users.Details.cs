namespace Api.Constants;

public static partial class Routes
{
    /// <inheritdoc/>
    public static partial class Users
    {
        /// <summary>
        /// Route for updating user details.
        /// </summary>
        public static class Details
        {
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
        }

    }
}