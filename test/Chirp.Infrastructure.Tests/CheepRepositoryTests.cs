namespace Chirp.Infrastructure.Tests;

public class CheepRepositoryTests
{
    private readonly ICheepRepository _repository;

    public CheepRepositoryTests()
    {
        using SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using ChirpContext context = new ChirpContext(builder.Options);
        context.Database.EnsureCreated();
        _repository = new CheepRepository(context);
    }

    [Fact]
    public void CanCreateCheepRepositoryTest()
    {
        //Assert
        Assert.NotNull(_repository);
    }

    [Theory]
    [InlineData()]
    public void CanCreateCheep(string author, string text, string timeStamp)
    {
        //Arrange
        CheepDTO cheep = new CheepDTO(author, text, timeStamp);

        //Act
        _repository.CreateCheep(cheep);

        //Assert
    }
}