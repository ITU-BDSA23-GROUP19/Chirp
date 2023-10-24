namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }

    public async void CreateCheep(CheepDTO cheepDTO)
    {
        Author author = await _context.Authors.Where(a => a.Name.Equals(cheepDTO.Author))
                                               .FirstOrDefaultAsync() ?? throw new ArgumentException();

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