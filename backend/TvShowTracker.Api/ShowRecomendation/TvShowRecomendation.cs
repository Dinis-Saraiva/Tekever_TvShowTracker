using System.Text.Json;
using HotChocolate.Data.Projections;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using static TvShowVectorCalculator;


public class RecommendationService
{

    private readonly IDbContextFactory<ApplicationDbContext> context;
    private readonly IEmailService _emailService;

    public RecommendationService(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IEmailService emailService)
    {
        context = dbContextFactory;
        _emailService = emailService;
    }
    public async Task sendEmailRecomendations(ApplicationUser user)
{
    using var db = context.CreateDbContext();

    // fetch favorites first
    var favourites = await db.FavoriteTvShows
        .Where(f => f.UserId == user.Id)
        .Include(f => f.TvShow)
        .ToListAsync(); // fetch to memory

    if (!favourites.Any())
        return;

    var rng = new Random();
    var randomFavorites = favourites
        .OrderBy(f => rng.Next())
        .Take(5)
        .Select(f => f.TvShow)
        .ToList();

    var recommendations = getSimilarShows(randomFavorites, 10);

    var body = "We picked 10 of your favorites, and here are your recommendations:\n" +
               string.Join("\n", recommendations.Select(r => $"{r.Item1.Name} (Score: {r.Score:F2})"));

    if (!string.IsNullOrEmpty(user.Email))
        await _emailService.SendEmailAsync(user.Email, "Your TV Show Recommendations", body);
}
    public List<(TvShow, double Score)> getSimilarShows(List<TvShow> input, int n)
    {
        using var db = context.CreateDbContext();
        var similarities = new List<(TvShow tvShow, double Score)>();
        var lista = db.TvShowFeatures.Include(s => s.TvShow).ToList();
        var average = GetAverageSparseVector(input, lista);
        foreach (var l in lista)
        {
            if (input.Any(t => t.Id == l.TvShowId))
                continue;
            var vector = JsonSerializer.Deserialize<SparseVector>(l.CombinedVectorJson);
            if (vector == null)
                continue;
            var sim = CosineSimilarity(vector, average);
            similarities.Add((l.TvShow, sim));
        }
        var topSimilar = similarities.OrderByDescending(s => s.Score).Take(n).ToList();
        return topSimilar;
    }

    public static double CosineSimilarity(SparseVector v1, SparseVector v2)
    {
        double dot = 0.0, normA = 0.0, normB = 0.0;
        var dict1 = v1.Indices.Zip(v1.Values, (i, val) => new { i, val })
                               .ToDictionary(x => x.i, x => x.val);

        var dict2 = v2.Indices.Zip(v2.Values, (i, val) => new { i, val })
                               .ToDictionary(x => x.i, x => x.val);

        foreach (var kvp in dict1)
        {
            if (dict2.TryGetValue(kvp.Key, out var val2))
            {
                dot += kvp.Value * val2;
            }

            normA += kvp.Value * kvp.Value;
        }

        foreach (var val in dict2.Values)
        {
            normB += val * val;
        }

        return (normA == 0 || normB == 0) ? 0 : dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
    }
    private static SparseVector GetAverageSparseVector(List<TvShow> input, List<TvShowFeatures> lista)
    {
        var filtered = lista.Where(l => input.Any(tv => tv.Id == l.TvShowId)).ToList();
        if (!filtered.Any())
            return new SparseVector();

        var sumDict = new Dictionary<int, double>();

        foreach (var f in filtered)
        {
            var vector = JsonSerializer.Deserialize<SparseVector>(f.CombinedVectorJson);
            if (vector == null)
                continue;
            for (int i = 0; i < vector.Indices.Length; i++)
            {
                int index = vector.Indices[i];
                double value = vector.Values[i];

                if (!sumDict.ContainsKey(index))
                    sumDict[index] = 0;

                sumDict[index] += value;
            }
        }

        int count = filtered.Count;
        var indices = sumDict.Keys.ToArray();
        var values = indices.Select(i => (float)(sumDict[i] / count)).ToArray();

        return new SparseVector { Indices = indices, Values = values };
    }

}