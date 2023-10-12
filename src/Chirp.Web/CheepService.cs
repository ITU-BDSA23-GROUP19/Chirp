namespace Chirp.Web;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int pageNumber);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNumber);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository = new CheepRepository();

    public List<CheepViewModel> GetCheeps(int pageNumber)
    {
        return CheepDTOToCheepViewModel(_cheepRepository.GetCheeps(pageNumber));
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNumber)
    {
        return CheepDTOToCheepViewModel(_cheepRepository.GetCheepsFromAuthor(author, pageNumber));
    }

    private List<CheepViewModel> CheepDTOToCheepViewModel(List<CheepDTO> cheepsDTO)
    {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        foreach (CheepDTO cheep in cheepsDTO)
        {
            cheeps.Add(new CheepViewModel(cheep.Author, cheep.Message, cheep.Timestamp));
        }

        return cheeps;
    }
}

public record CheepViewModel(string Author, string Message, string Timestamp);
