using Api.Constants;
using Application.Activities;
using Application.DTOs;
using Ardalis.ApiEndpoints;
using Domain.Contracts.Stores;
using Domain.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Runtime.InteropServices;

namespace Api.Endpoints.Admin;

/// <summary>
/// Endpoint for reading all applications and returning their responses.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
[OutputCache]
public sealed class ReadAllActivitiesEndpoint : EndpointBaseAsync
                                                .WithoutRequest
                                                .WithActionResult<IList<ActivityResponse>>
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisableMultiFactorEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public ReadAllActivitiesEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles reading all events in the system.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpGet($"{Routes.Admin.ReadAllActivities}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    public override async Task<ActionResult<IList<ActivityResponse>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var readStore = Services.GetRequiredService<IActivityReadStore>();
        var writeStore = Services.GetRequiredService<IActivityWriteStore>();

        List<ActivityDto> activities = await readStore.GetActivitiesAsync();

        ReadAllActivitiesActivity record = new()
        {
            Email = User.Identity?.Name ?? "Unknown"
        };

        List<ActivityResponse> responses = new(activities.Count);

        Span<ActivityDto> activitySpan = CollectionsMarshal.AsSpan(activities);

        for (int i = 0; i < activitySpan.Length; i++)
        {
            var activity = activitySpan[i];

            responses.Add(new ActivityResponse()
            {
                SequenceId = activity.SequenceId,
                ActivityType = activity.ActivityType,
                CreatedOn = activity.CreatedOn,
                Data = activity.Data
            });
        }

        await writeStore.SaveActivityAsync(record);

        return Ok(responses);
    }
}