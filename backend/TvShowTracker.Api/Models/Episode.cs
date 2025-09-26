namespace TvShowTracker.Api.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime AirDate { get; set; }
        public string Summary { get; set; } = string.Empty;
        public required TvShow TvShow { get; set; }
    }
}