namespace Chirp.Core;

public interface IFollowRepository
{
    public void CreateFollow(string follower, string following);
    public void DeleteFollow(string follower, string following);
    public void DeleteFollowsFromAuthor(string auhtor);
    public Task<bool> CheckFollowExistsAsync(string follower, string following);
    public Task<int> GetFollowersCountAsync(string author);
    public Task<int> GetFollowingsCountAsync(string author);
    public Task<IEnumerable<string>> GetFollowersAsync(string author);
    public Task<IEnumerable<string>> GetFollowingsAsync(string author);
}