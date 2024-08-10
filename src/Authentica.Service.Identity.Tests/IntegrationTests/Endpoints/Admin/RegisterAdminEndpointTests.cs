using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class RegisterAdminEndpoint
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
    public async Task RegisterAdmin_Returns201Created_WhenRegisterIsSuccessful()
    {
        var request = new RegisterRequest()
        {
            Email = "admintest@default.com",
            Password = "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`",
            PhoneNumber = "+447760162366",
            Address = new Domain.ValueObjects.Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
        };

        using var client = _fixture.CreateAuthenticatedClient();

        using var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.Create}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
}
