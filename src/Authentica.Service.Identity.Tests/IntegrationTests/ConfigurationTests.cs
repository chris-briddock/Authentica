namespace Authentica.Service.Identity.Tests.IntegrationTests;

public class ConfigurationTests
{
    private CustomWebApplicationFactory<Program> _fixture;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _fixture = new CustomWebApplicationFactory<Program>();
        _fixture.StartTestContainer();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _fixture.StopTestContainer();
        _fixture.Dispose();
    }

    [Test]
    public async Task Server_Should_Respond_On_Configured_Port()
    {
        // Arrange
        var client =_fixture.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true
        });

        // Act
        HttpResponseMessage response = await client.GetAsync("/health");

        string? errorContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.That(response.IsSuccessStatusCode, Is.True, $"Expected success status code but got {(int)response.StatusCode}");
    }
}