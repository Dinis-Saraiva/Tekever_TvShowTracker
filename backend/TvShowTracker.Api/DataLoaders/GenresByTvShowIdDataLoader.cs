using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

public class GenresByTvShowIdDataLoader : BatchDataLoader<int, List<Genre>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public GenresByTvShowIdDataLoader(
    IBatchScheduler batchScheduler,
    IDbContextFactory<ApplicationDbContext> dbFactory,
    DataLoaderOptions options)
    : base(batchScheduler, options)
    {
        _dbFactory = dbFactory;
    }

    protected override async Task<IReadOnlyDictionary<int, List<Genre>>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        await using var db = _dbFactory.CreateDbContext();

        var tvShowGenres = await db.TvShowGenres
         .Where(tg => keys.Contains(tg.TvShowId))
         .Include(tg => tg.Genre)
         .ToListAsync(cancellationToken);

        return tvShowGenres
            .GroupBy(tg => tg.TvShowId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(tg => tg.Genre).Where(genre => genre != null).ToList()
            );


    }
}
