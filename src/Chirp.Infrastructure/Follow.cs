namespace Chirp.Infrastructure;

public class Follow{

    public Guid FollowerId {get; set;}
    public Guid FollowingId {get; set;}

    public required Author FollowerAuthor {get; set;}
    public required Author FollowingAuthor {get; set;}
     
}