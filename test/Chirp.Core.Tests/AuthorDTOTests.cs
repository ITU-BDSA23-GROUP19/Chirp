namespace Chirp.Core.Tests;

public class AuthorDTOTests
{
    [Theory]
    [InlineData("Rasmus", "rnie@itu.dk")]
    [InlineData("Helge", "ropf@itu.dk")]
    [InlineData("Johnnie Calixto", "Jacqualine.Gilcoine@gmail.com")]
    public void CanCreateAuthorDTO(string name, string email)
    {
        // Arrange
        AuthorDTO authorDTO = new AuthorDTO(name, email);

        // Act
        string Name = authorDTO.Name;
        string Email = authorDTO.Email!;

        // Assert
        Assert.Equal(name, Name);
        Assert.Equal(email, Email);
    }
}