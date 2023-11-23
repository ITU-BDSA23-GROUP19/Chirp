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

        if (_context.Authors.Any(a => a.Email.Equals(authorDTO.Email)))
        {
            throw new ArgumentException($"An author already exists with email: '{authorDTO.Email}'");
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

    public async Task<AuthorDTO> GetAuthorFromEmailAsync(string email)
    {
        return await _context.Authors.Where(a => a.Email.Equals(email))
                                     .Select(a => new AuthorDTO(a.Name, a.Email))
                                     .FirstOrDefaultAsync() ?? throw new ArgumentException($"No author with email: '{email}'");
    }

    public void FollowAuthor(FollowDTO followDTO)
    {
        Author followingAuthor = _context.Authors.Where(a => a.Name.Equals(followDTO.FollowingName))
                                        .FirstOrDefault() ?? throw new ArgumentException($"No author with name: '{followDTO.FollowingName}'");
        Author followerAuthor = _context.Authors.Where(a => a.Name.Equals(followDTO.FollowerName))
                                        .FirstOrDefault() ?? throw new ArgumentException($"No author with name: '{followDTO.FollowerName}'");

        _context.Follows.Add(new Follow()
        {
            FollowerAuthor = followerAuthor,
            FollowingAuthor = followingAuthor
        });

        _context.SaveChanges();
    }

    public Task<IEnumerable<FollowDTO>> GetFollowings(AuthorDTO AuthorDTO){
        return await _context.Follows.Where(f => f.FollowerAuthor.Name.Equals(AuthorDTO.Name))
                                     .Select(f => new FollowDTO(f.FollowerAuthor.Name, f.FollowingAuthor.Name))
                                     .ToListAsync();
    }

    public Task<IEnumerable<FollowDTO>> GetFollowers(AuthorDTO AuthorDTO){
        return await _context.Follows.Where(f => f.FollowingName.Name.Equals(AuthorDTO.Name))
                                     .Select(f => new followDTO(f.FollowerAuthor.Name, f.FollowingAuthor.Name))
    }                                .ToListAsync();
}