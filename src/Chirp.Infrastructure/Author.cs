namespace Chirp.Infrastructure;

public class Author
{
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public required ICollection<Cheep> Cheeps { get; set; }
    public required ICollection<Follow> Following { get; set; }
    public required ICollection<Follow> Follower { get; set; }
}