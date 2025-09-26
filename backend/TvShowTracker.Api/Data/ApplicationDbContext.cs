using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Models;

namespace TvShowTracker.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TvShow> TvShows { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Episode> Episodes { get; set; }
    }
}