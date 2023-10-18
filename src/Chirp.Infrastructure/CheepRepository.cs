namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDbContext _context;

    public CheepRepository(ChirpDbContext context)
    {
        _context = context;
    }

    public IEnumerable<CheepDTO> GetCheeps(int pageNumber, int pageSize)
    {
        var cheeps = from c in _context.Cheeps
                     orderby c.TimeStamp descending
                     select new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString());

        var pagedCheeps = cheeps.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        List<CheepDTO> cheepList = new List<CheepDTO>();

        foreach (var cheep in pagedCheeps)
        {
            cheepList.Add(cheep);
        }

        return cheepList;
    }

    public IEnumerable<CheepDTO> GetCheepsFromAuthor(string author, int pageNumber, int pageSize)
    {
        var cheeps = from c in _context.Cheeps
                     where c.Author.Name.Contains(author)
                     orderby c.TimeStamp descending
                     select new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString());

        var pagedCheeps = cheeps.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        List<CheepDTO> cheepList = new List<CheepDTO>();

        foreach (var cheep in pagedCheeps)
        {
            cheepList.Add(cheep);
        }

        return cheepList;
    }
}