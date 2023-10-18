namespace Chirp.Infrastructure;

public class ChirpDbContext : DbContext
{
    public DbSet<Cheep> Cheeps => Set<Cheep>();
    public DbSet<Author> Authors => Set<Author>();

    public ChirpDbContext(DbContextOptions<ChirpDbContext> options) : base(options)
    {
    }
}