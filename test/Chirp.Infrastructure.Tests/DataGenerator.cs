namespace Chirp.Infrastructure.Tests;

public static class DataGenerator
{
    public static AuthorDTO GenerateAuthorDTO(int seed)
    {
        Randomizer.Seed = new Random(seed);

        return new Faker<AuthorDTO>().CustomInstantiator(a =>
            new AuthorDTO
            (
                a.Name.FullName(),
                a.Internet.Email(a.Name.FirstName(), a.Name.LastName())
            )
        ).Generate();
    }

    public static Author GenerateAuthor(int seed)
    {
        Randomizer.Seed = new Random(seed);

        return new Faker<Author>().CustomInstantiator(a =>
            new Author()
            {
                Name = a.Name.FullName(),
                Email = a.Internet.Email(a.Name.FirstName(), a.Name.LastName()),
                Cheeps = new List<Cheep>()
            }
        ).Generate();
    }

    public static CheepDTO GenerateCheepDTO(int seed, AuthorDTO authorDTO)
    {
        Randomizer.Seed = new Random(seed);

        return new Faker<CheepDTO>().CustomInstantiator(a =>
            new CheepDTO
            (
                authorDTO.Name,
                a.Lorem.Sentence(5),
                GetTimeStamp(a.Random.Double(0, 1000000000))
            )
        ).Generate();
    }

    public static Cheep GenerateCheep(int seed, Author author)
    {
        Randomizer.Seed = new Random(seed);

        return new Faker<Cheep>().CustomInstantiator(a =>
            new Cheep()
            {
                Author = author,
                Text = a.Lorem.Sentence(5),
                TimeStamp = DateTime.Parse(GetTimeStamp(a.Random.Double(0, 1000000000)))
            }
        ).Generate();
    }

    private static string GetTimeStamp(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
    }
}