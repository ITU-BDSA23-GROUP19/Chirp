namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpContext _context;

    public AuthorRepository(ChirpContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new author entity. If author already exists it throws an ArgumentException().
    /// </summary>
    /// <param name="authorDTO"></param>
    /// <exception cref="ArgumentException"></exception>
    public void CreateAuthor(AuthorDTO authorDTO)
    {
        // Check if authorDTO follow chosen rules, look in AuthorValidator.
        AuthorValidator validator = new AuthorValidator();
        ValidationResult result = validator.Validate(authorDTO);
        if (!result.IsValid)
        {
            foreach (ValidationFailure error in result.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            throw new ArgumentException("AuthorDTO failed validation");
        }

        // Check if author already exists
        if (_context.Authors.Any(a => a.Name.Equals(authorDTO.Name)))
        {
            throw new ArgumentException($"An author already exists with name: '{authorDTO.Name}'");
        }

        _context.Authors.Add(new Author()
        {
            Name = authorDTO.Name,
            Email = authorDTO.Email,
            Cheeps = new List<Cheep>(),
            Following = new HashSet<Follow>(),
            Follower = new HashSet<Follow>()
        });

        _context.SaveChanges();
    }

    /// <summary>
    /// Deletes author entity with matching name.
    /// </summary>
    /// <param name="author"></param>
    public void DeleteAuthor(string author)
    {
        _context.Authors.Remove(_context.Authors.Single(a => a.Name.Equals(author)));

        _context.SaveChanges();
    }

    /// <summary>
    /// Check if any author entities matches name.
    /// </summary>
    /// <param name="author"></param>
    /// <returns>True if author exists, otherwise false.</returns>
    public async Task<bool> CheckAuthorExistsAsync(string author)
    {
        return await _context.Authors.AnyAsync(a => a.Name.Equals(author));
    }

    /// <summary>
    /// Get author entity matching name, if author does not exists thorw ArgumentException.
    /// </summary>
    /// <param name="author"></param>
    /// <returns>An AuthorDTO matching name.</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<AuthorDTO> GetAuthorAsync(string author)
    {
        return await _context.Authors.Where(a => a.Name.Equals(author))
                                     .Select(a => new AuthorDTO(a.Name, a.Email))
                                     .FirstOrDefaultAsync() ?? throw new ArgumentException($"No author with name: '{author}'");
    }
}