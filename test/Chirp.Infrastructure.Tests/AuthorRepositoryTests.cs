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
        Author a5 = new Author() { Name = "ThisAuthorHasNoCheeps", Email = "DAMthisisamail@email.dk", Cheeps = new List<Cheep>() };

        List<Author> authors = new List<Author>() { a1, a2, a3, a4, a5 };

        context.Authors.AddRange(authors);
        context.SaveChanges();
    }

    [Fact]
    public void CanCreateAuthorRepository()
    {
        // Assert
        Assert.NotNull(_repository);
    }

    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    [InlineData("Johnnie Calixto", "Jacqualine.Gilcoine@gmail.com")]
    public async void CanCreateAuthorCanGetAuthorFromName(string name, string email)
    {
        // Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        // Act
        _repository.CreateAuthor(authorDTO);
        AuthorDTO author = await _repository.GetAuthorFromNameAsync(name);

        // Assert
        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData("Simon", "simr@itu.dk")]
    [InlineData("Annabell", "apno@itu.dk")]
    [InlineData("Johnnie Calixto", "Jacqualine.Gilcoine@gmail.com")]
    public async void CanCreateAuthorCanGetAuthorFromEmail(string name, string email)
    {
        // Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        // Act
        _repository.CreateAuthor(authorDTO);
        AuthorDTO author = await _repository.GetAuthorFromEmailAsync(email);

        // Assert
        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
        Assert.Equal(authorDTO, author);
    }

    [Theory]
    [InlineData("hejsameddejsa", "simr@itu.dk")]
    [InlineData("f1skef1let", "apno@itu.dk")]
    [InlineData("IsbjørnOgSkruetrækker", "Jacqualine.Gilcoine@gmail.com")]
    public void CanCreateAuthorWhereNameExists(string name, string email)
    {
        // Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        // Act and Assert
        try
        {
            _repository.CreateAuthor(authorDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"An author already exists with name: '{name}'", e.Message);
        }
    }

    [Theory]
    [InlineData("Simon", "hejsameddejsa@gmail.com")]
    [InlineData("Annabell", "f1skef1let@coldmail.com")]
    [InlineData("Johnnie Calixto", "isbjørnogskruetrækker@hotmail.com")]
    public void CanCreateAuthorWhereEmailExists(string name, string email)
    {
        //Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        //Act and Assert
        try
        {
            _repository.CreateAuthor(authorDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"An author already exists with email: '{email}'", e.Message);
        }
    }

    [Fact]
    public void CanCreateAuthorWithLongName()
    {
        // Arrange
        AuthorDTO authorDTO = new AuthorDTO("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "hejsameddejsa@gmail.com");

        try
        {
            _repository.CreateAuthor(authorDTO);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal("AuthorDTO failed validation", e.Message);
        }
    }

    [Theory]
    [InlineData("hejsameddejsa", "hejsameddejsa@gmail.com")]
    [InlineData("f1skef1let", "f1skef1let@coldmail.com")]
    [InlineData("IsbjørnOgSkruetrækker", "isbjørnogskruetrækker@hotmail.com")]
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
    [InlineData("skrubbe")]
    [InlineData("søtunge")]
    public async void CanGetAuthorFromNameWhichDoesNotExists(string name)
    {
        // Act and Assert
        try
        {
            Assert.Null(await _repository.GetAuthorFromNameAsync(name));
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"No author with name: '{name}'", e.Message);
        }
    }

    [Theory]
    [InlineData("hejsameddejsa", "hejsameddejsa@gmail.com")]
    [InlineData("f1skef1let", "f1skef1let@coldmail.com")]
    [InlineData("IsbjørnOgSkruetrækker", "isbjørnogskruetrækker@hotmail.com")]
    public async void CanGetAuthorFromEmail(string name, string email)
    {
        // Act
        AuthorDTO author = await _repository.GetAuthorFromEmailAsync(email);

        // Assert
        Assert.Equal(name, author.Name);
        Assert.Equal(email, author.Email);
    }

    [Theory]
    [InlineData("rødspætte@hotmail.com")]
    [InlineData("skrubbe@email.dk")]
    [InlineData("søtunge@gmail.com")]
    public async void CanGetAuthorFromEmailWhichDoesNotExists(string email)
    {
        // Act and Assert
        try
        {
            Assert.Null(await _repository.GetAuthorFromEmailAsync(email));
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"No author with email: '{email}'", e.Message);
        }
    }
}