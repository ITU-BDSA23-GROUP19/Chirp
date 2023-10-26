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

    private static void SeedDatabase(ChirpContext context)
    {
        Author a1 = new Author() { Name = "hejsameddejsa", Email = "hejsameddejsa@gmail.com", Cheeps = new List<Cheep>() };
        Author a2 = new Author() { Name = "f1skef1let", Email = "f1skef1let@coldmail.com", Cheeps = new List<Cheep>() };
        Author a3 = new Author() { Name = "IsbjørnOgSkruetrækker", Email = "isbjørnogskruetrækker@hotmail.com", Cheeps = new List<Cheep>() };
        Author a4 = new Author() { Name = "GetCheepsFromAuthor", Email = "anotheremail@email.dk", Cheeps = new List<Cheep>() };
        Author a5 = new Author() { Name = "ThisAuthorHasNoCheeps", Email = "DAMthisisamail@email.dk", Cheeps = new List<Cheep>() };

        Cheep c1 = new Cheep() { Author = a4, Text = "Totalsupercool", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        Cheep c2 = new Cheep() { Author = a4, Text = "wow hvad foregår der", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        Cheep c3 = new Cheep() { Author = a4, Text = "vent jeg tror det virker", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        Cheep c4 = new Cheep() { Author = a4, Text = "you disrespect yourself and your nation", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };
        Cheep c5 = new Cheep() { Author = a4, Text = "mine to sidste hjerneceller", TimeStamp = DateTime.Parse("2023-08-01 13:15:21") };

        List<Author> authors = new List<Author>() { a1, a2, a3, a4, a5 };
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

    [Fact]
    public async void CanGetCheeps()
    {
        // Act
        var result = await _repository.GetCheepsAsync(1, 2);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("hejsameddejsa", result.First().Author);
        Assert.Equal("hejsa med dejsa", result.First().Text);
    }

    [Theory]
    [InlineData("GetCheepsFromAuthor", 1, 2)]
    public async void CanGetCheepsFromAuthorAsync(string author, int pageNumber, int pageSize)
    {
        // Act
        var result = await _repository.GetCheepsFromAuthorAsync(author, pageNumber, pageSize);

        // Assert
        //test if we get the correct amount of cheeps
        Assert.Equal(2, result.Count());
        //for all the shown cheeps, check if the cheep.author is the author
        Assert.All(result, cheep => Assert.Equal(author, cheep.Author));

    }

    [Theory]
    [InlineData("ThisAuthorHasNoCheeps", 1, 2)]
    public async void CanGetCheepsFromAuthorAsyncAuthorWithNoCheeps(string author, int pageNumber, int pageSize)
    {
        // Act
        var result = await _repository.GetCheepsFromAuthorAsync(author, pageNumber, pageSize);

        // Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("GetCheepsFromAuthor", 100, 10)]
    public async void CanGetCheepsFromAuthorAsyncTooBigPageNumber(string author, int pageNumber, int pageSize)
    {
        // Act
        var result = await _repository.GetCheepsFromAuthorAsync(author, pageNumber, pageSize);

        // Assert
        Assert.Empty(result);

    }
}