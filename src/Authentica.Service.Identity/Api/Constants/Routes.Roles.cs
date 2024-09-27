namespace Api.Constants;

public static partial class Routes
{
    /// <summary>
    /// Contains route constants specific to group-related operations.
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Route for adding a user to a role.
        /// </summary>
        public const string Add = "roles/add";

        /// <summary>
        /// Route for creating a new role.
        /// </summary>
        public const string Create = "roles/create";

        /// <summary>
        /// Route for deleting a role.
        /// </summary>
        public const string Delete = "roles/delete";

        /// <summary>
        /// Route for updating a role.
        /// </summary>
        public const string Update = "roles/update";

        /// <summary>
        /// Route for reading role information.
        /// </summary>
        public const string Read = "roles";
    }
}