using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // cookie settings
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // TTL = 1 hour
    options.SlidingExpiration = true;                  // refresh on activity
    options.LoginPath = "/api/auth/login";             // not used in API, but must be set
    options.Cookie.HttpOnly = true;                    // cookie not accessible via JS
    options.Cookie.SameSite = SameSiteMode.Strict;     // prevent CSRF
});

builder.Services.AddControllers();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();


