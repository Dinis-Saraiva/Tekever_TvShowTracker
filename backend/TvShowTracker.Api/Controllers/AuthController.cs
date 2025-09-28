using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Username, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(
            dto.Username, dto.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Unauthorized("Invalid username or password");
        
        var user = await _userManager.FindByNameAsync(dto.Username);
        return Ok(new { user = dto.Username , email = user.Email, id=user.Id });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out" });
    }

[Authorize]
[HttpGet("current-user")]
public async Task<IActionResult> GetCurrentUser()
{
    var username = User.Identity?.Name;

    if (string.IsNullOrEmpty(username))
        return Unauthorized("No user logged in");

    var user = await _userManager.FindByNameAsync(username);

    if (user == null)
        return NotFound("User not found");

    return Ok(new {user = new { id = user.Id, username = user.UserName, email = user.Email}});
}

}

