using System.Text;
using Api.Constants;
using Application.Factories;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

[TestFixture]
public class ConfirmEmailEndpointTests
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
    public async Task ConfirmEmail_Returns200OK_WhenConfirmationIsSuccessful()
    {
        var userReadStoreMock = new UserReadStoreMock();
        var userWriteStoreMock = new UserWriteStoreMock();
        var email = "test@default.com";

        userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
        .ReturnsAsync(UserStoreResult.Success(new User() { Email = "test@test.com"}));
        
        userWriteStoreMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
        .ReturnsAsync(UserStoreResult.Success());

        using var client = _fixture.WebApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Singleton(userReadStoreMock.Object));
                services.Replace(ServiceDescriptor.Singleton(userWriteStoreMock.Object));
            });
        }).CreateClient();

        ConfirmEmailRequest request = new()
        {
            Email= email,
            Token = "dklcmsdklmdsmk"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"api/v1/{Routes.Users.ConfirmEmail}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    }


    [Test]
    public async Task ConfirmEmail_Returns500InternalServerError_WhenErrorsAreReturned()
    {
        var userReadStoreMock = new UserReadStoreMock();
        var userWriteStoreMock = new UserWriteStoreMock();
        var email = "test@default.com";

        userReadStoreMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
        .ReturnsAsync(UserStoreResult.Success(new User() { Email = "test@test.com"}));

        userWriteStoreMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(new Exception())));

        using var client = _fixture.WebApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Singleton(userReadStoreMock.Object));
                services.Replace(ServiceDescriptor.Singleton(userWriteStoreMock.Object));
            });
        }).CreateClient();

        ConfirmEmailRequest request = new()
        {
            Email= email,
            Token = "dklcmsdklmdsmk"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var sut = await client.PostAsync($"api/v1/{Routes.Users.ConfirmEmail}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
    
}