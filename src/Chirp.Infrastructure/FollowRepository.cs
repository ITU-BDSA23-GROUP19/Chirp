namespace Chirp.Infrastructure;

public class FollowRepository : IFollowRepository
{
    private readonly ChirpContext _context;

    public FollowRepository(ChirpContext context)
    {
        _context = context;
    }

    public void CreateFollow(string follower, string following)
    {
        if (_context.Follows.Any(f => f.FollowerAuthor.Name.Equals(follower) && f.FollowingAuthor.Name.Equals(following)))
        {
            throw new ArgumentException($"A follow already exists between Follower: '{follower}' and Following: '{following}'.");
        }

        Author followerAuthor = _context.Authors.Where(a => a.Name.Equals(follower))
                                        .FirstOrDefault() ?? new Author()
                                        {
                                            Name = follower,
                                            Cheeps = new List<Cheep>(),
                                            Following = new HashSet<Follow>(),
                                            Follower = new HashSet<Follow>()
                                        };

        Author followingAuthor = _context.Authors.Where(a => a.Name.Equals(following))
                                        .FirstOrDefault() ?? new Author()
                                        {
                                            Name = following,
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

    public void DeleteFollow(string follower, string following)
    {
        _context.Remove(_context.Follows.Single(f => f.FollowerAuthor.Name.Equals(follower) && f.FollowingAuthor.Name.Equals(following)));

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

    public async Task<bool> CheckFollowExistsAsync(string follower, string following)
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

    public async Task<IEnumerable<string>> GetFollowersAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowingAuthor.Name.Equals(author))
                                     .Select(f => new string(f.FollowerAuthor.Name))
                                     .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetFollowingsAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowerAuthor.Name.Equals(author))
                                     .Select(f => new string(f.FollowingAuthor.Name))
                                     .ToListAsync();
    }
}