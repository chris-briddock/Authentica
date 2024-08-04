using Api.Constants;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class SendTokenEndpointTests
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

    [TestCase(EmailPublisherConstants.TwoFactor)]
    [TestCase(EmailPublisherConstants.ConfirmEmail)]
    [TestCase(EmailPublisherConstants.ResetPassword)]
    [TestCase(EmailPublisherConstants.UpdateEmail)]
    [TestCase(EmailPublisherConstants.UpdatePhoneNumber)]
    public async Task SendTokenEndpoint_ReturnsOK_ForDifferentTokenTypes(string tokenType)
    {
        // Arrange
        var client = _fixture.WebApplicationFactory.CreateClient();

        var request = new SendTokenRequest()
        {
            Email = "admin@default.com",
            TokenType = tokenType
        };

        // Act
        using var sut = await client.PostAsync($"/api/v1/{Routes.Users.Tokens}?email_address={request.Email}&token_type={request.TokenType}", null!);
        var errorResponse = await sut.Content.ReadAsStringAsync();

        // Assert
        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}