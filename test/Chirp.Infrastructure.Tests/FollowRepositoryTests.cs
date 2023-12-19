namespace Chirp.Infrastructure.Tests;

public class FollowRepositoryTests
{
    private readonly IFollowRepository _repository;
    private readonly ChirpContext _context;

    public FollowRepositoryTests()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        ChirpContext context = new ChirpContext(builder.Options);
        _context = context;
        SeedDatabase(context);
        _repository = new FollowRepository(context);
    }

    private static void SeedDatabase(ChirpContext context)
    {
        Author a1 = new Author() { Name = "hejsameddejsa", Email = "hejsameddejsa@gmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a2 = new Author() { Name = "f1skef1let", Email = "f1skef1let@coldmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a3 = new Author() { Name = "IsbjørnOgSkruetrækker", Email = "isbjørnogskruetrækker@hotmail.com", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a4 = new Author() { Name = "GetCheepsFromAuthor", Email = "anotheremail@email.dk", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };
        Author a5 = new Author() { Name = "ThisAuthorHasNoCheeps", Email = "DAMthisisamail@email.dk", Cheeps = new List<Cheep>(), Following = new HashSet<Follow>(), Follower = new HashSet<Follow>() };

        List<Author> authors = new List<Author>() { a1, a2, a3, a4, a5 };

        context.Authors.AddRange(authors);
        context.SaveChanges();
    }

    [Fact]
    public void CanCreateFollow()
    {
        //Arrange
        AuthorDTO author1 = new AuthorDTO("author1", "");
        AuthorDTO author2 = new AuthorDTO("author2", "");

        //Act
        _repository.CreateFollow("author1", "author2");

        //Assert
        var result = _context.Follows.Any(f => f.FollowerAuthor.Name.Equals("author1") && f.FollowingAuthor.Name.Equals("author2"));
        Assert.True(result);
        //check the follow is only created in one direction
        var nonResult = _context.Follows.Any(f => f.FollowerAuthor.Name.Equals("author2") && f.FollowingAuthor.Name.Equals("author1"));
        Assert.False(nonResult);
    }

    [Fact]
    public void CanDeleteFollowersFromAuthor()
    {
        //Arrange
        AuthorDTO author1 = new AuthorDTO("author1", "");
        AuthorDTO author2 = new AuthorDTO("author2", "");
        _repository.CreateFollow(author2.Name, author1.Name);
        //Act
        _repository.DeleteFollow(author2.Name, author1.Name);
        //Assert
        bool existsInRepository = _context.Follows.Any(f => f.FollowerAuthor.Name.Equals(author2.Name) || f.FollowingAuthor.Name.Equals(author1.Name));
        Assert.False(existsInRepository);
    }

    [Fact]
    public void CanDeleteAuthorFromFollowing()
    {
        //Arrange
        AuthorDTO author1 = new AuthorDTO("author1", "");
        AuthorDTO author2 = new AuthorDTO("author2", "");
        _repository.CreateFollow("author1", "author2");
        _repository.CreateFollow("author2", "author1");

        //Act
        _repository.DeleteFollowsFromAuthor("author1");
        //Assert
        bool existsInRepository = _context.Follows.Any(f => f.FollowerAuthor.Name.Equals(author2.Name) && f.FollowingAuthor.Name.Equals(author1.Name));
        Assert.False(existsInRepository);
    }

    [Fact]
    public async void CanCheckFollowExists()
    {
        //Arrange
        _repository.CreateFollow("author1", "author2");
        _repository.CreateFollow("author2", "author1");

        //Act
        bool result = await _repository.CheckFollowExistsAsync("author1", "author2");

        //Assert
        Assert.True(result);
    }
    [Fact]
    public async void CanCheckFollowExistsForNonExsistingFollow()
    {
        //Arrange
        _repository.CreateFollow("author1", "author2");
        _repository.CreateFollow("author2", "author1");

        //Act
        bool result = await _repository.CheckFollowExistsAsync("author1", "author3");

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async void CanGetFollowerCount()
    {
        //Arrange
        _repository.CreateFollow("author1", "author2");
        _repository.CreateFollow("author2", "author1");
        _repository.CreateFollow("author3", "author2");

        //Act
        int author1Followers = await _repository.GetFollowersCountAsync("author1");
        int author2Followers = await _repository.GetFollowersCountAsync("author2");

        //Assert
        Assert.Equal(1, author1Followers);
        Assert.Equal(2, author2Followers);
    }

    [Fact]
    public async void CanGetFollowingCount()
    {
        //Arrange
        _repository.CreateFollow("author1", "author2");
        _repository.CreateFollow("author2", "author1");
        _repository.CreateFollow("author2", "author3");

        //Act
        int author1Following = await _repository.GetFollowingsCountAsync("author1");
        int author2Following = await _repository.GetFollowingsCountAsync("author2");

        //Assert
        Assert.Equal(1, author1Following);
        Assert.Equal(2, author2Following);
    }

    [Fact]
    public async void CanGetFollowers()
    {
        //Arrange
        _repository.CreateFollow("author2", "author1");
        _repository.CreateFollow("author3", "author1");
        _repository.CreateFollow("author4", "author1");

        //Act
        IEnumerable<string> author1Followers = await _repository.GetFollowersAsync("author1");

        //Assert
        Assert.Equal(3, author1Followers.Count());
        Assert.Contains("author2", author1Followers.ToList());
        Assert.Contains("author3", author1Followers.ToList());
        Assert.Contains("author4", author1Followers.ToList());
    }

    [Fact]
    public async void CanGetFollowings()
    {
        //Arrange
        _repository.CreateFollow("author1", "author2");
        _repository.CreateFollow("author1", "author3");
        _repository.CreateFollow("author1", "author4");

        //Act
        IEnumerable<string> author1Followings = await _repository.GetFollowingsAsync("author1");

        //Assert
        Assert.Equal(3, author1Followings.Count());
        Assert.Contains("author2", author1Followings.ToList());
        Assert.Contains("author3", author1Followings.ToList());
        Assert.Contains("author4", author1Followings.ToList());
    }
}