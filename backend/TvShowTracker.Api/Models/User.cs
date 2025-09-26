namespace TvShowTracker.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<TvShow> FavoriteShows { get; set; } = new List<TvShow>();
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}