using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Controller for managing user's favorite TV shows, including retrieval, addition, and removal of favorites.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="FavoritesController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager used to retrieve the current user.</param>
    /// <param name="context">The database context for accessing TV show and favorites data.</param>
    public FavoritesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    /// <summary>
    /// Retrieves the currently authenticated user's favorite TV shows with pagination.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <returns>An <see cref="IActionResult"/> containing a paginated list of favorite TV shows.</returns>
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

    /// <summary>
    /// Adds a TV show to the currently authenticated user's list of favorites.
    /// </summary>
    /// <param name="tvShowIds">The ID of the TV show to add to favorites.</param>
    /// <returns>An <see cref="IActionResult"/> indicating success or failure.</returns>
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

    /// <summary>
    /// Removes a TV show from the currently authenticated user's list of favorites.
    /// </summary>
    /// <param name="tvShowId">The ID of the TV show to remove from favorites.</param>
    /// <returns>An <see cref="IActionResult"/> indicating success or failure.</returns>
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveFavoriteTvShows([FromQuery] int tvShowId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var removeFavorite = await _context.FavoriteTvShows
            .FirstOrDefaultAsync(f => f.TvShowId == tvShowId && f.UserId == user.Id);

        if (removeFavorite == null)
        {
            return NotFound(new { message = "Favorite TV show not found" });
        }

        _context.FavoriteTvShows.Remove(removeFavorite);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"{tvShowId} TV show removed from favorites" });
    }
}
