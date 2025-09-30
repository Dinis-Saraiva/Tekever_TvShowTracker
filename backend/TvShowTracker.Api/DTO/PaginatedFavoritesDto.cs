
using TvShowTracker.Api.Models;

public class PaginatedFavoritesDto
{
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public List<FavoriteTvShows> Items { get; set; }= new List<FavoriteTvShows>();
}
