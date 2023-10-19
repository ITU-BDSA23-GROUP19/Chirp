namespace Chirp.Infrastructure.Tests;

public class CheepRepositoryTests
{
    private ICheepRepository _repository;

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
    public void Test()
    {
    }
}