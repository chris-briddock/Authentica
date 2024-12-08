using Api.Requests;
using Api.Responses;

namespace Application.Activities;

/// <summary>
/// Represents an activity that occurs when applications are read.
/// </summary>
public sealed class ReadApplicationByNameActivity : ActivityBase<ReadApplicationByNameRequest, ReadApplicationResponse>
{
    /// <summary>
    /// Gets or sets the payload containing the request data for reading applications.
    /// </summary>
    public override ReadApplicationByNameRequest Request { get; set; } = default!;
    /// <summary>
    /// Gets or sets the payload containing the response data for reading applications.
    /// </summary>
    public override ReadApplicationResponse Response { get; set; } = default!;
}
