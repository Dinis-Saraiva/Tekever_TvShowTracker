using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using Microsoft.Extensions.Options;
using System.Text.Json;
public class TvShowVectorCalculator
{
    public class TvShowForMl
    {
        public int TvShowId { get; set; }
        public string CombinedText { get; set; } = string.Empty;
    }
    public class TvShowFeatures1
    {
        public int TvShowId { get; set; }
        [VectorType]
        public float[] CombinedVector { get; set; } = Array.Empty<float>();
    }
    
    public class SparseVector
{
    public int[] Indices { get; set; } = Array.Empty<int>();
    public float[] Values { get; set; } = Array.Empty<float>();
}

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


   public static string CombineString(TvShow tvShow)
{
    var combinedText = (tvShow.Name ?? "")
        + " " + (tvShow.Description ?? "")
        + " " + (tvShow.Origin ?? "")
        + " "
        + string.Join(" ", tvShow.TvShowGenres.Select(g => g.Genre.Name ?? ""))
        + " " + string.Join(" ", tvShow.WorkedOn.Select(w => w.Person.Name ?? ""))
        + " " + tvShow.Rating.ToString()
        + " " + tvShow.ReleaseDate.Year
        + " " + tvShow.Seasons;
    return combinedText.Trim();
}

public static SparseVector ToSparseVector(float[] dense)
{
    var indices = new List<int>();
    var values = new List<float>();

    for (int i = 0; i < dense.Length; i++)
    {
        if (dense[i] != 0f) // store only non-zero entries
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