using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;


[ApiController]
[Route("api/[controller]")]

public class TvShowController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public TvShowController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet("GetTvShowByID")]
    public async Task<IActionResult> GetTvShowByID([FromQuery] int tvShowId)
    {
        var tvShow = await _context.TvShows.FirstOrDefaultAsync(t => t.Id == tvShowId);
        if (tvShow == null)
            return NotFound(new { message = "TV show not found" });

        var workedOn = await _context.WorkedOns
            .Include(w => w.Person)
            .Where(w => w.TvShowId == tvShowId)
            .ToListAsync();

        var cast = workedOn
            .Where(w => w.Role == JobTitle.Actor)
            .Select(w => new SimplePersonDto { Id = w.Person.Id, Name = w.Person.Name })
            .ToList();

        var directors = workedOn
            .Where(w => w.Role == JobTitle.Director)
            .Select(w => new SimplePersonDto { Id = w.Person.Id, Name = w.Person.Name })
            .ToList();

        var tvShowGenres = await _context.TvShowGenres
            .Include(tg => tg.Genre)
            .Where(tg => tg.TvShowId == tvShowId)
            .ToListAsync();
        var genres = tvShowGenres.Select(tg => tg.Genre).ToList();
        var user = await _userManager.GetUserAsync(User);
        var isFavorite = false;
        if (user != null)
        {
            isFavorite = await _context.FavoriteTvShows.AnyAsync(f => f.TvShowId == tvShowId && f.UserId == user.Id);
        }
        var tvShowDto = new TvShowDto
        {
            Id = tvShow.Id,
            Name = tvShow.Name,
            Description = tvShow.Description,
            ReleaseDate = tvShow.ReleaseDate,
            Seasons = tvShow.Seasons,
            ImageUrl = tvShow.ImageUrl,
            Origin = tvShow.Origin,
            Rating = tvShow.Rating.ToString(),
            Cast = cast,
            Directors = directors,
            Genres = genres,
            IsFavorite = isFavorite,
        };
        return Ok(tvShowDto);
    }
}