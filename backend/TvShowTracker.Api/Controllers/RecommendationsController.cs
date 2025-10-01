using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly RecommendationService _recommendationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecommendationsController(
        UserManager<ApplicationUser> userManager,
        RecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("send")]
    public async Task<IActionResult> SendRecommendations()
    {
        var user = await _userManager.GetUserAsync(User);
        if(user==null)
            return Unauthorized();
            
        await _recommendationService.sendEmailRecomendations(user);

        return Ok(new { message = "Recommendations email sent!" });
    }
}
