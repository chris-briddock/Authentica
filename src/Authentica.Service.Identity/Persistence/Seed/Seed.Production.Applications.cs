using ChristopherBriddock.AspNetCore.Extensions;
using Domain.Aggregates.Identity;
using Domain.Contracts.Cryptography;
using Microsoft.AspNetCore.Identity;
using Persistence.Contexts;

namespace Persistence.Seed;

public static partial class Seed
{
    /// <summary>
    /// Seeds a client application into the database.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SeedProdApplicationAsync(WebApplication app) => 
            await SeedClientApplicationAsync(app, "Authentica Default Application", false);
}