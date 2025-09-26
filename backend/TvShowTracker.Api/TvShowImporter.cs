using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

class CsvImporter
{
    public static void ImportTvShows(string filePath, ApplicationDbContext context)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            BadDataFound = null
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<dynamic>().ToList();

foreach (var record in records)
{
    string category = record.Category;
    if (!string.Equals(category, "TV Show", StringComparison.OrdinalIgnoreCase))
        continue;

    var tvShow = new TvShow
    {
        Name = record.Title,
        Description = record.Description,
        ReleaseDate = DateTime.TryParse(record.Release_Date, out DateTime releaseDate) ? releaseDate : DateTime.Now,
        EndDate = null,
        Genre = record.Type,
        Seasons = ParseSeasons(record.Duration),
        Rating = record.Rating,
        ImageUrl = ""
    };

    if (!string.IsNullOrEmpty(record.Cast))
    {
        var actorNames = ((string)record.Cast)
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(a => a.Trim());

        foreach (var name in actorNames)
        {
            var actor = context.Actors.Local.FirstOrDefault(a => a.Name == name)
                        ?? context.Actors.SingleOrDefault(a => a.Name == name);

            if (actor == null)
            {
                actor = new Actor { Name = name };
                context.Actors.Add(actor);
            }
            tvShow.Cast.Add(actor);
        }
    }

    context.TvShows.Add(tvShow);
}

// Save everything once
context.SaveChanges();

    }

    static int ParseSeasons(string duration)
    {
        if (string.IsNullOrEmpty(duration)) return 0;

        if (duration.Contains("Season"))
        {
            var number = new string(duration.TakeWhile(char.IsDigit).ToArray());
            return int.TryParse(number, out var result) ? result : 0;
        }

        return 0;
    }
}
