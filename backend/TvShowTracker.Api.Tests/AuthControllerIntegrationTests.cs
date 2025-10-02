using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using TvShowTracker.Api.Tests.Dtos;
using TvShowTracker.Api.Tests.Factories;
using Xunit;

namespace TvShowTracker.Api.Tests
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });
        }

        [Fact]
        public async Task Register_ThenLogin_Success()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "alice",
                Email = "alice@test.com",
                Password = "Password123!"
            };

            // Act: register
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act: login
            var loginDto = new LoginDto { Username = "alice", Password = "Password123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Assert
            var loginContent = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
            loginContent.Should().NotBeNull();
            loginContent.User.Username.Should().Be("alice");
            loginContent.User.Email.Should().Be("alice@test.com");
        }

        [Fact]
        public async Task Login_Fails_With_WrongPassword()
        {
            var dto = new LoginDto { Username = "nosuch", Password = "bad" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", dto);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CurrentUser_Returns_User_After_Login()
        {
            var registerDto = new RegisterDto
            {
                Username = "bob",
                Email = "bob@test.com",
                Password = "Password123!"
            };
            await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            await _client.PostAsJsonAsync("/api/auth/login", new LoginDto { Username = "bob", Password = "Password123!" });

            var response = await _client.GetAsync("/api/auth/current-user");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            content.User.Username.Should().Be("bob");
        }

        [Fact]
        public async Task Logout_Clears_Session()
        {
            var registerDto = new RegisterDto
            {
                Username = "charlie",
                Email = "charlie@test.com",
                Password = "Password123!"
            };
            await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            await _client.PostAsJsonAsync("/api/auth/login", new LoginDto { Username = "charlie", Password = "Password123!" });

            // Logout
            var logoutResponse = await _client.PostAsync("/api/auth/logout", null);
            logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Current user after logout should be unauthorized
            var currentUserResponse = await _client.GetAsync("/api/auth/current-user");
            currentUserResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
