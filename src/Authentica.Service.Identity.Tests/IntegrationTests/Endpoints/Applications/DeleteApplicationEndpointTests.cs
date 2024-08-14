using System.Text;
using Api.Constants;
using Application.Contracts;
using Application.DTOs;
using Application.Factories;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class DeleteApplicationEndpointTests
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
    public async Task DeleteApplication_Returns204NoContent_WhenRequestIsValid()
    {
        // Arrange
        var content = new DeleteApplicationByNameRequest()
        {
            Name = "Default Test Application"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"http://localhost/api/v1/{Routes.Applications.DeleteByName}"),
            Content = jsonContent
        };

        using var sutClient = _fixture.CreateAuthenticatedClient();

        // Act
        using var response = await sutClient.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeleteApplication_Returns400BadRequest_WhenApplicationIsNotFound()
    {
        // Arrange
        var content = new DeleteApplicationByNameRequest()
        {
            Name = "Application"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"http://localhost/api/v1/{Routes.Applications.DeleteByName}"),
            Content = jsonContent
        };

        using var sutClient = _fixture.CreateAuthenticatedClient();

        // Act
        using var response = await sutClient.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task DeleteApplication_Returns500InternalServerError_WhenApplicationIsNotFound()
    {
        // Arrange
        var content = new DeleteApplicationByNameRequest()
        {
            Name = "Default Test Application"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"http://localhost/api/v1/{Routes.Applications.DeleteByName}"),
            Content = jsonContent
        };

        var writeStoreMock = new ApplicationWriteStoreMock();

        writeStoreMock.Setup(x => x.SoftDeleteApplicationAsync(It.IsAny<ApplicationDto<DeleteApplicationByNameRequest>>(), It.IsAny<CancellationToken>())).ReturnsAsync(ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(new Exception())));

        using var sutClient = _fixture.CreateAuthenticatedClient(s =>
        {
            s.Replace(new ServiceDescriptor(typeof(IApplicationWriteStore), writeStoreMock.Object));
        });

        // Act
        using var response = await sutClient.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
}
