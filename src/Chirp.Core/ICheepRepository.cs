namespace Chirp.Core;

public interface ICheepRepository
{
    public Task<IEnumerable<CheepDTO>> GetCheepsAsync(int pageNumber = 1, int pageSize = 32);
    public Task<IEnumerable<CheepDTO>> GetCheepsFromAuthorAsync(string author, int pageNumber = 1, int pageSize = 32);
}