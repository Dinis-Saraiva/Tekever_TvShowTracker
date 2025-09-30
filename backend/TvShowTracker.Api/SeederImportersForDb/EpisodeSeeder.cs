using System;
using System.Linq;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

class EpisodeSeeder
{
    private static readonly Random _random = new Random();

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
                int episodesPerSeason = _random.Next(3, 7); // 5 to 10 episodes per season
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

    private static DateTime RandomAirDate(DateTime releaseDate, int season, int episode)
    {
        // Roughly estimate air date: add weeks for season/episode
        int daysOffset = ((season - 1) * 12 * 7) + ((episode - 1) * 7);
        return releaseDate.AddDays(daysOffset);
    }
}
