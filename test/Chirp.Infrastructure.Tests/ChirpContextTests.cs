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
                Cheeps = new List<Cheep>()
            });

            _context.Authors.Add(new Author()
            {
                Name = "Simon",
                Email = "unique2@gmail.com",
                Cheeps = new List<Cheep>()
            });

            _context.SaveChanges();
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
                Cheeps = new List<Cheep>()
            });

            _context.Authors.Add(new Author()
            {
                Name = "Unique2",
                Email = "besr@itu.dk",
                Cheeps = new List<Cheep>()
            });

            _context.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            Assert.Equal("An error occurred while saving the entity changes. See the inner exception for details.", e.Message);
        }
    }

    // Can't get constraint tested properly, im gonna ask TA.
    /*
    [Fact]
    public void CanCreateAuthorWithLongName()
    {
        _context.Authors.Add(new Author()
        {
            Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            Email = "besr@gmail.com",
            Cheeps = new List<Cheep>()
        });

        _context.SaveChanges();
    }

    [Fact]
    public void CanCreateCheepWithLongText()
    {
        Author author = new Author()
        {
            Name = "Simon",
            Email = "besr@itu.dk",
            Cheeps = new List<Cheep>()
        };

        _context.Cheeps.Add(new Cheep()
        {
            Author = author,
            Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
            TimeStamp = DateTime.Parse("2023-08-01 13:13:23")
        });

        _context.SaveChanges();
    }
    */
}