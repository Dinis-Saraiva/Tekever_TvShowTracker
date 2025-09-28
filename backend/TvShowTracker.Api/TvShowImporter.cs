/* using System.Globalization;
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
                Origin = record.Country,
                Seasons = ParseSeasons(record.Duration),
                ImageUrl = "",
                Rating = ParseRating(record.Rating),
                //Genre = record.Type,
            };
            context.TvShows.Add(tvShow);
            helperFunctionAddPerson(context, record.Director, tvShow, JobTitle.Director);
            helperFunctionAddPerson(context, record.Cast, tvShow, JobTitle.Actor);
            // Handle Genres
            if (!string.IsNullOrEmpty(record.Type))
            {
                var genreNames = ((string)record.Type)
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(g => g.Trim());

                foreach (var genreName in genreNames)
                {
                    var genre = context.Genres.Local.FirstOrDefault(g => g.Name == genreName);

                    if (genre == null)
                    {
                        genre = new Genre { Name = genreName };
                        context.Genres.Add(genre);
                    }

                    var tvShowGenre = new TvShowGenre
                    {
                        TvShow = tvShow,
                        Genre = genre
                    };
                    context.TvShowGenres.Add(tvShowGenre);
                }
            }
        }
        // Save everything once
        context.SaveChanges();

    }

    static void helperFunctionAddPerson (ApplicationDbContext context,string names, TvShow tvShow, JobTitle role)
    {
        if (!string.IsNullOrEmpty(names))
        {
            var personNames = names
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(a => a.Trim());

            foreach (var name in personNames)
            {
                var person = context.Person.Local.FirstOrDefault(a => a.Name == name);

                if (person == null)
                {
                    person = new Person { Name = name };
                    context.Person.Add(person);
                }
                var workedOn = new WorkedOn
                {
                    Name = person,
                    TvShow = tvShow,
                    Role = role
                };
                context.WorkedOns.Add(workedOn);
            }
        }
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
    
    // Helper method to parse string to Rating enum
private static Rating ParseRating(string ratingString)
{
    if (string.IsNullOrWhiteSpace(ratingString))
        return Rating.other;

        ratingString = ratingString.Trim().ToUpper();

    return ratingString switch
    {
        "TV-MA" => Rating.TV_MA,
        "TV-14" => Rating.TV_14,
        "TV-Y7" => Rating.TV_Y7,
        "TV-PG" => Rating.TV_PG,
        "TV-G" => Rating.TV_G,
        "TV-Y" => Rating.TV_Y,
        _ => Rating.other
    };
}
}
 */