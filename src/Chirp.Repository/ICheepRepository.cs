using Chirp.Razor;

namespace Chirp.Repository;
public interface ICheepRepository
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);

}