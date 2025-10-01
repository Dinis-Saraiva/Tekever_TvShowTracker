using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.GraphQL;
using TvShowTracker.Api.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
/* builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); */

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddMemoryCache();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/api/auth/login";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // your frontend dev server
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});



builder.Services.AddControllers();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddDataLoader<GenresByTvShowIdDataLoader>()
    .AddType<TvShowType>()
    .AddType<ObjectType<Genre>>()
    .ModifyCostOptions(opt =>
    {
        opt.MaxFieldCost = 4000;
    })
    .AddFiltering()
    .AddSorting()
    .AddProjections();
    

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<RecommendationService>();
builder.Services.AddHostedService<RecommendationWorker>();

var app = builder.Build();
// Configure the HTTP request pipeline.CsvImporter.ImportTvShows("data.csv", db);
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

/* using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    TvShowVectorCalculator.CalculateVectors(db);
    CsvImporter.ImportTvShows("tvshows.csv", db);
 
} */

/*  using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Make sure the database is created
    db.Database.EnsureCreated();

    // Seed episodes
    PersonSeeder.SeedPeople(db);
}
  */

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.MapControllers();
app.MapGraphQL();
app.Run();

public partial class Program { }

