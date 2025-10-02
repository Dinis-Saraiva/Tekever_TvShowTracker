using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Api.Models;
using System.Threading.Tasks;

/// <summary>
/// Controller for handling user authentication operations such as registration, login, logout, and retrieving the current user.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager used for creating and finding users.</param>
    /// <param name="signInManager">The sign-in manager used for handling login and logout.</param>
    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Registers a new user with the provided information.
    /// </summary>
    /// <param name="dto">The registration data including username, email, and password.</param>
    /// <returns>An <see cref="IActionResult"/> indicating success or failure of registration.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Username, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);


        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _signInManager.SignInAsync(user, isPersistent: false);

        return Ok(new { user = new { id = user.Id, username = user.UserName, email = user.Email } });
    }

    /// <summary>
    /// Logs in a user using the provided username and password.
    /// </summary>
    /// <param name="dto">The login data including username and password.</param>
    /// <returns>An <see cref="IActionResult"/> containing the user info if login succeeds, or an error if it fails.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(
            dto.Username, dto.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Unauthorized("Invalid username or password");

        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null)
            return Unauthorized("Invalid user");

        return Ok(new { user = new { id = user.Id, username = user.UserName, email = user.Email } });
    }

    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> indicating successful logout.</returns>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out" });
    }

    /// <summary>
    /// Retrieves the currently authenticated user's information.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the user's details or an error if no user is logged in.</returns>
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

        return Ok(new { user = new { id = user.Id, username = user.UserName, email = user.Email } });
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating success or failure.</returns>
    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUser()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Unauthorized("No user logged in");
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
                return NotFound(new { message = "User not found" });
        // Delete the user
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User deleted successfully" });
    }

}
