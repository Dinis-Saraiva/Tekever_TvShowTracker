/// <summary>
/// Represents a simple person with an identifier and name.
/// </summary>
public class SimplePersonDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the person.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the person.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
