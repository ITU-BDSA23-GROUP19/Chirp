namespace Chirp.Infrastructure.Tests;

public class AuthorRepositoryTests
{
    private readonly IAuthorRepository _repository;

    public AuthorRepositoryTests()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        ChirpContext context = new ChirpContext(builder.Options);
        SeedDatabase(context);
        _repository = new AuthorRepository(context);
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

        context.Authors.AddRange(authors);
        context.SaveChanges();
    }

    [Fact]
    public void CanCreateAuthorRepository()
    {
        // Assert
        Assert.NotNull(_repository);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public async void CanCreateAuthorCanGetAuthorFromName(int seed)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(seed);

        // Act
        _repository.CreateAuthor(authorDTO);
        AuthorDTO author = await _repository.GetAuthorFromNameAsync(authorDTO.Name);

        // Assert
        Assert.Equal(authorDTO.Name, author.Name);
        Assert.Equal(authorDTO.Email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public async void CanCreateAuthorCanGetAuthorFromEmail(int seed)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(seed);

        // Act
        _repository.CreateAuthor(authorDTO);
        AuthorDTO author = await _repository.GetAuthorFromEmailAsync(authorDTO.Email);

        // Assert
        Assert.Equal(authorDTO.Name, author.Name);
        Assert.Equal(authorDTO.Email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void CanCreateAuthorWhereNameExists(int seed)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(seed);

        // Act and Assert
        try
        {
            _repository.CreateAuthor(authorDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"An author already exists with name: '{authorDTO.Name}'", e.Message);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void CanCreateAuthorWhereEmailExists(int seed)
    {
        //Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO tempAuthorDTO = dataGenerator.GenerateAuthorDTO(seed);
        AuthorDTO authorDTO = new AuthorDTO("Cohen Spears", tempAuthorDTO.Email);

        //Act and Assert
        try
        {
            _repository.CreateAuthor(authorDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"An author already exists with email: '{authorDTO.Email}'", e.Message);
        }
    }

    [Fact]
    public void CanCreateAuthorWithLongName()
    {
        // Arrange
        AuthorDTO authorDTO = new AuthorDTO("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "Cohen.Spears@gmail.com");

        // Act and Assert
        try
        {
            _repository.CreateAuthor(authorDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal("Name length exceeds 50 characters using 56 characters", e.Message);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void CanGetAuthorFromName(int seed)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(seed);

        // Act
        AuthorDTO author = await _repository.GetAuthorFromNameAsync(authorDTO.Name);

        // Assert
        Assert.Equal(author.Name, author.Name);
        Assert.Equal(author.Email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public async void CanGetAuthorFromNameWhichDoesNotExists(int seed)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(seed);

        // Act and Assert
        try
        {
            Assert.Null(await _repository.GetAuthorFromNameAsync(authorDTO.Name));
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"No author with name: '{authorDTO.Name}'", e.Message);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void CanGetAuthorFromEmail(int seed)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(seed);

        // Act
        AuthorDTO author = await _repository.GetAuthorFromEmailAsync(authorDTO.Email);

        // Assert
        Assert.Equal(authorDTO.Name, author.Name);
        Assert.Equal(authorDTO.Email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public async void CanGetAuthorFromEmailWhichDoesNotExists(int seed)
    {
        // Arrange
        DataGenerator dataGenerator = new DataGenerator();

        AuthorDTO authorDTO = dataGenerator.GenerateAuthorDTO(seed);

        // Act and Assert
        try
        {
            Assert.Null(await _repository.GetAuthorFromEmailAsync(authorDTO.Email));
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"No author with email: '{authorDTO.Email}'", e.Message);
        }
    }
}