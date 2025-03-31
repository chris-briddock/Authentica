using Persistence.Seed;
using System.Text;

namespace Authentica.Service.Identity.Tests.IntegrationTests.Endpoints;

public class RegisterEndpointTests
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
    public async Task Register_Returns201Created_WhenUserRegistrationIsSuccessful()
    {
        var client = _fixture.WebApplicationFactory.CreateClient();

        var request = new RegisterRequest()
        {
            Email = "admin@admin.com",
            Password = "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`",
            PhoneNumber = "+447760162366",
            Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var sut = await client.PostAsync($"{Routes.Users.Create}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    [Test]
    public async Task Register_Returns409Conflict_WhenUserIsAlreadyRegistered()
    {
        var client = _fixture.WebApplicationFactory.CreateClient();

        var request = new RegisterRequest()
        {
            Email = Seed.AdminEmail,
            Password = "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`",
            PhoneNumber = "+447760162366",
            Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var sut = await client.PostAsync($"{Routes.Users.Create}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
    [Test]
    public async Task Register_Returns500InternalServerError_WhenCreateFails()
    {
        var userWriteStoreMock = new UserWriteStoreMock();

        userWriteStoreMock.Setup(x => x.CreateUserAsync(It.IsAny<RegisterRequest>(),
                                                        It.IsAny<CancellationToken>()))
                                                        .ReturnsAsync(UserStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(new Exception("error occured."))));

        var client = _fixture.WebApplicationFactory.WithWebHostBuilder(c =>
        {
            c.ConfigureServices(s =>
            {
                s.Replace(new ServiceDescriptor(typeof(IUserWriteStore), userWriteStoreMock.Object));
            });
        }).CreateClient();

        var request = new RegisterRequest()
        {
            Email = "admin@re.com",
            Password = "7XAl@Dg()[=8rV;[wD[:GY$yw:$ltHAuaf!UQ`",
            PhoneNumber = "+447760162366",
            Address = new Address("DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT")
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var sut = await client.PostAsync($"{Routes.Users.Create}", jsonContent);

        Assert.That(sut.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }
}