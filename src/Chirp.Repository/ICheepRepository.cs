using Chirp.Razor;

namespace Chirp.Repository;
public interface ICheepRepository
{
    public void GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);

}