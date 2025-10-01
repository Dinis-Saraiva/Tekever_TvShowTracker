using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

/// <summary>
/// Provides methods to calculate TF-IDF vectors for TV shows based on their text features
/// (title, description, genres, cast, etc.) and store them in a sparse vector format.
/// </summary>
public class TvShowVectorCalculator
{
    /// <summary>
    /// Represents a TV show input for ML processing.
    /// </summary>
    public class TvShowForMl
    {
        public int TvShowId { get; set; }
        public string CombinedText { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a TV show with its computed dense feature vector.
    /// </summary>
    public class TvShowFeatures1
    {
        public int TvShowId { get; set; }

        [VectorType]
        public float[] CombinedVector { get; set; } = Array.Empty<float>();
    }

    /// <summary>
    /// Represents a sparse vector using indices and values.
    /// </summary>
    public class SparseVector
    {
        public int[] Indices { get; set; } = Array.Empty<int>();
        public float[] Values { get; set; } = Array.Empty<float>();
    }

    /// <summary>
    /// Calculates TF-IDF vectors for all TV shows and stores them in the database as sparse vectors.
    /// </summary>
    /// <param name="context">The database context used to retrieve TV shows and save vectors.</param>
    public static void CalculateVectors(ApplicationDbContext context)
    {
        var shows = context.TvShows
            .Include(s => s.TvShowGenres)
                .ThenInclude(tg => tg.Genre)
            .Include(s => s.WorkedOn)
                .ThenInclude(w => w.Person)
            .ToList();

        var mlData = shows.Select(s => new TvShowForMl
        {
            TvShowId = s.Id,
            CombinedText = CombineString(s)
        }).ToList();

        Console.WriteLine("ML Data prepared with " + mlData.Count + " entries.");
        Console.WriteLine("ML Data sample: " + (mlData.Count > 0 ? mlData[0].CombinedText : "No data"));

        var mlContext = new MLContext();

        var data = mlContext.Data.LoadFromEnumerable(mlData);

        var featurizeOptions = new TextFeaturizingEstimator.Options
        {
            CaseMode = TextNormalizingEstimator.CaseMode.Lower,
            KeepDiacritics = false,
            KeepPunctuations = false,
            KeepNumbers = true,
            StopWordsRemoverOptions = new StopWordsRemovingEstimator.Options(),
            WordFeatureExtractor = new WordBagEstimator.Options
            {
                NgramLength = 1,
                UseAllLengths = true,
                Weighting = NgramExtractingEstimator.WeightingCriteria.TfIdf
            },
            CharFeatureExtractor = null,
            OutputTokensColumnName = null
        };

        var pipeline = mlContext.Transforms.Text.FeaturizeText(
            outputColumnName: "CombinedVector",
            options: featurizeOptions,
            inputColumnNames: ["CombinedText"]);

        foreach (var col in data.Schema)
            Console.WriteLine($"{col.Name} : {col.Type}");

        // Fit the pipeline
        var transformer = pipeline.Fit(data);

        // Transform the data
        var transformedData = transformer.Transform(data);

        // Extract TF-IDF vectors
        var featureVectors = mlContext.Data.CreateEnumerable<TvShowFeatures1>(transformedData, reuseRowObject: false).ToList();

        foreach (var v in featureVectors)
        {
            var sparse = ToSparseVector(v.CombinedVector);
            var json = JsonSerializer.Serialize(sparse);

            var existing = context.TvShowFeatures.SingleOrDefault(x => x.TvShowId == v.TvShowId);
            if (existing == null)
            {
                context.TvShowFeatures.Add(new TvShowFeatures
                {
                    TvShowId = v.TvShowId,
                    CombinedVectorJson = json
                });
            }
            else
            {
                existing.CombinedVectorJson = json;
                context.TvShowFeatures.Update(existing);
            }
        }

        context.SaveChanges();
    }

    /// <summary>
    /// Combines textual information of a TV show into a single string for ML processing.
    /// Includes name, description, origin, genres, cast, rating, release year, and seasons.
    /// </summary>
    /// <param name="tvShow">The TV show to combine text from.</param>
    /// <returns>A combined string representing all textual features of the TV show.</returns>
    public static string CombineString(TvShow tvShow)
    {
        var combinedText = (tvShow.Name ?? "")
            + " " + (tvShow.Description ?? "")
            + " " + (tvShow.Origin ?? "")
            + " " + string.Join(" ", tvShow.TvShowGenres.Select(g => g.Genre.Name ?? ""))
            + " " + string.Join(" ", tvShow.WorkedOn.Select(w => w.Person.Name ?? ""))
            + " " + tvShow.Rating.ToString()
            + " " + tvShow.ReleaseDate.Year
            + " " + tvShow.Seasons;
        return combinedText.Trim();
    }

    /// <summary>
    /// Converts a dense float vector into a sparse vector format storing only non-zero entries.
    /// </summary>
    /// <param name="dense">The dense vector array.</param>
    /// <returns>A <see cref="SparseVector"/> containing indices and values of non-zero elements.</returns>
    public static SparseVector ToSparseVector(float[] dense)
    {
        var indices = new List<int>();
        var values = new List<float>();

        for (int i = 0; i < dense.Length; i++)
        {
            if (dense[i] != 0f)
            {
                indices.Add(i);
                values.Add(dense[i]);
            }
        }

        return new SparseVector
        {
            Indices = indices.ToArray(),
            Values = values.ToArray()
        };
    }
}
