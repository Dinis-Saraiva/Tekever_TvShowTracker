using TvShowTracker.Api.Models;
public class TvShowDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public int Seasons { get; set; } = 0;
    public string ImageUrl { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Rating { get; set; } = string.Empty;
    public List<SimplePersonDto> Cast { get; set; } = new List<SimplePersonDto>();
    public List<SimplePersonDto> Directors { get; set; } = new List<SimplePersonDto>();
    public bool IsFavorite { get; set; } = false;
    public List<Genre> Genres { get; set; } = new List<Genre>();
}