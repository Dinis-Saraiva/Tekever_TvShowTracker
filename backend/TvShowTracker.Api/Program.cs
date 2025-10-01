using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.GraphQL;
using TvShowTracker.Api.Models;

// Load .env file from project root
DotNetEnv.Env.Load(System.IO.Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\.env"));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Database
var defaultConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
                        ?? throw new InvalidOperationException("Missing DEFAULT_CONNECTION in .env");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(defaultConnection));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();

// Configure cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/api/auth/login";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// CORS
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:3000";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Controllers and GraphQL
builder.Services.AddControllers();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddDataLoader<GenresByTvShowIdDataLoader>()
    .AddType<TvShowType>()
    .AddType<ObjectType<Genre>>()
    .ModifyCostOptions(opt => opt.MaxFieldCost = 4000)
    .AddFiltering()
    .AddSorting()
    .AddProjections();

// EmailService using SendGrid
builder.Services.AddScoped<IEmailService>(sp =>
{
    var sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
        ?? throw new InvalidOperationException("Missing SENDGRID_API_KEY in .env");
    var fromEmail = Environment.GetEnvironmentVariable("FROM_EMAIL")
        ?? throw new InvalidOperationException("Missing FROM_EMAIL in .env");
    var fromName = Environment.GetEnvironmentVariable("FROM_NAME") ?? "No-Reply";

    return new EmailService(sendGridApiKey, fromEmail, fromName);
});

// Recommendation service
builder.Services.AddScoped<RecommendationService>();
builder.Services.AddHostedService<RecommendationWorker>();

var app = builder.Build();

// OpenAPI in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseHttpsRedirection();

// Endpoints
app.MapControllers();
app.MapGraphQL();

app.Run();

public partial class Program { }
