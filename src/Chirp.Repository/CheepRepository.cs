using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;

namespace Chirp.Repository;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _context;

    public CheepRepository()
    {
        _context = new ChirpDBContext();
        DbInitializer.SeedDatabase(_context);
    }
    public List<CheepDTO> GetCheeps()
    {
        var cheeps = from c in _context.Cheeps
                     orderby c.TimeStamp descending
                     select new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString());

        List<CheepDTO> cheepList = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            cheepList.Add(cheep);
        }

        return cheepList;
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author)
    {
        var cheeps = from c in _context.Cheeps
                     where c.Author.Name.Contains(author)
                     orderby c.TimeStamp descending
                     select new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString());

        List<CheepDTO> cheepList = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            cheepList.Add(cheep);
        }

        return cheepList;
    }
}