using Api.Constants;
using Application.Contracts;
using Application.Factories;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class DeleteAccountEndpointTests
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
    public async Task DeleteAccount_Returns204NoContent_WhenSoftDeletionIsSuccessful()
    {
       var client = _fixture.CreateAuthenticatedClient();

       var result = await client.DeleteAsync($"api/v1/{Routes.Users.DeleteByEmail}");

       Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeleteAccount_Returns500InternalServerError_WhenSoftDeletionFails()
    {
       var mockWriteStore = new UserWriteStoreMock();
       
       mockWriteStore.Setup(x => x.SoftDeleteUserAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
       .ReturnsAsync(UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(new Exception())));

       var client = _fixture.CreateAuthenticatedClient(x => x.Replace(new ServiceDescriptor(typeof(IUserWriteStore), mockWriteStore.Object)));

       var result = await client.DeleteAsync($"api/v1/{Routes.Users.DeleteByEmail}");

       Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
}
