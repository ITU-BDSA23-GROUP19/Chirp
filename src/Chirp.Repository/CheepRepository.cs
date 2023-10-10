using Chirp.Razor;

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
    public List<CheepViewModel> GetCheeps()
    {
        var cheeps = from c in _context.Cheeps
                     orderby c.TimeStamp descending
                     select new { Author = c.Author.Name, Message = c.Text, Timestamp = c.TimeStamp };

        List<CheepViewModel> cheepList = new List<CheepViewModel>();

        foreach (var cheep in cheeps)
        {
            cheepList.Add(new CheepViewModel(cheep.Author, cheep.Message, cheep.Timestamp.ToString()));
        }

        return cheepList;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        var cheeps = from c in _context.Cheeps
                     where c.Author.Name.Contains(author)
                     orderby c.TimeStamp descending
                     select new { Author = c.Author.Name, Message = c.Text, Timestamp = c.TimeStamp };

        List<CheepViewModel> cheepList = new List<CheepViewModel>();

        foreach (var cheep in cheeps)
        {
            cheepList.Add(new CheepViewModel(cheep.Author, cheep.Message, cheep.Timestamp.ToString()));
        }

        return cheepList;
    }
}