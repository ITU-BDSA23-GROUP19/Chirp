namespace Chirp.Infrastructure.Tests;

public class CheepRepositoryTests
{
    private readonly ICheepRepository _repository;

    public CheepRepositoryTests()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        ChirpContext context = new ChirpContext(builder.Options);
        SeedDatabase(context);
        _repository = new CheepRepository(context);
    }

    private void SeedDatabase(ChirpContext context)
    {
        Author a1 = new Author() { Name = "hejsameddejsa", Email = "hejsameddejsa@gmail.com" };
        Author a2 = new Author() { Name = "f1skef1let", Email = "f1skef1let@coldmail.com" };
        Author a3 = new Author() { Name = "IsbjørnOgSkruetrækker", Email = "isbjørnogskruetrækker@hotmail.com" };
        Author a4 = new Author() { Name = "GetCheepsFromAuthor", Email = "anotheremail@email.dk" };

        Cheep c1 = new Cheep() { Author = a4, Text = "Totalsupercool", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        Cheep c2 = new Cheep() { Author = a4, Text = "wow hvad foregår der", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        Cheep c3 = new Cheep() { Author = a4, Text = "vent jeg tror det virker", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        Cheep c4 = new Cheep() { Author = a4, Text = "you disrespect yourself and your nation", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        Cheep c5 = new Cheep() { Author = a4, Text = "mine to sidste hjerneceller", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };

        List<Author> authors = new List<Author>() { a1, a2, a3, a4 };
        List<Cheep> cheeps = new List<Cheep>() { c1, c2, c3, c4, c5 };

        context.Authors.AddRange(authors);
        context.Cheeps.AddRange(cheeps);
        context.SaveChanges();
    }

    [Fact]
    public void CanCreateCheepRepositoryTest()
    {
        // Assert
        Assert.NotNull(_repository);
    }

    [Theory]
    [InlineData("hejsameddejsa", "hejsa med dejsa", "2023-08-01 13:13:23")]
    [InlineData("f1skef1let", "Jeg elsker fiskefilet.", "2023-08-03 13:13:23")]
    [InlineData("IsbjørnOgSkruetrækker", "lidt effektikt: lidt godt, meget effektivt: meget godt", "2023-10-01 13:13:23")]
    public async void CanCreateCheep(string author, string text, string timeStamp)
    {
        // Arrange
        CheepDTO cheepDTO = new CheepDTO(author, text, timeStamp);

        // Act
        _repository.CreateCheep(cheepDTO);

        // Assert
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(author);
        CheepDTO cheep = cheeps.Single();

        Assert.Single(cheeps);
        Assert.Equal(author, cheep.Author);
        Assert.Equal(text, cheep.Text);
        Assert.Equal(timeStamp, cheep.TimeStamp);
        Assert.Equal(cheepDTO, cheep);
    }

    public async void CanCreateCheepFromAuthorWhichExists()
    {
        // Arrange

        // Act

        // Assert
    }

    public async void CanGetCheeps()
    {
        // Arrange

        // Act

        // Assert
    }

    [Theory]
    [InlineData("Test", 0, 0)]
    public async void GetCheepsFromAuthor(string author, int pageNumber, int pageSize)
    {
        // Arrange

        // Act

        // Assert
    }

    public async void CanGetCheepsFromAuthorWhichDoesNotExists()
    {
        // Arrange

        // Act

        // Assert
    }
}