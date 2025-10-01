using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using static TvShowVectorCalculator;

/// <summary>
/// Provides TV show recommendation functionality based on user favorites and feature vectors.
/// </summary>
public class RecommendationService
{
    private readonly IDbContextFactory<ApplicationDbContext> context;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RecommendationService"/> class.
    /// </summary>
    /// <param name="dbContextFactory">The factory to create <see cref="ApplicationDbContext"/> instances.</param>
    /// <param name="emailService">The email service used to send recommendations to users.</param>
    public RecommendationService(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IEmailService emailService)
    {
        context = dbContextFactory;
        _emailService = emailService;
    }

    /// <summary>
    /// Sends TV show recommendations via email to the specified user based on their favorite shows.
    /// </summary>
    /// <param name="user">The user to send recommendations to.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task sendEmailRecomendations(ApplicationUser user)
    {
        using var db = context.CreateDbContext();

        // Fetch favorites
        var favourites = await db.FavoriteTvShows
            .Where(f => f.UserId == user.Id)
            .Include(f => f.TvShow)
            .ToListAsync();

        if (!favourites.Any())
            return;

        var rng = new Random();
        var randomFavorites = favourites
            .OrderBy(f => rng.Next())
            .Take(5)
            .Select(f => f.TvShow)
            .ToList();
        
        var recommendations = getSimilarShows(randomFavorites, 10);
        var htmlBody = $@"
            <h2>We picked 10 of your favorites!</h2>
            <p>Here are your recommendations:</p>
            <ul>
                {string.Join("<br>", recommendations.Select(r => $"<li>{r.Item1.Name} (Score: {r.Score:F2})</li>"))}
            </ul>";

        var body = "We picked 10 of your favorites, and here are your recommendations:\n" +
                   string.Join("\n", recommendations.Select(r => $"{r.Item1.Name} (Score: {r.Score:F2})"));

        if (!string.IsNullOrEmpty(user.Email))
            await _emailService.SendEmailAsync(user.Email, "Your TV Show Recommendations", body,htmlBody);
    }

    /// <summary>
    /// Returns a list of TV shows similar to the input list based on feature vectors.
    /// </summary>
    /// <param name="input">A list of TV shows to base recommendations on.</param>
    /// <param name="n">The number of recommendations to return.</param>
    /// <returns>A list of tuples containing the recommended TV show and its similarity score.</returns>
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

        return similarities.OrderByDescending(s => s.Score).Take(n).ToList();
    }

    /// <summary>
    /// Computes the cosine similarity between two sparse vectors.
    /// </summary>
    /// <param name="v1">The first sparse vector.</param>
    /// <param name="v2">The second sparse vector.</param>
    /// <returns>The cosine similarity score between 0 and 1.</returns>
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
                dot += kvp.Value * val2;

            normA += kvp.Value * kvp.Value;
        }

        foreach (var val in dict2.Values)
            normB += val * val;

        return (normA == 0 || normB == 0) ? 0 : dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
    }

    /// <summary>
    /// Computes the average sparse vector from a list of TV shows using their feature vectors.
    /// </summary>
    /// <param name="input">The input list of TV shows.</param>
    /// <param name="lista">The list of all TV show features in the database.</param>
    /// <returns>The average <see cref="SparseVector"/> representing the input TV shows.</returns>
    private static SparseVector GetAverageSparseVector(List<TvShow> input, List<TvShowFeatures> lista)
    {
        var filtered = lista.Where(l => input.Any(tv => tv.Id == l.TvShowId)).ToList();
        if (!filtered.Any())
            return new SparseVector();

        var sumDict = new Dictionary<int, double>();

        foreach (var f in filtered)
        {
            var vector = JsonSerializer.Deserialize<SparseVector>(f.CombinedVectorJson);
            if (vector == null) continue;

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
