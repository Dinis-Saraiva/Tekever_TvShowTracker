using Microsoft.AspNetCore.Identity;

namespace TvShowTracker.Api.Models
{
    public class ApplicationUser : IdentityUser
    {

    }
    public class FavoriteTvShows
    {
        public int Id { get; set; }
        public int TvShowId { get; set; }
        public int UserId { get; set; }
        public required ApplicationUser User { get; set; }
        public required TvShow TvShow { get; set; }
    }
}