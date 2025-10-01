using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

public class TvShowType : ObjectType<TvShow>
{
    protected override void Configure(IObjectTypeDescriptor<TvShow> descriptor)
    {

        descriptor.Field("genres")
        .UseFiltering()
        .UseSorting()
        .Type<ListType<ObjectType<Genre>>>()
        .Resolve(async (ctx, ct) =>
        {
            var tvShow = ctx.Parent<TvShow>();
            var loader = ctx.DataLoader<GenresByTvShowIdDataLoader>();
            var genres = await loader.LoadAsync(tvShow.Id, ct);
            return genres ?? new List<Genre>();
        });
    }
}
