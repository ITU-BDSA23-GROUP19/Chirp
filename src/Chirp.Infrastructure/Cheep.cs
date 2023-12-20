namespace Chirp.Infrastructure;

/// <summary>
/// <para>A cheep is an entity, which is used to represent a message.</para>
/// A cheep contains information about the content of the cheep (<c>Text</c>),
/// when the cheep was created (<c>TimeStamp</c>) and who created the cheep (<c>Author</c>).
/// </summary>
public class Cheep
{
    public Guid CheepId { get; set; }
    public Guid AuthorId { get; set; }
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }

    public required Author Author { get; set; }
}