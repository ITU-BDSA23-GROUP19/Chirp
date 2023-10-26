namespace Chirp.Infrastructure.Tests;

public class AuthorRepositoryTests
{
    private readonly IAuthorRepository _repository;

    public AuthorRepositoryTests()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        DbContextOptionsBuilder<ChirpContext> builder = new DbContextOptionsBuilder<ChirpContext>().UseSqlite(connection);
        ChirpContext context = new ChirpContext(builder.Options);
        SeedDatabase(context);
        _repository = new AuthorRepository(context);
    }

    private static void SeedDatabase(ChirpContext context)
    {
        Author a1 = new Author() { Name = "hejsameddejsa", Email = "hejsameddejsa@gmail.com", Cheeps = new List<Cheep>() };
        Author a2 = new Author() { Name = "f1skef1let", Email = "f1skef1let@coldmail.com", Cheeps = new List<Cheep>() };
        Author a3 = new Author() { Name = "IsbjørnOgSkruetrækker", Email = "isbjørnogskruetrækker@hotmail.com", Cheeps = new List<Cheep>() };
        Author a4 = new Author() { Name = "GetCheepsFromAuthor", Email = "anotheremail@email.dk", Cheeps = new List<Cheep>() };

        List<Author> authors = new List<Author>() { a1, a2, a3, a4 };

        context.Authors.AddRange(authors);
        context.SaveChanges();
    }

    [Fact]
    public void CanCreateAuthorRepositoryTest()
    {
        //Assert
        Assert.NotNull(_repository);
    }

    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    public async void CanCreateAuthorGetAuthorFromName(string name, string email)
    {
        //Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        //Act
        _repository.CreateAuthor(authorDTO);

        //Assert
        AuthorDTO author = await _repository.GetAuthorFromNameAsync(name);

        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    public async void CanCreateAuthorGetAuthorFromEmail(string name, string email)
    {
        //Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        //Act
        _repository.CreateAuthor(authorDTO);

        //Assert
        AuthorDTO author = await _repository.GetAuthorFromEmailAsync(email);

        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData("hejsameddejsa", "simr@itu.dk")]
    [InlineData("f1skef1let", "apno@itu.dk")]
    public void CanCreateAuthorWhichExists(string name, string email)
    {
        //Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        //Act and Assert
        try
        {
            _repository.CreateAuthor(authorDTO);
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"An author already exists with name: '{name}'", e.Message);
        }
    }

    [Theory]
    [InlineData("hejsameddejsa", "hejsameddejsa@gmail.com")]
    [InlineData("f1skef1let", "f1skef1let@coldmail.com")]
    public async void CanGetAuthorFromName(string name, string email)
    {
        // Act
        AuthorDTO author = await _repository.GetAuthorFromNameAsync(name);

        // Assert
        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
    }

    [Theory]
    [InlineData("rødspætte")]
    [InlineData("fisker")]
    public async void CanGetAuthorFromNameWhichDoesNotExists(string name)
    {
        try
        {
            Assert.Null(await _repository.GetAuthorFromNameAsync(name));
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"No author with name: '{name}'", e.Message);
        }
    }

    [Theory]
    [InlineData("IsbjørnOgSkruetrækker", "isbjørnogskruetrækker@hotmail.com")]
    [InlineData("GetCheepsFromAuthor", "anotheremail@email.dk")]
    public async void CanGetAuthorFromEmail(string name, string email)
    {
        // Act
        AuthorDTO author = await _repository.GetAuthorFromEmailAsync(email);

        // Assert
        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
    }

    [Theory]
    [InlineData("hellow@hotmail.com")]
    [InlineData("dasbot@email.dk")]
    public async void CanGetAuthorFromEmailWhichDoesNotExists(string email)
    {
        // Act and Assert
        try
        {
            Assert.Null(await _repository.GetAuthorFromEmailAsync(email));
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"No author with email: '{email}'", e.Message);
        }
    }
}