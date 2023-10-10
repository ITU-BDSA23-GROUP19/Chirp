using Chirp.Repository;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _cheepRepository = new CheepRepository();

    public List<CheepViewModel> GetCheeps()
    {
        return CheepDTOToCheepViewModel(_cheepRepository.GetCheeps());
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        return CheepDTOToCheepViewModel(_cheepRepository.GetCheepsFromAuthor(author));
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
