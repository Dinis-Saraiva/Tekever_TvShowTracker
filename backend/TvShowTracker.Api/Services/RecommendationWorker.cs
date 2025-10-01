using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

public class RecommendationWorker : BackgroundService
{
    /*  private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
     private readonly RecommendationService _recommendationService; */
    private readonly IServiceProvider _services;

    public RecommendationWorker(
        /* IDbContextFactory<ApplicationDbContext> dbContextFactory,
        RecommendationService recommendationService, */
        IServiceProvider services

        )
    {
        /*  _dbContextFactory = dbContextFactory;
         _recommendationService = recommendationService; */
        _services = services;

    }

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

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }



}
