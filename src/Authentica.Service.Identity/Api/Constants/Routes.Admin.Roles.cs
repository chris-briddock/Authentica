namespace Api.Constants;

public static partial class Routes
{
    public static partial class Admin
    {
        /// <summary>
        /// Contains route constants specific to group-related operations.
        /// </summary>
        public static partial class Roles
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