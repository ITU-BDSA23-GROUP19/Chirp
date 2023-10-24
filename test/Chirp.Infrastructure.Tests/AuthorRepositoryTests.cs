namespace Chirp.Infrastructure.Tests;

public class AuthorRepositoryTests
{
    private IAuthorRepository _repository;

    public AuthorRepositoryTests()
    {
        using SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using ChirpContext context = new ChirpContext(builder.Options);
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


    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    public async void CanCreateAuthor(String name, String email)
    {
        //Arrange
        AuthorDTO author = new AuthorDTO(name, email);
        _repository.CreateAuthor(author);

        //Act
        String Name = author.Name;
        String Email = author.Email;

        AuthorDTO authorFromDatabase = await _repository.GetAuthorFromNameAsync(Name);


        //Assert
        Assert.Equal(authorFromDatabase, author);
    }

    [Fact]
    public async void CanGetNull()
    {
        Assert.Null(await _repository.GetAuthorFromNameAsync("John"));
    }

}