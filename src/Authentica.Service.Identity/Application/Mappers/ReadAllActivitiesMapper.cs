using System.Collections.Immutable;
using Api.Responses;
using Domain.Aggregates.Identity;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

/// <summary>
/// Provides mapping functionality to convert event domain models to response models.
/// </summary>
[Mapper]
public partial class ReadAllActivitiesMapper
{
    /// <summary>
    /// Maps a list of activity domain models to a list of activity response models.
    /// </summary>
    /// <param name="activities">The list of activity domain models to be mapped.</param>
    /// <returns>A list of activity response models.</returns>
    public partial ImmutableList<ActivityResponse> ToResponse(ImmutableList<Activity> activities);
}
