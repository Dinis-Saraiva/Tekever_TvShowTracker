using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using Microsoft.Extensions.Caching.Memory;


namespace TvShowTracker.Api.GraphQL
{

    public class Query
    {
        private readonly IMemoryCache _cache;

        public Query(IMemoryCache cache)
        {
            _cache = cache;
        }
        [UsePaging(DefaultPageSize = 10, MaxPageSize = 15)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TvShow> GetTvShows([Service] ApplicationDbContext db)
        {
            const string cacheKey = "AllTvShows";
            var cachedList = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return db.TvShows.Include(t => t.TvShowGenres)
                .ThenInclude(tg => tg.Genre)
                .ToList();
            });
            return cachedList.AsQueryable();
        }

        [UsePaging(DefaultPageSize = 10, MaxPageSize = 15)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Person> GetPeople([Service] ApplicationDbContext db)
        {
            const string cacheKey = "AllPeople";
            var cachedList = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return db.Person.ToList();
            });
            return cachedList.AsQueryable();
        }

        [UsePaging(DefaultPageSize = 15, MaxPageSize = 50)]
        public IQueryable<Episode> GetEpisodesByTvShowId(int tvShowId, [Service] ApplicationDbContext db)
            => db.Episodes.Where(e => e.TvShowId == tvShowId);

        public Task<Person?> GetPersonByIdAsync(int id, [Service] ApplicationDbContext db)
        {
            return db.Person
                     .Include(p => p.WorkedOn)
                     .ThenInclude(w => w.TvShow)
                     .FirstOrDefaultAsync(p => p.Id == id);
        }

        [UsePaging(DefaultPageSize = 10, MaxPageSize = 15)]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TvShow> GetTvShowsByCastId(int personId, [Service] ApplicationDbContext db)
            => db.TvShows
                .Where(t => t.WorkedOn.Any(w => w.PersonId == personId));
    }

}

