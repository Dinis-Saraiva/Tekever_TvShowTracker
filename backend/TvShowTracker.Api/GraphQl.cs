using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

namespace TvShowTracker.Api.GraphQL
{
    public class Query
    {
        [UsePaging(
    DefaultPageSize = 10,
    MaxPageSize = 50,
    IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TvShow> GetTvShows([Service] ApplicationDbContext context)
            => context.TvShows;
    }
}

