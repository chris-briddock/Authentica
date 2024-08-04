﻿namespace Authentica.Service.Identity.Tests.Mocks;

public class SignInManagerMock<TUser> : IMockBase<Mock<SignInManager<TUser>>> where TUser : IdentityUser<string>
{
    public Mock<SignInManager<TUser>> Mock()
    {
        return new Mock<SignInManager<TUser>>(new UserManagerMock<TUser>().Mock().Object,
                                          new Mock<IHttpContextAccessor>().Object,
                                          new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
                                          new Mock<IOptions<IdentityOptions>>().Object,
                                          new Mock<ILogger<SignInManager<TUser>>>().Object,
                                          new Mock<IAuthenticationSchemeProvider>().Object,
                                          new Mock<IUserConfirmation<TUser>>().Object);
    }
}
