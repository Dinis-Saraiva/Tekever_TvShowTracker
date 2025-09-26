namespace TvShowTracker.Api.Models
{
    public class TvShow
    {
        public required int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public List<Actor> Actors { get; set; } = new List<Actor>();
    }
}