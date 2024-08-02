using Api.Responses;
using Domain.Aggregates.Identity;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

/// <summary>
/// Mapper class to convert <see cref="ClientApplication"/> 
/// objects to <see cref="ReadApplicationResponse"/> objects.
/// </summary>
[Mapper]
public partial class ClientApplicationMapper
{
    /// <summary>
    /// Converts a <see cref="ClientApplication"/> object to a <see cref="ReadApplicationResponse"/> object.
    /// </summary>
    /// <param name="clientApplication">The <see cref="ClientApplication"/> object to convert.</param>
    /// <returns>A <see cref="ReadApplicationResponse"/> object representing the mapped data.</returns>
    public partial ReadApplicationResponse ToReadByNameResponse(ClientApplication clientApplication);
}
