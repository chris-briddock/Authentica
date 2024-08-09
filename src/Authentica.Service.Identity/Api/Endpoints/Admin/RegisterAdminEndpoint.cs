using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Admin;

[Route($"{Routes.BaseRoute.Name}")]
public class RegisterAdminEndpoint : EndpointBaseAsync
                                    .WithRequest<RegisterRequest>
                                    .WithActionResult
{

    public IServiceProvider Services { get; }

    public RegisterAdminEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    [HttpPost($"{Routes.Admin.Create}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    public override async Task<ActionResult> HandleAsync(RegisterRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userWriteStore = Services.GetRequiredService<IUserWriteStore>();
        var eventStore = Services.GetRequiredService<IEventStore>();
        var userManager = Services.GetRequiredService<UserManager<User>>();

        var result = await userWriteStore.CreateUserAsync(request, cancellationToken);

        RegisterAdminEvent @event = new()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);

        if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        if (!await userManager.IsInRoleAsync(result.User, RoleDefaults.Admin))
            await userManager.AddToRoleAsync(result.User, RoleDefaults.Admin);

        // call token endpoint for a email confirmation token.
        return StatusCode(StatusCodes.Status201Created);
    }
}