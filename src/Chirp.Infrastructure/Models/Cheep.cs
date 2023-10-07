namespace Chirp.Infrastructure.Models;

public class Cheep
{
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public required string Message { get; set; }
    public DateTime Timestamp { get; set; }

    public required Author Author { get; set; }
}