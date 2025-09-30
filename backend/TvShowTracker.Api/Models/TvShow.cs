using System.ComponentModel.DataAnnotations;
using HotChocolate;
namespace TvShowTracker.Api.Models
{

    public enum Rating
    {
        TV_MA,
        TV_14,
        TV_PG,
        TV_G,
        TV_Y,
        TV_Y7,
        other
    }
    public class TvShow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int Seasons { get; set; } = 0;
        public string ImageUrl { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public Rating Rating { get; set; } = Rating.other;
        public ICollection<WorkedOn> WorkedOn { get; set; } = new List<WorkedOn>();
        public ICollection<TvShowGenre> TvShowGenres { get; set; } = new List<TvShowGenre>();
        public TvShowFeatures? Features { get; set; }
    }

    public class TvShowFeatures
    {
        [Key]
        public int TvShowId { get; set; }
        public string CombinedVectorJson { get; set; } = string.Empty;
        public TvShow TvShow { get; set; } = null!;

    }
}