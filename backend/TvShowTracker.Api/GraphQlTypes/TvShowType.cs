using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// GraphQL type definition for the <see cref="TvShow"/> model, including custom fields and resolvers.
/// </summary>
public class TvShowType : ObjectType<TvShow>
{
    /// <summary>
    /// Configures the GraphQL type, adding custom fields, filtering, sorting, and resolvers.
    /// </summary>
    /// <param name="descriptor">The descriptor used to configure the GraphQL object type.</param>
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
