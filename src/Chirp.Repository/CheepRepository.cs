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
    public void GetCheeps()
    {
        var authors = from c in _context.Cheeps
                      select new { c.Text };

        foreach (var author in authors)
        {
            Console.WriteLine(author.Text);
        }
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        throw new NotImplementedException();
    }
}