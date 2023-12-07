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

    [Fact]
    public void CanCreateAuthorWhereNameExists()
    {
        // Act and Assert
        try
        {
            _context.Authors.Add(new Author()
            {
                Name = "Simon",
                Email = "unique1@gmail.com",
                Cheeps = new List<Cheep>(),
                Following = new HashSet<Follow>(),
                Follower = new HashSet<Follow>()
            });

            _context.Authors.Add(new Author()
            {
                Name = "Simon",
                Email = "unique2@gmail.com",
                Cheeps = new List<Cheep>(),
                Following = new HashSet<Follow>(),
                Follower = new HashSet<Follow>()
            });

            _context.SaveChanges();
            Assert.Fail();
        }
        catch (DbUpdateException e)
        {
            Assert.Equal("An error occurred while saving the entity changes. See the inner exception for details.", e.Message);
        }
    }

    [Fact]
    public void CanCreateAuthorWhereEmailExists()
    {
        // Act and Assert
        try
        {
            _context.Authors.Add(new Author()
            {
                Name = "Unique1",
                Email = "besr@itu.dk",
                Cheeps = new List<Cheep>(),
                Following = new HashSet<Follow>(),
                Follower = new HashSet<Follow>()
            });

            _context.Authors.Add(new Author()
            {
                Name = "Unique2",
                Email = "besr@itu.dk",
                Cheeps = new List<Cheep>(),
                Following = new HashSet<Follow>(),
                Follower = new HashSet<Follow>()
            });

            _context.SaveChanges();
            Assert.Fail();
        }
        catch (DbUpdateException e)
        {
            Assert.Equal("An error occurred while saving the entity changes. See the inner exception for details.", e.Message);
        }
    }
}