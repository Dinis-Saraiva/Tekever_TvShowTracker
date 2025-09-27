using HotChocolate;
namespace TvShowTracker.Api.Models
{
    public class TvShow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int Seasons { get; set; } = 0;
        public string Rating { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        
        [GraphQLIgnore]
        public ICollection<ApplicationUser> UsersWhoFavourited { get; set; } = new List<ApplicationUser>();
        public ICollection<Actor> Cast { get; set; } = new List<Actor>();
    }
}