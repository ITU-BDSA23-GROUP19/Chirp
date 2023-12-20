namespace Chirp.Infrastructure;

public class FollowRepository : IFollowRepository
{
    private readonly ChirpContext _context;

    public FollowRepository(ChirpContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new follow entity.
    /// </summary>
    /// <param name="follower"></param>
    /// <param name="following"></param>
    /// <exception cref="ArgumentException"></exception>
    public void CreateFollow(string follower, string following)
    {
        // Check if follow already exists
        if (_context.Follows.Any(f => f.FollowerAuthor.Name.Equals(follower) && f.FollowingAuthor.Name.Equals(following)))
        {
            throw new ArgumentException($"A follow already exists between Follower: '{follower}' and Following: '{following}'.");
        }

        // Finds author matching follower name, if that author does not exists a new author is created.
        Author followerAuthor = _context.Authors.Where(a => a.Name.Equals(follower))
                                        .FirstOrDefault() ?? new Author()
                                        {
                                            Name = follower,
                                            Cheeps = new List<Cheep>(),
                                            Following = new HashSet<Follow>(),
                                            Follower = new HashSet<Follow>()
                                        };

        // Finds author matching following name, if that author does not exists a new author is created.
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

    /// <summary>
    /// Deletes follow entity with matching name.
    /// </summary>
    /// <param name="follower"></param>
    /// <param name="following"></param>
    public void DeleteFollow(string follower, string following)
    {
        _context.Follows.Remove(_context.Follows.Single(f => f.FollowerAuthor.Name.Equals(follower) && f.FollowingAuthor.Name.Equals(following)));

        _context.SaveChanges();
    }

    /// <summary>
    /// Delete all follow entities that matches author.
    /// </summary>
    /// <param name="author"></param>
    public void DeleteFollowsFromAuthor(string author)
    {
        List<Follow> follows = _context.Follows.Where(f => f.FollowerAuthor.Name.Equals(author) || f.FollowingAuthor.Name.Equals(author)).ToList();

        foreach (Follow follow in follows)
        {
            _context.Follows.Remove(follow);
        }

        _context.SaveChanges();
    }

    /// <summary>
    /// Check if any follow entities matches follower and following.
    /// </summary>
    /// <param name="follower"></param>
    /// <param name="following"></param>
    /// <returns>True if author exists, otherwise false.</returns>
    public async Task<bool> CheckFollowExistsAsync(string follower, string following)
    {
        return await _context.Follows.AnyAsync(f => f.FollowerAuthor.Name.Equals(follower) && f.FollowingAuthor.Name.Equals(following));
    }

    /// <summary>
    /// Count all follow entities, where following matches author.
    /// </summary>
    /// <param name="author"></param>
    /// <returns>Amount of followers.</returns>
    public async Task<int> GetFollowersCountAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowingAuthor.Name.Equals(author))
                                     .CountAsync();
    }

    /// <summary>
    /// Count all follow entities, where follower matches author.
    /// </summary>
    /// <param name="author"></param>
    /// <returns>Amount of followings.</returns>
    public async Task<int> GetFollowingsCountAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowerAuthor.Name.Equals(author))
                                     .CountAsync();
    }

    /// <summary>
    /// Get all follow entities, where following matches author.
    /// </summary>
    /// <param name="author"></param>
    /// <returns>All followers.</returns>
    public async Task<IEnumerable<string>> GetFollowersAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowingAuthor.Name.Equals(author))
                                     .Select(f => f.FollowerAuthor.Name)
                                     .ToListAsync();
    }

    /// <summary>
    /// Get all follow entities, where follower matches author.
    /// </summary>
    /// <param name="author"></param>
    /// <returns>All followings.</returns>
    public async Task<IEnumerable<string>> GetFollowingsAsync(string author)
    {
        return await _context.Follows.Where(f => f.FollowerAuthor.Name.Equals(author))
                                     .Select(f => f.FollowingAuthor.Name)
                                     .ToListAsync();
    }
}