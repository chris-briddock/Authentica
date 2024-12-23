using System.Text;
using Api.Constants;
using Application.Contracts;
using Persistence.Seed;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class ResetPasswordEndpointTests
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
    public async Task ResetPassword_Returns204NoContent_WhenResetIsSucessful()
    {
        var userWriteStoreMock = new UserWriteStoreMock();

        userWriteStoreMock.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(UserStoreResult.Success);
        
        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(x => 
        {
            x.ConfigureTestServices(s => s.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStoreMock.Object)));
        }).CreateClient();

        var request = new PasswordResetRequest()
        {
            Email = Seed.Test.AdminEmail,
            Token = "ajdndjnksdn",
            Password = "dsknsdkfnkEewFDfsdFKe8fe'']']]'"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var sut = await client.PostAsync($"api/v1/{Routes.Users.ResetPassword}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task ResetPassword_Returns500InternalServerError_WhenResetIsUnsucessful()
    {
        var userWriteStoreMock = new UserWriteStoreMock();

        userWriteStoreMock.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(UserStoreResult.Failed());
        
        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(x => 
        {
            x.ConfigureTestServices(s => s.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStoreMock.Object)));
        }).CreateClient();

        var request = new PasswordResetRequest()
        {
            Email = Seed.Test.AdminEmail,
            Token = "ajdndjnksdn",
            Password = "dsknsdkfnkEewFDfsdFKe8fe'']']]'"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var sut = await client.PostAsync($"api/v1/{Routes.Users.ResetPassword}", jsonContent);;

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
}
