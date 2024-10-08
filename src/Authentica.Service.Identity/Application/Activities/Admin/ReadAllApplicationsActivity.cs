using Api.Responses;

namespace Application.Activities;

/// <summary>
/// Represents an event that is triggered to read all applications.
/// </summary>
public class ReadAllApplicationsActivity : ActivityBase<IList<ReadApplicationResponse>>
{
    /// <summary>
    /// Gets or sets the payload containing the list of read application responses.
    /// </summary>
    /// <value>
    /// A list of <see cref="ReadApplicationResponse"/> objects representing the applications read.
    /// </value>
    public override IList<ReadApplicationResponse> Payload { get; set; } = default!;
}
