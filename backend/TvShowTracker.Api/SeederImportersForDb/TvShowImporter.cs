using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using System.Collections.Generic;

/// <summary>
/// Provides functionality to import TV shows, genres, and people from a CSV file into the database.
/// </summary>
class CsvImporter
{
    /// <summary>
    /// Imports TV shows from a CSV file and populates the database with TV shows, directors, cast, and genres.
    /// </summary>
    /// <param name="filePath">The path to the CSV file containing TV show data.</param>
    /// <param name="context">The database context used to save TV shows and related data.</param>
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
            };

            context.TvShows.Add(tvShow);

            helperFunctionAddPerson(context, record.Director, tvShow, JobTitle.Director);
            helperFunctionAddPerson(context, record.Cast, tvShow, JobTitle.Actor);

            // Handle genres
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

    /// <summary>
    /// Helper method to add people (directors or actors) to the database and associate them with a TV show.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="names">Comma-separated names of people.</param>
    /// <param name="tvShow">The TV show to associate with.</param>
    /// <param name="role">The role of the person in the TV show (Actor or Director).</param>
    static void helperFunctionAddPerson(ApplicationDbContext context, string names, TvShow tvShow, JobTitle role)
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

                /* Uncomment if WorkedOn relationship should be tracked
                var workedOn = new WorkedOn
                {
                    Name = person,
                    TvShow = tvShow,
                    Role = role
                };
                context.WorkedOns.Add(workedOn); */
            }
        }
    }

    /// <summary>
    /// Parses the duration string to determine the number of seasons.
    /// </summary>
    /// <param name="duration">The duration string from CSV (e.g., "3 Seasons").</param>
    /// <returns>The number of seasons as an integer.</returns>
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

    /// <summary>
    /// Parses a rating string into a <see cref="Rating"/> enum.
    /// </summary>
    /// <param name="ratingString">The rating string from CSV (e.g., "TV-MA").</param>
    /// <returns>The corresponding <see cref="Rating"/> enum value.</returns>
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
