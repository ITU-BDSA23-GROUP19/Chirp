namespace Chirp.Infrastructure;

public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public IEnumerable<Cheep> Cheeps { get; set; } = new List<Cheep>();
}