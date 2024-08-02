using Domain.ValueObjects;

namespace Api.Requests;

/// <summary>
/// Represents a request to update an address.
/// </summary>
public sealed record UpdateAddressRequest
{
    /// <summary>
    /// Gets or sets the address to be updated.
    /// </summary>
    public Address Address { get; set; } = default!;
}
