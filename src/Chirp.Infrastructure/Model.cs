namespace Chirp.Infrastructure;

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