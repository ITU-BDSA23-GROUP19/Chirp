namespace Chirp.Infrastructure;

public class FollowRepository : IFollowRepository
{
    private readonly ChirpContext _context;

    public FollowRepository(ChirpContext context)
    {
        _context = context;
    }

    public void CreateFollow(FollowDTO follower, FollowDTO following)
    {
        FollowValidator validator = new FollowValidator();
        ValidationResult result = validator.Validate(follower);
        if (!result.IsValid)
        {
            foreach (ValidationFailure error in result.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            throw new ArgumentException("FollowDTO failed validation");
        }

        result = validator.Validate(following);
        if (!result.IsValid)
        {
            foreach (ValidationFailure error in result.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            throw new ArgumentException("FollowDTO failed validation");
        }

        if (_context.Follows.Any(f => f.FollowerAuthor.Name.Equals(follower.Author) && f.FollowingAuthor.Name.Equals(following.Author)))
        {
            throw new ArgumentException($"A follow already exists between Follower: '{follower.Author}' and Following: '{following.Author}'.");
        }

        Author followerAuthor = _context.Authors.Where(a => a.Name.Equals(follower.Author))
                                        .FirstOrDefault() ?? new Author()
                                        {
                                            Name = follower.Author,
                                            Cheeps = new List<Cheep>(),
                                            Following = new HashSet<Follow>(),
                                            Follower = new HashSet<Follow>()
                                        };

        Author followingAuthor = _context.Authors.Where(a => a.Name.Equals(following.Author))
                                        .FirstOrDefault() ?? new Author()
                                        {
                                            Name = following.Author,
                                            Cheeps = new List<Cheep>(),
                                            Following = new HashSet<Follow>(),
                                            Follower = new HashSet<Follow>()
                                        };

        _context.Follows.Add(new Follow()
        {
            FollowerAuthor = followerAuthor,
            FollowingAuthor = followingAuthor
        });

        _context.SaveChanges();
    }

    public void DeleteFollow(FollowDTO follower, FollowDTO following)
    {
        FollowValidator validator = new FollowValidator();
        ValidationResult result = validator.Validate(follower);
        if (!result.IsValid)
        {
            foreach (ValidationFailure error in result.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            throw new ArgumentException("FollowDTO failed validation");
        }

        result = validator.Validate(following);
        if (!result.IsValid)
        {
            foreach (ValidationFailure error in result.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            throw new ArgumentException("FollowDTO failed validation");
        }

        _context.Remove(_context.Follows.Single(f => f.FollowerAuthor.Name.Equals(follower.Author) && f.FollowingAuthor.Name.Equals(following.Author)));

        _context.SaveChanges();
    }

    public void DeleteFollowsFromAuthor(string author)
    {
        List<Follow> follows = _context.Follows.Where(f => f.FollowerAuthor.Name.Equals(author) || f.FollowingAuthor.Name.Equals(author)).ToList();

        foreach (Follow follow in follows)
        {
            _context.Remove(follow);
        }

        _context.SaveChanges();
    }

    public async Task<bool> CheckFollowExistsAsync(FollowDTO follower, FollowDTO following)
    {
        return await _context.Follows.AnyAsync(f => f.FollowerAuthor.Name.Equals(follower.Author) && f.FollowingAuthor.Name.Equals(following.Author));
    }

    public async Task<int> GetFollowersCountAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowingAuthor.Name.Equals(author))
                                     .CountAsync();
    }

    public async Task<int> GetFollowingsCountAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowerAuthor.Name.Equals(author))
                                     .CountAsync();
    }

    public async Task<IEnumerable<FollowDTO>> GetFollowersAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowingAuthor.Name.Equals(author))
                                     .Select(f => new FollowDTO(f.FollowerAuthor.Name))
                                     .ToListAsync();
    }

    public async Task<IEnumerable<FollowDTO>> GetFollowingsAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowerAuthor.Name.Equals(author))
                                     .Select(f => new FollowDTO(f.FollowingAuthor.Name))
                                     .ToListAsync();
    }
}