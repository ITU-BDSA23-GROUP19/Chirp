namespace Chirp.Infrastructure;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps => Set<Cheep>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Follow> Follows => Set<Follow>();

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
        });

        modelBuilder.Entity<Cheep>(cheep =>
        {
            cheep.Property(c => c.Text).IsRequired();
            cheep.Property(c => c.Text).HasMaxLength(160);
            cheep.Property(c => c.TimeStamp).IsRequired();
        });

        modelBuilder.Entity<Follow>(follow =>
        {
            follow.HasKey(f => new { f.FollowerId, f.FollowingId });
            follow.HasIndex(f => new { f.FollowerId, f.FollowingId }).IsUnique();

            follow.HasOne(f => f.FollowerAuthor)
                .WithMany(a => a.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            follow.HasOne(f => f.FollowingAuthor)
                .WithMany(a => a.Follower)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}