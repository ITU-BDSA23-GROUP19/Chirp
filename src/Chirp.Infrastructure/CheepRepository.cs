namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }

    public void CreateCheep(CheepDTO cheep, AuthorDTO author)
    {
        Author? cheepAuthor = _context.Authors.Find(author.Name);
        if (cheepAuthor == null)
        {
            // you should not be able to Cheep if you do not have an account
            throw new ArgumentException();
        }
        else
        {
            Cheep cheepToAdd = new Cheep() { Author = cheepAuthor, Text = cheep.Text, TimeStamp = DateTime.Parse(cheep.TimeStamp) };
            cheepAuthor.Cheeps = cheepAuthor.Cheeps.Concat(new[] { cheepToAdd });
            _context.Cheeps.Add(cheepToAdd);
        }

        _context.SaveChanges();
    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsAsync(int pageNumber, int pageSize)
    {
        return await _context.Cheeps.OrderByDescending(c => c.TimeStamp)
                                    .Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .Select(c => new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString()))
                                    .ToListAsync();
    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthorAsync(string author, int pageNumber, int pageSize)
    {
        return await _context.Cheeps.Where(c => c.Author.Name.Equals(author))
                                    .OrderByDescending(c => c.TimeStamp)
                                    .Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .Select(c => new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString()))
                                    .ToListAsync();
    }
}