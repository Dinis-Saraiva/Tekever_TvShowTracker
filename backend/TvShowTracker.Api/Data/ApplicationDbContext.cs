using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Models;

namespace TvShowTracker.Api.Data
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the TV Show Tracker application,
    /// including Identity tables and application-specific tables for TV shows, people, episodes, genres, and favorites.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to configure the database context.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        /// <summary>
        /// Gets or sets the TV shows in the database.
        /// </summary>
        public DbSet<TvShow> TvShows { get; set; }

        /// <summary>
        /// Gets or sets the people in the database.
        /// </summary>
        public DbSet<Person> Person { get; set; }

        /// <summary>
        /// Gets or sets the episodes in the database.
        /// </summary>
        public DbSet<Episode> Episodes { get; set; }

        /// <summary>
        /// Gets or sets the records linking people to TV shows they worked on.
        /// </summary>
        public DbSet<WorkedOn> WorkedOns { get; set; }

        /// <summary>
        /// Gets or sets the associations between TV shows and genres.
        /// </summary>
        public DbSet<TvShowGenre> TvShowGenres { get; set; }

        /// <summary>
        /// Gets or sets the genres in the database.
        /// </summary>
        public DbSet<Genre> Genres { get; set; }

        /// <summary>
        /// Gets or sets the features of TV shows in the database.
        /// </summary>
        public DbSet<TvShowFeatures> TvShowFeatures { get; set; }

        /// <summary>
        /// Gets or sets the user's favorite TV shows in the database.
        /// </summary>
        public DbSet<FavoriteTvShows> FavoriteTvShows { get; set; }
    }
}
