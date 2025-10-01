using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Api.Data;
using TvShowTracker.Api.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Controller for managing TV show-related operations such as retrieving details and exporting PDFs.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TvShowController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TvShowController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager used to retrieve user information.</param>
    /// <param name="context">The database context for accessing TV show data.</param>
    public TvShowController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    /// <summary>
    /// Retrieves a TV show by its ID.
    /// </summary>
    /// <param name="tvShowId">The unique identifier of the TV show.</param>
    /// <returns>An <see cref="IActionResult"/> containing the TV show data or a 404 if not found.</returns>
    [HttpGet("GetTvShowByID/{tvShowId}")]
    public async Task<IActionResult> GetTvShowByID([FromRoute] int tvShowId)
    {
        var user = await _userManager.GetUserAsync(User);
        var dto = await BuildTvShowDto(tvShowId, user);
        if (dto == null) return NotFound(new { message = "TV show not found" });

        return Ok(dto);
    }

    /// <summary>
    /// Exports a TV show as a PDF document by its ID.
    /// </summary>
    /// <param name="tvShowId">The unique identifier of the TV show.</param>
    /// <returns>An <see cref="IActionResult"/> containing the PDF file or a 404 if not found.</returns>
    [HttpGet("{tvShowId}/export-pdf")]
    public async Task<IActionResult> ExportTvShowPdf([FromRoute] int tvShowId)
    {
        var user = await _userManager.GetUserAsync(User);
        var dto = await BuildTvShowDto(tvShowId, user);
        if (dto == null) return NotFound(new { message = "TV show not found" });

        var pdfBytes = PdfGenerator.GenerateTvShowPdf(dto);

        return File(pdfBytes, "application/pdf", $"{dto.Name}.pdf");
    }

    /// <summary>
    /// Builds a <see cref="TvShowDto"/> from the database for a specific TV show and user.
    /// </summary>
    /// <param name="tvShowId">The ID of the TV show to build.</param>
    /// <param name="user">The current authenticated user, if any.</param>
    /// <returns>A <see cref="TvShowDto"/> with details, cast, directors, genres, and favorite status, or null if not found.</returns>
    private async Task<TvShowDto?> BuildTvShowDto(int tvShowId, ApplicationUser? user)
    {
        var tvShow = await _context.TvShows.FirstOrDefaultAsync(t => t.Id == tvShowId);
        if (tvShow == null) return null;

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

        var isFavorite = false;
        if (user != null)
        {
            isFavorite = await _context.FavoriteTvShows.AnyAsync(f => f.TvShowId == tvShowId && f.UserId == user.Id);
        }

        return new TvShowDto
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
    }
}
