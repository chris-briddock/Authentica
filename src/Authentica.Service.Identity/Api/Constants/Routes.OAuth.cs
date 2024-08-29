namespace Api.Constants;

public static partial class Routes
{
    /// <summary>
    /// Routes related to OAuth operations.
    /// </summary>
    public static class OAuth
    {
        /// <summary>
        /// Route for authorizing OAuth requests.
        /// </summary>
        public const string Authorize = "oauth2/authorize";

        /// <summary>
        /// Route for obtaining OAuth tokens.
        /// </summary>
        public const string Token = "oauth2/token";
        /// <summary>
        /// Routes for device code.
        /// </summary>
        public const string Device = "oauth2/device";

    }
}
