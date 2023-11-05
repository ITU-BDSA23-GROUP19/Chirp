namespace Chirp.Infrastructure.Tests;

public class ChirpContextTests
{
    private readonly ChirpContext _context;

    public ChirpContextTests()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        _context = new ChirpContext(builder.Options);
    }

    [Fact]
    public void CanCreateChirpRepository()
    {
        // Assert
        Assert.NotNull(_context);
    }
}