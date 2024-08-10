using System.Text;
using Api.Constants;
using Application.Contracts;
using Application.DTOs;
using Application.Factories;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class UpdateApplicationByNameEndpointTests
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
    public async Task UpdateApplication_Returns200OK_WhenUpdateIsSuccessful()
    {
        using var sutClient = _fixture.CreateAuthenticatedClient();
        
        var request = new UpdateApplicationByNameRequest()
        {
            CurrentName = "Default Application",
            NewName = "Default App",
            NewCallbackUri = "https://localhost:7255/callback",
            NewRedirectUri = "https://localhost:7255"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var response = await sutClient.PutAsync($"api/v1/{Routes.Applications.UpdateByName}", jsonContent);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task UpdateApplication_Returns400BadRequest_WhenUserIsNull()
    {
        var storeMock = new UserReadStoreMock();

        storeMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>())).ReturnsAsync(UserStoreResult.Failed());

        using var sutClient = _fixture.CreateAuthenticatedClient(s => 
        {
            s.Replace(new ServiceDescriptor(typeof(IUserReadStore), storeMock.Object));
        });

        var request = new UpdateApplicationByNameRequest()
        {
            CurrentName = "Default Application",
            NewName = "Default App",
            NewCallbackUri = "https://localhost:7255/callback",
            NewRedirectUri = "https://localhost:7255"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var response = await sutClient.PutAsync($"api/v1/{Routes.Applications.UpdateByName}", jsonContent);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateApplication_Returns400BadRequest_WhenAppIsNull()
    {
        var storeMock = new ApplicationReadStoreMock();

        storeMock.Setup(x => x.GetClientApplicationByNameAndUserIdAsync(It.IsAny<string>(),
                                                                        It.IsAny<string>(),
                                                                        It.IsAny<CancellationToken>()));

        using var sutClient = _fixture.CreateAuthenticatedClient(s => 
        {
            s.Replace(new ServiceDescriptor(typeof(IApplicationReadStore), storeMock.Object));
        });

        var request = new UpdateApplicationByNameRequest()
        {
            CurrentName = "Default Application",
            NewName = "Default App",
            NewCallbackUri = "https://localhost:7255/callback",
            NewRedirectUri = "https://localhost:7255"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var response = await sutClient.PutAsync($"api/v1/{Routes.Applications.UpdateByName}", jsonContent);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateApplication_Returns500InternalServerError_WhenUpdateFails()
    {
        var userWriteStoreMock = new ApplicationWriteStoreMock();

        userWriteStoreMock.Setup(x => x.UpdateApplicationAsync(It.IsAny<ApplicationDTO<UpdateApplicationByNameRequest>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(new Exception())));

         using var sutClient = _fixture.CreateAuthenticatedClient(s => 
        {
            s.Replace(new ServiceDescriptor(typeof(IApplicationWriteStore), userWriteStoreMock.Object));
        });
        
        var request = new UpdateApplicationByNameRequest()
        {
            CurrentName = "Default Recent Deleted Application",
            NewName = "Default Recent Application",
            NewCallbackUri = "https://localhost:7255/callback",
            NewRedirectUri = "https://localhost:7255"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        using var response = await sutClient.PutAsync($"api/v1/{Routes.Applications.UpdateByName}", jsonContent);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
}
