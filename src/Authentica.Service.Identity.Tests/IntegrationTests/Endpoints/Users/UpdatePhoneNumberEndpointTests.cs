using Api.Constants;
using Application.Contracts;
using System.Text;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class UpdatePhoneNumberEndpointTests
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
    public async Task UpdatePhoneNumber_Returns200OK_WhenUpdateIsSuccessful()
    {
        var userWriteStoreMock = new UserWriteStoreMock();

        userWriteStoreMock.Setup(x => x.UpdatePhoneNumberAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(UserStoreResult.Success());

        var client = _fixture.CreateAuthenticatedClient(x => x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStoreMock.Object)));

        var request = new UpdatePhoneNumberRequest()
        {
            PhoneNumber = "1234567890",
            Token = "888888"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await client.PutAsync($"api/v1/{Routes.Users.UpdatePhoneNumber}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task UpdatePhoneNumber_ReturnsBadRequest_WhenUpdateIsUnsuccessful()
    {
        var userWriteStoreMock = new UserWriteStoreMock();

        userWriteStoreMock.Setup(x => x.UpdatePhoneNumberAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(UserStoreResult.Failed());

        var client = _fixture.CreateAuthenticatedClient(x => x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStoreMock.Object)));

        var request = new UpdatePhoneNumberRequest()
        {
            PhoneNumber = "1234567890",
            Token = "888888"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await client.PutAsync($"api/v1/{Routes.Users.UpdatePhoneNumber}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}