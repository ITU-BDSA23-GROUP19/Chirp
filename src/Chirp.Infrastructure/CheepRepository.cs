namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new cheep entity.
    /// </summary>
    /// <param name="cheepDTO"></param>
    /// <exception cref="ArgumentException"></exception>
    public void CreateCheep(CheepDTO cheepDTO)
    {
        // Check if cheepDTO follow chosen rules, look in CheepValidator.
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

        // Finds author matching name, if that author does not exists a new author is created.
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

    /// <summary>
    /// Delete all cheep entities that matches author.
    /// </summary>
    /// <param name="author"></param>
    public void DeleteCheepsFromAuthor(string author)
    {
        List<Cheep> cheeps = _context.Cheeps.Where(c => c.Author.Name.Equals(author)).ToList();

        foreach (Cheep cheep in cheeps)
        {
            _context.Cheeps.Remove(cheep);
        }

        _context.SaveChanges();
    }

    /// <summary>
    /// Count all cheeps entities.
    /// </summary>
    /// <returns>Amount of cheep entities.</returns>
    public async Task<int> GetAllCheepCountAsync()
    {
        return await _context.Cheeps.CountAsync();
    }

    /// <summary>
    /// Count all cheeps entities matching author.
    /// </summary>
    /// <param name="author"></param>
    /// <returns>Amount of cheep entities matching author.</returns>
    public async Task<int> GetMyCheepCountAsync(string author)
    {
        return await _context.Cheeps.Where(c => c.Author.Name.Equals(author))
                                    .CountAsync();
    }

    /// <summary>
    /// Count all cheeps entities matching author or followings.
    /// </summary>
    /// <param name="author"></param>
    /// <param name="followings"></param>
    /// <returns>Amount of cheep entities matching author or followings.</returns>
    public async Task<int> GetUserCheepCountAsync(string author, IEnumerable<string> followings)
    {
        return await _context.Cheeps.Where(c => c.Author.Name.Equals(author) || followings.Contains(c.Author.Name))
                                    .CountAsync();
    }

    /// <summary>
    /// Get all cheep entities on a given page, chosen by pageNumber and pageSize.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns>All cheep entities on a given page.</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<IEnumerable<CheepDTO>> GetAllCheepsAsync(int pageNumber, int pageSize)
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

    /// <summary>
    /// Get all cheep entities matching author on a given page, chosen by pageNumber and pageSize.
    /// </summary>
    /// <param name="author"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns>All cheep entities matching author on a given page.</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<IEnumerable<CheepDTO>> GetMyCheepsAsync(string author, int pageNumber, int pageSize)
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

    /// <summary>
    /// Get all cheep entities matching author or followings on a given page, chosen by pageNumber and pageSize.
    /// </summary>
    /// <param name="author"></param>
    /// <param name="followings"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns>All cheep entities matching author or followings on a given page.</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<IEnumerable<CheepDTO>> GetUserCheepsAsync(string author, IEnumerable<string> followings, int pageNumber = 1, int pageSize = 32)
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

        return await _context.Cheeps.Where(c => c.Author.Name.Equals(author) || followings.Contains(c.Author.Name))
                                    .OrderByDescending(c => c.TimeStamp)
                                    .Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .Select(c => new CheepDTO(c.Author.Name, c.Text, c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")))
                                    .ToListAsync();
    }
}