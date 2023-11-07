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

        Cheep c1 = new Cheep() { Author = a4, Text = "Totalsupercool", TimeStamp = DateTime.Parse("2023-08-01 13:15:25") };
        Cheep c2 = new Cheep() { Author = a4, Text = "wow hvad foregår der", TimeStamp = DateTime.Parse("2023-08-01 13:15:24") };
        Cheep c3 = new Cheep() { Author = a4, Text = "vent jeg tror det virker", TimeStamp = DateTime.Parse("2023-08-01 13:15:23") };
        Cheep c4 = new Cheep() { Author = a4, Text = "you disrespect yourself and your nation", TimeStamp = DateTime.Parse("2023-08-01 13:15:22") };
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
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(author);
        CheepDTO cheep = cheeps.Single();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal(author, cheep.Author);
        Assert.Equal(text, cheep.Text);
        Assert.Equal(timeStamp, cheep.TimeStamp);
        Assert.Equal(cheepDTO, cheep);
    }

    [Fact]
    public void CanCreateCheepWhereAuthorDoesNotExists()
    {
        //Arrange
        CheepDTO cheepDTO = new CheepDTO("This author does not exists", "hejsa med dejsa", "2023-08-01 13:13:23");

        //Act and Assert
        try
        {
            _repository.CreateCheep(cheepDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"No author with name: 'This author does not exists'", e.Message);
        }
    }

    [Fact]
    public void CanCreateCheepWithLongText()
    {
        // Arrange
        CheepDTO cheepDTO = new CheepDTO("hejsameddejsa", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", "2023-08-01 13:13:23");

        // Act and Assert
        try
        {
            _repository.CreateCheep(cheepDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal("Text length exceeds 160 characters using 231 characters", e.Message);
        }
    }

    [Fact]
    public async void CanGetCheeps()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsAsync(1, 1);
        CheepDTO cheep = cheeps.Single();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal("GetCheepsFromAuthor", cheep.Author);
        Assert.Equal("Totalsupercool", cheep.Text);
        Assert.Equal("2023-08-01 13:15:25", cheep.TimeStamp);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public async void CanGetCheepsPageSize(int pageSize)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsAsync(1, pageSize);

        // Assert
        Assert.Equal(pageSize, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetCheepsPageSizeTooLarge(int pageSize)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsAsync(1, pageSize);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Fact]
    public async void CanGetCheepsPageNumber()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsAsync(1, 5);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetCheepsPageNumberTooLarge(int pageNumber)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsAsync(pageNumber, 5);

        // Assert
        Assert.Empty(cheeps);
    }

    [Fact]
    public async void CanGetCheepsFromAuthor()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync("GetCheepsFromAuthor", 1, 1);
        CheepDTO cheep = cheeps.Single();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal("GetCheepsFromAuthor", cheep.Author);
        Assert.Equal("Totalsupercool", cheep.Text);
        Assert.Equal("2023-08-01 13:15:25", cheep.TimeStamp);
    }

    [Fact]
    public async void CanGetCheepsFromAuthorWithNoCheeps()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync("ThisAuthorHasNoCheeps", 1, 1);

        // Assert
        Assert.Empty(cheeps);
    }

    [Fact]
    public async void CanGetCheepsFromAuthorWhichDoesNotExists()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync("ThisAuthorDoesNotExists", 1, 1);

        // Assert
        Assert.Empty(cheeps);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public async void CanGetCheepsFromAuthorPageSize(int pageSize)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync("GetCheepsFromAuthor", 1, pageSize);

        // Assert
        Assert.Equal(pageSize, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetCheepsFromAuthorPageSizeTooLarge(int pageSize)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync("GetCheepsFromAuthor", 1, pageSize);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Fact]
    public async void CanGetCheepsFromAuthorPageNumber()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync("GetCheepsFromAuthor", 1, 5);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetCheepsFromAuthorPageNumberTooLarge(int pageNumber)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync("GetCheepsFromAuthor", pageNumber, 5);

        // Assert
        Assert.Empty(cheeps);
    }
}