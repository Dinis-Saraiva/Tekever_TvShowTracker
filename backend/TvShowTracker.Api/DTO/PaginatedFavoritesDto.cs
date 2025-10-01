using TvShowTracker.Api.Models;
using System.Collections.Generic;

/// <summary>
/// Represents a paginated result set for a user's favorite TV shows.
/// </summary>
public class PaginatedFavoritesDto
{
    /// <summary>
    /// Gets or sets the total number of favorite TV shows for the user.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number in the paginated result.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages available based on <see cref="TotalCount"/> and <see cref="PageSize"/>.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the list of favorite TV shows on the current page.
    /// </summary>
    public List<FavoriteTvShows> Items { get; set; } = new List<FavoriteTvShows>();
}
