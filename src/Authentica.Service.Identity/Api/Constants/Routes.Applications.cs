namespace Api.Constants;

public static partial class Routes
{
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
}
