namespace Chirp.Core;

public interface IFollowRepository
{
    public void CreateFollow(FollowDTO follower, FollowDTO following);
    public void DeleteFollows(FollowDTO followDTO);
    public Task<bool> CheckFollowExistsAsync(FollowDTO follower, FollowDTO following);
    public Task<int> GetFollowersCountAsync(string author);
    public Task<int> GetFollowingsCountAsync(string author);
    public Task<IEnumerable<FollowDTO>> GetFollowersAsync(string author);
    public Task<IEnumerable<FollowDTO>> GetFollowingsAsync(string author);
}