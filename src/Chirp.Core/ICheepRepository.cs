namespace Chirp.Core;

public interface ICheepRepository
{
    public void CreateCheep(CheepDTO cheep);
    public void DeleteCheepsFromAuthor(string author);
    public Task<int> GetAllCheepCountAsync();
    public Task<int> GetMyCheepCountAsync(string author);
    public Task<int> GetUserCheepCountAsync(string author, IEnumerable<string> followings);
    public Task<IEnumerable<CheepDTO>> GetAllCheepsAsync(int pageNumber = 1, int pageSize = 32);
    public Task<IEnumerable<CheepDTO>> GetMyCheepsAsync(string author, int pageNumber = 1, int pageSize = 32);
    public Task<IEnumerable<CheepDTO>> GetUserCheepsAsync(string author, IEnumerable<string> followings, int pageNumber = 1, int pageSize = 32);
}