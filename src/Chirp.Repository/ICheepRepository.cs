namespace Chirp.Repository;
public interface ICheepRepository
{
    public List<CheepDTO> GetCheeps();
    public List<CheepDTO> GetCheepsFromAuthor(string author);

}