namespace TvShowTracker.Api.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Bio { get; set; }
        public List<WorkedOn> WorkedOn { get; set; } = new List<WorkedOn>();
        public string? ProfileImageUrl { get; set; }
    }
}