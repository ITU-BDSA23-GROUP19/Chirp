using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;

public class CheepRepository : ICheepReposity
{
    private ChirpDBContext _context;

    public CheepRepository()
    {
        _context = new ChirpDBContext();
    }
    public void GetCheeps()
    {
        var test = from c in _context.Authors
                   where c.Name.Contains("a")
                   orderby c.Name descending
                   select new { Author = c.Name };

        foreach (var name in test)
        {
            Console.WriteLine(name.Author);
        }
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        throw new NotImplementedException();
    }
}