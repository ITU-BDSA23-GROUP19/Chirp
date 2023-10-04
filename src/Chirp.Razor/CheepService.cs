public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly DBFacade _facade = new DBFacade();

    public List<CheepViewModel> GetCheeps()
    {
        return _facade.GetCheeps();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        return _facade.GetAuthorCheeps(author);
    }
}
