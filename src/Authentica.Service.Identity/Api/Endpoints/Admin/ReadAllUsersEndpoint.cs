using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts.Stores;
using Domain.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Admin;

/// <summary>
/// Endpoint for reading all users.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class ReadAllUsersEndpoint : EndpointBaseAsync
                                           .WithoutRequest
                                           .WithActionResult<IList<GetUserResponse>>
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ReadAllUsersEndpoint"/>
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public ReadAllUsersEndpoint(IServiceProvider services)
    {
        Services = services;
    }
    /// <summary>
    /// Reads all users in the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the users being returned</returns>
    [HttpGet($"{Routes.Admin.ReadAllUsers}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult<IList<GetUserResponse>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var readStore = Services.GetRequiredService<IUserReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();
        IList<GetUserResponse> response = [];

        IList<User> users = await readStore.GetAllUsersAsync();

        ReadAllUsersActivity activity = new()
        {
            Email = User.Identity?.Name ?? "Unknown"
        };

        await activityStore.SaveActivityAsync(activity);

        foreach (var user in users)
        {
            response.Add(new GetUserResponse()
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber!,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnd = user.LockoutEnd,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,
                IsDeleted = user.EntityDeletionStatus.IsDeleted,
                DeletedOnUtc = user.EntityDeletionStatus.DeletedOnUtc,
                DeletedBy = user.EntityDeletionStatus.DeletedBy,
                CreatedBy = user.EntityCreationStatus.CreatedBy,
                CreatedOnUtc = user.EntityCreationStatus.CreatedOnUtc,
                ModifiedOnUtc = user.EntityModificationStatus.ModifiedOnUtc,
                ModifiedBy = user.EntityModificationStatus.ModifiedBy,
                Address = user.Address
            });
        }

        return Ok(response);
    }
}
