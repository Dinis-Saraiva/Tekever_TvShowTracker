using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;


[ApiController]
[Route("api/[controller]")]

public class FavoritesController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public FavoritesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFavoriteTvShows(
       [FromQuery] int pageNumber = 1,
       [FromQuery] int pageSize = 10)
    {
        var userId = _userManager.GetUserId(User);

        var query = _context.FavoriteTvShows
            .Where(f => f.User.Id == userId)
            .Include(f => f.TvShow)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var favoriteTvShows = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PaginatedFavoritesDto
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            Items = favoriteTvShows
        };

        return Ok(result);
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddFavoriteTvShow([FromQuery] int tvShowIds)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var tvShow = await _context.TvShows.FirstOrDefaultAsync(t => t.Id == tvShowIds);
        if (tvShow == null)
            return NotFound(new { message = "TV show not found" });
        _context.FavoriteTvShows.Add(new FavoriteTvShows { User = user, TvShow = tvShow });
        await _context.SaveChangesAsync();
        return Ok(new { message = $"{tvShow} TV show added to favorites" });
    }


    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveFavoriteTvShows([FromQuery]int tvShowId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();


        var removeFavorite = await _context.FavoriteTvShows.FirstOrDefaultAsync(f => f.TvShowId == tvShowId && f.UserId == user.Id);
        if (removeFavorite == null) {
            return NotFound(new { message = "Favorite TV show not found" }); }


        _context.FavoriteTvShows.Remove(removeFavorite);
        await _context.SaveChangesAsync();

        return Ok(new { message =  $"{tvShowId} TV show removed from favorites" });
    }

}