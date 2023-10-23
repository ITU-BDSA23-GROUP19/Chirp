
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
        throw new NotImplementedException();
    }

    public async Task<AuthorDTO> GetAuthorFromEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthorDTO> GetAuthorFromNameAsync(string name)
    {
        throw new NotImplementedException();
    }
}