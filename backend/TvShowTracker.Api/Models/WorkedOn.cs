namespace TvShowTracker.Api.Models
{

    public enum JobTitle
    {
        Actor,
        Director,
    }
    public class WorkedOn
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int TvShowId { get; set; }

        public required Person Person { get; set; }
        public required TvShow TvShow { get; set; }
        public required JobTitle Role { get; set; }
    }
}