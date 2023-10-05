using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) 
        => options.UseSqlite($"Data Source={Path.Combine(Path.GetTempPath(), "chirp.db")}");
}

public class Cheep
{
    public int CheepId { get; set; }
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; }

    public int AuthorId { get; set; }
    public required Author Author { get; set; }
}

public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public List<Cheep> Cheeps { get; } = new List<Cheep>();
}