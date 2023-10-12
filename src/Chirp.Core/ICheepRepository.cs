namespace Chirp.Core;

public interface ICheepRepository
{
    public List<CheepDTO> GetCheeps(int pageNumber = 1, int pageSize = 32);
    public List<CheepDTO> GetCheepsFromAuthor(string author, int pageNumber = 1, int pageSize = 32);

}