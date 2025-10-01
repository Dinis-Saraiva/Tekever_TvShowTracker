using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

/// <summary>
/// A background worker service that periodically generates and sends TV show recommendations to users via email.
/// </summary>
public class RecommendationWorker : BackgroundService
{
    private readonly IServiceProvider _services;

    /// <summary>
    /// Initializes a new instance of the <see cref="RecommendationWorker"/> class.
    /// </summary>
    /// <param name="services">The service provider used to create scoped services.</param>
    public RecommendationWorker(IServiceProvider services)
    {
        _services = services;
    }

    /// <summary>
    /// Executes the background service logic, periodically sending recommendations to all users.
    /// </summary>
    /// <param name="stoppingToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the background execution.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"RecommendationWorker running at: {DateTimeOffset.Now}");

            using var scope = _services.CreateScope();
            var recommendationService = scope.ServiceProvider.GetRequiredService<RecommendationService>();
            var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            using var db = dbFactory.CreateDbContext();

            var users = await db.Users.ToListAsync(stoppingToken);

            foreach (var user in users)
            {
                await recommendationService.sendEmailRecomendations(user);
            }

            await Task.Delay(TimeSpan.FromHours(30), stoppingToken);
        }
    }
}
