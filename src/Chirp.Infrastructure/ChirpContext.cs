namespace Chirp.Infrastructure;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps => Set<Cheep>();
    public DbSet<Author> Authors => Set<Author>();

    public ChirpContext(DbContextOptions<ChirpContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(author =>
        {
            author.HasIndex(a => a.Name).IsUnique();
            author.Property(a => a.Name).IsRequired();
            author.Property(a => a.Name).HasMaxLength(50);
            author.HasIndex(a => a.Email).IsUnique();
            author.Property(a => a.Email).IsRequired();
        });

        modelBuilder.Entity<Cheep>(cheep =>
        {
            cheep.Property(c => c.Text).IsRequired();
            cheep.Property(c => c.Text).HasMaxLength(160);
            cheep.Property(c => c.TimeStamp).IsRequired();
        });
    }
}