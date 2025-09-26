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
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.FavoriteTvShows)
                .WithMany(t => t.UsersWhoFavourited)
                .UsingEntity(j => j.ToTable("UserFavoriteTvShows"));
        }
    }

}
