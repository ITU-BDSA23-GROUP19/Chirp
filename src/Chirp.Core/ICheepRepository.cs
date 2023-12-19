namespace Chirp.Core;

public interface ICheepRepository
{
    public void CreateCheep(CheepDTO cheep);
    public void DeleteCheepsFromAuthor(string author);
    public Task<int> GetCheepCountAsync();
    public Task<int> GetCheepCountFromAuthorAsync(string author);
    public Task<int> GetUserTimelineCheepCountAsync(string author, IEnumerable<FollowDTO> followings);
    public Task<IEnumerable<CheepDTO>> GetCheepsAsync(int pageNumber = 1, int pageSize = 32);
    public Task<IEnumerable<CheepDTO>> GetCheepsFromAuthorAsync(string author, int pageNumber = 1, int pageSize = 32);
    public Task<IEnumerable<CheepDTO>> GetUserTimelineCheepsAsync(string author, IEnumerable<FollowDTO> followings, int pageNumber = 1, int pageSize = 32);
}