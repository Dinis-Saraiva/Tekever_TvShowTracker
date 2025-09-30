using Xunit;
using Moq;
using FluentAssertions;
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TvShowTracker.Api.Models;


public static class IdentityMocks
{
    public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        return new Mock<UserManager<TUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    public static Mock<SignInManager<TUser>> MockSignInManager<TUser>(UserManager<TUser> userManager) where TUser : class
    {
        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
        return new Mock<SignInManager<TUser>>(userManager, contextAccessor.Object, claimsFactory.Object, null!, null!, null!, null!);
    }
}
public class AuthControllerTests
{
    [Fact]
    public async Task Register_Success_ReturnsOk()
    {
        // Arrange
        var userMgr = IdentityMocks.MockUserManager<ApplicationUser>();
        var signInMgr = IdentityMocks.MockSignInManager<ApplicationUser>(userMgr.Object);

        userMgr.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), "Password123!"))
               .ReturnsAsync(IdentityResult.Success);

        var controller = new AuthController(userMgr.Object, signInMgr.Object);

        // Act
        var result = await controller.Register(new RegisterDto { Username = "bob", Email = "bob@test.com", Password = "Password123!" });

        // Assert
        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(new { message = "User registered successfully" });
    }

    [Fact]
    public async Task Register_Failure_ReturnsBadRequest()
    {
        var userMgr = IdentityMocks.MockUserManager<ApplicationUser>();
        var signInMgr = IdentityMocks.MockSignInManager<ApplicationUser>(userMgr.Object);

        userMgr.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
               .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        var controller = new AuthController(userMgr.Object, signInMgr.Object);

        var result = await controller.Register(new RegisterDto { Username = "x", Email = "x@test.com", Password = "bad" });

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Login_Success_ReturnsUserData()
    {
        var userMgr = IdentityMocks.MockUserManager<ApplicationUser>();
        var signInMgr = IdentityMocks.MockSignInManager<ApplicationUser>(userMgr.Object);

        signInMgr.Setup(m => m.PasswordSignInAsync("bob", "Password123!", false, false))
                 .ReturnsAsync(IdentitySignInResult.Success);

        userMgr.Setup(m => m.FindByNameAsync("bob"))
               .ReturnsAsync(new ApplicationUser { UserName = "bob", Email = "bob@test.com", Id = "123" });

        var controller = new AuthController(userMgr.Object, signInMgr.Object);

        var result = await controller.Login(new LoginDto { Username = "bob", Password = "Password123!" });

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(new { user = "bob", email = "bob@test.com", id = "123" });
    }

    [Fact]
    public async Task Login_Failure_ReturnsUnauthorized()
    {
        var userMgr = IdentityMocks.MockUserManager<ApplicationUser>();
        var signInMgr = IdentityMocks.MockSignInManager<ApplicationUser>(userMgr.Object);

        signInMgr.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                 .ReturnsAsync(IdentitySignInResult.Failed);

        var controller = new AuthController(userMgr.Object, signInMgr.Object);

        var result = await controller.Login(new LoginDto { Username = "bad", Password = "bad" });

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }
}