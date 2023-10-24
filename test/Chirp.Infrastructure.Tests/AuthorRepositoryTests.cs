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

    /*
    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    public void CanCreate()
    {

    }*/
}