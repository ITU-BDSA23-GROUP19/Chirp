namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }

    public void CreateCheep(CheepDTO cheepDTO)
    {
        if (cheepDTO.Text.Length > 160)
        {
            throw new ArgumentException($"Text length exceeds 160 characters using {cheepDTO.Text.Length} characters");
        }

        Author author = _context.Authors.Where(a => a.Name.Equals(cheepDTO.Author))
                                        .FirstOrDefault() ?? throw new ArgumentException($"No author with name: '{cheepDTO.Author}'");

        Cheep cheep = new Cheep()
        {
            Author = author,
            Text = cheepDTO.Text,
            TimeStamp = DateTime.Parse(cheepDTO.TimeStamp)
        };

        _context.Cheeps.Add(cheep);
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