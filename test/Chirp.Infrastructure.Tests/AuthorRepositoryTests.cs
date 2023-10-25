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

    private void SeedDatabase(ChirpContext context)
    {

    }

    [Fact]
    public void CanCreateAuthorRepositoryTest()
    {
        //Assert
        Assert.NotNull(_repository);
    }

    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    public async void CanCreateAuthorGetAuthorFromName(string name, string email)
    {
        //Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        //Act
        _repository.CreateAuthor(authorDTO);

        //Assert
        AuthorDTO author = await _repository.GetAuthorFromNameAsync(name);

        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    public async void CanCreateAuthorGetAuthorFromEmail(string name, string email)
    {
        //Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        //Act
        _repository.CreateAuthor(authorDTO);

        //Assert
        AuthorDTO author = await _repository.GetAuthorFromEmailAsync(email);

        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    public async void CanCreateAuthorWhichExists()
    {
        // Arrange

        // Act

        // Assert
    }


    public async void CanGetAuthorFromName()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async void CanGetAuthorFromNameWhichDoesNotExists()
    {
        try
        {
            Assert.Null(await _repository.GetAuthorFromNameAsync("John"));
        }
        catch (ArgumentException e)
        {
            Assert.Equal("No author with name: 'John'", e.Message);
        }
    }

    public async void CanGetAuthorFromEmail()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async void CanGetAuthorFromEmailWhichDoesNotExists()
    {
        try
        {
            Assert.Null(await _repository.GetAuthorFromEmailAsync("jjjj@itu.dk"));
        }
        catch (ArgumentException e)
        {
            Assert.Equal("No author with email: 'jjjj@itu.dk'", e.Message);
        }
    }
}