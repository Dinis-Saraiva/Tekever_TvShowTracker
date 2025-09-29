using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

namespace TvShowTracker.Api.GraphQL
{
    public class Query
    {
        [UsePaging(DefaultPageSize = 10, MaxPageSize = 15)]
        [UseFiltering]
        [UseSorting]
       
        public IQueryable<TvShow> GetTvShows([Service] ApplicationDbContext db)
            => db.TvShows;


        public Task<TvShow?> GetTvShowByIdAsync(int id, [Service] ApplicationDbContext db)
            => db.TvShows.FirstOrDefaultAsync(t => t.Id == id);

        [UsePaging(DefaultPageSize = 15, MaxPageSize = 50)]
        public IQueryable<Episode> GetEpisodesByTvShowId(int tvShowId, [Service] ApplicationDbContext db)
            => db.Episodes.Where(e => e.TvShowId == tvShowId);

        public Task<Person?> GetPersonByIdAsync(int id, [Service] ApplicationDbContext db)
            => db.Person.FirstOrDefaultAsync(p => p.Id == id);
            
        [UsePaging(DefaultPageSize = 10, MaxPageSize = 15)]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TvShow> GetTvShowsByCastId(int personId, [Service] ApplicationDbContext db)
            => db.TvShows
                .Where(t => t.WorkedOn.Any(w => w.PersonId == personId));
    }

}

