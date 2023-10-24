namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpContext _context;

    public AuthorRepository(ChirpContext context)
    {
        _context = context;
    }

    public void CreateAuthor(AuthorDTO author)
    {
        _context.Authors.Add(new Author() { Name = author.Name, Email = author.Email });
        _context.SaveChanges();
    }

    public async Task<AuthorDTO> GetAuthorFromNameAsync(string name)
    {
        AuthorDTO? author = await _context.Authors.Where(a => a.Name.Equals(name))
                                                  .Select(a => new AuthorDTO(a.Name, a.Email))
                                                  .FirstOrDefaultAsync();

        return author ?? throw new ArgumentException($"No author with name: '{name}'");
    }

    public async Task<AuthorDTO> GetAuthorFromEmailAsync(string email)
    {
        AuthorDTO? author = await _context.Authors.Where(a => a.Email.Equals(email))
                                                  .Select(a => new AuthorDTO(a.Name, a.Email))
                                                  .FirstOrDefaultAsync();

        return author ?? throw new ArgumentException($"No author with email '{email}'.");
    }
}