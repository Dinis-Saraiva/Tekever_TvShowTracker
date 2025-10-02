namespace TvShowTracker.Api.Tests.Dtos
{
    public class LoginResponseDto
    {
        public bool Ok { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
