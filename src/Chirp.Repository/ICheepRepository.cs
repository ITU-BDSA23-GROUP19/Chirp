using Chirp.Razor;
public interface ICheepRepository
{
    public void GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);

}