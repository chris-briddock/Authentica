using Api.Constants;
using Domain.ValueObjects;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class UpdateAddressEndpointTests
{
    private TestFixture<Program> _fixture;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _fixture = new TestFixture<Program>();
        await _fixture.OneTimeSetUpAsync();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _fixture.OneTimeTearDown();
    }

    [Test]
    public async Task UpdateAddress_Returns200OK_WhenUpdateIsSuccessful()
    {
        var client = _fixture.CreateAuthenticatedClient();

        var request = new UpdateAddressRequest()
        {
            Address = new Address("UPDATED", "UPDATED", "UPDATED", "UPDATED", "UPDATED", "UPDATED", "UPDATED")
        };

        var sut = await client.PutAsJsonAsync($"api/v1/{Routes.Users.UpdateAddress}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    }
}
