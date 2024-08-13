namespace Authentica.Common;

/// <summary>
/// Constants related to feature flags used in the application.
/// </summary>
public static class FeatureFlagConstants
{
    /// <summary>
    /// Constant representing the Redis feature flag.
    /// </summary>
    public const string Cache = "Cache";

    /// <summary>
    /// Constant representing the Azure Application Insights feature flag.
    /// </summary>
    public const string AzApplicationInsights = "AppInsights";

    /// <summary>
    /// Gets or sets the boolean to enable or disable 
    /// </summary>
    /// <remarks>
    /// This const string value is used by <see cref="Microsoft.FeatureManagement"/> to check 
    /// the appsetings.json when this method is called <see cref="FeatureManager.IsEnabledAsync"/>
    /// </remarks>
    public const string AzServiceBus = "ServiceBus";

    /// <summary>
    /// Gets or sets the boolean to enable or disable 
    /// </summary>
    /// <remarks>
    /// This const string value is used by <see cref="Microsoft.FeatureManagement"/> to check 
    /// the appsetings.json when this method is called <see cref="FeatureManager.IsEnabledAsync"/>
    /// </remarks>
    public const string RabbitMq = "RabbitMq";
}