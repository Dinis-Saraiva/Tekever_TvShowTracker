using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Models;

namespace TvShowTracker.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<TvShow> TvShows { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<WorkedOn> WorkedOns { get; set; }
        public DbSet<TvShowGenre> TvShowGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<FavoriteTvShows> FavoriteTvShows { get; set; }

    }
}
