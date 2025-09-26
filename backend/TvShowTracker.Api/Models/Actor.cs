namespace TvShowTracker.Api.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string Bio { get; set; } = string.Empty;
        public ICollection<TvShow> TvShows { get; set; } = new List<TvShow>();
    }
}