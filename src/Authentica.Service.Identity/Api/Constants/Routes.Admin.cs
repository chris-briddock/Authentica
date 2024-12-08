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
        public const string DisableMultiFactor = "admin/mfa/disable";
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

        /// <summary>
        /// Contains route constants specific to group-related operations.
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// Route for adding a user to a role.
            /// </summary>
            public const string Add = "admin/roles/add";

            /// <summary>
            /// Route for creating a new role.
            /// </summary>
            public const string Create = "admin/roles/create";

            /// <summary>
            /// Route for deleting a role.
            /// </summary>
            public const string Delete = "admin/roles/delete";

            /// <summary>
            /// Route for updating a role.
            /// </summary>
            public const string Update = "admin/roles/update";

            /// <summary>
            /// Route for reading role information.
            /// </summary>
            public const string Read = "admin/roles";
        }
    }

}