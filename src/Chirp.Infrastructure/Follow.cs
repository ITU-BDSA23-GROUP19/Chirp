namespace Chirp.Infrastructure;

/// <summary>
/// <para>A follow is an entity, which is used to represent an author following another author.</para>
/// A follow contains information about who is the follower (<c>FollowingAuthor</c>) and who the follower follows (<c>FollowingAuthor</c>).
/// </summary>
public class Follow
{
    public Guid FollowerId { get; set; }
    public Guid FollowingId { get; set; }

    public required Author FollowerAuthor { get; set; }
    public required Author FollowingAuthor { get; set; }
}