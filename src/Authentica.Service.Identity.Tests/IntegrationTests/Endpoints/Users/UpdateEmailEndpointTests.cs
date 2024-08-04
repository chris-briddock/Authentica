using Api.Constants;
using Application.Contracts;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class UpdateEmailEndpointTests
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
    public async Task UpdateEmail_Returns200OK_WhenUpdateIsSuccessful()
    {

        var userWriteStoreMock = new UserWriteStoreMock();

        userWriteStoreMock.Setup(x => x.UpdateEmailAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(UserStoreResult.Success());

        var client = _fixture.CreateAuthenticatedClient(x => x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStoreMock.Object)));

        var request = new UpdateEmailRequest()
        {
            Email = "administrator@default.com",
            Token = "888888"
        };

        using var sut = await client.PutAsJsonAsync($"api/v1/{Routes.Users.UpdateEmail}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task UpdateEmail_ReturnsBadRequest_WhenUpdateIsUnsuccessful()
    {

        var userWriteStoreMock = new UserWriteStoreMock();

        userWriteStoreMock.Setup(x => x.UpdateEmailAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(UserStoreResult.Failed());

        var client = _fixture.CreateAuthenticatedClient(x => x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStoreMock.Object)));

        var request = new UpdateEmailRequest()
        {
            Email = "administrator@default.com",
            Token = "888888"
        };

        using var sut = await client.PutAsJsonAsync($"api/v1/{Routes.Users.UpdateEmail}", request);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}