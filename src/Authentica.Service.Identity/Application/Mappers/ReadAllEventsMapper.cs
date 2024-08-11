using Api.Responses;
using Domain.Aggregates.Identity;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

/// <summary>
/// Provides mapping functionality to convert event domain models to response models.
/// </summary>
[Mapper]
public partial class ReadAllEventsMapper
{
    /// <summary>
    /// Maps a list of event domain models to a list of event response models.
    /// </summary>
    /// <param name="events">The list of event domain models to be mapped.</param>
    /// <returns>A list of event response models.</returns>
    public partial List<EventResponse> ToResponse(List<Event> events);
}
