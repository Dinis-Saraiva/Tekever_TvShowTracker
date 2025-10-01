/// <summary>
/// Data transfer object used for user login requests.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Gets or sets the username of the user attempting to log in.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password of the user attempting to log in.
    /// </summary>
    public string Password { get; set; } = null!;
}
