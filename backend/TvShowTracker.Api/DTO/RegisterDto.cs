/// <summary>
/// Data transfer object used for user registration requests.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Gets or sets the username for the new user.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email address for the new user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password for the new user.
    /// </summary>
    public string Password { get; set; } = null!;
}
