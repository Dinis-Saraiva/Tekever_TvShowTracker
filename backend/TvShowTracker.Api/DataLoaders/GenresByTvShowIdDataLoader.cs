using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// DataLoader that batches and caches requests for genres associated with TV shows by their IDs.
/// </summary>
public class GenresByTvShowIdDataLoader : BatchDataLoader<int, List<Genre>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenresByTvShowIdDataLoader"/> class.
    /// </summary>
    /// <param name="batchScheduler">The batch scheduler used to schedule batched loads.</param>
    /// <param name="dbFactory">The database context factory for creating <see cref="ApplicationDbContext"/> instances.</param>
    /// <param name="options">Options to configure the DataLoader behavior.</param>
    public GenresByTvShowIdDataLoader(
        IBatchScheduler batchScheduler,
        IDbContextFactory<ApplicationDbContext> dbFactory,
        DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbFactory = dbFactory;
    }

    /// <summary>
    /// Loads a batch of genres for the specified TV show IDs.
    /// </summary>
    /// <param name="keys">The list of TV show IDs to fetch genres for.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A read-only dictionary mapping each TV show ID to its corresponding list of <see cref="Genre"/> objects.
    /// </returns>
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
