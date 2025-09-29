using TvShowTracker.Api.Data;

class PersonSeeder
{
    private static readonly Random _random = new Random();
    public static void SeedPeople(ApplicationDbContext context)
    {
        var people = context.TvShows.ToList();

        foreach (var person in people)
        {
            person.ImageUrl = $"/images/TvShows/image{_random.Next(1,8)}.jpg";
/*             person.BirthDate = RandomBirthDate();
                        person.Bio = $"This is a detailed fabricated bio for {person.Name}. " +
                         "They were born in a small town and went on to have a remarkable career in television. " +
                         "Known for their charisma and dedication, they have contributed to numerous successful shows. " +
                         "Outside of acting, they enjoy writing, traveling, and mentoring new talent. " +
                         "Their journey continues to inspire fans around the world.";
                        person.ProfileImageUrl = $"/images/person/person{_random.Next(1,3)}.jpg";
             */
            // WorkedOn connections remain untouched
        }

        context.SaveChanges();

        // Helper for random birthdate
        DateTime RandomBirthDate()
        {
            int year = _random.Next(1950, 2005);
            int month = _random.Next(1, 13);
            int day = _random.Next(1, 29); // Simple, avoids month-length issues
            return new DateTime(year, month, day);
        }
    }


}
