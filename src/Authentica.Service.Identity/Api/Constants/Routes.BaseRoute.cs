namespace Api.Constants;

public static partial class Routes
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
}