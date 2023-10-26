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
        if (_context.Authors.Any(a => a.Name.Equals(authorDTO.Name)))
        {
            throw new ArgumentException($"An author already exists with name: '{authorDTO.Name}'");
        }

        if (_context.Authors.Any(a => a.Email.Equals(authorDTO.Email)))
        {
            throw new ArgumentException($"An author already exists with email: '{authorDTO.Email}'");
        }

        _context.Authors.Add(new Author()
        {
            Name = authorDTO.Name,
            Email = authorDTO.Email,
            Cheeps = new List<Cheep>()
        });

        _context.SaveChanges();
    }

    public async Task<AuthorDTO> GetAuthorFromNameAsync(string name)
    {
        return await _context.Authors.Where(a => a.Name.Equals(name))
                                     .Select(a => new AuthorDTO(a.Name, a.Email))
                                     .FirstOrDefaultAsync() ?? throw new ArgumentException($"No author with name: '{name}'");
    }

    public async Task<AuthorDTO> GetAuthorFromEmailAsync(string email)
    {
        return await _context.Authors.Where(a => a.Email.Equals(email))
                                     .Select(a => new AuthorDTO(a.Name, a.Email))
                                     .FirstOrDefaultAsync() ?? throw new ArgumentException($"No author with email: '{email}'");
    }
}