namespace Chirp.Core;

public interface ICheepRepository
{
    public IEnumerable<CheepDTO> GetCheeps(int pageNumber = 1, int pageSize = 32);
    public IEnumerable<CheepDTO> GetCheepsFromAuthor(string author, int pageNumber = 1, int pageSize = 32);
}