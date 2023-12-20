namespace Chirp.Infrastructure.Tests;

public class CheepRepositoryTests
{
    private readonly ICheepRepository _repository;
    private readonly ChirpContext _context;

    public CheepRepositoryTests()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        ChirpContext context = new ChirpContext(builder.Options);
        _context = context;
        SeedDatabase(context);
        _repository = new CheepRepository(context);
    }

    private static void SeedDatabase(ChirpContext context)
    {
        Author a1 = new Author() { Name = "hejsameddejsa", Email = "hejsameddejsa@gmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a2 = new Author() { Name = "f1skef1let", Email = "f1skef1let@coldmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a3 = new Author() { Name = "IsbjørnOgSkruetrækker", Email = "isbjørnogskruetrækker@hotmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a4 = new Author() { Name = "GetCheepsFromAuthor", Email = "anotheremail@email.dk", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a5 = new Author() { Name = "ThisAuthorHasNoCheeps", Email = "DAMthisisamail@email.dk", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };

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
        IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync(author);
        CheepDTO cheep = cheeps.Single();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal(author, cheep.Author);
        Assert.Equal(text, cheep.Text);
        Assert.Equal(timeStamp, cheep.TimeStamp);
        Assert.Equal(cheepDTO, cheep);
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
            Assert.Equal("CheepDTO failed validation", e.Message);
        }
    }

    [Fact]
    public async void CanGetAllCheepCount()
    {
        int cheepCount = await _repository.GetAllCheepCountAsync();

        Assert.Equal(5, cheepCount);
    }

    [Fact]
    public async void CanGetMyCheepCount()
    {
        int cheepCount = await _repository.GetMyCheepCountAsync("GetCheepsFromAuthor");

        Assert.Equal(5, cheepCount);
    }

    [Fact]
    public async void CanGetAllCheeps()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetAllCheepsAsync(1, 1);
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
    public async void CanGetAllCheepsPageSize(int pageSize)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetAllCheepsAsync(1, pageSize);

        // Assert
        Assert.Equal(pageSize, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetAllCheepsPageSizeTooLarge(int pageSize)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetAllCheepsAsync(1, pageSize);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(-10)]
    public async void CanGetAllCheepsPageSizeTooSmall(int pageSize)
    {
        // Act and Assert
        try
        {
            IEnumerable<CheepDTO> cheeps = await _repository.GetAllCheepsAsync(1, pageSize);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal("Page size below 1 is not allowed.", e.Message);
        }
    }

    [Fact]
    public async void CanGetAllCheepsPageNumber()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetAllCheepsAsync(1, 5);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetAllCheepsPageNumberTooLarge(int pageNumber)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetAllCheepsAsync(pageNumber, 5);

        // Assert
        Assert.Empty(cheeps);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(-10)]
    public async void CanGetAllCheepsPageNumberTooSmall(int pageNumber)
    {
        // Act and Assert
        try
        {
            IEnumerable<CheepDTO> cheeps = await _repository.GetAllCheepsAsync(pageNumber, 5);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal("Page number below 1 is not allowed.", e.Message);
        }
    }

    [Fact]
    public async void CanGetMyCheeps()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("GetCheepsFromAuthor", 1, 1);
        CheepDTO cheep = cheeps.Single();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal("GetCheepsFromAuthor", cheep.Author);
        Assert.Equal("Totalsupercool", cheep.Text);
        Assert.Equal("2023-08-01 13:15:25", cheep.TimeStamp);
    }

    [Fact]
    public async void CanGetMyCheepsWithNoCheeps()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("ThisAuthorHasNoCheeps", 1, 1);

        // Assert
        Assert.Empty(cheeps);
    }

    [Fact]
    public async void CanGetMyCheepsWhichDoesNotExists()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("ThisAuthorDoesNotExists", 1, 1);

        // Assert
        Assert.Empty(cheeps);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public async void CanGetMyCheepsPageSize(int pageSize)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("GetCheepsFromAuthor", 1, pageSize);

        // Assert
        Assert.Equal(pageSize, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetMyCheepsPageSizeTooLarge(int pageSize)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("GetCheepsFromAuthor", 1, pageSize);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(-10)]
    public async void CanGetMyCheepsPageSizeTooSmall(int pageSize)
    {
        // Act and Assert
        try
        {
            IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("GetCheepsFromAuthor", 1, pageSize);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal("Page size below 1 is not allowed.", e.Message);
        }
    }

    [Fact]
    public async void CanGetMyCheepsPageNumber()
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("GetCheepsFromAuthor", 1, 5);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetMyCheepsPageNumberTooLarge(int pageNumber)
    {
        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("GetCheepsFromAuthor", pageNumber, 5);

        // Assert
        Assert.Empty(cheeps);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(-10)]
    public async void CanGetMyCheepsPageNumberTooSmall(int pageNumber)
    {
        // Act and Assert
        try
        {
            IEnumerable<CheepDTO> cheeps = await _repository.GetMyCheepsAsync("GetCheepsFromAuthor", pageNumber, 5);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal("Page number below 1 is not allowed.", e.Message);
        }
    }

    [Fact]
    public void CanDeleteAuthorCheepsFromCheepRepository()
    {
        //Arrange
        AuthorDTO author = new AuthorDTO("author1", "");
        CheepDTO cheep = new CheepDTO("author1", "Lorem ipsum dolor sit amet, consect ut lemco laboris nisi ut aliquip ex ea commodo consequat.", "2023-08-01 13:13:23");
        //Act
        _repository.CreateCheep(cheep);
        _repository.DeleteCheepsFromAuthor("author1");
        //Assert
        bool existsInRepository = _context.Cheeps.Any(c => c.Author.Name.Equals(author.Name));
        Assert.False(existsInRepository);
    }
}