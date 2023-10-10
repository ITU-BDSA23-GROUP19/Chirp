public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _repository = new CheepRepository();

    public List<CheepViewModel> GetCheeps()
    {
        return _repository.GetCheeps();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        return _repository.GetAuthorCheeps(author);
    }
}
