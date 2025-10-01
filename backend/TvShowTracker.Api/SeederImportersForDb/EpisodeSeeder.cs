using System;
using System.Linq;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

/// <summary>
/// Provides functionality to seed random episodes for TV shows in the database.
/// </summary>
class EpisodeSeeder
{
    private static readonly Random _random = new Random();

    /// <summary>
    /// Seeds episodes for all TV shows in the provided <see cref="ApplicationDbContext"/>.
    /// Skips shows without season information or shows that already have episodes.
    /// </summary>
    /// <param name="context">The database context used to access TV shows and save episodes.</param>
    public static void SeedEpisodes(ApplicationDbContext context)
    {
        var tvShows = context.TvShows.ToList();

        foreach (var show in tvShows)
        {
            // Skip shows without season information
            if (show.Seasons <= 0)
                continue;

            // If the show already has episodes, skip
            if (context.Episodes.Any(e => e.TvShow.Id == show.Id))
                continue;

            for (int season = 1; season <= show.Seasons; season++)
            {
                int episodesPerSeason = _random.Next(3, 7); // 3 to 6 episodes per season
                for (int epNum = 1; epNum <= episodesPerSeason; epNum++)
                {
                    var episode = new Episode
                    {
                        Title = $"{show.Name} S{season:00}E{epNum:00}",
                        SeasonNumber = season,
                        EpisodeNumber = epNum,
                        AirDate = RandomAirDate(show.ReleaseDate, season, epNum),
                        Summary = $"This is a randomly generated summary for episode {epNum} of season {season}.",
                        TvShow = show
                    };
                    context.Episodes.Add(episode);
                }
            }
        }

        context.SaveChanges();
        Console.WriteLine("Episodes seeded successfully!");
    }

    /// <summary>
    /// Generates a random air date for an episode based on the TV show's release date, season, and episode number.
    /// </summary>
    /// <param name="releaseDate">The release date of the TV show.</param>
    /// <param name="season">The season number of the episode.</param>
    /// <param name="episode">The episode number within the season.</param>
    /// <returns>A <see cref="DateTime"/> representing the estimated air date.</returns>
    private static DateTime RandomAirDate(DateTime releaseDate, int season, int episode)
    {
        // Roughly estimate air date: add weeks for season/episode
        int daysOffset = ((season - 1) * 12 * 7) + ((episode - 1) * 7);
        return releaseDate.AddDays(daysOffset);
    }
}
