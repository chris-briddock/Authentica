using Api.Constants;
using Application.Contracts;
using Application.Factories;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class RegisterAdminEndpointTests
{
    private TestFixture<Program> _fixture;

    private const string AdminEmail = "admintest@test.com";

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
            Email = AdminEmail,
            Password = "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`",
            PhoneNumber = "+447760162366",
            Address = new Domain.ValueObjects.Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
        };

        using var client = _fixture.CreateAuthenticatedClient();

        using var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.Create}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task RegisterAdmin_Returns500InternalServerError_WhenRegisterFails()
    {
        var request = new RegisterRequest()
        {
            Email = AdminEmail,
            Password = "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`",
            PhoneNumber = "+447760162366",
            Address = new Domain.ValueObjects.Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
        };

        var userWriteStoreMock = new UserWriteStoreMock();

        userWriteStoreMock.Setup(x => x.CreateUserAsync(It.IsAny<RegisterRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(new Exception())));

        using var client = _fixture.CreateAuthenticatedClient(x => x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStoreMock.Object)));

        using var sut = await client.PostAsJsonAsync($"api/v1/{Routes.Admin.Create}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
}
