using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Api.Models;
using System.Threading.Tasks;

/// <summary>
/// Controller for managing TV show recommendations and sending recommendation emails.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly RecommendationService _recommendationService;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="RecommendationsController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager used to retrieve the current user.</param>
    /// <param name="recommendationService">The service responsible for sending recommendations.</param>
    public RecommendationsController(
        UserManager<ApplicationUser> userManager,
        RecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
        _userManager = userManager;
    }

    /// <summary>
    /// Sends personalized TV show recommendations to the currently authenticated user via email.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> indicating success or failure of sending recommendations.</returns>
    [Authorize]
    [HttpPost("send")]
    public async Task<IActionResult> SendRecommendations()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        try
        {
            await _recommendationService.sendEmailRecomendations(user);
            return Ok("Recommendations email sent!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Failed to send recommendations email: {ex.Message}");
        }
    }

}
