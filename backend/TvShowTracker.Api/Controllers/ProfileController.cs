using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;


[ApiController]
[Route("api/[controller]")]

public class ProfileController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [Authorize]
    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavoriteTvShows()
    {
        var userId = _userManager.GetUserId(User);

        var favoriteTvShows = await _context.Users
    .Where(u => u.Id == userId)
    .SelectMany(u => u.FavoriteTvShows)
    .Include(tv => tv.Cast)
    .ToListAsync();

        var favoriteTvShowDtos = favoriteTvShows.Select(tv => new TvShowDto
        {
            Id = tv.Id,
            Name = tv.Name,
            Description = tv.Description,
            ReleaseDate = tv.ReleaseDate,
            Genre = tv.Genre,
            ImageUrl = tv.ImageUrl,
            rating = tv.Rating,
            NumberOfSeasons = tv.Seasons,
            Cast = tv.Cast.Select(a => new ActorDto { Id = a.Id, Name = a.Name }).ToList()
        }).ToList();

        return Ok(favoriteTvShowDtos);
    }


    [Authorize]
    [HttpPost("favorites/{tvShowId}")]
    [Authorize]
    [HttpPost("favorites")]
    public async Task<IActionResult> AddFavoriteTvShows([FromBody] List<int> tvShowIds)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        foreach (var tvShowId in tvShowIds)
        {
            var tvShow = await _context.TvShows.FindAsync(tvShowId);
            if (tvShow != null)
            {
                user.FavoriteTvShows.Add(tvShow);
            }
        }

        await _userManager.UpdateAsync(user);

        return Ok(new { message = $"{tvShowIds.Count} TV show(s) added to favorites" });
    }

    [Authorize]
    [HttpDelete("favorites")]
    public async Task<IActionResult> RemoveFavoriteTvShows([FromBody] List<int> tvShowIds)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        foreach (var tvShowId in tvShowIds)
        {
            var tvShow = user.FavoriteTvShows.FirstOrDefault(t => t.Id == tvShowId);
            if (tvShow != null)
                user.FavoriteTvShows.Remove(tvShow);
        }

        await _userManager.UpdateAsync(user);

        return Ok(new { message = $"{tvShowIds.Count} TV show(s) removed from favorites" });
    }
}