/* using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

public class TvShowRecomendation
{
    public static List<(TvShow, double Score)> getSimilarShows(List<TvShow> input, ApplicationDbContext context)
    {
        var similarities = new List<(TvShow, double Score)>();
        var lista = context.TvShowFeatures;
        var target = input.JsonSerializer.Deserialize<float[]>(json) 

        foreach (var l in lista)
        {
            if ()
        }
        return new List<(TvShow, double Score)>();
    }

    double CosineSimilarity(float[] vec1, float[] vec2)
    {
        double dot = 0.0, normA = 0.0, normB = 0.0;
        for (int i = 0; i < vec1.Length; i++)
        {
            dot += vec1[i] * vec2[i];
            normA += vec1[i] * vec1[i];
            normB += vec2[i] * vec2[i];
        }
        return dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
    }
} */