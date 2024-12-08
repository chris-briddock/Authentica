using Api.Constants;
using Domain.ValueObjects;
using System.Text;

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

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var sut = await client.PutAsync($"api/v1/{Routes.Users.UpdateAddress}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    }
}
