using HotChocolate;
using HotChocolate.Data;                 // For UseDbContext & ScopedService
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

namespace TvShowTracker.Api.GraphQL
{
    public class Query
    {
        //[UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TvShow> GetTvShows([Service] ApplicationDbContext context)
            => context.TvShows;

        //[UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Actor> GetActors([Service] ApplicationDbContext context)
            => context.Actors;
    }
}

