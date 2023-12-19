namespace Chirp.Infrastructure.Tests;

public class FollowRepositoryTests
{
    private readonly IFollowRepository _repository;
    private readonly ChirpContext _context;

    public FollowRepositoryTests()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        ChirpContext context = new ChirpContext(builder.Options);
        _context = context;
        SeedDatabase(context);
        _repository = new FollowRepository(context);
    }

    private static void SeedDatabase(ChirpContext context)
    {
        Author a1 = new Author() { Name = "hejsameddejsa", Email = "hejsameddejsa@gmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a2 = new Author() { Name = "f1skef1let", Email = "f1skef1let@coldmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a3 = new Author() { Name = "IsbjørnOgSkruetrækker", Email = "isbjørnogskruetrækker@hotmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a4 = new Author() { Name = "GetCheepsFromAuthor", Email = "anotheremail@email.dk", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a5 = new Author() { Name = "ThisAuthorHasNoCheeps", Email = "DAMthisisamail@email.dk", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };

        List<Author> authors = new List<Author>() { a1, a2, a3, a4, a5 };

        context.Authors.AddRange(authors);
        context.SaveChanges();
    }
}