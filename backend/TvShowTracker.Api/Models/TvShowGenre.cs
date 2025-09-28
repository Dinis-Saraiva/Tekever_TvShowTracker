namespace TvShowTracker.Api.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

public class TvShowGenre
{
    public int Id { get; set; }
    public int TvShowId { get; set; }
    public int GenreId { get; set; }

    public required TvShow TvShow { get; set; }
    public required Genre Genre { get; set; }
}
}