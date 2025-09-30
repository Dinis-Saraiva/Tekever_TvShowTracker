using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TvShowTracker.Api.Data;

namespace TvShowTracker.Api.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext registration (SQLite)
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using InMemory for tests
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Build a service provider to create the DB
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
