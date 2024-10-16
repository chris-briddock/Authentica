using Api.Requests;
using Api.Constants;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Contexts;
using Application.Activities;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for updating a user's address.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class UpdateAddressEndpoint : EndpointBaseAsync
                                            .WithRequest<UpdateAddressRequest>
                                            .WithActionResult
{
    /// <summary>
    /// Gets the service provider instance for resolving services.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAddressEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when the services parameter is null.</exception>
    public UpdateAddressEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP PUT request to update a user's address.
    /// </summary>
    /// <param name="request">The request containing the new address details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    /// <response code="200">The user's address was successfully updated.</response>
    [HttpPut($"{Routes.Users.UpdateAddress}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(UpdateAddressRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var dbContext = Services.GetRequiredService<AppDbContext>();
        var activityWriteStore = Services.GetRequiredService<IActivityWriteStore>();

        var user = (await userReadStore.GetUserByEmailAsync(User, cancellationToken)).User;

        user.Address = request.Address;
        user.EntityModificationStatus.ModifiedBy = user.Id;
        user.EntityModificationStatus.ModifiedOnUtc = DateTime.UtcNow;

        dbContext.Users.Update(user);

        UpdateAddressActivity activity = new()
        {
            Payload = request
        };

        await activityWriteStore.SaveActivityAsync(activity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}

