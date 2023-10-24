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
        context.Authors.Add(new Author(){ Name = "hejsameddejsa", Email = "hejsameddejsa@gmail.com"});
        _repository = new CheepRepository(context);
    }

    [Fact]
    public void CanCreateCheepRepositoryTest()
    {
        //Assert
        Assert.NotNull(_repository);
    }

    [Theory]
    [InlineData("hejsameddejsa", "hejsa med dejsa", "2023-08-01 13:13:23")]
    public async void CanCreateCheep(string author, string text, string timeStamp)
    {
        //Arrange
        CheepDTO cheep = new CheepDTO(author, text, timeStamp);

        //Act
        _repository.CreateCheep(cheep);

        //Assert
        IEnumerable<CheepDTO> cheeeeeeeeeeeeps = await _repository.GetCheepsFromAuthorAsync(author);
        foreach (CheepDTO cheep1 in cheeeeeeeeeeeeps) {
            Assert.Equal(text, cheep1.Text);
        }
    }
}