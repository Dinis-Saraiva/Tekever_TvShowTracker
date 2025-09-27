public class TvShowDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string rating { get; set; } = string.Empty;
    public List<ActorDto> Cast { get; set; } = new();
    public int NumberOfSeasons { get; set; }
}

public class ActorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
