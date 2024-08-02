﻿namespace Authentica.Service.Identity.Tests.Mocks;

public class UserManagerMock<TUser> : IMockBase<Mock<UserManager<TUser>>> where TUser : class
{
    public Mock<UserManager<TUser>> Mock()
    {
        var userManagerMock = new Mock<UserManager<TUser>>(
            new Mock<IUserStore<TUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<TUser>>().Object,
            Array.Empty<IUserValidator<TUser>>(),
            Array.Empty<IPasswordValidator<TUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new IdentityErrorDescriber(),
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<TUser>>>().Object);

        return userManagerMock;
    }
}
