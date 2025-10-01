using TvShowTracker.Api.Models;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a TV show with detailed information, including cast, directors, genres, and metadata.
/// </summary>
public class TvShowDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the TV show.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the TV show.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the TV show.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the release date of the TV show.
    /// </summary>
    public DateTime ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the number of seasons of the TV show.
    /// </summary>
    public int Seasons { get; set; } = 0;

    /// <summary>
    /// Gets or sets the URL of the TV show's image.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the origin (country or network) of the TV show.
    /// </summary>
    public string Origin { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the rating of the TV show.
    /// </summary>
    public string Rating { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of cast members of the TV show.
    /// </summary>
    public List<SimplePersonDto> Cast { get; set; } = new List<SimplePersonDto>();

    /// <summary>
    /// Gets or sets the list of directors of the TV show.
    /// </summary>
    public List<SimplePersonDto> Directors { get; set; } = new List<SimplePersonDto>();

    /// <summary>
    /// Gets or sets a value indicating whether the TV show is marked as favorite.
    /// </summary>
    public bool IsFavorite { get; set; } = false;

    /// <summary>
    /// Gets or sets the list of genres associated with the TV show.
    /// </summary>
    public List<Genre> Genres { get; set; } = new List<Genre>();
}
