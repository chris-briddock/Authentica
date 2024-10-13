using Api.Responses;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when applications are read.
/// </summary>
public sealed class ReadApplicationsActivity : ActivityBase<List<ReadApplicationResponse>>
{
    /// <summary>
    /// Gets or sets the payload containing the response data for reading applications.
    /// </summary>
    public override List<ReadApplicationResponse> Payload { get; set; } = [];
}
