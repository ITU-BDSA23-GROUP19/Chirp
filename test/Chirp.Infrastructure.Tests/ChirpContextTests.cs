namespace Chirp.Infrastructure.Tests;

public class ChirpContextTests
{
    private ChirpContext _context;

    public ChirpContextTests()
    {
        using SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        using ChirpContext context = new ChirpContext(builder.Options);
        context.Database.EnsureCreated();
        _context = context;
    }

    [Fact]
    public void Test()
    {
    }
}