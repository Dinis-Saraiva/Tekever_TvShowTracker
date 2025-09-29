using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

public class FavouriteByTvShowIdDataLoader : BatchDataLoader<int, List<FavoriteTvShows>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public FavouriteByTvShowIdDataLoader(
        IBatchScheduler batchScheduler,
        IDbContextFactory<ApplicationDbContext> dbFactory,
    DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbFactory = dbFactory;
    }

    protected override async Task<IReadOnlyDictionary<int, List<FavoriteTvShows>>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        await using var db = _dbFactory.CreateDbContext();

        var favourite = await db.FavoriteTvShows
            .Where(w => keys.Contains(w.TvShowId))
            .ToListAsync(cancellationToken);

        return favourite.GroupBy(tg => tg.TvShowId)
            .ToDictionary(
                g => g.Key,g =>g.ToList());
    }
}
