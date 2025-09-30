using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TvShowTracker.Api.Tests
{
    public class AuthControllerIntegrationTests 
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            // spin up the API with test server
            _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true // important: keeps auth cookies for login/logout
            });
        }

        [Fact]
        public async Task Register_ThenLogin_Success()
        {
            // arrange
            var registerDto = new RegisterDto
            {
                Username = "alice",
                Email = "alice@test.com",
                Password = "Password123!"
            };

            // act: register
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            // assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // act: login
            var loginDto = new LoginDto { Username = "alice", Password = "Password123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginContent = await loginResponse.Content.ReadFromJsonAsync<dynamic>();
            ((string)loginContent.user).Should().Be("alice");
            ((string)loginContent.email).Should().Be("alice@test.com");
        }

        [Fact]
        public async Task Login_Fails_With_WrongPassword()
        {
            // arrange: user does not exist
            var dto = new LoginDto { Username = "nosuch", Password = "bad" };

            // act
            var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CurrentUser_Returns_User_After_Login()
        {
            // arrange: register + login
            var registerDto = new RegisterDto
            {
                Username = "bob",
                Email = "bob@test.com",
                Password = "Password123!"
            };
            await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            await _client.PostAsJsonAsync("/api/auth/login", new LoginDto { Username = "bob", Password = "Password123!" });

            // act: call current-user
            var response = await _client.GetAsync("/api/auth/current-user");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<dynamic>();
            ((string)content.user.username).Should().Be("bob");
        }

        [Fact]
        public async Task Logout_Clears_Session()
        {
            // arrange: register + login
            var registerDto = new RegisterDto
            {
                Username = "charlie",
                Email = "charlie@test.com",
                Password = "Password123!"
            };
            await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            await _client.PostAsJsonAsync("/api/auth/login", new LoginDto { Username = "charlie", Password = "Password123!" });

            // act: logout
            var logoutResponse = await _client.PostAsync("/api/auth/logout", content: null);

            // assert logout worked
            logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // act: try current-user after logout
            var currentUserResponse = await _client.GetAsync("/api/auth/current-user");
            currentUserResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
