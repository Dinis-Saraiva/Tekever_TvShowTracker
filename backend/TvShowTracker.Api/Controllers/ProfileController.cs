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

        var favoriteTvShows = await _context.FavoriteTvShows
            .Where(f => f.User.Id == userId)
            .Include(f => f.TvShow)
            .ToListAsync();

        return Ok(favoriteTvShows);
    }


[Authorize]
[HttpPost("favorites")]
public async Task<IActionResult> AddFavoriteTvShows([FromBody] List<int> tvShowIds)
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null)
        return Unauthorized();

    var tvShows = await _context.TvShows
        .Where(t => tvShowIds.Contains(t.Id))
        .ToListAsync();

    foreach (var tvShow in tvShows){
        _context.FavoriteTvShows.Add(new FavoriteTvShows
            {User = user,TvShow = tvShow});
        }

    await _context.SaveChangesAsync();
    return Ok(new { message = $"{tvShows.Count} TV show(s) added to favorites" });
}


   [Authorize]
[HttpDelete("favorites")]
public async Task<IActionResult> RemoveFavoriteTvShows([FromBody] List<int> tvShowIds)
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null)
        return Unauthorized();

    var favorites = await _context.FavoriteTvShows
        .Where(f => f.User == user && tvShowIds.Contains(f.TvShow.Id))
        .ToListAsync();

    if (favorites.Count == 0)
        return NotFound(new { message = "No favorites found to remove" });

    _context.FavoriteTvShows.RemoveRange(favorites);
    await _context.SaveChangesAsync();

    return Ok(new { message = $"{favorites.Count} TV show(s) removed from favorites" });
}

}