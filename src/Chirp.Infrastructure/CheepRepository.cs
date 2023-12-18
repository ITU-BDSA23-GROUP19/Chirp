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
        CheepValidator validator = new CheepValidator();
        ValidationResult result = validator.Validate(cheepDTO);
        if (!result.IsValid)
        {
            foreach (ValidationFailure error in result.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            throw new ArgumentException("CheepDTO failed validation");
        }

        Author author = _context.Authors.Where(a => a.Name.Equals(cheepDTO.Author))
                                        .FirstOrDefault() ?? new Author()
                                        {
                                            Name = cheepDTO.Author,
                                            Cheeps = new List<Cheep>(),
                                            Following = new HashSet<Follow>(),
                                            Follower = new HashSet<Follow>()
                                        };

        _context.Cheeps.Add(new Cheep()
        {
            Author = author,
            Text = cheepDTO.Text,
            TimeStamp = DateTime.Parse(cheepDTO.TimeStamp)
        });

        _context.SaveChanges();
    }

    public async Task<int> GetCheepCountAsync()
    {
        return await _context.Cheeps.CountAsync();
    }

    public async Task<int> GetCheepCountFromAuthorAsync(string author)
    {
        return await _context.Cheeps.Where(c => c.Author.Name.Equals(author))
                                    .CountAsync();
    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number below 1 is not allowed.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentException("Page size below 1 is not allowed.");
        }
        else if (pageSize > 32)
        {
            throw new ArgumentException("Page size above 32 is not allowed.");
        }

        return await _context.Cheeps.OrderByDescending(c => c.TimeStamp)
                                    .Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .Select(c => new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")))
                                    .ToListAsync();
    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthorAsync(string author, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number below 1 is not allowed.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentException("Page size below 1 is not allowed.");
        }
        else if (pageSize > 32)
        {
            throw new ArgumentException("Page size above 32 is not allowed.");
        }

        return await _context.Cheeps.Where(c => c.Author.Name.Equals(author))
                                    .OrderByDescending(c => c.TimeStamp)
                                    .Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .Select(c => new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")))
                                    .ToListAsync();
    }

    public void DeleteCheepsFromAuthor(string author)
    {
        _context.Remove(_context.Cheeps.Any(c => c.Author.Name.Equals(author)));
        _context.SaveChanges();
    }
}