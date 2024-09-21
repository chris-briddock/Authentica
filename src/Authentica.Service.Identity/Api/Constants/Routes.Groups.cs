namespace Api.Constants;

public static partial class Routes
{
    /// <summary>
    /// Contains route constants specific to group-related operations.
    /// </summary>
    public static class Groups
    {
        /// <summary>
        /// Route for creating a new group.
        /// </summary>
        public const string Create = "groups/create";

        /// <summary>
        /// Route for deleting a group.
        /// </summary>
        public const string Delete = "groups/delete";

        /// <summary>
        /// Route for updating a group.
        /// </summary>
        public const string Update = "groups/update";

        /// <summary>
        /// Route for reading group information.
        /// </summary>
        public const string Read = "groups";
    }
}