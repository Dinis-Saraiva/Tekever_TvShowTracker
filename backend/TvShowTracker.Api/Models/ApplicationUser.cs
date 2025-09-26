using Microsoft.AspNetCore.Identity;

namespace TvShowTracker.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<TvShow> FavoriteShows { get; set; } = new List<TvShow>();
    }
}