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
        context.Database.EnsureCreated();
        _repository = new AuthorRepository(context);
    }

    [Fact]
    public void CanCreateAuthorRepositoryTest()
    {
        //Arrange

        //Act

        //Assert
        Assert.NotNull(_repository);
    }

    [Fact]
    public async void CanGetAuthorNull()
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

    [Fact]
    public async void CanGetEmailNull()
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


    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    public async void CanCreateAuthor(String name, String email)
    {
        //Arrange
        AuthorDTO author = new AuthorDTO(name, email);

        //Act
        _repository.CreateAuthor(author);
        string Name = author.Name;

        //Assert
        AuthorDTO authorFromDatabase = await _repository.GetAuthorFromNameAsync(Name);
        Assert.Equal(authorFromDatabase, author);
    }

    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    public async void CanCreateEmail(String name, String email)
    {
        //Arrange
        AuthorDTO author = new AuthorDTO(name, email);

        //Act
        _repository.CreateAuthor(author);
        string Email = author.Email;

        //Assert
        AuthorDTO emailFromDatabase = await _repository.GetAuthorFromEmailAsync(Email);
        Assert.Equal(emailFromDatabase, author);
    }
}