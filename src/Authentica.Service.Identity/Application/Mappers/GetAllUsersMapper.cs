using Api.Responses;
using Domain.Aggregates.Identity;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

/// <summary>
/// Provides mapping functionality for converting a list of users to their response representations.
/// </summary>
[Mapper]
public partial class GetAllUsersMapper
{
    /// <summary>
    /// Converts a list of <see cref="User"/> objects to a list of <see cref="GetUserResponse"/> objects.
    /// </summary>
    /// <param name="Users">The list of users to convert.</param>
    /// <returns>A list of <see cref="GetUserResponse"/> objects representing the users.</returns>
    public partial List<GetUserResponse> ToResponse(IList<User> Users);
}
