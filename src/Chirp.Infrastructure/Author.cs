namespace Chirp.Infrastructure;

/// <summary>
/// <para>An author is an entity, which is used to represent a user.</para>
/// An author contains information about the name (<c>Name</c>) and email of the author (<c>Email</c>),
/// all the cheeps by the author (<c>Cheeps</c>), who the author is following (<c>Following</c>) and the followers of the author (<c>Follower</c>).
/// </summary>
public class Author
{
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }

    public required ICollection<Cheep> Cheeps { get; set; }
    public required ICollection<Follow> Following { get; set; }
    public required ICollection<Follow> Follower { get; set; }
}