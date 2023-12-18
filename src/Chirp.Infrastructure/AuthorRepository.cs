namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpContext _context;

    public AuthorRepository(ChirpContext context)
    {
        _context = context;
    }

    public void CreateAuthor(AuthorDTO authorDTO)
    {
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

    public async Task<AuthorDTO> GetAuthorFromNameAsync(string name)
    {
        return await _context.Authors.Where(a => a.Name.Equals(name))
                                     .Select(a => new AuthorDTO(a.Name, a.Email))
                                     .FirstOrDefaultAsync() ?? throw new ArgumentException($"No author with name: '{name}'");
    }

    public void DeleteAuthor(string author) {
        _context.Remove(_context.Authors.Any(a => a.Name.Equals(author)));
        _context.SaveChanges();
    }
}