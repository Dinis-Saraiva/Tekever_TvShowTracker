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
    .UsePaging()
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

        descriptor.Field("actors")
        .Type<ListType<NonNullType<ObjectType<Person>>>>()
        .UsePaging()
        .Resolve(async (ctx,ct) =>
        {
            var tvShow = ctx.Parent<TvShow>();
            var loader = ctx.DataLoader<WorkedOnByTvShowIdDataLoader>();
            var workedOnList = await loader.LoadAsync(tvShow.Id, ct);
            workedOnList = workedOnList ?? new List<WorkedOn>();
            return workedOnList
                .Where(w => w.Role == JobTitle.Actor)
                .Select(w => w.Person) ?? new List<Person>();
        });



        /*         descriptor.Field("isFavourite")
                    .Type<NonNullType<BooleanType>>()
                    .Resolve(async (ctx, ct) =>
                    {
                        var userId = ctx.ContextData.ContainsKey("UserId")
                            ? ctx.ContextData["UserId"] as string
                            : null;

                        if (userId == null) return false;

                        var db = ctx.Service<ApplicationDbContext>();
                        var tvShow = ctx.Parent<TvShow>();

                        return await db.FavoriteTvShows
                            .AnyAsync(f => f.User.Id == userId && f.TvShow.Id == tvShow.Id, ct);
                    }); */




        descriptor.Field(t => t.WorkedOn)
           .Type<ListType<NonNullType<ObjectType<WorkedOn>>>>();



        descriptor.Field("directors")
            .Type<ListType<NonNullType<ObjectType<Person>>>>()
            .UsePaging()
            .Resolve(async ctx =>
            {
                var loader = ctx.DataLoader<WorkedOnByTvShowIdDataLoader>();
                var tvShow = ctx.Parent<TvShow>();
                var workedOnList = await loader.LoadAsync(tvShow.Id, ctx.RequestAborted);
                workedOnList = workedOnList ?? new List<WorkedOn>();
                return workedOnList
                    .Where(w => w.Role == JobTitle.Director)
                    .Select(w => w.Person) ?? new List<Person>();
            });

    }
}
