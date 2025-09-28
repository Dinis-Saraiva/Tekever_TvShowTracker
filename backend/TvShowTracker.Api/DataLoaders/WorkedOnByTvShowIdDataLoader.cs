using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

public class WorkedOnByTvShowIdDataLoader : BatchDataLoader<int, List<WorkedOn>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public WorkedOnByTvShowIdDataLoader(
        IBatchScheduler batchScheduler,
        IDbContextFactory<ApplicationDbContext> dbFactory,
    DataLoaderOptions options)
        : base(batchScheduler, options)
    {
        _dbFactory = dbFactory;
    }

    protected override async Task<IReadOnlyDictionary<int, List<WorkedOn>>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        await using var db = _dbFactory.CreateDbContext();

        var workedOn = await db.WorkedOns
            .Where(w => keys.Contains(w.TvShowId))
            .Include(w => w.Person)
            .ToListAsync(cancellationToken);

        return workedOn
            .GroupBy(w => w.TvShowId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}
