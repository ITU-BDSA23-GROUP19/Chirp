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
        DataGenerator dataGenerator = new DataGenerator();

        Author a1 = dataGenerator.GenerateAuthor(1);
        Author a2 = dataGenerator.GenerateAuthor(2);
        Author a3 = dataGenerator.GenerateAuthor(3);
        Author a4 = dataGenerator.GenerateAuthor(4);
        Author a5 = dataGenerator.GenerateAuthor(5);

        List<Author> authors = new List<Author>() { a1, a2, a3, a4, a5 };

        Cheep c1 = dataGenerator.GenerateCheep(1, a5);
        Cheep c2 = dataGenerator.GenerateCheep(2, a5);
        Cheep c3 = dataGenerator.GenerateCheep(3, a5);
        Cheep c4 = dataGenerator.GenerateCheep(4, a5);
        Cheep c5 = dataGenerator.GenerateCheep(5, a5);

        List<Cheep> cheeps = new List<Cheep>() { c1, c2, c3, c4, c5 };
        a5.Cheeps = new List<Cheep>() { c1, c2, c3, c4, c5 };

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
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public async void CanCreateCheep(int seed)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(1);
        CheepDTO cheepDTO = dataGenerator.GenerateCheepDTO(seed, authorDTO);

        // Act
        _repository.CreateCheep(cheepDTO);
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(authorDTO.Name);
        CheepDTO cheep = cheeps.Single();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal(cheepDTO.Author, cheep.Author);
        Assert.Equal(cheepDTO.Text, cheep.Text);
        Assert.Equal(cheepDTO.TimeStamp, cheep.TimeStamp);
        Assert.Equal(cheepDTO, cheep);
    }

    [Fact]
    public void CanCreateCheepWhereAuthorDoesNotExists()
    {
        //Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(6);
        CheepDTO cheepDTO = dataGenerator.GenerateCheepDTO(6, authorDTO);

        //Act and Assert
        try
        {
            _repository.CreateCheep(cheepDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"No author with name: '{authorDTO.Name}'", e.Message);
        }
    }

    [Fact]
    public void CanCreateCheepWithLongText()
    {
        // Arrange
        CheepDTO cheepDTO = new CheepDTO("Cohen Spears", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", "2023-08-01 13:13:23");

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
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(5);
        CheepDTO cheepDTO = dataGenerator.GenerateCheepDTO(5, authorDTO);

        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsAsync(1, 1);
        CheepDTO cheep = cheeps.Single();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal(cheepDTO.Author, cheep.Author);
        Assert.Equal(cheepDTO.Text, cheep.Text);
        Assert.Equal(cheepDTO.TimeStamp, cheep.TimeStamp);
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
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(5);
        CheepDTO cheepDTO = dataGenerator.GenerateCheepDTO(5, authorDTO);

        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(authorDTO.Name, 1, 1);
        CheepDTO cheep = cheeps.Single();

        // Assert
        Assert.Single(cheeps);
        Assert.Equal(cheepDTO.Author, cheep.Author);
        Assert.Equal(cheepDTO.Text, cheep.Text);
        Assert.Equal(cheepDTO.TimeStamp, cheep.TimeStamp);
    }

    [Fact]
    public async void CanGetCheepsFromAuthorWithNoCheeps()
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(1);

        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(authorDTO.Name, 1, 1);

        // Assert
        Assert.Empty(cheeps);
    }

    [Fact]
    public async void CanGetCheepsFromAuthorWhichDoesNotExists()
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(6);

        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(authorDTO.Name, 1, 1);

        // Assert
        Assert.Empty(cheeps);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public async void CanGetCheepsFromAuthorPageSize(int pageSize)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(5);

        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(authorDTO.Name, 1, pageSize);

        // Assert
        Assert.Equal(pageSize, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetCheepsFromAuthorPageSizeTooLarge(int pageSize)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(5);

        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(authorDTO.Name, 1, pageSize);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Fact]
    public async void CanGetCheepsFromAuthorPageNumber()
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(5);

        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(authorDTO.Name, 1, 5);

        // Assert
        Assert.Equal(5, cheeps.Count());
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async void CanGetCheepsFromAuthorPageNumberTooLarge(int pageNumber)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(5);

        // Act
        IEnumerable<CheepDTO> cheeps = await _repository.GetCheepsFromAuthorAsync(authorDTO.Name, pageNumber, 5);

        // Assert
        Assert.Empty(cheeps);
    }
}